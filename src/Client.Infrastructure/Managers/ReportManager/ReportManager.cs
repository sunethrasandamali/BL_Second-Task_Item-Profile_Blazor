using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
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

namespace BlueLotus360.Com.Infrastructure.Managers.ReportManager
{
    public class ReportManager : IReportManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<ReportConversion> _localizer;
        private readonly IConfiguration _config;

        public ReportManager(
            HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<ReportConversion> localizer,
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


        public async Task<ReportConversion> PreparePDFFromHtml(ReportConversion requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.HtmlToPdfReportEndPoint, requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var list = JsonConvert.DeserializeObject<ReportConversion>(content);
                return list;
            }
            catch (Exception exp)
            {
                return new ReportConversion();
            }
        }

        public async Task<ReportCompanyDetailsResponse> GetReportCompanyInformation(ReportCompanyDetailsRequest requestDTO)
        {
            var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CompanyReportInformationEndPoint, requestDTO);
            try
            {

                string content = await response.Content.ReadAsStringAsync();
                var companyInfor = JsonConvert.DeserializeObject<ReportCompanyDetailsResponse>(content);
                return companyInfor;
            }
            catch (Exception exp)
            {
                return new ReportCompanyDetailsResponse();
            }
        }
    }
}
