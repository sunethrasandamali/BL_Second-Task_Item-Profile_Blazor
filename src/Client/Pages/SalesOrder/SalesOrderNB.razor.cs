using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Settings;
using BlueLotus360.Com.Client.Shared.Dialogs;
using BlueLotus360.Com.Client.Shared.Popups.MasterDetailPopup;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Pages.SalesOrder
{
    public partial class SalesOrderNB
    {
        private BLUIElement formDefinition;
        private Order order;
        private BLUIElement findOrderUI;
        private BLUIElement getFromQuoteUI;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private bool tableloading = false;
        private ReportCompanyDetailsResponse reportCompanyDetailsResponse;
        MudMessageBox addItem { get; set; }

        private CodeBaseResponse ParentLocation;
        private int CodeKey;
        private bool isSaving = false;
        private bool isLoading = false;
        private bool isItemPopupShown = false;
        private bool isClickSaveButton;
        BLUIElement modalUIElement;
        private IOrderValidator validator;
        private bool FindOrderShown = false;
        private bool FindGetFromQuoteShown = false;
        private AddNewAddress _refNewAddressCreation;
        private TerlrikReportOptions _salesOrderReportOption;
        private bool ReportShown = false;
        long elementKey;
        CompletedUserAuth auth;
        protected override async Task OnInitializedAsync()
        {
            
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);
            }
            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            HookInteractions();
            InitilizeNewOrder();
            validator = new SalesOrderValidator(order);

            findOrderUI = formDefinition.Children.Where(x => x._internalElementName.Equals("_SearchSalesOrder_")).FirstOrDefault();
            getFromQuoteUI= formDefinition.Children.Where(x => x._internalElementName.Equals("_GetFromQuotation_")).FirstOrDefault();
            
            auth = await _authenticationManager.GetUserInformation();
            _salesOrderReportOption = new TerlrikReportOptions();
            _salesOrderReportOption.ReportName = "SalesOrder_MMN.trdp";
            _salesOrderReportOption.ReportParameters = new Dictionary<string, object>();
        }

        void InitilizeNewOrder()
        {
            order = new();
            order.FormObjectKey = elementKey;
            _refNewAddressCreation = new();
            _salesOrderReportOption = new TerlrikReportOptions();
            ParentLocation = new();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            AppSettings.RefreshTopBar("Sales Order");
        }

        #region UI Interaction Logics
        private void OnOrderLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            this.order.OrderLocation = args.DataObject;
            ParentLocation = this.order.OrderLocation;
            CodeKey = args.DataObject.CodeKey;
            UIStateChanged();
        }
        private void OnLineLevelLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.SelectedOrderItem.OrderLineLocation = args.DataObject;
            UIStateChanged();
        }
        private void OnPayementTermChanged(UIInterectionArgs<CodeBaseResponse> args)
        {
            order.OrderPaymentTerm = args.DataObject;
            UIStateChanged();
        }
        private void OnOrderCustomerChanged(UIInterectionArgs<AddressResponse> args)
        {
            order.OrderCustomer = args.DataObject;
            UIStateChanged();
        }

        private void OnHeaderLevelDiscountClick(UIInterectionArgs<decimal> args)
        {
            order.HeaderLevelDisountPrecentage = args.DataObject;
            UIStateChanged();
        }

        private void OnOrderRepChanged(UIInterectionArgs<AddressResponse> args)
        {
            //order.OrderCustomer = args.DataObject;
            //UIStateChanged();

        }


        private void OnLineTransactionUnitChange(UIInterectionArgs<UnitResponse> args)
        {

        }


        private async void OnTransactionItemChange(UIInterectionArgs<ItemResponse> args)
        {
            args.CancelChange = true;
            if (BaseResponse.IsValidData(args.DataObject))
            {
                await ShowAddNewItem(args.DataObject);

            }

            UIStateChanged();

        }
        private void TransactionUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("ItemKey", this.order.SelectedOrderItem.TransactionItem.ItemKey);
        }
        private void OnLineTransactionQuantityChanged(UIInterectionArgs<decimal> args)
        {
            order.SelectedOrderItem.TransactionQuantity = args.DataObject;
            order.SelectedOrderItem.LineTotal = order.SelectedOrderItem.GetLineTotalWithDiscount();

            ToggleViisbility("TotalRate", true);

            if (order.SelectedOrderItem.NeedToRequestFromAnotherLocation())
            {
                order.SelectedOrderItem.RequestedQuantity = order.SelectedOrderItem.TransactionQuantity - Math.Max(order.SelectedOrderItem.AvailableStock, 0);
                ToggleViisbility("RequiredQuantity", true);
                ToggleViisbility("LineLevelLocation", true);
                ToggleViisbility("IsTransfer", true);
                ToggleViisbility("IsConfirmed", true);

            }
            else
            {
                order.SelectedOrderItem.RequestedQuantity = 0;
                //ToggleViisbility("RequiredQuantity", false);
                //ToggleViisbility("LineLevelLocation", false);
                //ToggleViisbility("IsTransfer", false);
                //ToggleViisbility("IsConfirmed", false);



            }





        }

        private void OnLineDisPerChange(UIInterectionArgs<decimal> args)
        {
            order.SelectedOrderItem.LineTotal = order.SelectedOrderItem.GetLineTotalWithDiscount();

            ToggleViisbility("TotalRate", true);
        }

        private void OnRateChange(UIInterectionArgs<decimal> args)
        {
            order.SelectedOrderItem.LineTotal = order.SelectedOrderItem.GetLineTotalWithDiscount();

            ToggleViisbility("TotalRate", true);
        }
        private void OnIsTransferClick(UIInterectionArgs<int> args)
        {
            this.order.SelectedOrderItem.IsTransfer = args.DataObject;
            UIStateChanged();
        }

        private void OnIsConfirmedClick(UIInterectionArgs<int> args)
        {
            this.order.SelectedOrderItem.IsTransferConfirmed = args.DataObject;
            UIStateChanged();
        }

        private void TotalRateChange(UIInterectionArgs<decimal> args)
        {

        }
        private void RequiredQuantityChanged(UIInterectionArgs<decimal> args)
        {
            //  this.order.SelectedOrderItem.RequestedQuantity = args.DataObject;
        }
        public async Task<ItemRateResponse> RetriveRate(ItemResponse transactionItem)
        {

            ItemRateRequest request = new ItemRateRequest();
            request.LocationKey = order.OrderLocation.CodeKey;
            request.ItemKey = transactionItem.ItemKey;
            request.EffectiveDate = DateTime.Now.Date;
            request.ConditionCode = "OrdTyp";
            request.ObjectKey = order.FormObjectKey;
            return (await _comboManager.GetRate(request));
        }
        private async Task ShowAddNewItem(ItemResponse Item)
        {
            ToggleViisbility("AddItemToGrod", true);
            ToggleViisbility("CancelItemPopup", true);
            UIStateChanged();

            order.SelectedOrderItem = order.CreateOrderItem(Item, order.OrderLocation);
            if (modalUIElement == null)
            {
                if (!_modalDefinitions.TryGetValue("OrderItemPopUps", out modalUIElement))
                {
                    modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();
                    if (modalUIElement != null && modalUIElement.Children.Count > 0)
                    {
                        _modalDefinitions.Add("OrderItemPopUps", modalUIElement);

                    }
                }
            }
            ItemRateResponse rates = await RetriveRate(Item);
            StockAsAtRequest request = new StockAsAtRequest();
            request.ElementKey = order.FormObjectKey;
            request.LocationKey = order.OrderLocation.CodeKey;
            request.ItemKey = order.SelectedOrderItem.TransactionItem.ItemKey;
            StockAsAtResponse response = await _transactionManager.GetStockAsAt(request);
            order.SelectedOrderItem.AvailableStock = Math.Max(response.StockAsAt, 0);
            order.SelectedOrderItem.TransactionRate = rates.TransactionRate;
            order.SelectedOrderItem.DiscountPercentage = Math.Max(rates.DiscountPercentage, order.HeaderLevelDisountPrecentage);
            order.SelectedOrderItem.ItemTaxType1Per = rates.ItemTaxType1;
            order.SelectedOrderItem.ItemTaxType4Per = rates.ItemTaxType4;
            order.SelectedOrderItem.Rate = rates.Rate;

            isItemPopupShown = true;





        }
        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        private void ToggleViisbility(string name, bool visible)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.UpdateVisibility(visible);
                UIStateChanged();
            }
        }


        private void OnOrderNewClick(UIInterectionArgs<object> args)
        {
            InitilizeNewOrder();
            UIStateChanged();
        }
        private async void OnOrderSaveClick(UIInterectionArgs<object> args)
        {
            if (this.order.OrderItems.Count() > 0 )
            {
                if (order.OrderKey == 1)
                {
                    isSaving = true;
                    UIStateChanged();
                   
                    await _orderManager.SaveOrder(order);
                    isSaving = false;
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Sales Order has been  Saved Successfully", Severity.Success);
                    ReportCompanyDetailsRequest request = new ReportCompanyDetailsRequest();
                    request.BussinessUnit = new CleanArchitecture.Domain.DTO.Object.CodeBaseResponse();
                    request.TransactionKey = 1;
                    request.Location = order.OrderLocation;
                    request.OrderKey = order.OrderKey;
                    request.EmployeeKey = 1;
                    reportCompanyDetailsResponse = await _reportManager.GetReportCompanyInformation(request);
                    isClickSaveButton = true;
                    UIStateChanged();
                }
                else
                {
                    isSaving = true;
                    UIStateChanged();
                    
                    await _orderManager.EditOrder(order);
                    isSaving = false;
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Sales Order has been  Updated Successfully", Severity.Success);
                    ReportCompanyDetailsRequest request = new ReportCompanyDetailsRequest();
                    request.BussinessUnit = new CleanArchitecture.Domain.DTO.Object.CodeBaseResponse();
                    request.TransactionKey = 1;
                    request.Location = order.OrderLocation;
                    request.OrderKey = order.OrderKey;
                    request.EmployeeKey = 1;
                    reportCompanyDetailsResponse = await _reportManager.GetReportCompanyInformation(request);
                    isClickSaveButton = true;
                    UIStateChanged();
                }
                

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please Add Items to Sales Order", Severity.Error);
            }


        }

        private async void onScan(UIInterectionArgs<object> args)
        {
            if (ParentLocation != null)
            {
                var parameters = new DialogParameters
                {


                };
                DialogOptions options = new DialogOptions();
                var dialog = _dialogService.Show<QrDialog>("QR Scanner", parameters, options);
                DialogResult dialogResult = await dialog.Result;
                if (!dialogResult.Cancelled)
                {
                    if (dialogResult.Data != null)
                    {
                        string itemCode = dialogResult.Data as string;
                        ItemRequestModel requestModel = new ItemRequestModel(); ;
                        requestModel.ItemCode = itemCode;
                        requestModel.LocationKey = order.OrderLocation.CodeKey;
                        requestModel.AddressKey = order.OrderCustomer.AddressKey;
                        IList<ItemCodeResponse> items = await _comboManager.GetItemByItemCode(requestModel);
                        if (items.Count > 0)
                        {
                            ItemResponse response = new ItemResponse();
                            response.ItemName = items[0].ItemCodeName;
                            response.ItemKey = items[0].ItemKey;
                            await ShowAddNewItem(response);
                        }
                        else
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Invalid Item Code", Severity.Error);
                        }
                    }

                }

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please enter a location before scaning", Severity.Error);
            }
            UIStateChanged();

        }


        private async void ShowAddNewCustomer(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewAddressCreation.SetParametersAsync(values);
            _refNewAddressCreation.ShowPopUp();

        }

        private void OnOrderPrintClick(UIInterectionArgs<object> args)
        {
            if (order.OrderKey>1)
            {
                _salesOrderReportOption.ReportParameters.Clear();
                _salesOrderReportOption.ReportName = "SalesOrder_MMN.trdp";
                _salesOrderReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyId);
                _salesOrderReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                _salesOrderReportOption.ReportParameters.Add("UsrId", auth.AuthenticatedUser.UserId);
                _salesOrderReportOption.ReportParameters.Add("OrdKy", order.OrderKey);

                ReportShown = true;
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
            }
            

            StateHasChanged();
        }

        private async void OnOrderFindClick(UIInterectionArgs<object> args)
        {
            findOrderUI = args.InitiatorObject;

            await ShowFindOrderWindow();
        }

        private async void OnGetFromQuotationClick(UIInterectionArgs<object> args)
        {
            getFromQuoteUI = args.InitiatorObject;

            await ShowGetFromQuoteWindow();
        }

        private async Task ShowFindOrderWindow()
        {
            HideAllPopups();
            FindOrderShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async Task ShowGetFromQuoteWindow()
        {
            HideAllPopups();
            FindGetFromQuoteShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }
        private async void OnOrderItemEdit(decimal index)
        {
            var updateOrderItem = order.OrderItems.ElementAt((int)index);

            await this.ShowEditItem(updateOrderItem);

        }

        private async void OnOrderItemDelete(OrderItem item)
        {

            bool? result = await _dialogService.ShowMessageBox(
                "Warning",
                $"Do you want to remove Item {item.TransactionItem.ItemName}",
                yesText: "Delete!", cancelText: "Cancel");

            if (result.HasValue && result.Value)
            {
                this.order.OrderItems.Remove(item);

                for (int i = 0; i < order.OrderItems.Count(); i++)
                {
                    order.OrderItems[i].LineNumber = i + 1;
                }
                StateHasChanged();
            }

            if (order.OrderItems.Count() == 0)
            {
                this.ToggleEditability("Location", true);
            }



        }




        #endregion


        private void Done()
        {
            InitilizeNewOrder();
            isClickSaveButton = false;
        }

        private async Task Print()
        {
            var parameters = new DialogParameters
            {


            };

            DialogOptions options = new DialogOptions();
            options.FullScreen = true;
            var dialog = _dialogService.Show<PrintDialog>("Print", parameters, options);
            await Task.CompletedTask;

        }

        private async Task ShowEditItem(OrderItem Item)
        {
            BLUIElement modalUIElement;

            order.SelectedOrderItem = Item;

            if (Item.RequestedQuantity > 0)
            {
                ToggleViisbility("RequiredQuantity", true);
                ToggleViisbility("LineLevelLocation", true);
                ToggleViisbility("IsTransfer", true);
                ToggleViisbility("IsConfirmed", true);
            }
            ToggleViisbility("AddItemToGrod", false);
            ToggleViisbility("CancelItemPopup", false);
            ToggleViisbility("TransactionItem", true);

            if (!_modalDefinitions.TryGetValue("OrderItemPopUps", out modalUIElement))
            {
                modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("OrderItemPopUp")).FirstOrDefault();
                if (modalUIElement != null && modalUIElement.Children.Count > 0)
                {
                    _modalDefinitions.Add("OrderItemPopUps", modalUIElement);


                }
            }

            if (modalUIElement != null)
            {

                var parameters = new DialogParameters
                {
                    ["OrderItem"] = Item,
                    ["ModalUIElement"] = modalUIElement,
                    ["InteractionLogics"] = _interactionLogic,
                    ["ObjectHelpers"] = _objectHelpers,
                    ["ParentLocation"] = ParentLocation,
                    ["Validaor"] = validator,
                    ["ButtonName"] = "Update",
                    ["HeadingPopUp"] = "Edit Item",
                };
                DialogOptions options = new DialogOptions();
                var dialog = _dialogService.Show<OrderItemDialog>("Edit Item", parameters, options);
                var result = await dialog.Result;

                if (!result.Cancelled)
                {
                    order.Update(this.order.SelectedOrderItem, (int)(this.order.SelectedOrderItem.LineNumber - 1));
                }
                else
                {

                }

            }

            if (!validator.CanChangeHeaderInformatiom())
            {
                this.ToggleEditability("Location", false);
            }
        }


        private async void OnCancelPopupClick(UIInterectionArgs<object> args)
        {
            isItemPopupShown = false;
            UIStateChanged();
            await Task.CompletedTask;
        }

        private async void OnAddItemButtonClick(UIInterectionArgs<object> args)
        {

            if (this.order.SelectedOrderItem.TransactionQuantity <= this.order.SelectedOrderItem.AvailableStock ||
            (this.order.SelectedOrderItem.TransactionQuantity <= (this.order.SelectedOrderItem.AvailableStock + this.order.SelectedOrderItem.RequestedQuantity) &&
            (this.order.SelectedOrderItem.OrderLineLocation != ParentLocation)))
            {
                this.order.SelectedOrderItem.LineTotal = this.order.SelectedOrderItem.TransactionRate * this.order.SelectedOrderItem.TransactionQuantity;
                this.order.SelectedOrderItem.LineTotalWithoutDiscount = this.order.SelectedOrderItem.TransactionRate * this.order.SelectedOrderItem.TransactionQuantity;
                this.order.SelectedOrderItem.LineNumber = this.order.OrderItems.Count() + 1;
                this.order.OrderDate = DateTime.Now;
                this.order.OrderItems.Add(this.order.SelectedOrderItem);
            }

            _objectHelpers["TransactionItem"].ResetToInitialValue();
            isItemPopupShown = false;
            UIStateChanged();
            await Task.CompletedTask;

        }

        private async void HideAllPopups()
        {
            FindOrderShown = false;
            isItemPopupShown = false;
            FindGetFromQuoteShown = false;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void LoadOrder(OrderOpenRequest request)
        {
            HideAllPopups();
            isLoading = true;
            UIStateChanged();

            Order loaded_order = await _orderManager.OpenOrder(request);
            loaded_order.FormObjectKey = order.FormObjectKey;
            order.CopyFrom(loaded_order);
  
            await SetValue("HeaderTitle", order.OrderNumber);
            if (!validator.CanChangeHeaderInformatiom())
            {
                this.ToggleEditability("Location", false);
            }
            isLoading = false;
            UIStateChanged();
        }

        private async void LoadOrderFromQuotation(OrderOpenRequest request)
        {
            HideAllPopups();
            isLoading = true;
            UIStateChanged();

            Order loaded_order = await _orderManager.OpenQuotation(request);
            loaded_order.FormObjectKey = order.FormObjectKey;
            order.CopyFrom(loaded_order);

            await SetValue("HeaderTitle", order.OrderNumber);
            isLoading = false;
            UIStateChanged();
        }

        #region customer creation 
        private async Task OnCustomerCreateSuccess(AddressMaster address)
        {
            await ReadData("Customer");
            await SetValue("Customer", address);

        }

        private async Task ReadData(string name, bool UseLocalStorage = false)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await (helper as IBLServerDependentComponent).FetchData(UseLocalStorage);

                StateHasChanged();
            }
        }
        #endregion

        #region object helpers
        private async Task SetValue(string name, object value)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                await helper.SetValue(value);
                StateHasChanged();
                await Task.CompletedTask;
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
        #endregion
    }
}
