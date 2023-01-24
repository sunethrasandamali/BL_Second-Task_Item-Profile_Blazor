using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using BlueLotus.Com.Domain.Entity;

namespace BlueLotus360.Com.Infrastructure.Managers.OrderManager
{
    public class OrderManager : IOrderManager
    {


        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<OrderManager> _localizer;
        private readonly IConfiguration _config;
         
        public OrderManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<OrderManager> localizer,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _localizer = localizer;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);
        }

        public async Task SaveOrder(Order order)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderSaveEndpoint, order);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                order.OrderNumber = ser.OrderNumber;
                order.Prefix=ser.Prefix;
                order.OrderKey = ser.OrderKey;

            }
            catch (Exception exp)
            {
                order.OrderNumber = "ERR";
            }
        }

        public async Task EditOrder(Order order)
        {
           
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OrderEditEndpoint, order);
                var ser = await response.Content.ReadFromJsonAsync<OrderSaveResponse>();
                order.OrderNumber = ser.OrderNumber;
                order.Prefix = ser.Prefix;
                order.OrderKey = ser.OrderKey;


            }
            catch (Exception exp)
            {

            }
        }

        public async Task<IList<OrderFindResults>> FindOrders(OrderFindDto request, URLDefinitions uRL)
        {
            IList<OrderFindResults> findOrders = new List<OrderFindResults>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FindOrder, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<OrderFindResults>>(content);
                findOrders = obj;


            }
            catch (Exception exp)
            {

            }
            return findOrders;
        }

        public async Task<Order> OpenOrder(OrderOpenRequest request)
        {
            Order loaded_order = new Order();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                loaded_order = JsonConvert.DeserializeObject<Order>(content);



            }
            catch (Exception exp)
            {

            }
            return loaded_order;
        }

        public async Task<Order> OpenQuotation(OrderOpenRequest request)
        {
            Order loaded_order = new Order();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderEndPointFromQuotation, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                loaded_order = JsonConvert.DeserializeObject<Order>(content);



            }
            catch (Exception exp)
            {

            }
            return loaded_order;
        }
        public async Task<IList<GetFromQuotResults>>  FindFromQuotation(GetFromQuoatationDTO request, URLDefinitions uRL)
        {
            IList<GetFromQuotResults> findOrders = new List<GetFromQuotResults>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FindFromQuotation, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<GetFromQuotResults>>(content);
                findOrders = obj;


            }
            catch (Exception exp)
            {

            }
            return findOrders;
        }

        public async Task<Order> OpenQuotationAsSalesOrder(OrderOpenRequest request)
        {
            Order loaded_order = new Order();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                loaded_order = JsonConvert.DeserializeObject<Order>(content);



            }
            catch (Exception exp)
            {

            }
            return loaded_order;
        }

        public async Task<IList<OrderFindResults>> LoadOrderApprovals(OrderFindDto request)
        {
            IList<OrderFindResults> list = new List<OrderFindResults>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadOrderApprovalDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<IList<OrderFindResults>>(content);


            }
            catch (Exception exp)
            {

            }
            return list;
        }

        public async Task UpadteOrderApprovals(OrderFindResults request)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateOrderApproval, request);



            }
            catch (Exception exp)
            {

            }

        }

        public async Task<IList<CodeBaseResponse>> GetOrderStatus()
        {
            IList<CodeBaseResponse> Orderstatus = new List<CodeBaseResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetOrderStatus,new ComboRequestDTO());
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                Orderstatus = obj;


            }
            catch (Exception exp)
            {

            }
            return Orderstatus;
        }

        public async Task<int> PartnerOrderCount(RequestParameters partnerOrder)
        {
            int Count = 0;
            try
            {
                
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.PartnerOrderCount, partnerOrder);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<int>(content);
                Count = obj;


            }
            catch (Exception exp)
            {

            }
            return Count;
        }

        public async Task<IList<PartnerOrder>> GetAllPartnerOrder(RequestParameters partnerOrder)
        {
            IList<PartnerOrder> orders = new List<PartnerOrder>();
            try
            {
                
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAllPartnerOrders, partnerOrder);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<PartnerOrder>>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<PartnerOrder> GetLastSyncTime(APIRequestParameters request)
        {
            PartnerOrder orders = new PartnerOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetLastSyncTime, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<PartnerOrder>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<CodeBaseResponse> GetOrderStatusByPartnerStatus(CodeBaseResponse request)
        {
            CodeBaseResponse orders = new CodeBaseResponse();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetOrderStatusByPartnerStatus, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<CodeBaseResponse>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<PartnerOrder> SavePartnerOrders(PartnerOrder request)
        {
            PartnerOrder orders = new PartnerOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SavePartnerOrder, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<PartnerOrder>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<ItemResponse> GetItemByItemCode(ItemResponse request)
        {
            ItemResponse orders = new ItemResponse();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetItemsByItemCode, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<ItemResponse>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<PartnerOrder> GetPartnerOrdersByOrderKy(RequestParameters request)
        {
            PartnerOrder orders = new PartnerOrder();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetPartnerOrdersByOrderKy, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<PartnerOrder>(content);
                orders = obj;


            }
            catch (Exception exp)
            {

            }
            return orders;
        }

        public async Task<bool> InsertLastOrderSync(RequestParameters request)
        {
            bool success = false;
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertLastOrderSync, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<bool>(content);
                success = obj;


            }
            catch (Exception exp)
            {

            }
            return success;
        }


    }
}
