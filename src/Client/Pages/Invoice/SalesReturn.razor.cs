using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Shared.Popups;
using BlueLotus360.Com.Client.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Toolbelt.Blazor.HotKeys;
using BlueLotus360.Com.Client.Shared.Components;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Application.Responses.ServerResponse;

namespace BlueLotus360.Com.Client.Pages.Invoice
{
    public partial class SalesReturn : IDisposable
    {
        #region parameter
        private BLUIElement formDefinition;//wht
        private BLTransaction transaction = new();

        private GetFromTransaction _refGetFromUI;



        private UIBuilder _refBuilder;


        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        /** followi is must and you need to pass **/
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private bool tableloading = false;
        MudMessageBox addItem { get; set; }

        private MudTable<TransactionLineItem> _table;

        private ITransactionValidator validator;
        bool isSaving;
        public IList<PriceListResponse> price_list_response { get; set; }
        bool showAlert;
        bool isExpansionPanelOpen;
        HotKeysContext ShortCutKeysContext;

        private BLUIElement GetFromTransactionUI;
        private BLUIElement InvoicePrinterURL;
        private BLUIElement findTrandsactionUI;
        private bool GetFromInvoiceShown = false;
        private bool FindTransactionShown = false;



        private PriceListResponse __currentPriceListResponse;

        #endregion


        #region General
        protected override async Task OnInitializedAsync()
        {

            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                GetFromTransactionUI = formDefinition.Children.Where(x => x._internalElementName.Equals("_GetFrom_Transaction_")).FirstOrDefault();
                InvoicePrinterURL = formDefinition.Children.Where(x => x._internalElementName.Equals("_Invoice_Printer_URL_")).FirstOrDefault();

                if (GetFromTransactionUI != null)
                {
                    GetFromTransactionUI.Children = formDefinition.Children.Where(x => x.ParentKey == GetFromTransactionUI.ElementKey).ToList();
                }

            }
            // return  base.OnInitializedAsync();
            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            HookInteractions();

            await _sessionStorage.ClearAsync();
            this.ShortCutKeysContext = this.HotKeys.CreateContext()
                .Add(ModKeys.Ctrl, Keys.S, SaveInvoice)
                .Add(ModKeys.Shift, Keys.N, InitilizeNewOrder)
                .Add(ModKeys.Ctrl, Keys.F, ShowFindTransactionWindow)
                .Add(ModKeys.Ctrl, Keys.R, ShowGetFromTransaction)
                ;
            transaction.ElementKey = elementKey;
            InitilizeNewOrder();

        }



        private async void InitilizeNewOrder()
        {
            transaction = new BLTransaction();
            transaction.ElementKey = formDefinition.ElementKey;
            validator = new SalesReturnValidator(transaction);
            ToggleEditability("LineEditCancel", false);
            await SetValue("HeaderTitle", "New");


        }




        public void Dispose()
        {
            ShortCutKeysContext.Dispose();
        }



        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();//

        }

        private void UIStateChanged()
        {

            this.StateHasChanged();
        }
        #endregion

        #region Invoice Related Events
        /// <summary>
        /// Following will be called when the Header Location Combo is changed
        /// </summary>
        /// <param name="args"></param>
        private async void OnTransactionLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
          //  await SelectAccountByLocationAndPayementTerm();
            UIStateChanged();
        }
        private void OnTransactionCustomerChanged(UIInterectionArgs<AddressResponse> args)
        {

            UIStateChanged();
        }


        private async void OnPayementTermChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.PaymentTerm = args.DataObject;
           // await SelectAccountByLocationAndPayementTerm();
            UIStateChanged();

        }
        private async void OnLineTransactionUnitChange(UIInterectionArgs<UnitResponse> args)
        {
            if (transaction.SelectedLineItem.IsInEditMode)
            {
                return;
            }
            transaction.SelectedLineItem.TransactionUnit = args.DataObject;
            if (__currentPriceListResponse != null)
            {
                decimal? TransactionRate = __currentPriceListResponse.GetPriceByUnit(args.DataObject);
                if (TransactionRate.HasValue)
                {
                    this.transaction.SelectedLineItem.TransactionRate = TransactionRate.Value;
                    RefreshComponent("ItemName");
                    RefreshComponent("TransactionQuantiy");
                    await Focus("TransactionQuantiy");
                }
            }
            UIStateChanged();
        }
        private async void TryAddItemOnEnter(UIInterectionArgs<decimal> args)
        {
            onAddToGridClick(null);
            await Task.CompletedTask;

        }


        private async Task PostItemAddActions()
        {
            RefreshComponent("ItemName");
            RefreshComponent("TransactionQuantiy");
            await ReadData("LineTransactionUnit");
            await Focus("TransactionQuantiy");
            StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();

            BLUIElement textElement = GetLinkedUIElement("ItemCode");
            if (textElement != null)
            {
                textElement.ElementCaption = "Item Code - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
                await SetValue("ItemCode", transaction.SelectedLineItem.TransactionItem.ItemCode);
                RefreshComponent("ItemCode");
                await CalculatePriceBasedOnPriceList(1);
            }



        }

        private async Task<StockAsAtResponse> GetStockResponseForCurrentItem()
        {
            StockAsAtRequest request = new StockAsAtRequest();
            request.ElementKey = transaction.ElementKey;
            request.LocationKey = transaction.Location.CodeKey;
            request.ProjectKey = 1;
            request.ItemKey = transaction.SelectedLineItem.TransactionItem.ItemKey;
            StockAsAtResponse stockAsAtResponse = await _transactionManager.GetStockAsAt(request);
            return stockAsAtResponse;
        }

        private async void OnLineTransactionQuantityChanged(UIInterectionArgs<decimal> args)
        {
            //ItemCode

            if (transaction.SelectedLineItem.BalanceQuantity<=args.DataObject)
            {
                args.CancelChange = true;
                args.OverrideValue = true;
                args.OverriddenValue = transaction.SelectedLineItem.BalanceQuantity;
            }
            await CalculatePriceBasedOnPriceList(args.DataObject);
            UIStateChanged();

        }

        private async Task CalculatePriceBasedOnPriceList(decimal Qty)
        {


            transaction.SelectedLineItem.TransactionQuantity = Qty;
            await Task.CompletedTask;

        }

        private void OnLineTransactionRateChanged(UIInterectionArgs<decimal> args)
        {
            transaction.SelectedLineItem.TransactionRate = args.DataObject;
            transaction.SelectedLineItem.LineNetRate = transaction.SelectedLineItem.GetLineTotalWithDiscount();
        }

        private void OnLineDisPerChange(UIInterectionArgs<decimal> args)
        {
            transaction.SelectedLineItem.DiscountPercentage = args.DataObject;
            transaction.SelectedLineItem.LineNetRate = transaction.SelectedLineItem.GetLineTotalWithDiscount();

        }

        private async void OnLineCancelClick(UIInterectionArgs<object> args)
        {
            InitNewLine();
            await Task.CompletedTask;
        }

        private async void InitNewLine()
        {
            this.transaction.SelectedLineItem = new TransactionLineItem();
            ToggleEditability("ItemCode", true);
            _objectHelpers["ItemCode"].ResetToInitialValue();
            _objectHelpers["ItemName"].ResetToInitialValue();
            _objectHelpers["TransactionRate"].ResetToInitialValue();
            _objectHelpers["TransactionQuantiy"].ResetToInitialValue();
            _objectHelpers["LineTransactionUnit"].ResetToInitialValue();
            _objectHelpers["DiscountPercentage"].ResetToInitialValue();
            this.transaction.EditingLineItem = null;
            ToggleEditability("LineEditCancel", false);
            await Focus("ItemCode");
            StateHasChanged();
        }

        private void OnContraAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            transaction.ContraAccountObjectKey = args.InitiatorObject.ElementKey;
        }
        private void OnAccountChanged(UIInterectionArgs<AccountResponse> args)
        {
            transaction.AccountObjectKey = args.InitiatorObject.ElementKey;
        }






        private void OnNewClick(UIInterectionArgs<object> args)
        {
            InitilizeNewOrder();
            UIStateChanged();
        }
        #endregion

        #region Read Hooks




        private async void LineTransactionUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", this.transaction.SelectedLineItem.TransactionItem.ItemKey);
            await Task.CompletedTask;
        }













        #endregion

        #region Add/Edit/Delete methods

        private async void onAddToGridClick(UIInterectionArgs<object> args)
        {
            StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
            if (validator.CanAddItemToGrid(transaction.SelectedLineItem.BalanceQuantity))
            {
                if (transaction.SelectedLineItem.IsInEditMode)
                {
                    if (transaction.EditingLineItem != null)
                    {
                        transaction.SelectedLineItem.IsInEditMode = false;
                        transaction.SelectedLineItem.IsDirty = true;
                        transaction.EditingLineItem.CopyFrom(transaction.SelectedLineItem);
                    }
                }
                else
                {
                    await AddToGrid();

                    if (showAlert)
                    {
                        showAlert = false;
                    }
                    if (isExpansionPanelOpen)
                    {
                        isExpansionPanelOpen = false;
                    }
                }
            }
            else
            {
                showAlert = true;
                isExpansionPanelOpen = true;
            }
            InitNewLine();
            UIStateChanged();
        }


        private async Task AddToGrid()
        {

            this.transaction.SelectedLineItem.LineNumber = this.transaction.InvoiceLineItems.Count() + 1;
            transaction.AddGridItems(this.transaction.SelectedLineItem);
            _objectHelpers["ItemCode"].ResetToInitialValue();
            _objectHelpers["ItemName"].ResetToInitialValue();
            _objectHelpers["TransactionRate"].ResetToInitialValue();
            _objectHelpers["TransactionQuantiy"].ResetToInitialValue();
            _objectHelpers["LineTransactionUnit"].ResetToInitialValue();
            _objectHelpers["DiscountPercentage"].ResetToInitialValue();
            isExpansionPanelOpen = true;
            UIStateChanged();
            await Task.CompletedTask;
        }

        //private async void OnOrderItemEdit(decimal index)
        //{
        //    var updateOrderItem = transaction.InvoiceLineItems.ElementAt((int)index);

        //    await this.ShowEditItem(updateOrderItem);

        //}

        private async void OnItemEditClick(TransactionLineItem item)
        {

            item.IsInEditMode = true;
            transaction.EditingLineItem = item;
            this.transaction.SelectedLineItem.CopyFrom(item);
            StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
            long currentUnitKey = item.TransactionUnit.UnitKey;


            await ReadData("LineTransactionUnit");
            await SetValue("LineTransactionUnit", currentUnitKey);

            BLUIElement textElement = GetLinkedUIElement("ItemCode");
            if (textElement != null)
            {
                textElement.ElementCaption = "Item Code - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
                await SetValue("ItemCode", transaction.SelectedLineItem.TransactionItem.ItemCode);
                RefreshComponent("ItemCode");
                RefreshComponent("ItemName");
                ToggleEditability("ItemCode", false);
            }
            ToggleEditability("LineEditCancel", true);


            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void OnOrderItemDelete(TransactionLineItem item)
        {

            bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to remove Item {item.TransactionItem.ItemName}",
                yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value)
            {
                if (item.IsPersisted)
                {
                    item.IsDirty = true;
                    item.IsActive = 0;
                }
                else
                {
                    this.transaction.InvoiceLineItems.Remove(item);

                    for (int i = 0; i < transaction.InvoiceLineItems.Count(); i++)
                    {
                        transaction.InvoiceLineItems[i].LineNumber = i + 1;
                    }

                }
                StateHasChanged();
            }

            if (transaction.InvoiceLineItems.Count() == 0)
            {
                this.ToggleEditability("Location", true);
            }


        }

        #endregion

        #region save invoice

        private async void OnInvoiceSaveClick(UIInterectionArgs<object> args)
        {
            await SaveInvoice();

        }

        private async Task SaveInvoice()
        {
            if (this.transaction.InvoiceLineItems.Count() > 0)
            {
                isSaving = true;
                UIStateChanged();
                await _transactionManager.SaveTransaction(transaction);
                isSaving = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invoice has been  Saved Successfully", Severity.Success);
                await SetValue("HeaderTitle", transaction.TransactionNumber);

                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = transaction.TransactionKey;
                await LoadTransaction(request);
                await DirectPrintInvoice();
                UIStateChanged();

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please Add Items to Invoice ", Severity.Error);
            }
        }

        #endregion

        #region UI Object Helpers 

        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                UIStateChanged();
            }
        }
        private void ToggleEditability(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(visible);
                UIStateChanged();
            }
        }


        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                UIStateChanged();
            }
        }
        private async Task Focus(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.FocusComponentAsync();
                UIStateChanged();
            }
        }
        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                UIStateChanged();
                await Task.CompletedTask;
            }
        }

        private BLUIElement GetLinkedUIElement(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                return helper.LinkedUIObject;
            }
            return null;
        }

        #endregion




        #region Price List Related



        public void OnAfterPriceListRequest(IEnumerable<PriceListResponse> response)
        {

            UIStateChanged();
        }



        public void OnPriceListClose()
        {

            UIStateChanged();

        }




        public async void OnPriceListItemSelected(PriceListResponse response)
        {
            transaction.SelectedLineItem.TransactionItem.ItemKey = response.ItemKey;
            transaction.SelectedLineItem.TransactionItem.ItemName = response.ItemName;
            transaction.SelectedLineItem.TransactionItem.ItemCode = response.ItemCode;
            __currentPriceListResponse = response;
            await PostItemAddActions();

        }

        public async Task SelectAccountByLocationAndPayementTerm()
        {
            if (transaction.Location.CodeKey > 10 && transaction.PaymentTerm.CodeKey > 10)
            {
                AccPaymentMappingRequest request = new AccPaymentMappingRequest();
                request.ELementKey = this.formDefinition.ElementKey;
                request.Location = transaction.Location;
                request.PayementTerm = transaction.PaymentTerm;
                IList<AccPaymentMappingResponse> responses = await _comboManager.GetPayementAccountMapping(request);
                if (responses.Count > 0)
                {
                    AccPaymentMappingResponse response = responses[0];
                    await SetValue("_Account_", response.Account.AccountKey);
                }
                else
                {
                    if (!transaction.IsPersisted)
                    {
                        await SetValue("_Account_", 1);
                    }
                }
            }
        }

        #endregion

        #region Popups

        private async void OnSearchTracsactionClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;
            await ShowFindTransactionWindow();
            UIStateChanged();
        }

        private async void OnSearchCashOutClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;
            await ShowFindTransactionWindow();
            UIStateChanged();
        }


        private async void GetFromInvoiceClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;
            await ShowGetFromTransaction();
            UIStateChanged();
        }


        private async Task ShowFindTransactionWindow()
        {
            HideAllPopups();
            FindTransactionShown = true;

            await Task.CompletedTask;
        }

        private async Task ShowGetFromTransaction()
        {
            HideAllPopups();
            GetFromInvoiceShown = true;
            await Task.CompletedTask;
        }
        private async Task CloseFindTransactionWindow()
        {
            HideAllPopups();
            await Task.CompletedTask;

        }


        private async void CloseGetFromTransaction(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            await Task.CompletedTask;
        
        }

        private async void GetFromFindClick(UIInterectionArgs<object> args)
        {
            await _refGetFromUI.GetTransactionFromServer();
            await Task.CompletedTask;
        }




        private void HideAllPopups()
        {
            GetFromInvoiceShown = false;
            FindTransactionShown = false;
            UIStateChanged();

        }

        private async void OnOpenTransactionClick(TransactionOpenRequest request)
        {
            HideAllPopups();
            await LoadTransaction(request);

        }

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request);
            otransaction.ElementKey = transaction.ElementKey;
            transaction.CopyFrom(otransaction);
            string valueN = "";
            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {

                await helper.Value.Refresh();


            }
            DateTime dateTime2 = DateTime.Now;

            var t = dateTime2 - dateTime1;

            isSaving = false;

            await SetValue("HeaderTitle", transaction.TransactionNumber);
            StateHasChanged();
          
        }


        private async void OnFromTransactionOpened(BaseServerResponse<BLTransaction> stransaction)
        {

            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            HideAllPopups();
            stransaction.DataObject.ElementKey = transaction.ElementKey;
            transaction.CopyFrom(stransaction.DataObject);
            string valueN = "";
            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {

                await helper.Value.Refresh();


            }
            DateTime dateTime2 = DateTime.Now;

            var t = dateTime2 - dateTime1;

           
            isSaving = false;
            Console.WriteLine(t.TotalMilliseconds);
          
        }


        #endregion

        #region Printing




        private async void PrintInvocieClick(UIInterectionArgs<object> args)
        {
            await DirectPrintInvoice();
        }
        private async Task DirectPrintInvoice()
        {
            TransactionReportLocal reportLocal = new TransactionReportLocal();
            if (transaction.IsPersisted && !transaction.IsDirty)
            {
                reportLocal.TransactionKey = transaction.TransactionKey;
                reportLocal.TransactionDate = transaction.TransactionDate.Value;
                reportLocal.CashRecvAmt1 = 0;
                reportLocal.CardAmt3 = 0;
                reportLocal.BalanceAmt6 = 0;

                foreach (var item in transaction.InvoiceLineItems)
                {
                    TrasnsactionReportLineItem reportLine = new TrasnsactionReportLineItem();
                    reportLine.ItemCode = item.TransactionItem.ItemCode;
                    reportLine.ItemName = item.TransactionItem.ItemName;
                    reportLine.ItemKey = item.TransactionItem.ItemKey;
                    reportLine.Quantity = item.TransactionQuantity;
                    reportLine.TransactionRate = item.TransactionRate;
                    reportLine.LineDiscountAmount = item.GetLineDiscount();
                    reportLocal.LineItems.Add(reportLine);
                }
                reportLocal.CompanyName = "Demo Company";
                reportLocal.CompanyAddress = "";
                reportLocal.LocCity = "";
                reportLocal.LocBusinessPhone = "";
                reportLocal.LocBusinessEmail = "";
                reportLocal.TrasnsactionNumber = transaction.TransactionNumber;
                reportLocal.Customer = transaction.Account.AccountName;
                reportLocal.EntUsrId = "";
                reportLocal.TotalDiscount = transaction.GetOrderDiscountTotal();
                URLDefinitions definitions = new URLDefinitions();
                definitions.URL = InvoicePrinterURL.UrlAction;
                await _printerManager.PrintTransactionBillLocalAsync(reportLocal, definitions);

            }
        }



        #endregion
    }
}
