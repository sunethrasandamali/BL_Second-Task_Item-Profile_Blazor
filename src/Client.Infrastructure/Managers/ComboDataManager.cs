using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
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
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace BlueLotus360.Com.Infrastructure.Managers
{
    public class ComboDataManager : IComboDataManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<ComboDataManager> _localizer;
        private readonly IConfiguration _config;
        private readonly IHttpClientFactory _factory;

        public ComboDataManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<ComboDataManager> localizer,
            IConfiguration config,
            IHttpClientFactory factory)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _localizer = localizer;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);
            _factory = factory;
            if (_httpClient.DefaultRequestHeaders.Count() == 0)
            {
               

            }
        }

        public async Task<IList<AddressResponse>> GetAddressResponses(ComboRequestDTO requestDTO)
        {
           // var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<AddressResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<AddressResponse>();
            }
        }

        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);
        }

        public async Task<IList<AccountResponse>> GetAccountResponse(ComboRequestDTO requestDTO)
        {


            var cl = _factory.CreateClient();
            AssignClientData(cl);
            var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            string content = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<IList<AccountResponse>>(content);                  
            return list;


        }

        public async Task<IList<CodeBaseResponse>> GetCodeBaseResponses(ComboRequestDTO requestDTO)
        {
           
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<CodeBaseResponse>();
            }
        }

        public async Task<IList<ItemResponse>> GetItemResponses(ComboRequestDTO requestDTO)
        {
          //  var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<ItemResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<ItemResponse>();
            }
        }

        public async Task<IList<UnitResponse>> GetItemUnits(ComboRequestDTO requestDTO)
        {
            try
            {
                //  var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<UnitResponse>>(content);
               
                return list;
            }
            catch (Exception exp)
            {
                return new List<UnitResponse>();
            }

        }

        //get price
        public async Task<ItemRateResponse> GetRate(ItemRateRequest baseRequest)
        {
            HttpResponseMessage response = null;
            ItemRateResponse rateResponse = new ItemRateResponse();
            try
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), TokenEndpoints.ItemRateEndPoint))
                {
                    request.Headers.TryAddWithoutValidation("Timestamp", DateTime.Now.Ticks.ToString());
                    request.Content = JsonContent.Create(baseRequest);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    response = await _httpClient.SendAsync(request);
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                rateResponse = JsonConvert.DeserializeObject<ItemRateResponse>(responseBody);


            }
            catch (Exception exp)
            {
                Console.WriteLine("exp is {0}", exp);


            }
            finally
            {
            }

            return rateResponse;


        }

        //get by itemcode

        public async Task<IList<ItemCodeResponse>> GetItemByItemCode(ItemRequestModel itemRequest)
        {

            HttpResponseMessage response = null;
            IList<ItemCodeResponse> itemResponse;
            try
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), TokenEndpoints.GetItemByItemCodeEndPoint))
                {
                    //request.Headers.TryAddWithoutValidation("Timestamp", DateTime.Now.Ticks.ToString());
                    request.Content = JsonContent.Create(itemRequest);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    response = await _httpClient.SendAsync(request);
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                itemResponse = JsonConvert.DeserializeObject<IList<ItemCodeResponse>>(responseBody);


            }
            catch (Exception exp)
            {

                itemResponse = null;

            }

            return itemResponse;
        }

        public async Task<IList<PriceListResponse>> GetPriceLists(PriceListRequest price_list_request)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetPriceListEndPoint, price_list_request);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<PriceListResponse>>(content);
                return list;

            }
            catch (Exception exp)
            {
                return new List<PriceListResponse>();
            }
        }

        public async Task<IList<AccPaymentMappingResponse>> GetPayementAccountMapping(AccPaymentMappingRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAccountMapping, request);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<AccPaymentMappingResponse>>(content);
                return list;

            }
            catch (Exception exp)
            {
                return new List<AccPaymentMappingResponse>();
            }
        }

        public async Task<IList<BinaryDocument>> GetItemDocuments(ItemRequestModel Req)
        {
            HttpResponseMessage response = null;
            IList<BinaryDocument> ImagesResponse = new List<BinaryDocument>();
            try
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), TokenEndpoints.ItemRateEndPoint))
                {
                    request.Headers.TryAddWithoutValidation("Timestamp", DateTime.Now.Ticks.ToString());
                    request.Content = JsonContent.Create(Req);
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    response = await _httpClient.SendAsync(request);
                }

                string responseBody = await response.Content.ReadAsStringAsync();
                ImagesResponse = JsonConvert.DeserializeObject<IList<BinaryDocument>>(responseBody);


            }
            catch (Exception exp)
            {
                Console.WriteLine("exp is {0}", exp);


            }
            finally
            {
            }

            return ImagesResponse;
        }

        public async Task<AddressCreateResponse> CreateNewAddress(AddressResponse request)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateNewAddressURL, request);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var addressCreate = JsonConvert.DeserializeObject<AddressCreateResponse>(content);
                return addressCreate;

            }
            catch (Exception exp)
            {
                return new AddressCreateResponse() { IsSuccess = false,Message="Connection Error" };
            }
        }

        public async Task<Base64Document> GetBase64TopDocument(DocumentRetrivaltDTO request)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.ItemImageReadURL, request);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var base64Document = JsonConvert.DeserializeObject<Base64Document>(content);
                return base64Document;

            }
            catch (Exception exp)
            {
                return new Base64Document();
            }
        }

        public async Task<IList<CodeBaseResponse>> GetNextApproveStatusResponses(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<CodeBaseResponse>();
            }
        }

        public async Task<IList<CodeBaseResponse>> GetApproveStatusResponses(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<CodeBaseResponse>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<CodeBaseResponse>();
            }
        }
        //lnd
        public async Task<CodeBaseResponseExtended> GetCodeBaseResponseExtended(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CodeBaseDetailRequest, requestDTO);
            try
            {
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<CodeBaseResponseExtended>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new CodeBaseResponseExtended();
            }
        }

        public async Task<IList<ItemSerialNumber>> GetSerialNumberResponses(ComboRequestDTO requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(requestDTO.RequestingURL, requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<IList<ItemSerialNumber>>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new List<ItemSerialNumber>();
            }
        }
    }
}
