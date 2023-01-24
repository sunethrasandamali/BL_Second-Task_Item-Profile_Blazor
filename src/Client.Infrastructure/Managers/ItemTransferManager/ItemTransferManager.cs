using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
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

namespace BlueLotus360.Com.Infrastructure.Managers.ItemTransferManager
{
    public class ItemTransferManager:IItemTransferManager
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly IStringLocalizer<ItemTransferManager> _localizer;
        private bool _checkIfExceptionReturn;

        public ItemTransferManager(HttpClient httpClient,
            ILocalStorageService localStorage,
            AuthenticationStateProvider authenticationStateProvider,
            IStringLocalizer<ItemTransferManager> localizer)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
            _authenticationStateProvider = authenticationStateProvider;
            _localizer = localizer;
        }


        public async Task<int> CreateItemTransfer(ItemTransfer itm)
        {
            _checkIfExceptionReturn = false;
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.CreateItemTransfer_EndPoint, itm);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<int>(content);
            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                return 1;
            }

        }

        public async Task<ItemTransferLineItem> GetItemsData(ItemTransferLineItem res)
        {
            _checkIfExceptionReturn = false;
            
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Data_EndPoint, res);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                res = JsonConvert.DeserializeObject<ItemTransferLineItem>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return res;
        }

        public async Task<List<ItemTransferLineItem>> GetInvoiceData(LNDInvoice res)
        {
            _checkIfExceptionReturn = false;
            List<ItemTransferLineItem> itemList = new List<ItemTransferLineItem>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Get_Invoice_Data_EndPoint, res);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                itemList = JsonConvert.DeserializeObject<List<ItemTransferLineItem>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return itemList;
        }

        public async Task<List<string>> GetInvoiceSerialNoList(LNDInvoice res)
        {
            _checkIfExceptionReturn = false;
            List<string> serialNoList = new List<string>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.GetInvoiceItemsSerialNoList, res);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                serialNoList = JsonConvert.DeserializeObject<List<string>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }

            return serialNoList;
        }

        public async Task<ItmtrnsferValidationResponse> TransferValidator(ItemTransfer itm)
        {
            ItmtrnsferValidationResponse res = new ItmtrnsferValidationResponse();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.ItemTransferValidationEndpoint, itm);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                res = JsonConvert.DeserializeObject<ItmtrnsferValidationResponse>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return res;
        }

        public async Task<ItmtrnsferValidationResponse> InvoiceTransferValidator(LNDInvoice invoice)
        {
            ItmtrnsferValidationResponse res = new ItmtrnsferValidationResponse();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.InvoiceItemTransferValidationEndpoint, invoice);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                res = JsonConvert.DeserializeObject<ItmtrnsferValidationResponse>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return res;
        }

        public async Task<List<FindItemTransferResponse>> Find(FindItemTransferRequest req)
        {
            _checkIfExceptionReturn = false;
            List<FindItemTransferResponse> itmtrn = new List<FindItemTransferResponse>();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Find_ItmTrn_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                itmtrn = JsonConvert.DeserializeObject<List<FindItemTransferResponse>>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return itmtrn;

        }

        public async Task<ItemTransfer> RefreshForm(TransferOpenRequest req)
        {
            _checkIfExceptionReturn = false;
            ItemTransfer refData = new ItemTransfer();
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Refresh_Header_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                refData = JsonConvert.DeserializeObject<ItemTransfer>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;

            }
            return refData;
        }

        public async Task<int> UpdateItemTransfer(ItemTransfer req)
        {
            _checkIfExceptionReturn = false;
            
            try
            {
                var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Update_ItmTransfer_EndPoint, req);
                await response.Content.LoadIntoBufferAsync();
                string content = response.Content.ReadAsStringAsync().Result;
                return JsonConvert.DeserializeObject<int>(content);

            }
            catch (Exception exp)
            {
                _checkIfExceptionReturn = true;
                return 1;
            }
           
        }
        

        
        public bool IsExceptionthrown()
        {
            if (_checkIfExceptionReturn)
                return true;
            return false;
        }

        #region hold

        //public async Task<ItemTransfer> SaveTrnHeaderFromLoc(ItemTransfer itm)
        //{
        //    _checkIfExceptionReturn = false;
        //    ItemTransfer itmtrnsfr = new ItemTransfer();
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Save_Trn_Hdr_From_Location_EndPoint, itm);
        //        await response.Content.LoadIntoBufferAsync();
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        itmtrnsfr = JsonConvert.DeserializeObject<ItemTransfer>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }

        //    return itmtrnsfr;
        //}

        //public async Task<ItemTransfer> SaveTrnHeaderToLoc(ItemTransfer itm)
        //{
        //    _checkIfExceptionReturn = false;
        //    ItemTransfer itmtrnsfr = new ItemTransfer();
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Save_Trn_Hdr_To_Location_EndPoint, itm);
        //        await response.Content.LoadIntoBufferAsync();
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        itmtrnsfr = JsonConvert.DeserializeObject<ItemTransfer>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }

        //    return itmtrnsfr;
        //}

        //public async Task<int> SaveLine(ItemTransferLineItem itm)
        //{
        //    _checkIfExceptionReturn = false;
        //    int returnVal = 0;
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Save_Line_EndPoint, itm);
        //        await response.Content.LoadIntoBufferAsync();
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        returnVal = JsonConvert.DeserializeObject<int>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }
        //    return returnVal;

        //}

        //public async Task<ItemTransfer> UpdateHeaderForOut(ItemTransfer req)
        //{
        //    _checkIfExceptionReturn = false;
        //    ItemTransfer refData = new ItemTransfer();
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Update_Header_For_Out_EndPoint, req);
        //        await response.Content.LoadIntoBufferAsync();
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        refData = JsonConvert.DeserializeObject<ItemTransfer>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }
        //    return refData;
        //}

        //public async Task<ItemTransfer> UpdateHeaderForIn(ItemTransfer req)
        //{
        //    _checkIfExceptionReturn = false;
        //    ItemTransfer refData = new ItemTransfer();
        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Update_Header_For_In_EndPoint, req);
        //        await response.Content.LoadIntoBufferAsync();
        //        string content = response.Content.ReadAsStringAsync().Result;
        //        refData = JsonConvert.DeserializeObject<ItemTransfer>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }
        //    return refData;
        //}

        //public async Task UpdateLineForOut(ItemTransferLineItem req)
        //{
        //    _checkIfExceptionReturn = false;

        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Update_Line_For_Out_EndPoint, req);
        //        //await response.Content.LoadIntoBufferAsync();
        //        //string content = response.Content.ReadAsStringAsync().Result;
        //        //refData = JsonConvert.DeserializeObject<ItemTransfer>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }
        //}
        //public async Task UpdateLineForIn(ItemTransferLineItem req)
        //{
        //    _checkIfExceptionReturn = false;

        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.Update_Line_For_In_EndPoint, req);
        //        //await response.Content.LoadIntoBufferAsync();
        //        //string content = response.Content.ReadAsStringAsync().Result;
        //        //refData = JsonConvert.DeserializeObject<ItemTransfer>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }
        //}

        //public async Task FIFOPosting(ItemTransfer req)
        //{
        //    _checkIfExceptionReturn = false;

        //    try
        //    {
        //        var response = await _httpClient.PostAsJsonAsync(TokenEndpoints.FIFO_Posting_EndPoint, req);
        //        //await response.Content.LoadIntoBufferAsync();
        //        //string content = response.Content.ReadAsStringAsync().Result;
        //        //refData = JsonConvert.DeserializeObject<ItemTransfer>(content);

        //    }
        //    catch (Exception exp)
        //    {
        //        _checkIfExceptionReturn = true;

        //    }
        //}
        #endregion
    }
}
