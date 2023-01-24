using BlueLotus360.CleanArchitecture.Domain.Entities.AccountProfile;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.AccountProfile
{
    public class ProfileManager : IProfileManager
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        public ProfileManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IList<AccountProfileResponse>> GetAccountProfileList(AccountProfileRequest request)
        {
            List<AccountProfileResponse> stockList = new List<AccountProfileResponse>();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetAccountProfileList, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                stockList = JsonConvert.DeserializeObject<List<AccountProfileResponse>>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                stockList = new List<AccountProfileResponse>();
            }
            finally
            {

            }

            return stockList;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        public async Task<AccountProfileInsertResponse> InsertAccountProfile(AccountProfileInsertRequest request)
        {
            AccountProfileInsertResponse responseAccount = new AccountProfileInsertResponse();

            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InsertAccountProfileItem, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                responseAccount = JsonConvert.DeserializeObject<AccountProfileInsertResponse>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                responseAccount = new AccountProfileInsertResponse();
            }
            finally
            {

            }
            return responseAccount;
        }

        public async Task<AccountProfileResponse> UpdatedAccountProfile(AccountProfileResponse request)
        {
            AccountProfileResponse response = new AccountProfileResponse();

            try
            {
                var responsedata = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateAccountProfile, request);
                await responsedata.Content.LoadIntoBufferAsync();
                string content = responsedata.Content.ReadAsStringAsync().Result;
                response = JsonConvert.DeserializeObject<AccountProfileResponse>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                response = new AccountProfileResponse();
            }
            finally
            {

            }
            return response;
        }
    }
}
