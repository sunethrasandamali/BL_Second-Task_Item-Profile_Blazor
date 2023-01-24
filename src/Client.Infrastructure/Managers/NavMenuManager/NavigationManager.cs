using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Shared.Models;
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

namespace BlueLotus360.Com.Infrastructure.Managers.NavMenuManager
{
	public class NavMenuManager : INavMenuManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<NavMenuManager> _localizer;
        private readonly IHttpClientFactory _factory;
        private readonly IConfiguration _config;
        public NavMenuManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<NavMenuManager> localizer,
            IHttpClientFactory factory,
            IConfiguration config)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _localizer = localizer;
            _factory = factory;
            _config = config;

            if (_httpClient.DefaultRequestHeaders.Count() == 0)
            {
                _httpClient.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);

            }
        }

        public async Task<BLUIElement> GetMenuUIElement(ObjectFormRequest request)
        {
            try
            {
                var cl = _factory.CreateClient();
                cl.BaseAddress = _httpClient.BaseAddress;
                cl.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue(
                    _httpClient.DefaultRequestHeaders.Authorization.Scheme,
                    _httpClient.DefaultRequestHeaders.Authorization.Parameter);
                cl.DefaultRequestHeaders.Add("IntegrationID", _config["IntergrationID"]);

                var response = await cl.PostAsJsonAsync(ObjectEndpoints.FormDefinitionURL, request);
                

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<BLUIElement>(content);
                cl.Dispose();
                return list;
            }
            catch (Exception exp)
            {
                return null;
            }
        }

        //public async Task<MenuItem> GetNavOrPinnedMenus(string menuType)
        //{
        //    MenuItem menu = new MenuItem();

        //    switch (menuType)
        //    {
        //        case "nav-menu": menu = await this.GetNavigationMenu(); break;
        //        case "pin-menu": menu = await this.GetPinnedMenus(); break;
        //        case "": menu=new MenuItem();break;
        //    }
        //    return menu;
        //}

        public async Task<IDictionary<string,MenuItem>> GetNavAndPinnedMenus()
        {
            IDictionary<string, MenuItem> menus = new Dictionary<string, MenuItem>();

            menus["nav-menu"] = await this.GetNavigationMenu();

            menus["pin-menu"] = await this.GetPinnedMenus();
            return menus;         
        }

        private async  Task<MenuItem> GetNavigationMenu()
		{
            CompanyResponse resp = new CompanyResponse();            
            
            try
            {
                //var response = await _httpClient.PostAsJsonAsync(ObjectEndpoints.SideMenuURL, resp);
                var response = await _httpClient.GetAsync(ObjectEndpoints.SideMenuURL);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<MenuItem>(content);
                _httpClient.CancelPendingRequests();

                return list;
            }
            catch(Exception exp)
            {
                _httpClient.CancelPendingRequests();

                return null;
            }
        }

        private async Task<MenuItem> GetPinnedMenus()
        {
            CompanyResponse resp = new CompanyResponse();
            try
            {

                //var response = await _httpClient.PostAsJsonAsync(ObjectEndpoints.GetPinnedMenusEndpoint, resp);
                var response = await _httpClient.GetAsync(ObjectEndpoints.GetPinnedMenusEndpoint);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<MenuItem>(content);

                return list;
            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();

                return null;
            }
        }

        public async Task UpdatePinnedMenus(MenuItem menurequest)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(ObjectEndpoints.UpdatePinnedMenusEndpoint, menurequest);

            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();

                
            }
        }

        public async Task<UserConfigObjectsBlLite> LoadObjectsForUserConfiguration(ObjectFormRequest request)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync(ObjectEndpoints.LoadAllObjectsForUserConfigEndPoint, request);
                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<UserConfigObjectsBlLite>(content);
                _httpClient.CancelPendingRequests();

                return list;
            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();

                return null;
            }
        }

        public async Task UpdateObjectsForUserConfiguration(UserConfigObjectsBlLite request)
        {

            try
            {
                var response = await _httpClient.PostAsJsonAsync(ObjectEndpoints.UpdateAllObjectsForUserConfigEndPoint, request);
                
            }
            catch (Exception exp)
            {
                _httpClient.CancelPendingRequests();

               
            }
        }
    }
}
