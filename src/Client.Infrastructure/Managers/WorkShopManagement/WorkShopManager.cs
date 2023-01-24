using BL10.CleanArchitecture.Domain.Entities.Booking;
using BL10.CleanArchitecture.Domain.Entities.MaterData;
using BL10.CleanArchitecture.Domain.Entities.WorkShopManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.WorkShopManagement
{
    public class WorkShopManager:IWorkShopManager
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public WorkShopManager(HttpClient httpClient, IConfiguration config) 
        { 
            _httpClient = httpClient;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);

        }

        public async Task<IList<Vehicle>> GetVehicleDetailsByVehregNo(VehicleSearch request)
        {
            IList<Vehicle> vehicles = new List<Vehicle>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SearchVehicleDetailsEndpoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                vehicles = JsonConvert.DeserializeObject<IList<Vehicle>>(content);
               


            }
            catch (Exception exp)
            {

            }
            return vehicles;
        }

        public async Task<IList<WorkOrder>> GetJobHistory(Vehicle request)
        {
            IList<WorkOrder> vehicles = new List<WorkOrder>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetJobHistoryDetailsEndpoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                vehicles = JsonConvert.DeserializeObject<IList<WorkOrder>>(content);



            }
            catch (Exception exp)
            {

            }
            return vehicles;
        }

        public async Task<ProjectResponse> CreateJob(Project request)
        {
            ProjectResponse pro = new ProjectResponse();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateProjectEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                pro = JsonConvert.DeserializeObject<ProjectResponse>(content);



            }
            catch (Exception exp)
            {

            }
            return pro;
        }

        public async Task<IList<ProjectResponse>> SelectOngoingProjectDetails(Vehicle request)
        {
            IList<ProjectResponse> proList = new List<ProjectResponse>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SelectCarMartOngoingProjectDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                proList = JsonConvert.DeserializeObject<List<ProjectResponse>>(content);



            }
            catch (Exception exp)
            {

            }
            return proList;
        }

        public async Task SaveWorkOrder(Order order)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveWorkOrderEndPoint, order);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                order.OrderNumber = ser.OrderNumber;
                order.OrderPrefix = new CodeBaseResponse() { CodeName = ser.Prefix };
                order.OrderKey = ser.OrderKey;

            }
            catch (Exception exp)
            {
                order.OrderNumber = "ERR";
            }
        }
        public async Task SaveIRNWorkOrder(Order request) 
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveIRNWorkOrderEndPoint, request);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                request.OrderNumber = ser.OrderNumber;
                request.OrderPrefix = new CodeBaseResponse() { CodeName = ser.Prefix };
                request.OrderKey = ser.OrderKey;
            }
            catch (Exception exp)
            {
                request.OrderNumber = "ERR";
            }
        }

        public async Task EditWorkOrder(Order order)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateWorkOrderEndpoint, order);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                order.OrderNumber = ser.OrderNumber;
                order.OrderPrefix = new CodeBaseResponse() { CodeName = ser.Prefix };
                order.OrderKey = ser.OrderKey;


            }
            catch (Exception exp)
            {

            }
        }

        public async Task EditIRNWorkOrder(Order request) 
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.EditIRNWorkOrderEndPoint, request);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                request.OrderNumber = ser.OrderNumber;
                request.OrderPrefix = new CodeBaseResponse() { CodeName = ser.Prefix };
                request.OrderKey = ser.OrderKey;
            }
            catch (Exception exp)
            {

            }
        }
        public async Task<WorkOrder> OpenWorkOrderV2(OrderOpenRequest request)
        {
            WorkOrder work_order = new WorkOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OpenWorkorderEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                work_order = JsonConvert.DeserializeObject<WorkOrder>(content);

                foreach (var itm in work_order.OrderItems)
                {
                    itm.SubTotal = itm.GetLineTotalWithTax();
                    if (itm.TransactionItem.ItemType.Code.Equals("SERVICE", StringComparison.OrdinalIgnoreCase))
                    {
                        itm.IsServiceItem = true;
                        itm.Time = itm.TransactionQuantity;
                        work_order.WorkOrderServices.Add(itm);
                    }
                    else if(itm.TransactionItem.ItemType.Code.Equals("SubContractor", StringComparison.OrdinalIgnoreCase))
                    {
                        itm.IsServiceItem = true;
                        work_order.OtherServices.Add(itm);
                    }
                    else if (itm.TransactionItem.ItemType.Code.Equals("Note",StringComparison.OrdinalIgnoreCase))
                    {

                        itm.IsNoteItem = true;
                        work_order.CustomerComplains.Add(itm);
                    }
                    else
                    {
                        itm.IsMaterialItem = true;
                        work_order.WorkOrderMaterials.Add(itm);
                    }
                }

            }
            catch (Exception exp)
            {

            }
            return work_order; 
        }

        public async Task<IList<BookingDetails>> GetRecentBookingDetails(Vehicle request)
        {
            IList<BookingDetails> booked_list = new List<BookingDetails>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetRecentBookingDetailsEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                booked_list = JsonConvert.DeserializeObject<IList<BookingDetails>>(content);



            }
            catch (Exception exp)
            {

            }
            return booked_list;
        }

        public async Task SaveWorkOrderTransaction(BLTransaction transaction)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveWorkOrderTransactionEndpoint, transaction);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<BLTransaction>(content);
                transaction.TransactionNumber = obj.TransactionNumber;
                transaction.TransactionKey = obj.TransactionKey;
                transaction.IsPersisted = obj.IsPersisted;


            }
            catch (Exception exp)
            {

            }
        }

        public async Task<BLTransaction> OpenWorkOrderTransaction(TransactionOpenRequest request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OpenWorkOrderTransactionEndpoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<BLTransaction>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new BLTransaction();
            }
        }

        public async Task<UserRequestValidation> GetWorkShopValidatoion(WorkOrder request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.WorkShopValidationEndpoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<UserRequestValidation>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new UserRequestValidation();
            }
        }

        public async Task<StockAsAtResponse> GetAvailableStock(StockAsAtRequest request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAvailableStockEndpoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<StockAsAtResponse>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new StockAsAtResponse();
            }
        }

        public async Task<IList<WorkOrder>> GetPendingIRNs(WorkOrder request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetIRNDetailsEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<WorkOrder>>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new List<WorkOrder>();
            }
        }
    }
}
