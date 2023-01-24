using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Routes;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
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

namespace BlueLotus360.Com.Infrastructure.Managers.TransactionManager
{
    public class TransactionManager : ITransactionManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<TransactionManager> _localizer;
        private readonly IConfiguration _config;

        public TransactionManager(HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<TransactionManager> localizer,
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

        public async Task<FindTransactionResponse> FindTransactions(TransactionFindRequest request, URLDefinitions uRL)
        {
            FindTransactionResponse findTransaction = new FindTransactionResponse();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FindTransaction, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<FindTransactionLineItem>>(content);
                findTransaction.LineItems = obj;


            }
            catch (Exception exp)
            {

            }
            return findTransaction;
        }

        public async Task<BaseServerResponse<IList<GetFromTransactionResponse>>> GetFromTransactions(GetFromTransactionRequest request, URLDefinitions urlDef)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(BaseEndpoint.BaseURL + urlDef.URL, request);
                string content = response.Content.ReadAsStringAsync().Result;
                var ServerResponse = JsonConvert.DeserializeObject<BaseServerResponse<IList<GetFromTransactionResponse>>>(content);
                return ServerResponse;


            }
            catch (Exception exp)
            {
                var ServerResponse = new BaseServerResponse<IList<GetFromTransactionResponse>>();
                ServerResponse.DataObject = new List<GetFromTransactionResponse>();
                ServerResponse.ResponseType = ServerResponseType.ProcessingError;
                ServerResponse.AddErrorMessage("Faild to Execute the request");
                return ServerResponse;
            }
        }

        public async Task<IList<RecieptDetailResponse>> GetRecieptDetailResponses(RecieptDetailRequest request, URLDefinitions urlDef)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(BaseEndpoint.BaseURL + urlDef.URL, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<RecieptDetailResponse>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new List<RecieptDetailResponse>();
            }
        }

        public async Task<StockAsAtResponse> GetStockAsAt(StockAsAtRequest rquest)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.StockAsAtEndpoint, rquest);
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

        public async Task HoldTransaction(BLTransaction transaction)
        {
            if ((await _localStorage.ContainKeyAsync(transaction.DocumentNumber)))
            {
                await _localStorage.RemoveItemAsync(transaction.DocumentNumber);
            }
            await _localStorage.SetItemAsync(transaction.DocumentNumber, transaction);
        }

        public async Task<BLTransaction> OpenTransaction(TransactionOpenRequest request)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.OpenTransaction, request);
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

        public async Task<BLTransaction> OpenTransaction(TransactionOpenRequest request,URLDefinitions definitions)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(BaseEndpoint.BaseURL+definitions.URL, request);
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


        public async Task<IList<DenominationEntry>> ReadDenominationEntries(DenominationRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CashDenominationRead, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<DenominationEntry>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new List<DenominationEntry>();
            }
        }

        public async Task<BaseServerResponse<BLTransaction>> ReadFromTransaction(FromTransactionOpenRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.ReadFromTransaction, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<BaseServerResponse<BLTransaction>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new BaseServerResponse<BLTransaction>();
            }
        }

        public async Task<IList<ItemSerialNumber>> RetriveItemTransactionSerials(ItemTransactionSerialRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.ItemTrnasactionSerialsURL, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<IList<ItemSerialNumber>>(content);
                return obj;


            }
            catch (Exception exp)
            {
                return new List<ItemSerialNumber>();
            }
        }

        public async Task SaveAccountRecieptPayement(AccoutRecieptPayment accoutReciept)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveAccountRecieptURL, accoutReciept);



            }
            catch (Exception exp)
            {
               
            }
        }

        public async Task SaveCashInOutTransaction(CashInOutTransaction transaction, URLDefinitions uRL)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(BaseEndpoint.BaseURL + uRL.URL, transaction);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<CashInOutTransaction>(content);
                transaction.TransactionNumber = obj.TransactionNumber;


            }
            catch (Exception exp)
            {

            }
        }

        public async Task SaveDenominations(IList<DenominationEntry> denominations)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveDenominationEndpint, denominations);         
               
            }
            catch (Exception exp)
            {

            }
        }

        public async Task<BLTransaction> SaveTransaction(BLTransaction transaction)
        {
            BLTransaction responses = new BLTransaction();
            try 
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.TransactionSaveEndpoint, transaction);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                responses = JsonConvert.DeserializeObject<BLTransaction>(content);

            }
            catch (Exception exp)
            {

            }
            return responses;
        }

        public async Task<IList<BLTransaction>> LoadTransactionApprovals(FindTransactionStatus request)
        {
            IList<BLTransaction> list=new List<BLTransaction>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.LoadTransactionApprovalDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = await response.Content.ReadAsStringAsync();
                list = JsonConvert.DeserializeObject<IList<BLTransaction>>(content);
                

            }
            catch (Exception exp)
            {

            }
            return list;
        }

        public async Task UpadteTransactionApprovals(BLTransaction request)
        {
            
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.UpdateTransactionApproval, request);
               


            }
            catch (Exception exp)
            {

            }
      
        }

        //lnd
        public async Task<RecviedAmountResponse> GetTotalPayedAmount(RecieptDetailRequest request)
        {
            try
            {
                var url = TokenEndpoints.TotalPayedRequestURL;
                var response = await _httpClient.PostAsJsonAsync(url, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                var obj = JsonConvert.DeserializeObject<RecviedAmountResponse>(content);

                return obj;


            }
            catch (Exception exp)
            {
                return new RecviedAmountResponse();
            }
        }

        public async Task<InvoiceDetailsByHdrSerNo> GetInvoiceFromSerialNumber(ItemSerialNumber serialNumber)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InvoiceBySerial, serialNumber);
                string content = response.Content.ReadAsStringAsync().Result;
                var ServerResponse = JsonConvert.DeserializeObject<InvoiceDetailsByHdrSerNo>(content);
                return ServerResponse;


            }
            catch (Exception exp)
            {
                return null;
            }
        }

        public async Task SaveItemSerialNumber(ItemSerialNumber serialNumber)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveItemSerialURL, serialNumber);



            }
            catch (Exception exp)
            {

            }
        }

        public async Task SaveAccountRecieptPayementEx(PayementModeReciept accoutReciept)
        {
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.SaveAccountRecieptURLEx, accoutReciept);



            }
            catch (Exception exp)
            {

            }
        }
    }
}
