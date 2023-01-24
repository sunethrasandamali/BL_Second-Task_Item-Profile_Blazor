using BlueLotus360.CleanArchitecture.Application.Validators.Transaction;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.DTO.Report;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Settings;
using BlueLotus360.Com.Client.Shared;
using BlueLotus360.Com.Client.Shared.Components;
using BlueLotus360.Com.Client.Shared.Dialogs;
using BlueLotus360.Com.Client.Shared.Popups;
using BlueLotus360.Com.Client.Shared.Popups.MasterDetailPopup;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Pages.Invoice
{
    public partial class LaundroCareTransaction
    {
        private long elementKey = 1;
        private BLUIElement UIDefinition;//wht
        private BLUIElement findTrandsactionUI;//wht
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private LaundercareItemPiicker _refItemPicker;
        private BLTransaction transaction = new();
        private IList<CodeBaseResponse> Services = new List<CodeBaseResponse>();
        private IList<CodeBaseResponse> HumanTypes = new List<CodeBaseResponse>();
        private IList<ItemResponse> Items = new List<ItemResponse>();
        private ITransactionValidator validator;
        private string _tableHeight = "200px";
        private MudTable<TransactionLineItem> _table;
        private bool isInEditMode = false;
        private bool isSaving = false;
        private bool FindTransactionShown = false;
        private AddNewAddress _refNewAddressCreation;
        private UserMessageDialog _refUserMessage;
        private GenericReciept _refgenericReciept;
        private bool ReportShown = false;
        private ReportCompanyDetailsResponse _companyDetails;
        private TerlrikReportOptions _dayEndReportOption;

        protected override async Task OnInitializedAsync()
        {

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            await InitilizeFormDefinitions();
            await base.OnInitializedAsync();
            CompletedUserAuth auth = await _authenticationManager.GetUserInformation();
            _dayEndReportOption = new TerlrikReportOptions();
            _dayEndReportOption.ReportName = "Invoice_LND.trdp";
            _dayEndReportOption.ReportParameters = new Dictionary<string, object>();
            _dayEndReportOption.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyId);
            _dayEndReportOption.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
            _dayEndReportOption.ReportParameters.Add("ObjKy", elementKey);



        }

        private async Task InitilizeFormDefinitions()
        {
            if (transaction == null)
            {
                transaction = new BLTransaction();
            }
            else
            {
                transaction.InvoiceLineItems.Clear();
                transaction.Address = new AddressResponse();
                transaction.Account = new AccountResponse();
                transaction.Amount = 0;
                transaction.Amount2 = 0;
                transaction.Amount3 = 0;
                transaction.Amount4 = 0;
                transaction.Amount5 = 0;
                transaction.Amount6 = 0;
                transaction.SubTotal = 0;
                transaction.NetAmount = 0;
                transaction.DeliveryDate = DateTime.Now;
                transaction.TransactionKey = 1;
                transaction.IsPersisted = false;
                transaction.PaymentTerm = new CodeBaseResponse();
                transaction.Code1 = new CodeBaseResponse();
                transaction.SelectedLineItem = new TransactionLineItem();
                transaction.CalculateTotals();
            }

            if (_objectHelpers == null)
            {
                _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();
            }

            var formrequest = new ObjectFormRequest();
            formrequest.MenuKey = elementKey;
            if (UIDefinition == null || UIDefinition.Children.Count == 0)
            {
                UIDefinition = await _navManger.GetMenuUIElement(formrequest);

            }
            UIDefinition.CssClass = "laund";
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();

            validator = new LaundroCareValidator(transaction);
            transaction.ElementKey = elementKey;
            HookInteractions();
        }


        private async void NewTransaction(UIInterectionArgs<object> uIInterectionArgs)
        {
            await InitilizeFormDefinitions();
            StateHasChanged();
        }


        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, UIDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();//
            InitilizeOtherUIControle();
            AppSettings.RefreshTopBar("Invoice");
        }



        private void InitilizeOtherUIControle()
        {
            //    HookInteractions();
        }


        #region UIEvents

        private async void OnTransactionLocationChanged(UIInterectionArgs<CodeBaseResponse> args)
        {

            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void ServiceName_AfterComboLoaded(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            Services = args.DataObject;
            await Task.CompletedTask;
            StateHasChanged();
        }
        private async void HumanType_AfterComboLoaded(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            HumanTypes = args.DataObject;
            await Task.CompletedTask;
            StateHasChanged();
        }


        private async void OnItemCategory1Change(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.SelectedLineItem.ItemCategory1 = args.DataObject;
            await ReadData("Article");
            StateHasChanged();
        }

        private async void OnItemCategory2Change(UIInterectionArgs<CodeBaseResponse> args)
        {
            transaction.SelectedLineItem.ItemCategory2 = args.DataObject;
            await ReadData("Article");
            StateHasChanged();
        }

        private async void OnContraAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            await Task.CompletedTask;
            transaction.ContraAccountObjectKey = args.InitiatorObject.ElementKey;
            StateHasChanged();

        }

        private async void OnAccountChange(UIInterectionArgs<AccountResponse> args)
        {
            await Task.CompletedTask;
            transaction.AccountObjectKey = args.InitiatorObject.ElementKey;
            StateHasChanged();

        }

        private async void Article_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.CancelRead = !validator.CanItemComboRequestFromServer();
            args.DataObject.AddtionalData.Add("ItmCat2Ky", transaction.SelectedLineItem.ItemCategory2 == null
                ? 1 : transaction.SelectedLineItem.ItemCategory2.CodeKey
                );
            args.DataObject.AddtionalData.Add("ItmCat1Ky", transaction.SelectedLineItem.ItemCategory1 == null ?
                1 : transaction.SelectedLineItem.ItemCategory1.CodeKey
                );

            await Task.CompletedTask;
        }

        private async void OpenInvoiceReport(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            if (transaction.TransactionKey > 10)
            {
                object varLockKy;

                if (_dayEndReportOption.ReportParameters.TryGetValue("TrnKy", out varLockKy))
                {
                    _dayEndReportOption.ReportParameters["TrnKy"] = transaction.TransactionKey;
                }
                else
                {
                    _dayEndReportOption.ReportParameters.Add("TrnKy", transaction.TransactionKey);
                }
                ReportShown = true;
            }
            else
            {
                bool? result = await _dialogService.ShowMessageBox(
                  "Warning",
                  $"Please Select a Transaction"
                );
                return;
            }

            StateHasChanged();
        }
        private async void Article_AfterComboLoaded(UIInterectionArgs<IList<ItemResponse>> args)
        {
            Items = args.DataObject;
            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void OnTransactionItemChanged(UIInterectionArgs<ItemResponse> args)
        {
            transaction.SelectedLineItem.TransactionItem = args.DataObject;
            if (BaseResponse.IsValidData(args.DataObject))
            {
                ItemRateResponse response = await RetriveRate(transaction.SelectedLineItem);
                transaction.SelectedLineItem.TransactionRate = response.TransactionRate;
                transaction.SelectedLineItem.Rate = response.Rate;
            }
            await Task.CompletedTask;
            StateHasChanged();
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


        private async void OnInvoiceSaveClick(UIInterectionArgs<object> args)
        {
            if (validator.CanSaveTransaction())
            {
                await _transactionManager.SaveTransaction(transaction);
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invoice has been  Saved Successfully", Severity.Success);
                await SetValue("HeaderTitle", transaction.TransactionNumber);
                TransactionOpenRequest request = new TransactionOpenRequest();
                request.TransactionKey = transaction.TransactionKey;
                await LoadTransaction(request);
            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }

            await Task.CompletedTask;

        }

        private async void OnPaymementTermChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            await Task.CompletedTask;
            StateHasChanged();
        }



        private async void OnCodeBaseComboChange(UIInterectionArgs<CodeBaseResponse> args)
        {

            await Task.CompletedTask;
            StateHasChanged();
        }

        public async Task<ItemRateResponse> RetriveRate(TransactionLineItem transactionItem)
        {
            ItemService itemService = new ItemService();
            itemService.ComboManager = _comboManager;

            return await itemService.RequestItemRateForTransaction(transaction, transactionItem);



        }

        private async void OnItemEditClick(TransactionLineItem line)
        {
            isInEditMode = true;
            transaction.SelectedLineItem = line;
            StateHasChanged();
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
                item.IsActive = 0;
                transaction.CalculateCBalances();
                StateHasChanged();
            }
            await Task.CompletedTask;

        }
        private async void OnAddtoGridClick(UIInterectionArgs<object> args)
        {

            if (validator.CanAddItemToGrid())
            {
                if (!isInEditMode)
                {
                    transaction.InvoiceLineItems.Add(transaction.SelectedLineItem);
                }
                transaction.CalculateTotals();
                transaction.InitilizeNewLineItem();
                isInEditMode = false;


            }
            else
            {
                _refUserMessage.ShowUserMessageWindow();
            }

            StateHasChanged();

            await Task.CompletedTask;
        }


        private async void ShowScanSerialNumber(TransactionLineItem item)
        {
            DialogOptions dialogOptions = new DialogOptions();
            dialogOptions.MaxWidth = MaxWidth.Medium;
            DialogParameters dialogParam = new DialogParameters();
            dialogParam.Add("LineItem", item);
            var dialog = _dialogService.Show<ScanSerial>("Select Serial Number", dialogParam, dialogOptions);

            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void OnNumericBoxChnaged(UIInterectionArgs<decimal> args)
        {
            transaction.CalculateTotals();
            StateHasChanged();
            await Task.CompletedTask;

        }
        #endregion

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

        private async Task LoadTransaction(TransactionOpenRequest request)
        {
            HideAllPopups();
            DateTime dateTime1 = DateTime.Now;
            isSaving = true;
            URLDefinitions urlDefinitions = new URLDefinitions();
            urlDefinitions.URL = UIDefinition.ReadController + "/" + UIDefinition.ReadAction;
            BLTransaction otransaction = await _transactionManager.OpenTransaction(request, urlDefinitions);
            otransaction.ElementKey = transaction.ElementKey;
            transaction.CopyFrom(otransaction);
            string valueN = "";

            IList<KeyValuePair<string, IBLUIOperationHelper>> pairs = _objectHelpers.ToList();

            foreach (KeyValuePair<string, IBLUIOperationHelper> helper in pairs)
            {
                await helper.Value.Refresh();

            }

            transaction.CalculateTotals();
            // transaction.CalculateCBalances();
            await SetValue("HeaderTitle", transaction.TransactionNumber);

            AppSettings.RefreshTopBar("Invoice - " + transaction.TransactionNumber);
            isSaving = false;
        }


        private async void HideAllPopups()
        {
            FindTransactionShown = false;
            ReportShown = false;
            StateHasChanged();
            await Task.CompletedTask;
        }

        private async void OnSearchTracsactionClick(UIInterectionArgs<object> args)
        {
            findTrandsactionUI = args.InitiatorObject;

            await ShowFindTransactionWindow();

        }

        private async void ShowAddNewCustomer(UIInterectionArgs<object> args)
        {
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewAddressCreation.SetParametersAsync(values);
            _refNewAddressCreation.ShowPopUp();

        }

        private async Task ShowFindTransactionWindow()
        {
            HideAllPopups();
            FindTransactionShown = true;
            StateHasChanged();
            await Task.CompletedTask;
        }


        private async Task OnCustomerCreateSuccess(AddressMaster address)
        {
            await ReadData("Customer");
            await SetValue("Customer", address);

        }

        private async void OnRecieptsClick(UIInterectionArgs<object> args)
        {
            HideAllPopups();
            if (transaction.TransactionKey > 10)
            {
                IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
                ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
                ParameterView values = ParameterView.FromDictionary(ParamDictionary);
                await _refgenericReciept.SetParametersAsync(values);
                _refgenericReciept.ShowPopUp();
                StateHasChanged();
                await Task.CompletedTask;

            }

            else
            {
                bool? result = await _dialogService.ShowMessageBox(
                  "Warning",
                  $"Please Select a Transaction"
                );
                return;

            }

        }

        private async void OnAmount5Change(UIInterectionArgs<decimal> args)
        {
            if (!transaction.AddtionalData.ContainsKey("AdvancePaymentObjectKey"))
            {
                transaction.AddtionalData.Add("AdvancePaymentObjectKey", args.InitiatorObject.ElementKey);
            }
            else
            {
                transaction.AddtionalData["AdvancePaymentObjectKey"] = args.InitiatorObject.ElementKey;

            }

            transaction.CalculateTotals();
            StateHasChanged();
            await Task.CompletedTask;
        }



        private async void OnPriceListChange(UIInterectionArgs<CodeBaseResponse> uIInterectionArgs)
        {
            await ReadData("ServiceName");
            StateHasChanged();
            await Task.CompletedTask;
        }


        private async void ServiceName_OnBeforeDataFetch(UIInterectionArgs<ComboRequestDTO> args)
        {
            args.DataObject.AddtionalData.Add("PreKy", BaseResponse.GetKeyValue(transaction.Code1));
            await Task.CompletedTask;

        }


    }
}
