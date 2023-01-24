using BlueLotus360.Com.Client.Infrastructure.Extensions;
using BlueLotus360.Com.Shared.Wrapper;
using System.Net.Http;
using System.Threading.Tasks;
using BlueLotus360.Com.Application.Features.Dashboards.Queries.GetData;
using System.Collections.Generic;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using System.Net.Http.Json;
using BlueLotus360.Com.Client.Infrastructure.Routes;
using Newtonsoft.Json;
using System;


namespace BlueLotus360.Com.Infrastructure.Managers.Dashboard
{
    public class DashboardManager : IDashboardManager
    {
        private readonly HttpClient _httpClient;
        private bool _checkIfExceptionReturn;
        public DashboardManager(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IResult<DashboardDataResponse>> GetDataAsync()
        {
            var response = await _httpClient.GetAsync("");
            var data = await response.ToResult<DashboardDataResponse>();
            return data;
        }

        public async Task<IList<LocationViseStockResponse>> GetLocationViseStocks(LocationViseStockRequest request)
        {
            List<LocationViseStockResponse> stockList = new List<LocationViseStockResponse>();
            try
            {

                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetLocationViseStocks, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                stockList = JsonConvert.DeserializeObject<List<LocationViseStockResponse>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                stockList = new List<LocationViseStockResponse>();
            }
            finally
            {

            }

            return stockList;
        }

        

        public async Task<IList<SalesDetails>> GetSalesDetails(SalesDetails request)
        {
            List<SalesDetails> details = new List<SalesDetails>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetSalesHeaderDetails, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<SalesDetails>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<SalesDetails>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<SalesByLocationResponse>> GetLocationWiseSalesDetails(SalesDetails request)
        {
            List<SalesByLocationResponse> details = new List<SalesByLocationResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetSalesByLocationEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<SalesByLocationResponse>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<SalesByLocationResponse>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<SalesRepDetailsForSalesByLocationResponse>> GetLocationWiseSalesRepDetails(SalesRepDetailsForSalesByLocation request)
        {
            List<SalesRepDetailsForSalesByLocationResponse> details = new List<SalesRepDetailsForSalesByLocationResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetSalesByLocationRepEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<SalesRepDetailsForSalesByLocationResponse>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<SalesRepDetailsForSalesByLocationResponse>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<ActualVsBudgetedIncomeResponse>> GetActualVsBudgetedIncome(FinanceRequest request)
        {
            List<ActualVsBudgetedIncomeResponse> details = new List<ActualVsBudgetedIncomeResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetActualVsBudgetedIncomeEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<ActualVsBudgetedIncomeResponse>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<ActualVsBudgetedIncomeResponse>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<GPft_NetPft_Margin_Response>> GetGPft_NetPft_Margin(FinanceRequest request)
        {
            
            IList<GPft_NetPft_Margin_Response> details = new List<GPft_NetPft_Margin_Response>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GPft_NetPft_MarginEndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<GPft_NetPft_Margin_Response>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<GPft_NetPft_Margin_Response>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<GPft_NetPft_DT>> Get_Monthly_GPft_NetPft_DT(FinanceRequest request)
        {

            IList<GPft_NetPft_DT> details = new List<GPft_NetPft_DT>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GPft_NetPft_DT_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<GPft_NetPft_DT>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<GPft_NetPft_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis(FinanceRequest request)
        {
            IList<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Debtors_Age_Analysis_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis(FinanceRequest request)
        {
            IList<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Creditors_Age_Analysis_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Debtors_Age_Analysis_Overdue(FinanceRequest request)
        {
            IList<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Debtors_Age_Analysis_Overdue_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis>> Get_Creditors_Age_Analysis_Overdue(FinanceRequest request)
        {
            IList<Debtors_Creditors_Age_Analysis> details = new List<Debtors_Creditors_Age_Analysis>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Creditors_Age_Analysis_Overdue_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Debtors_DT_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Creditors_DT_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Debtors_Age_Analysis_Overdue_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Debtors_Overdue_DT_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Age_Analysis_DT>> Get_Creditors_Age_Analysis_Overdue_DT(FinanceRequestDT request)
        {
            IList<Debtors_Creditors_Age_Analysis_DT> details = new List<Debtors_Creditors_Age_Analysis_DT>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Creditors_Overdue_DT_EndPoint, request);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Age_Analysis_DT>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Age_Analysis_DT>();
            }
            finally
            {

            }

            return details;
        }

        public async Task<IList<Debtors_Creditors_Transaction_Details>> Get_Debtor_Creditor_DT_Transaction_Details(FinanceRequestDTDetails filter)
        {
            IList<Debtors_Creditors_Transaction_Details> details = new List<Debtors_Creditors_Transaction_Details>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Debtor_DT_Transaction_Details_EndPoint, filter);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                details = JsonConvert.DeserializeObject<List<Debtors_Creditors_Transaction_Details>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                details = new List<Debtors_Creditors_Transaction_Details>();
            }
            finally
            {

            }

            return details;
        }

        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;  
        }

        

        
    }
}