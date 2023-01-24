using BL10.CleanArchitecture.Domain.Entities;
using BL10.CleanArchitecture.Domain.Entities.APIInfo;
using Blazored.LocalStorage;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.APIManager
{
    public class APIManager:IAPIManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<APIManager> _localizer;
        private readonly IConfiguration _config;

        public APIManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<APIManager> localizer,
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

        public async Task<APIInformation> GetAPIInformation(APIRequestParameters parameters)
        {
            APIInformation apiData = new APIInformation();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAPIInfo, parameters);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<APIInformation>(content);
                apiData = obj;


            }
            catch (Exception exp)
            {

            }
            return apiData;
        }
        public async Task<APIInformation> GetAPIEndPoints(APIRequestParameters parameters)
        {
            APIInformation apiData = new APIInformation();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAPIEndPoints, parameters);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<APIInformation>(content);
                apiData = obj;


            }
            catch (Exception exp)
            {

            }
            return apiData;
        }
    }
}
