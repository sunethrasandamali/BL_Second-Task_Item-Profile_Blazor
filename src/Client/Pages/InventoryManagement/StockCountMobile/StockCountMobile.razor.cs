using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Settings;
using BlueLotus360.Com.Client.Shared.Components;
using BlueLotus360.Com.Client.Shared.Dialogs;
using BlueLotus360.Com.Client.Shared.Popups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toolbelt.Blazor.HotKeys;

namespace BlueLotus360.Com.Client.Pages.InventoryManagement.StockCountMobile
{
    public partial class StockCountMobile
    {
        #region parameter
        private BLUIElement formDefinition;//wht
        private BLTransaction transaction = new();

        private BLPriceList _refPriceList;

        private PriceListRequest __priceListRequest;

        private UIBuilder _refBuilder;


        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private bool tableloading = false;
        MudMessageBox addItem { get; set; }

        private MudTable<TransactionLineItem> _table;

        private IStockCountValidator validator;
        bool isSaving;
        public IList<PriceListResponse> price_list_response { get; set; }
        bool hasItemCode;
        bool showAlert;
        bool isExpansionPanelOpen;

        private PriceListResponse __currentPriceListResponse;


        private bool ReplacementMode = false;

        private BLTransaction ReplacementTransaction;

        private ReportCompanyDetailsResponse _companyDetails;

        private PriceListRequest request;
        private IList<PriceListResponse> list;
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
            }

            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            InitilizeNewOrder();
            HookInteractions();

            __priceListRequest = new PriceListRequest();
            __priceListRequest.ElementKey = elementKey;
            await _sessionStorage.ClearAsync();
            transaction.ElementKey = elementKey;

            request = new PriceListRequest();
            list = await _comboManager.GetPriceLists(request);
        } 

        private async void InitilizeNewOrder()
        {

            transaction.InvoiceLineItems.Clear();
            transaction.IsPersisted = false;
            transaction.TransactionKey = 1;
            transaction.ElementKey = formDefinition.ElementKey;
            validator = new StockCountValidator(transaction);
            await SetValue("HeaderTitle", "New");

        } 

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            AppSettings.RefreshTopBar("Stock Count");
        }

        private async void Refresh(long ItemKey)
        {
            if (ItemKey > 1 )
            {
                PriceListResponse uni = list.Where(x => x.ItemKey.Equals(ItemKey)).FirstOrDefault();
                transaction.SelectedLineItem.TransactionItem.ItemCode = uni.ItemCode;
                transaction.SelectedLineItem.TransactionItem.ItemName = uni.ItemName;
                await SetDataSource("LineTransactionUnit", uni.GetComboResponseByPriceList());
            }
            UIStateChanged();
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        #endregion

        #region Invoice Related Events
        private async void OnDescriptionChanged(UIInterectionArgs<string> args)
        {
            UIStateChanged();
        }

        private async void OnRemarksChanged(UIInterectionArgs<string> args)
        {
            UIStateChanged();
        }

        private async void OnItemNameChange(UIInterectionArgs<ItemResponse> args)
        {
            Refresh(args.DataObject.ItemKey);
            transaction.SelectedLineItem.TransactionItem = args.DataObject;
            UIStateChanged();
        }
        private async void OnUnitChange(UIInterectionArgs<UnitResponse> args)
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
                    RefreshComponent("Item");
                    RefreshComponent("Quantiy");
                    await Focus("Quantiy");
                }
            }
            UIStateChanged();
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

        private async void OnQuantityChanged(UIInterectionArgs<decimal> args)
        {

            this.transaction.SelectedLineItem.TransactionQuantity = args.DataObject;
            await CalculatePriceBasedOnPriceList(args.DataObject);
            UIStateChanged();

        }

        private async Task CalculatePriceBasedOnPriceList(decimal Qty)
        {

            transaction.SelectedLineItem.LineNetRate = transaction.SelectedLineItem.GetLineTotalWithDiscount();
            transaction.SelectedLineItem.TransactionQuantity = Qty;
            await Task.CompletedTask;

        }

        private async void OnRateChanged(UIInterectionArgs<decimal> args)
        {
            transaction.SelectedLineItem.TransactionRate = args.DataObject;
            await CalculatePriceBasedOnPriceList(transaction.SelectedLineItem.TransactionQuantity);
            UIStateChanged();
        }

        private async void OnSubTotalChanged(UIInterectionArgs<decimal> args)
        {
            UIStateChanged();
        }
        private async void OnTaskIDChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            UIStateChanged();
        }

        private async void OnTopDescriptionChanged(UIInterectionArgs<string> args)
        {
            transaction.SelectedLineItem.Description = args.DataObject;
            UIStateChanged();
        }
        //private async void OnSerialNoChanged(UIInterectionArgs<string> args)
        //{
        //    transaction.SelectedLineItem.SerialNumber = args.DataObject;
        //    UIStateChanged();
        //}


        private async void InitNewLine()
        {
            this.transaction.SelectedLineItem = new TransactionLineItem();
            _objectHelpers["Item"].ResetToInitialValue();
            _objectHelpers["Rate"].ResetToInitialValue();
            _objectHelpers["Quantiy"].ResetToInitialValue();
            _objectHelpers["LineTransactionUnit"].ResetToInitialValue();
            _objectHelpers["Description"].ResetToInitialValue();
            this.transaction.EditingLineItem = null;
            ToggleEditability("LineEditCancel", false);
            StateHasChanged();
        }

        #endregion

        #region Add/Edit/Delete methods

        private async void OnAddToGridClick(UIInterectionArgs<object> args)
        {
            // StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
            if (validator.CanAddItemToGrid(100))
            {
                if (transaction.SelectedLineItem.IsInEditMode)
                {
                    if (transaction.EditingLineItem != null)
                    {
                        transaction.SelectedLineItem.IsInEditMode = false;
                        transaction.SelectedLineItem.IsDirty = true;
                        transaction.EditingLineItem.CopyFrom(transaction.SelectedLineItem);
                        if (ReplacementMode)
                        {
                            transaction.EditingLineItem.Quantity2 *= -1;
                            transaction.EditingLineItem.TransactionQuantity *= -1;
                            transaction.EditingLineItem.Quantity *= -1;
                            transaction.SelectedLineItem = new();
                        }
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

            if (ReplacementMode)
            {
                this.ReplacementTransaction.SelectedLineItem.LineNumber = this.transaction.InvoiceLineItems.Count() + 1;
                ReplacementTransaction.AddGridItems(this.transaction.SelectedLineItem);
                this.transaction.SelectedLineItem = new();
            }
            else
            {

                transaction.AddGridItems(this.transaction.SelectedLineItem);
            }
            _objectHelpers["Item"].ResetToInitialValue();
            _objectHelpers["Rate"].ResetToInitialValue();
            _objectHelpers["Quantiy"].ResetToInitialValue();
            _objectHelpers["LineTransactionUnit"].ResetToInitialValue();
            _objectHelpers["SubTotal"].ResetToInitialValue();
            _objectHelpers["Description"].ResetToInitialValue();
            isExpansionPanelOpen = true;
            UIStateChanged();
            await Task.CompletedTask;
        }
        private async void OnItemEditClick(TransactionLineItem item)
        {

            item.IsInEditMode = true;
            transaction.EditingLineItem = item;
            this.transaction.SelectedLineItem.CopyFrom(item);
            StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
            long currentUnitKey = item.TransactionUnit.UnitKey;


            await ReadData("LineTransactionUnit");
            await SetValue("LineTransactionUnit", currentUnitKey);

            BLUIElement textElement = GetLinkedUIElement("Item");
            if (textElement != null)
            {
                textElement.ElementCaption = "Item - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
                await SetValue("Item", transaction.SelectedLineItem.TransactionItem.ItemCode);
                RefreshComponent("Item");
            }
            ToggleEditability("LineEditCancel", true);


            UIStateChanged();
            await Task.CompletedTask;
        }


        private async void OnReplacementEditClick(TransactionLineItem item)
        {

            item.IsInEditMode = true;
            transaction.EditingLineItem = item;
            this.transaction.SelectedLineItem.CopyFrom(item);
            this.transaction.SelectedLineItem.TransactionQuantity *= -1;
            this.transaction.SelectedLineItem.Quantity *= -1;
            this.transaction.SelectedLineItem.Quantity2 *= -1;

            StockAsAtResponse stockAsAtResponse = await GetStockResponseForCurrentItem();
            long currentUnitKey = item.TransactionUnit.UnitKey;


            await ReadData("LineTransactionUnit");
            await SetValue("LineTransactionUnit", currentUnitKey);

            BLUIElement textElement = GetLinkedUIElement("Item");
            if (textElement != null)
            {
                textElement.ElementCaption = "Item - AvlQty : " + stockAsAtResponse.StockAsAt.ToString();
                await SetValue("Item", transaction.SelectedLineItem.TransactionItem.ItemCode);
                RefreshComponent("Item");
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
                    item.IsActive = 1;
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

        #region save Stock Count

        private async void OnSaveStockCount(UIInterectionArgs<object> args)
        {
            //await SaveStockCount();

        }

        private async Task SaveStockCount()
        {
            if (ReplacementMode)
            {
                await SaveReplacementInvoice();
            }
            else
            {
                if (this.transaction.InvoiceLineItems.Count() > 0)
                {
                    isSaving = true;


                    UIStateChanged();
                    if (validator.CanSaveTransaction())
                    {
                        transaction.IsDirty = true;
                        transaction.IsHold = false;
                        transaction.IsApproved = 1;
                        foreach (var line in transaction.InvoiceLineItems)
                        {
                            line.IsApproved = 1;
                        }
                        await _transactionManager.SaveTransaction(transaction);

                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Stock Count has been  Saved Successfully", Severity.Success);
                        await SetValue("HeaderTitle", transaction.TransactionNumber);
                        TransactionOpenRequest request = new TransactionOpenRequest();
                        request.TransactionKey = transaction.TransactionKey;
                        await LoadTransaction(request);
                        //await DirectPrintInvoice();

                        ToggleEditability("LineEditCancel", false);
                        await SetValue("HeaderTitle", "New");
                    }
                    isSaving = false;

                    UIStateChanged();


                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Please Add Items to Stock Count ", Severity.Error);
                }
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


        private async Task SetDataSource(string name, object dataSource)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).SetDataSource(dataSource);
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

        private async Task ToggleEditable(string name, bool value) 
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(value);
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

        private async void OnOpenTransactionClick(TransactionOpenRequest request)
        {
            await LoadTransaction(request);

        }

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            request.ElementKey = transaction.ElementKey;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request);
            otransaction.ElementKey = transaction.ElementKey;
            transaction.CopyFrom(otransaction);
            string valueN = "";

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();


            }

             transaction.CalculateCBalances();
            await SetValue("HeaderTitle", transaction.TransactionNumber);
            isSaving = false;
        }

        private async Task SaveReplacementInvoice()
        {

            if (this.ReplacementTransaction.GetOrderTotalWithDiscounts() < 0)
            {

                bool? result = await _dialogService.ShowMessageBox(
               "Warning",
               $"Please Add Replacement Items. Cannot Proceed with Minus values");
                return;
            }

            if (this.ReplacementTransaction.InvoiceLineItems.Count() > 0)
            {
                isSaving = true;
                UIStateChanged();
                ReplacementTransaction.TransactionDate = DateTime.Now;
                await _transactionManager.SaveTransaction(ReplacementTransaction);
                isSaving = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Replacement Invoice has been  Saved Successfully", Severity.Success);


                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = ReplacementTransaction.TransactionKey;
                //await LoadReplacementTransaction(request);
                //await DirectPrintReplacementInvoice();
                UIStateChanged();

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please Add Items to Invoice ", Severity.Error);
            }
        }
    }
}
