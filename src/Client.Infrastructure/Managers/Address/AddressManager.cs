using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
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
using BL10.CleanArchitecture.Domain.Entities.Booking;
using System.Text.Json.Serialization;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;

namespace BlueLotus360.Com.Infrastructure.Managers.Address
{
    public class AddressManager : IAddressManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<AddressManager> _localizer;
        private readonly IConfiguration _config;

        public AddressManager(HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<AddressManager> localizer,
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

        public async Task<AddressCreateServerResponse> CreateNewAddress(AddressMaster record)
        {
            AddressCreateServerResponse addressIdCheckServerResponse = new();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateNewAddressURL, record);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                addressIdCheckServerResponse = JsonConvert.DeserializeObject<AddressCreateServerResponse>(content);



            }
            catch (Exception exp)
            {

            }
            return addressIdCheckServerResponse;
        }

        public async Task<AddressMaster> CreateCustomer(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateCustomer, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressMaster> CreateCustomerValidation(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateCustomerValidation, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressMaster> CheckAdvanceAnalysisAvailability(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CheckAdvanceAnalysisAvailability, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressMaster> CreateAdvanceAnalysis(AddressMaster customer)
        {
            AddressMaster responses = new AddressMaster();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateAdvanceAnalysis, customer);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressMaster>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<AddressResponse> GetAddressByUserKy()
        {
            AddressResponse responses = new AddressResponse();

            try
            {
                var response = await _httpClient.GetAsync(TokenEndpoints.GetAddressByUsrKyEndPoint);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responses = JsonConvert.DeserializeObject<AddressResponse>(content);
            }
            catch (Exception exp)
            {

            }
            return responses;
        }

    }
}
