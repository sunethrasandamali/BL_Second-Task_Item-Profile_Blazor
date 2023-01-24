using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Shared.Components;
using BlueLotus360.Com.Client.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BlueLotus360.Com.Client.Shared.Popups.InventoryManagement.ScanItemTransfer;
using BlueLotus360.CleanArchitecture.Application.Validators.InventoryManagement;
using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.Com.Client.Settings;

namespace BlueLotus360.Com.Client.Pages.InventoryManagement.ScanItemTransferLND
{
    public partial class ScanItem
    {
        #region parameter
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;

        private ItemTransfer _itmTransfer;
        private ItemTransfer _afterSaveHeaderResponse;
        private ItemTransferLineItem _oneLineItm;
        private long elementKey;
        private bool IsLoading;
        private bool IsExpand = false;
        private bool showAlert = false,hasError = false;
        private IItemTransferValidator validator;
        private string alertContent = "";
        private bool isProcessing = false;
        private int TrnkyForOut = 1, TrnKyForIn = 1;
        ItemTransfer _find_itm_trnsfer;
        private int total_scanned_session = 0;
        private bool FindTransferShown = false;
        private bool IsbukeySelected;
        private BLUIElement findTransferUI;
        private TransferOpenRequest trnOpReq;
        private string _serialNo = "";

        #endregion
        protected override async Task OnInitializedAsync()
        {
            elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 

            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();

                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements

                findTransferUI = formDefinition.Children.Where(x => x._internalElementName.Equals("FindPopUp")).FirstOrDefault();

                if (findTransferUI != null)
                {
                    findTransferUI.Children = formDefinition.Children.Where(x => x.ParentKey == findTransferUI.ElementKey).ToList();
                }

            }

            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            InitializeItemtransfer();
            HookInteractions();

            
           
        }
        private void InitializeItemtransfer()
        {
            _itmTransfer = new ItemTransfer();
            _afterSaveHeaderResponse = new ItemTransfer();
            _oneLineItm = new ItemTransferLineItem();
            trnOpReq=new TransferOpenRequest();
            _find_itm_trnsfer = new ItemTransfer();

            total_scanned_session = 0;
            validator = new ItemTransferValidator(_itmTransfer);

            _itmTransfer = new ItemTransfer {
                ElementKey = elementKey,
                FromLocation=new CodeBaseResponse { CodeKey=1,CodeName=""},
                ToLocation = new CodeBaseResponse { CodeKey = 1, CodeName = "" },
            };


        }
        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            AppSettings.RefreshTopBar("Scan Item");
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #region ui events
        private void OnEditClick(UIInterectionArgs<object> args) {
            UIStateChanged();
        }
        private async void OnNewClick(UIInterectionArgs<object> args) {
            InitializeItemtransfer();
            showAlert = false;
            hasError = false;
            validator.UserMessages.AlertMessages.Clear();
            await SetValue("TrnNo", "");
            UIStateChanged();
        }
        private async void OnClickSave(UIInterectionArgs<object> args)
        {
            isProcessing = true;
            showAlert = false;
            validator.UserMessages.AlertMessages.Clear();
            _itmTransfer.SerialNoList.Clear();
            total_scanned_session = 0;
            UIStateChanged();

            if (_itmTransfer.ScanItemTransferLineItem.Count()>0)
            {
                if (_itmTransfer.FromTransactionKey == 1)
                {
                    await saveTrnHeaderFrom();
                    await saveTrnHeaderTo();
                    await ItmSave();
                }
                else
                {
                    await UpdateHeaderofItmTransferOut();
                    await UpdateHeaderofItmTransferIn();
                    await UpdateItemsofItmTransfer();
                }
                trnOpReq.TrnKy = TrnkyForOut;
                await LoadTransfer(trnOpReq);
            }
            else
            {
                validator.AddAlertMessage("Add atleast one item to grid before saving!!", "Info");
                showAlert = true;
                UIStateChanged();
            }
            isProcessing = false;
            UIStateChanged();
           
        }
        private async void OnSaveNewClick(UIInterectionArgs<object> args) {
            showAlert = false;
            hasError = false;

            try
            {
                if (_itmTransfer.FromTransactionKey == 1)
                {
                    await saveTrnHeaderFrom();
                    await saveTrnHeaderTo();
                    await ItmSave();

                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Item Transfer has been  saved Successfully", Severity.Success);
                }
                else
                {
                    await UpdateHeaderofItmTransferOut();
                    await UpdateHeaderofItmTransferIn();
                    await UpdateItemsofItmTransfer();

                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("Item Transfer has been updaed Successfully", Severity.Success);
                }

                trnOpReq.TrnKy = TrnkyForOut;
                await LoadTransfer(trnOpReq);
                InitializeItemtransfer();
            }
            catch (Exception ex)
            {

            }
            
           
            UIStateChanged();
        }
        private async void OnPrintClick(UIInterectionArgs<object> args)
        {
            UIStateChanged();
        }
        private async void OnSearchClick(UIInterectionArgs<object> args)
        {
            //findTransferUI = args.InitiatorObject;
            await ShowFindTransferWindow();
            UIStateChanged();
        }
        private async void OnCancelClick(UIInterectionArgs<object> args) {

            InitializeItemtransfer();
            showAlert = false;
            hasError = false;
            validator.UserMessages.AlertMessages.Clear();
            await SetValue("TrnNo", "");
            UIStateChanged();
        }
        private void OnTrNoChange(UIInterectionArgs<string> args)
        {
            _itmTransfer.TransactionNumber = Convert.ToInt32(args.DataObject);
            UIStateChanged();
        }
        private void OnDateChange(UIInterectionArgs<DateTime?> args)
        {
            _itmTransfer.TransactionDate = (DateTime)args.DataObject;

            UIStateChanged();
        }
        private void OnFromLocChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _itmTransfer.FromLocation = args.DataObject;

            UIStateChanged();
        }
        private void OnToLocChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _itmTransfer.ToLocation = args.DataObject;

            UIStateChanged();
        }
        private async void OnSerialNoScan(UIInterectionArgs<object> args)
        {
            await Scan();
            _serialNo = "";
            UIStateChanged();

        }
        private async void OnAdornmentClick_ScanBox(UIInterectionArgs<object> args)
        {
            await Scan();
            _serialNo = "";
            UIStateChanged();
        }

        private async void OnSerialNoScanByType(UIInterectionArgs<string> args)
        {
            _serialNo = args.DataObject ;
            UIStateChanged();
        }

        #endregion

        #region scan

        private async Task Scan()
        {
            isProcessing = true;
            showAlert = false;
            validator.UserMessages.AlertMessages.Clear();
  
            UIStateChanged();

            validator = new ItemTransferValidator(_itmTransfer);

            if (validator.CanAddItemToGrid())
            {
                if(string.IsNullOrEmpty(_serialNo))
                {
                    await Task.Delay(1);
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
                            _serialNo = dialogResult.Data as string;

                            await LoadGrid(_serialNo);
                        }
                    }
                }
                else
                {
                    await LoadGrid(_serialNo);
                }
                
            }
            else
            {
                validator.AddAlertMessage("Error Ocured,Can't add details ,please check errors below", "Error");
                showAlert = true;
                hasError = true;
                UIStateChanged();
            }
            isProcessing = false;
            UIStateChanged();
        }

        private async Task LoadGrid(string serial_num)
        {
            if (!_itmTransfer.SerialNoList.Contains(serial_num))
            {
                _itmTransfer.SerialNoList.Add(serial_num);
                total_scanned_session++;

                await SetValue("ScanBox", serial_num);
                await SetValue("TotalScan", total_scanned_session);
                await GetGridData(serial_num);

                UIStateChanged();
            }
            else
            {
                validator.AddAlertMessage("This Item is already scanned", "Info");
                showAlert = true;
                UIStateChanged();
            }
        }
        private async Task ShowFindTransferWindow()
        {
            HideAllPopups();
            FindTransferShown = true;
            UIStateChanged();
            await Task.CompletedTask;
        }
        private async Task saveTrnHeaderFrom() 
        {
            _afterSaveHeaderResponse = await _itemTransferManager.SaveTrnHeaderFromLoc(_itmTransfer);
            TrnkyForOut = _afterSaveHeaderResponse.FromTransactionKey;
            await Task.CompletedTask;
            UIStateChanged();
        }
        private async Task saveTrnHeaderTo()
        {
            _itmTransfer.HdrTrfLnkKy = TrnkyForOut;
            _afterSaveHeaderResponse = await _itemTransferManager.SaveTrnHeaderToLoc(_itmTransfer);
            TrnKyForIn = _afterSaveHeaderResponse.ToTransactionKey;
            await Task.CompletedTask;
            UIStateChanged();
        }
        private async Task GetGridData(string serNo) {

            try
            {
                _oneLineItm = new();
                _oneLineItm.ObjectKey = (int)elementKey;
                _oneLineItm.serialNo = serNo;
                _oneLineItm = await _itemTransferManager.GetItemsData(_oneLineItm);
                _oneLineItm.isActive = 1;
               
                _itmTransfer.ScanItemTransferLineItem.Add(_oneLineItm);

                UIStateChanged();
            }
            catch (Exception e)
            {
                validator.AddAlertMessage("There is no item with this serial number", "Error");
                showAlert = true;
                UIStateChanged();
            }

            
        }
        private async Task ItmSave()
        {
            foreach (var list_itm in _itmTransfer.ScanItemTransferLineItem)
            {
                list_itm.ObjectKey = (int)elementKey;
                list_itm.LineNumber = _itmTransfer.ScanItemTransferLineItem.IndexOf(list_itm) + 1;
                list_itm.TransactionKey = TrnkyForOut;
                list_itm.LocationKey = _itmTransfer.FromLocation.CodeKey;
                list_itm.PreItmTrnKy = list_itm.FromItemTrnKey;
                list_itm.OurCode = "TRFOUTSCNV2";

                list_itm.ReturnVal = await _itemTransferManager.SaveLine(list_itm);

                list_itm.TransactionKey = TrnKyForIn;
                list_itm.LocationKey = _itmTransfer.ToLocation.CodeKey;
                list_itm.OurCode = "TRFINSCNV2";
                list_itm.PreItmTrnKy = list_itm.ReturnVal;
                list_itm.ItmTrnTrfLnkKy= list_itm.ReturnVal;

                await _itemTransferManager.SaveLine(list_itm);
            }

            await FifoPosting();

            await Task.CompletedTask;
        }
        private async void DeleteRow(int index,string serNo)
        {
            var parameters = new DialogParameters
            {
                ["ContentText"]= "Are you sure to delete this item?"
            };
            DialogOptions options = new DialogOptions();
            var dialog = _dialogService.Show<DeleteConfirmation>("Confirm", parameters, options);
            DialogResult dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {

                if (_itmTransfer.FromTransactionKey == 1)
                {
                    _itmTransfer.ScanItemTransferLineItem.RemoveAt(index);
                    _itmTransfer.SerialNoList.Remove(serNo);
                }
                else
                {
                    _itmTransfer.ScanItemTransferLineItem[index].isActive = 0;
                }
                total_scanned_session--;
                await SetValue("TotalScan", total_scanned_session);
            }

            

            UIStateChanged();
        }
        private void GridClear()
        {
            _itmTransfer.ScanItemTransferLineItem.Clear();
        }
        private void Closed(string chipToRemove)
        {
            _itmTransfer.SerialNoList.Remove(chipToRemove);
        }
        private async Task CloseFindTransferWindow()
        {
            HideAllPopups();

            await Task.CompletedTask;

        }
        private void HideAllPopups()
        {
            FindTransferShown = false;
            UIStateChanged();
        }
        private async void OnOpenTransferClick(TransferOpenRequest request)
        {
            HideAllPopups();
            InitializeItemtransfer();
            await LoadTransfer(request);
            UIStateChanged();
        }

        private async Task LoadTransfer(TransferOpenRequest request)
        {
            isProcessing = true;
            UIStateChanged();
            _find_itm_trnsfer = await _itemTransferManager.RefreshForm(request);
            _itmTransfer.CopyFrom(_find_itm_trnsfer);
            await SetValue("TrnNo", _find_itm_trnsfer.TransactionNumber);

            TrnkyForOut = _find_itm_trnsfer.FromTransactionKey;

            IsbukeySelected = false;

            if (_find_itm_trnsfer.ScanItemTransferLineItem.Count() > 0)
            {
                foreach (var itm in _find_itm_trnsfer.ScanItemTransferLineItem)
                {
                    if (itm.isActive == 1)
                    {
                        if (!IsbukeySelected)
                        {
                            _itmTransfer.FromBuKy = itm.FromBuKy;
                            _itmTransfer.ToBuKy = itm.ToBuKy;
                            IsbukeySelected = true;
                        }

                        _itmTransfer.SerialNoList.Add(itm.serialNo.ToString());
                    }

                }
            }
            isProcessing = false;
            UIStateChanged();
        }

        private async Task UpdateHeaderofItmTransferOut()
        {
            _itmTransfer.FromTransactionKey = _find_itm_trnsfer.FromTransactionKey;
            _itmTransfer.ElementKey = elementKey;
            _itmTransfer.FromTmStmp = _find_itm_trnsfer.FromTmStmp;
           
            await _itemTransferManager.UpdateHeaderForOut(_itmTransfer);
        }

        private async Task UpdateHeaderofItmTransferIn()
        {
            _itmTransfer.ToTransactionKey = _find_itm_trnsfer.ToTransactionKey;
            _itmTransfer.ElementKey = elementKey;
            _itmTransfer.ToTmStmp = _find_itm_trnsfer.ToTmStmp;
           
            await _itemTransferManager.UpdateHeaderForIn(_itmTransfer);
        }

        private async Task UpdateItemsofItmTransfer()
        {
            
            if (_itmTransfer.ScanItemTransferLineItem.Count()>0)
            {
                foreach (var list_itm in _itmTransfer.ScanItemTransferLineItem)
                {
                    list_itm.ObjectKey = (int)elementKey;

                    if (list_itm.FromItemTrnKey>1 && list_itm.ToItemTrnKey > 1)
                    {
                        list_itm.LineNumber = _itmTransfer.ScanItemTransferLineItem.IndexOf(list_itm) + 1;
                        list_itm.TransactionKey = _find_itm_trnsfer.FromTransactionKey;
                        list_itm.LocationKey = _itmTransfer.FromLocation.CodeKey;
                        list_itm.OurCode = "TRFOUTSCNV2";

                        await _itemTransferManager.UpdateLineForOut(list_itm);

                        list_itm.TransactionKey = _find_itm_trnsfer.ToTransactionKey;
                        list_itm.LocationKey = _itmTransfer.ToLocation.CodeKey;
                        list_itm.OurCode = "TRFINSCNV2";

                        await _itemTransferManager.UpdateLineForIn(list_itm);
                    }
                    else
                    {
                        list_itm.LineNumber = _itmTransfer.ScanItemTransferLineItem.IndexOf(list_itm) + 1;
                        list_itm.TransactionKey = _find_itm_trnsfer.FromTransactionKey; 
                        list_itm.LocationKey = _itmTransfer.FromLocation.CodeKey;
                        list_itm.OurCode = "TRFOUTSCNV2";

                        list_itm.ReturnVal = await _itemTransferManager.SaveLine(list_itm);

                        list_itm.TransactionKey = _find_itm_trnsfer.ToTransactionKey;
                        list_itm.LocationKey = _itmTransfer.ToLocation.CodeKey;
                        list_itm.OurCode = "TRFINSCNV2";
                        list_itm.PreItmTrnKy = list_itm.ReturnVal;
                        list_itm.ItmTrnTrfLnkKy = list_itm.ReturnVal;

                        await _itemTransferManager.SaveLine(list_itm);
                    }

                }

            }
            
            await Task.CompletedTask;
        }

        private  async Task FifoPosting()
        {
            _itmTransfer.FromTransactionKey = TrnkyForOut;
            _itmTransfer.ElementKey = elementKey;
            await _itemTransferManager.FIFOPosting(_itmTransfer);

        }

        #endregion

        #region object helpers
        private void ToggleEditability(string name, bool editable)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.ToggleEditable(editable);
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

        #endregion
    }
}
