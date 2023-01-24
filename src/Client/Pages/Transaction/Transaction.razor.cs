//using BL10.CleanArchitecture.Application.Validators.SalesOrder;
//using BL10.CleanArchitecture.Application.Validators.Transaction;
//using BL10.CleanArchitecture.Client.Infrastructure.Helpers;
//using BL10.CleanArchitecture.Domain.DTO.Object;
//using BL10.CleanArchitecture.Domain.Entities;
//using BL10.CleanArchitecture.Domain.Entities.Order;
//using BL10.CleanArchitecture.Domain.Entities.Transaction;
//using BL10.Com.Client.Extensions;
//using BL10.Com.Client.Shared.Dialogs;
//using Microsoft.AspNetCore.Components;
//using MudBlazor;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;

//namespace BL10.Com.Client.Pages.Transaction
//{
//	public partial class Transaction
//	{
//        private BLUIElement formDefinition;//wht

//        private BLTransaction transaction;
//        private CodeBaseResponse ParentLocation;

//        private IDictionary<string, EventCallback> _interactionLogic;
//        private IDictionary<string, BLUIElement> _modalDefinitions;
//        /** followi is must and you need to pass **/
//        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
//        private bool tableloading = false;
//        MudMessageBox addItem { get; set; }
//        private int CodeKey;
//        private decimal price;

//        private ITransactionValidator validator;
//        protected override async Task OnInitializedAsync()
//        {
//            transaction = new();
          
//            long elementKey = 1;
//            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
//            if (elementKey > 10)
//            {
//                var formrequest = new ObjectFormRequest();
//                formrequest.MenuKey = elementKey;
//                formDefinition = await _navManger.GetMenuUIElement(formrequest);
//            }
//            // return  base.OnInitializedAsync();
//            formDefinition.IsDebugMode = true;
//            _interactionLogic = new Dictionary<string, EventCallback>();
//            _modalDefinitions = new Dictionary<string, BLUIElement>();
//            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

//            validator = new TransactionValidator(transaction);
//            HookInteractions();
//        }

//        //make interaction
//        private void HookInteractions()
//        {
//            InteractionHelper helper = new InteractionHelper(this, formDefinition);
//            _interactionLogic = helper.GenerateEventCallbacks();


//        }

//        #region UI Interaction Logics

//        private void OnPayementTermChanged(UIInterectionArgs<CodeBaseResponse> args)
//        {
//            transaction.PaymentTerm = args.DataObject;
//            UIStateChanged();
//        }

//        private void OnOrderCustomerChanged(UIInterectionArgs<AddressResponse> args)
//        {
//            transaction.Address = args.DataObject;
//            UIStateChanged();
//        }

//        private void OnLineTransactionQuantityChanged(UIInterectionArgs<decimal> args)
//        {



//        }

//        private void OnTransactionLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
//        {

//            transaction.Location = args.DataObject;
//            ParentLocation = this.transaction.Location;
//            CodeKey = args.DataObject.CodeKey;
//            UIStateChanged();
//        }

//        private async void OnTransactionItemChange(UIInterectionArgs<ItemResponse> args)
//        {
//            args.CancelChange = true;
//            await ShowAddNewItem(args.DataObject);

//        }


//        private void TransactionUnit_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
//        {
//            args.DataObject.AddtionalData.Add("ItemKey", this.transaction.SelectedLineItem.TransactionItem.ItemKey);
//        }

//        public async Task<ItemRateResponse> RetriveRate(ItemResponse transactionItem)
//        {

//            RateRetrivalModel request = new RateRetrivalModel();
//            request.LocationKey = transaction.Location.CodeKey;
//            request.ItemKey = transactionItem.ItemKey;
//            request.EffectiveDate = DateTime.Now.Date;
//            return (await _comboManager.GetRate(request));
//        }

//        private async Task ShowAddNewItem(ItemResponse Item)
//        {
//            BLUIElement modalUIElement;

//            ToggleViisbility("TransactionItem", true);

//            if (!_modalDefinitions.TryGetValue("InvoiceItemPopUps", out modalUIElement))
//            {
//                modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("InvoiceItemPopUp")).FirstOrDefault();
//                if (modalUIElement != null && modalUIElement.Children.Count > 0)
//                {
//                    _modalDefinitions.Add("InvoiceItemPopUps", modalUIElement);

//                }
//            }

//            if (modalUIElement != null)
//            {
//                ItemRateResponse rates = await RetriveRate(Item);
//                var parameters = new DialogParameters
//                {
//                    ["InvoiceItem"] = transaction.CreateTransactionItem(Item, ParentLocation),
//                    ["ModalUIElement"] = modalUIElement,
//                    ["InteractionLogics"] = _interactionLogic,
//                    ["ObjectHelpers"] = _objectHelpers,
//                    ["ParentLocation"] = ParentLocation,
//                    ["Validaor"] = validator,
//                };
//                DialogOptions options = new DialogOptions();
//                var dialog = _dialogService.Show<InvoiceItemDialog>("Add New Item", parameters, options);
//                var result = await dialog.Result;
//                if (!result.Cancelled)
//                {
                
//                        this.transaction.SelectedLineItem.LineNumber = this.transaction.InvoiceLineItems.Count() + 1;
//                        this.transaction.InvoiceLineItems.Add(this.transaction.SelectedLineItem);
                    
//                }
//                _objectHelpers["TransactionItem"].ResetToInitialValue();
//            }

//            if (!validator.CanChangeHeaderInformatiom())
//            {
//                this.ToggleEditability("Location", false);
//            }



//            UIStateChanged();



//        }

//        private void ToggleViisbility(string name, bool visible)
//        {
//            IBLUIOperationHelper helper;

//            if (_objectHelpers.TryGetValue(name, out helper))
//            {
//                helper.UpdateVisibility(visible);
//                UIStateChanged();
//            }
//        }

//        private void ToggleEditability(string name, bool visible)
//        {
//            IBLUIOperationHelper helper;

//            if (_objectHelpers.TryGetValue(name, out helper))
//            {
//                helper.ToggleEditable(visible);
//                UIStateChanged();
//            }
//        }

//        private async void OnOrderItemDelete(TransactionLineItem item)
//        {

//            bool? result = await _dialogService.ShowMessageBox(
//                "Warning",
//                $"Do you want to remove Item {item.TransactionItem.ItemName}",
//                yesText: "Delete!", cancelText: "Cancel");

//            if (result.HasValue && result.Value)
//            {
//                this.transaction.InvoiceLineItems.Remove(item);
//                this.UIStateChanged();
//            }

//        }

//        private async void onScan(UIInterectionArgs<object> args)
//        {
//            if (ParentLocation != null)
//            {
//                var parameters = new DialogParameters
//                {


//                };
//                DialogOptions options = new DialogOptions();
//                var dialog = _dialogService.Show<QrDialog>("QR Scanner", parameters, options);
//                DialogResult dialogResult = await dialog.Result;
//                if (!dialogResult.Cancelled)
//                {
//                    if (dialogResult.Data != null)
//                    {
//                        string itemCode = dialogResult.Data as string;
//                        ItemRequestModel requestModel = new ItemRequestModel(); ;
//                        requestModel.ItemCode = itemCode;
//                        requestModel.LocationKey = transaction.Location.CodeKey;
//                        requestModel.AddressKey = transaction.Address.AddressKey;
//                        IList<ItemCodeResponse> items = await _comboManager.GetItemByItemCode(requestModel);
//                        if (items.Count > 0)
//                        {
//                            ItemResponse response = new ItemResponse();
//                            response.ItemName = items[0].ItemCodeName;
//                            response.ItemKey = items[0].ItemKey;
//                            await ShowAddNewItem(response);
//                        }
//                        else
//                        {
//                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
//                            _snackBar.Add("Invalid Item Code", Severity.Error);
//                        }
//                    }

//                }

//            }
//            else
//            {
//                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
//                _snackBar.Add("Please enter a location before scaning", Severity.Error);
//            }
//            UIStateChanged();

//        }

//        private void UIStateChanged()
//        {
//            StateHasChanged();
//        }
//        #endregion


//    }
//}
