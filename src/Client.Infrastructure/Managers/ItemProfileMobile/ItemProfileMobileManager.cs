
using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
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

namespace BlueLotus360.Com.Infrastructure.Managers.ItemProfileMobile
{
    public class ItemProfileMobileManager : IItemProfileMobileManager
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private bool _checkIfExceptionReturn;
        private readonly IHttpClientFactory _factory;

        public ItemProfileMobileManager(
            HttpClient httpClient,
            IConfiguration config, IHttpClientFactory factory) 
        {
            _httpClient = httpClient;
            _config = config;
            _factory = factory;

            //adding IntegrationID
            if (_httpClient.DefaultRequestHeaders.Count() == 0)
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);
        }
        private void AssignClientData(HttpClient cl)
        {
            cl.BaseAddress = _httpClient.BaseAddress;
            cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                _httpClient.DefaultRequestHeaders.Authorization.Parameter);
            cl.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);
        }

        //getItemList(override)
        public async Task<IList<ItemSelectList>> GetItemProfileList(ItemSelectListRequest request) 
        {
            IList<ItemSelectList> itemSelectListResponse = new List<ItemSelectList>();

            try
            {
                //var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetItemProfileSelectList, request); //(postmethod)call API by httpclient with URL(endpoints) and request parameters
                //await response.Content.LoadIntoBufferAsync();
                //string content = response.Content.ReadAsStringAsync().Result;
                //itemSelectListResponse = JsonConvert.DeserializeObject<IList<ItemSelectList>>(content);

                var cl = _factory.CreateClient();
                AssignClientData(cl);
                var response = await cl.PostAsJsonAsync(TokenEndpoints.GetItemProfileSelectList, request);
                string content = await response.Content.ReadAsStringAsync();
                itemSelectListResponse = JsonConvert.DeserializeObject<IList<ItemSelectList>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                itemSelectListResponse = new List<ItemSelectList>();
            }
            finally
            {

            }

            return itemSelectListResponse;
        }


        //InsertItem
        public async Task<ItemSelectList> InsertItem(ItemSelectList request)
        {
            ItemSelectList response = new ItemSelectList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertItem, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ItemSelectList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ItemSelectList();
            }
            finally
            {

            }
            return response;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }


        //UpdateItem
        public async Task<ItemSelectList> UpdateItem(ItemSelectList request)
        {
            ItemSelectList response = new ItemSelectList();

            try
            {
                var data = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateItem, request);
                await data.Content.LoadIntoBufferAsync();
                string content = data.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<ItemSelectList>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new ItemSelectList();
            }
            finally
            {

            }
            return response;
        }

        
    }
}
