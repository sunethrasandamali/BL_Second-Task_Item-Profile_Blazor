using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BL10.CleanArchitecture.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlueLotus360.Com.Infrastructure.Managers.APIManager;
using Newtonsoft.Json;
using RestSharp;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using BlueLotus360.Com.Client.Infrastructure.Extensions;
using BlueLotus360.Com.Infrastructure.Managers.OrderManager;
using System.Security.Cryptography;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus.Com.Domain.Entity;
using BlueLotus360.Com.Infrastructure.Managers.Address;

namespace BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe
{
    public class PickmeAPIHandler
    {
        public readonly IAPIManager _apiManager;
        public readonly IOrderManager _OrderManager;
        public readonly IAddressManager _addressManager;
        private readonly OrderPlatformAPIInformation orderPlatformAPI; 

        public PickmeAPIHandler(IAPIManager apiManager, IOrderManager orderManager,IAddressManager addressManager)
        {
            _apiManager = apiManager;
            _addressManager = addressManager;
            orderPlatformAPI = new OrderPlatformAPIInformation(_apiManager, _addressManager);
            _OrderManager = orderManager;
        }
        public async Task<GetOrder> GetPickmeOrder(int LocationKey)
        {
            APIRequestParameters apiParameters = new APIRequestParameters()
            {
                LocationKey = LocationKey,
                APIIntegrationName = "PickMe_" + LocationKey,
                APIName = "PickMe",
                EndPointName=""

            };
            PartnerOrder ord = await _OrderManager.GetLastSyncTime(apiParameters);
            var time = TimeSpan.FromMinutes(ord.OrderLastSyncMinutes);
            string hour = String.Format("{0:N2}", time.TotalHours);
            APIInformation tokenInfo = await orderPlatformAPI.GetAPIDetailsByEndpointName(apiParameters);
             APIRequestParameters endpointrequest = new APIRequestParameters()
            {
                LocationKey = LocationKey,
                APIIntegrationKey = tokenInfo.APIIntegrationKey,
                EndPointName = PickmeEndpoints.GetOrder.GetDescription()
            };
            APIInformation endpointInfo = await _apiManager.GetAPIEndPoints(endpointrequest);
            var client = new RestClient(tokenInfo.BaseURL);
            var request = new RestRequest(endpointInfo.EndPointURL + "?page=1&hours=" + hour, Method.Get);
            request.AddHeader("X-API-KEY", tokenInfo.SecretInstanceKey.Replace("\r", "").Replace("\n", "").Replace("\t", ""));
            request.AddHeader("Content-Type", "application/json");
            var response = client.Execute(request);
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                MissingMemberHandling = MissingMemberHandling.Ignore
            };
            GetOrder returnData = JsonConvert.DeserializeObject<GetOrder>(response.Content, settings);
            return returnData;
        }
        public DateTime ConvertTimestamp(long timestamp)
        {
            DateTime result = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;
            return result;
        }
        public async Task<bool> SavePickmeOrder(GetOrder getOrder,int LocationKey,string FromDate,string ToDate)
        {
            bool isSaved = false;
            RequestParameters requestParameters = new RequestParameters()
            {
                LocationKey = LocationKey,
                StatusKey = 1,
                FromDate = FromDate,
                ToDate = ToDate
            };

            foreach (data item in getOrder.Data)
            {
                bool IsAvailable = false;
                IList<PartnerOrder> List = await _OrderManager.GetAllPartnerOrder(requestParameters);
                if (List.Count > 0)
                {
                    PartnerOrder availableData = List.Where(c => c.OrderId == item.PickmeJobID).FirstOrDefault();
                    if (availableData != null)
                    {
                        if (availableData.PartnerOrderId > 11)
                        {
                            IsAvailable = true;
                        }
                    }
                    else
                    {
                        IsAvailable = false;
                    }
                }
                if (!IsAvailable)
                {
                    PartnerOrder saveOrder = new PartnerOrder();
                    saveOrder.Location.CodeKey = LocationKey;
                    // saveOrder.CreatedBy.UserKey = 1;
                    saveOrder.PartnerOrderId = 1;
                    saveOrder.OrderId = item.PickmeJobID;
                    saveOrder.OrderReference = item.PickmeJobID;
                    saveOrder.OrderDate = ConvertTimestamp(Convert.ToInt64(item.CreatedTimestamp)).ToString("yyyy/MM/dd hh:mm:ss tt");
                    saveOrder.DeliveryBrand = "";
                    saveOrder.IsActive = 1;
                    saveOrder.IsApproved = 1;
                    saveOrder.Platforms.AccountCode = "PickMe";
                    //string deliverytypecode = item.DeliveryMode;
                    saveOrder.OrderStatus.CodeName = item.Status.Name;
                    CodeBaseResponse code = new CodeBaseResponse()
                    {
                        CodeName = saveOrder.OrderStatus.CodeName,
                        OurCode = saveOrder.Platforms.AccountCode
                    };
                    CodeBaseResponse record = await _OrderManager.GetOrderStatusByPartnerStatus(code);
                    saveOrder.OrderStatus.CodeKey = record.CodeKey;

                    //PartnerOrderDetails partnerpaymentDetails = new PartnerOrderDetails();
                    //partnerpaymentDetails.OrderItem = GetConnection().PartnerOrderRepository.GetItemDetailsWithPrices(CompanyKey, LocationKey, saveOrder.CreatedBy.UserKey, "PickmeWallet").OrderItem;
                    //partnerpaymentDetails.ItemQuantity = 1;
                    //partnerpaymentDetails.TransactionPrice = Decimal.Round((Convert.ToDecimal(item.Payment.Total)), 2);
                    //partnerpaymentDetails.BaseTotalPrice = Decimal.Round((Convert.ToDecimal(item.Payment.Total)), 2);
                    //partnerpaymentDetails.SpecialInstructions = "";
                    //partnerpaymentDetails.ItemOfferKey = 1;
                    //partnerpaymentDetails.ApproveSate = 1;
                    //partnerpaymentDetails.IsActive = true;
                    //if (partnerpaymentDetails.OrderItem.EAN.Length > 15)
                    //{
                    //    partnerpaymentDetails.Remarks = partnerpaymentDetails.OrderItem.ItemCode;
                    //}
                    //else
                    //{
                    //    partnerpaymentDetails.Remarks = partnerpaymentDetails.OrderItem.EAN;
                    //}
                    //saveOrder.OrderItemDetails.Add(partnerpaymentDetails);

                    //saveOrder.PaymentKey = partnerpaymentDetails.OrderItem.ItemKey; //review


                    foreach (Items LineItem in item.Order.Items)
                    {
                        PartnerOrderDetails partnerOrderDetails = new PartnerOrderDetails();
                        ItemResponse itemrec = new ItemResponse()
                        {
                            ItemCode = LineItem.ID.ToString()
                        };
                        partnerOrderDetails.OrderItem = await _OrderManager.GetItemByItemCode(itemrec);
                        partnerOrderDetails.ItemQuantity = Convert.ToDecimal(LineItem.Qty);
                        partnerOrderDetails.TransactionPrice = Convert.ToDecimal(LineItem.Total) / Convert.ToDecimal(LineItem.Qty);
                        partnerOrderDetails.BaseTotalPrice = Convert.ToDecimal(LineItem.Total);
                        partnerOrderDetails.SpecialInstructions = LineItem.SpIns;
                        partnerOrderDetails.IsApproved = 1;
                        partnerOrderDetails.IsActive = 1;
                        partnerOrderDetails.Remarks = "";
                        saveOrder.OrderItemDetails.Add(partnerOrderDetails);
                    }

                    if (item.Customer != null)
                    {
                        saveOrder.Customer.Phone = item.Customer.ContactNumber;
                        saveOrder.Customer.Address = item.Customer.Location.Address;
                        saveOrder.Customer.AdrId = item.Customer.ContactNumber;
                        saveOrder.Customer.Name = item.Customer.ContactNumber;
                        await orderPlatformAPI.SaveCustomer(saveOrder);
                    }

                   PartnerOrder SavedData= await _OrderManager.SavePartnerOrders(saveOrder);
                    if(SavedData.PartnerOrderId > 11)
                    {
                        isSaved= true;
                    }
                    else
                    {
                        isSaved= false;
                    }
                }

            }

            return isSaved;

        }

    }
}
