using BlueLotus360.Com.Client.Infrastructure.Routes;
using Blazored.LocalStorage;
using BlueLotus360.CleanArchitecture.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net.Http;
using BlueLotus360.Com.Application.Responses.Identity;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.Client.Infrastructure.Authentication;
using BlueLotus360.Com.Shared.Wrapper;
using System.Net.Http.Headers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using Microsoft.Extensions.Configuration;

namespace BlueLotus360.Com.Infrastructure.Managers.Company
{
    public class CompanyManager : ICompanyManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<CompanyManager> _localizer;
        private readonly IConfiguration _config;
        public CompanyManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<CompanyManager> localizer,
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

        public async Task<ReportCompanyDetailsResponse> GetCompanyDetailsResponse()
        {
            CompanyResponse resp = new CompanyResponse();
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanyReportInformationEndPoint,resp);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                ReportCompanyDetailsResponse companyDetails = JsonConvert.DeserializeObject<ReportCompanyDetailsResponse>(content);
                return companyDetails;
            }
            return null;
        }

        public async Task<IList<CompanyResponse>> GetUserCompanies()
        {
            CompanyResponse resp = new CompanyResponse();
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanyListingEndPoint, resp);
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                string content = await response.Content.ReadAsStringAsync();
                IList<CompanyResponse> companies = JsonConvert.DeserializeObject<IList<CompanyResponse>>(content);
                return companies;
            }
            else
            {
                return new List<CompanyResponse>();
            }
            
        }

        public async Task UpdateSelectedCompany(CompanyResponse companyResponse)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanySelectedEndPoint, companyResponse);
            string content=await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<TokenResponse>(content);

            // var result = await response.ToResult<TokenResponse>();
            if (result.IsSuccess)
            {
                var token = result.Token;
                var refreshToken = result.RefreshToken;
                var userImageURL = "";
                await _localStorage.SetItemAsync(StorageConstants.Local.AuthToken, token);
                await _localStorage.SetItemAsync(StorageConstants.Local.RefreshToken, refreshToken);
                await _localStorage.SetItemAsync(StorageConstants.Local.CompanyName,companyResponse.CompanyName);
                if (!string.IsNullOrEmpty(userImageURL))
                {
                    await _localStorage.SetItemAsync(StorageConstants.Local.UserImageURL, userImageURL);
                }

                await ((BL10AuthProvider)this._authenticationStateProvider).StateChangedAsync();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

               
            }
            else
            {
               
            }
        }



    }
}
