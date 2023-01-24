﻿using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.CleanArchitecture.Domain.Entities.InventoryManagement.ItemTransfer;
using BlueLotus360.CleanArchitecture.Application.Validators.InventoryManagement;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Dialogs;

namespace BlueLotus360.Com.UI.Definitions.Pages.InventoryManagement.ScanItemTransfer
{
    public partial class ScanInvoiceTransfer
    {
        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;

        private ItemTransfer _itmTransfer;
        private ItemTransfer _afterSaveHeaderResponse;
        private LNDInvoice _invoiceRequest;
        private List<ItemTransferLineItem> _invoiceItemList;
        private long elementKey;
        private bool IsExpand = false;
        private IItemTransferValidator validator;
        private int TrnkyForOut = 1, TrnKyForIn = 1;
        ItemTransfer _find_itm_trnsfer;
        private int total_scanned_session = 0;
        private bool FindTransferShown = false;
        private bool IsbukeySelected;
        private BLUIElement findTransferUI;
        private TransferOpenRequest trnOpReq;
        private string _serialNo = "";
        private bool IsSerialNoValidationShown = false;
        private string TrnNoText = "";
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

            _itmTransfer.ElementKey = elementKey;

        }

        private void InitializeItemtransfer()
        {
            _itmTransfer = new ItemTransfer();
            _afterSaveHeaderResponse = new ItemTransfer();

            trnOpReq = new TransferOpenRequest();
            _find_itm_trnsfer = new ItemTransfer();
            _invoiceRequest = new LNDInvoice();
            _invoiceItemList = new List<ItemTransferLineItem>();
            validator = new ItemTransferValidator(_itmTransfer);
            TrnNoText = "New";
        }
        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            //AppSettings.RefreshTopBar("Scan Invoice-" + TrnNoText);
            appStateService._AppBarName = "Scan Invoice Transfer";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #region ui event
        private void OnEditClick(UIInterectionArgs<object> args)
        {
            UIStateChanged();
        }
        private async void OnNewClick(UIInterectionArgs<object> args)
        {
            InitializeItemtransfer();
            validator.UserMessages.AlertMessages.Clear();
            await SetValue("TrnNo", "");
            _objectHelpers["ScanBox"].ResetToInitialValue();
            this.ToggleEditability("FromLocation", true);
            this.ToggleEditability("Save", true);
            this.ToggleEditability("ToLocation", true);
            _objectHelpers["TotalScan"].ResetToInitialValue();
            this.total_scanned_session = 0;
            UIStateChanged();
        }
        //private async void OnSaveClick(UIInterectionArgs<object> args)
        //{
        //    //AppSettings.Loading(true);
        //    showAlert = false;
        //    validator.UserMessages.AlertMessages.Clear();

        //    total_scanned_session = 0;
        //    UIStateChanged();

        //    if (_itmTransfer != null)
        //    {
        //        _itmTransfer.SerialNoList.Clear();


        //            if (_itmTransfer.FromTransactionKey == 1)
        //            {
        //                _itmTransfer.LocationWiseSerialNoValidations = await _itemTransferManager.TransferValidator(_itmTransfer);

        //                validator = new ItemTransferValidator(_itmTransfer);
        //                if (validator != null && validator.CanSaveOrUpdateItemTransfer())
        //                {
        //                    await saveTrnHeaderFrom();
        //                    await saveTrnHeaderTo();
        //                    await ItmSave();

        //                    trnOpReq.TrnKy = TrnkyForOut;
        //                    await LoadTransfer(trnOpReq);
        //                }
        //                else
        //                {
        //                    IsSerialNoValidationShown = true;
        //                    UIStateChanged();
        //                }

        //            }
        //            else
        //            {
        //                await UpdateHeaderofItmTransferOut();
        //                await UpdateHeaderofItmTransferIn();
        //                await UpdateItemsofItmTransfer();

        //                trnOpReq.TrnKy = TrnkyForOut;
        //                //await RefreshTransfer();
        //                await LoadTransfer(trnOpReq);
        //            }


        //        }





        //    //AppSettings.Loading(false);
        //    UIStateChanged();
        //}
        private async void OnSaveClick(UIInterectionArgs<object> args)
        {
            
            total_scanned_session = 0;
            UIStateChanged();

            if (_itmTransfer != null && _itmTransfer.ScanItemTransferLineItem.Count() > 0)
            {
                // _itmTransfer.SerialNoList.Clear();

                if (_itmTransfer.FromTransactionKey == 1)
                {
                    //TrnkyForOut=await saveTrnHeaderFrom();
                    //TrnKyForIn=await saveTrnHeaderTo();
                    //await ItmSave();

                    foreach (LNDInvoice singleInvoice in _itmTransfer.Invoices)
                    {
                        singleInvoice.ElementKey = (int)elementKey;
                        singleInvoice.Location = _itmTransfer.FromLocation;
                        _itmTransfer.LocationWiseSerialNoValidations = await _itemTransferManager.InvoiceTransferValidator(singleInvoice);
                        if (_itmTransfer.LocationWiseSerialNoValidations.HasError)
                            break;
                    }

                    validator = new ItemTransferValidator(_itmTransfer);

                    if (validator != null && validator.CanSaveOrUpdateItemTransfer())
                    {
                        this.appStateService.IsLoaded = true;
                        _itmTransfer.ElementKey = elementKey;
                        TrnkyForOut = await _itemTransferManager.CreateItemTransfer(_itmTransfer);
                        
                        this.appStateService.IsLoaded = false;
                        if (!_itemTransferManager.IsExceptionthrown())
                        {
                            
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Item Transfer has been  Saved Successfully", Severity.Success);

                            if (trnOpReq != null)
                            {
                                trnOpReq.TrnKy = TrnkyForOut;
                                await LoadTransfer(trnOpReq);
                            }
                        }
                        else
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Error Occured", Severity.Error);
                        }
                    }
                    else
                    {
                       
                        IsSerialNoValidationShown = true;
                        UIStateChanged();
                    }



                }
                else
                {

                    foreach (LNDInvoice singleInvoice in _itmTransfer.Invoices)
                    {
                        singleInvoice.ElementKey = (int)elementKey;
                        singleInvoice.Location = _itmTransfer.FromLocation;

                        if (singleInvoice.IsJustScanned)
                        {
                            _itmTransfer.LocationWiseSerialNoValidations = await _itemTransferManager.InvoiceTransferValidator(singleInvoice);
                            if (_itmTransfer.LocationWiseSerialNoValidations.HasError)
                                break;
                        }

                    }
                    validator = new ItemTransferValidator(_itmTransfer);
                    if (validator != null && validator.CanSaveOrUpdateItemTransfer())
                    {
                        
                        await EditItmTransfer();
                        
                    }
                    else
                    {
                        
                        IsSerialNoValidationShown = true;
                        UIStateChanged();
                    }

                }

                UIStateChanged();
            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Please Select at least one item", Severity.Error);
            }
        }
        private void OnSaveNewClick(UIInterectionArgs<object> args)
        {
            validator.UserMessages.AlertMessages.Clear();
            UIStateChanged();
        }
        private async void OnPrintClick(UIInterectionArgs<object> args)
        {
            UIStateChanged();
        }
        private async void OnFindClick(UIInterectionArgs<object> args)
        {
            await ShowFindTransferWindow();
            UIStateChanged();
        }
        private async void OnCancelClick(UIInterectionArgs<object> args)
        {
            InitializeItemtransfer();
            validator.UserMessages.AlertMessages.Clear();
            await SetValue("TrnNo", "");
            _objectHelpers["ScanBox"].ResetToInitialValue();
            this.ToggleEditability("FromLocation", true);
            this.ToggleEditability("Save", true);
            this.ToggleEditability("ToLocation", true);
            _objectHelpers["TotalScan"].ResetToInitialValue();
            this.total_scanned_session = 0;
            UIStateChanged();
        }
        private void OnTrnNoClick(UIInterectionArgs<string> args)
        {
            _itmTransfer.TransactionNumber = args.DataObject;
            UIStateChanged();
        }
        private void OnDateClick(UIInterectionArgs<DateTime?> args)
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
            if (!string.IsNullOrEmpty(_serialNo)) { await Scan(); }
            
            _serialNo = "";
            UIStateChanged();

        }
        private async void OnAdornmentClick_ScanBox(UIInterectionArgs<object> args)
        {
            if (!string.IsNullOrEmpty(_serialNo)) { await Scan(); }
            _serialNo = "";
            UIStateChanged();
        }

        private async void OnSerialNoScanByType(UIInterectionArgs<string> args)
        {
            _serialNo = args.DataObject;
            UIStateChanged();
        }
        #endregion

        #region scan invoice 
        private async Task Scan()
        {
            
            validator.UserMessages.AlertMessages.Clear();

            validator = new ItemTransferValidator(_itmTransfer);

            if (validator.CanOpenScan())
            {
                //if (string.IsNullOrEmpty(_serialNo))
                //{
                //    await Task.Delay(1);
                //    var parameters = new DialogParameters
                //    {


                //    };
                //    DialogOptions options = new DialogOptions();
                //    var dialog = _dialogService.Show<QrDialog>("QR Scanner", parameters, options);
                //    DialogResult dialogResult = await dialog.Result;

                //    if (!dialogResult.Cancelled)
                //    {
                //        if (dialogResult.Data != null)
                //        {
                //            _serialNo = dialogResult.Data as string;
                //            await LoadGrid(_serialNo);

                            
                //        }
                //    }
                //}
                //else
                //{
                    await LoadGrid(_serialNo);
                //}
            }
            else
            {
                IsSerialNoValidationShown = true;
                
            }
            _objectHelpers["ScanBox"].ResetToInitialValue();
            UIStateChanged();
        }
        private async Task LoadGrid(string serial_num)
        {
            if (!_itmTransfer.SerialNoList.Contains(serial_num))
            {
                //_itmTransfer.SerialNoList.Add(serial_num);
                
                await GetGridData(serial_num);

                UIStateChanged();
            }
            else
            {
                ShowScannedValidations("This Invoice is already scanned");

                UIStateChanged();
            }
        }
        private async Task GetGridData(string _serialNo)
        {
            try
            {
                this.appStateService.IsLoaded = true;
                if (_itmTransfer != null)
                {
                    _itmTransfer.ValidationMessages.Clear();

                    _invoiceRequest.ElementKey = (int)elementKey;
                    _invoiceRequest.SerialNumber.SerialNumber = _serialNo;
                    if (_itmTransfer.FromLocation != null){ _invoiceRequest.Location = _itmTransfer.FromLocation; }
                        
                    _invoiceItemList.Clear();
                    _invoiceItemList = await _itemTransferManager.GetInvoiceData(_invoiceRequest);

                    if (_invoiceItemList != null && _invoiceItemList.Count() > 0)
                    {
                        _itmTransfer.SelectedInvoice.LocationWiseInvoiceSerialNoValidation= _invoiceItemList.FirstOrDefault().LocationWiseitemSerialNoValidation;
                        if (_itmTransfer.SelectedInvoice.LocationWiseInvoiceSerialNoValidation.HasError)
                        {
                            this.appStateService.IsLoaded = false;
                            
                            ShowScannedValidations(_itmTransfer.SelectedInvoice.LocationWiseInvoiceSerialNoValidation.Message);
                            UIStateChanged();
                        }
                        else {
                            total_scanned_session++;

                            await SetValue("ScanBox", _serialNo);
                            await SetValue("TotalScan", total_scanned_session);

                            _itmTransfer.SelectedInvoice.IsJustScanned = true;
                            _itmTransfer.SelectedInvoice.IsActive = 1;
                            _itmTransfer.SelectedInvoice.InvoiceNo = _invoiceItemList.FirstOrDefault().InvoiceNo;
                            _itmTransfer.SelectedInvoice.TransactionNumber = _invoiceItemList.FirstOrDefault().TrnNo;
                            _itmTransfer.SelectedInvoice.BranchCode = _invoiceItemList.FirstOrDefault().BranchCode;
                            _itmTransfer.SelectedInvoice.ServiceName = _invoiceItemList.FirstOrDefault().ServiceName;
                            _itmTransfer.SelectedInvoice.DeliveryDate = !string.IsNullOrEmpty(_invoiceItemList.FirstOrDefault().DeliveryDate) ? DateTime.Parse(_invoiceItemList.FirstOrDefault().DeliveryDate) : DateTime.Now;
                            _itmTransfer.SelectedInvoice.DeliveryType = _invoiceItemList.FirstOrDefault().DeliveryType;
                            _itmTransfer.SelectedInvoice.DlryTypColour = _invoiceItemList.FirstOrDefault().DlryTypColour;
                            _itmTransfer.SelectedInvoice.SerialNumber.SerialNumberKey = _invoiceItemList.FirstOrDefault().HdrSerNoKy;
                            _itmTransfer.SelectedInvoice.LineItems = _invoiceItemList;
                            _itmTransfer.SelectedInvoice.Location = _itmTransfer.FromLocation;

                            _itmTransfer.Invoices.Add(_itmTransfer.SelectedInvoice);
                            _itmTransfer.SelectedInvoice = new LNDInvoice();
                            _itmTransfer.SerialNoList = await _itemTransferManager.GetInvoiceSerialNoList(_invoiceRequest);


                            _itmTransfer.ScanItemTransferLineItem.AddRange(_invoiceItemList);
                            _itmTransfer.ScanItemTransferLineItem.ForEach(x => x.isActive = 1);

                            UIStateChanged();
                            this.appStateService.IsLoaded = false;
                        }
                    }
                    else
                    {
                        this.appStateService.IsLoaded = false;
                        ShowScannedValidations("There are no items in this invoice or Invalid Serial Number");
                        UIStateChanged();
                    }
                }
                
            }
            catch (Exception e)
            {
                //_itmTransfer.ValidationMessages.Add("something went wrong,please check again");
                //validator.AddValidationErrors();
                ShowScannedValidations("something went wrong,please check again");
            }

        }
        private async void ShowScannedValidations(string msg)
        {
            _itmTransfer.ValidationMessages.Add(msg);
            validator = new ItemTransferValidator(_itmTransfer);
            validator.AddValidationErrors();
            IsSerialNoValidationShown = true;
        }
        private async Task ShowFindTransferWindow()
        {
            HideAllPopups();
            FindTransferShown = true;
            UIStateChanged();
            await Task.CompletedTask;
        }
        private async void DeleteRow(int index, string invoiceNo, string serNo)
        {
            var parameters = new DialogParameters
            {
                ["ContentText"] = "Are you sure to delete this invoice?"
            };
            DialogOptions options = new DialogOptions();
            var dialog = _dialogService.Show<DeleteConfirmation>("Confirm", parameters, options);
            DialogResult dialogResult = await dialog.Result;

            if (!dialogResult.Cancelled)
            {
                if (_itmTransfer.Invoices[index].IsJustScanned)
                {
                    _itmTransfer.Invoices.RemoveAt(index);
                    _itmTransfer.ScanItemTransferLineItem.RemoveAll(x => x.InvoiceNo == invoiceNo);
                    _itmTransfer.SerialNoList.Remove(serNo);
                }
                else
                {
                    _itmTransfer.Invoices[index].IsActive = 0;
                    _itmTransfer.ScanItemTransferLineItem.Where(x => x.InvoiceNo == invoiceNo).ToList().ForEach(x => x.isActive = 0);
                }
                _itmTransfer.SerialNoList.Remove(serNo);
                if (total_scanned_session > 0)
                    total_scanned_session--;
                total_scanned_session = 0;
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
            IsSerialNoValidationShown = false;
            UIStateChanged();
        }
        private async void OnOpenTransferClick(TransferOpenRequest request)
        {
            HideAllPopups();
            InitializeItemtransfer();
            this.appStateService.IsLoaded = true;
            await LoadTransfer(request);
            this.appStateService.IsLoaded = false;
            UIStateChanged();
        }

        private async Task LoadTransfer(TransferOpenRequest request)
        {
            request.ObjKy = elementKey;
            _find_itm_trnsfer = await _itemTransferManager.RefreshForm(request);


            if (_find_itm_trnsfer != null && _find_itm_trnsfer.ScanItemTransferLineItem != null && _find_itm_trnsfer.ScanItemTransferLineItem.Count() > 0)
            {
                _itmTransfer = new ItemTransfer();
                _itmTransfer.CopyFrom(_find_itm_trnsfer);
                if (_itmTransfer.SerialNoList == null) { _itmTransfer.SerialNoList = new List<string>(); }

                TrnNoText = _find_itm_trnsfer.TransactionNumber;
                TrnkyForOut = _find_itm_trnsfer.FromTransactionKey;
                await SetValue("TrnNo", _find_itm_trnsfer.TransactionNumber);

                _itmTransfer.FromBuKy = _find_itm_trnsfer.ScanItemTransferLineItem.Where(x => x.isActive == 1).FirstOrDefault().FromBuKy;
                _itmTransfer.ToBuKy = _find_itm_trnsfer.ScanItemTransferLineItem.Where(x => x.isActive == 1).FirstOrDefault().ToBuKy;

                if (_itmTransfer!=null ) 
                {
                    if (_itmTransfer.SerialNoList != null)
                    {
                        _itmTransfer.SerialNoList.Clear();
                    }
                    _itmTransfer.Invoices = new();

                    _itmTransfer.SetInvoice();

                    if (!_itmTransfer.CanUpdateToLocation)
                    {
                        this.ToggleEditability("Save", false);
                        this.ToggleEditability("ToLocation", false);
                    }

                }

                
            }
            this.ToggleEditability("FromLocation", false);
            UIStateChanged();


        }

        private void ShowBtnPress(LNDInvoice inv)
        {
            inv.ShowDetails = !inv.ShowDetails;
        }

        private async Task EditItmTransfer()
        {
            this.appStateService.IsLoaded = true;
            _itmTransfer.FromTransactionKey = _find_itm_trnsfer.FromTransactionKey;
            _itmTransfer.ToTransactionKey = _find_itm_trnsfer.ToTransactionKey;
            _itmTransfer.ElementKey = elementKey;
            _itmTransfer.FromTmStmp = _find_itm_trnsfer.FromTmStmp;
            _itmTransfer.ToTmStmp = _find_itm_trnsfer.ToTmStmp;

            await _itemTransferManager.UpdateItemTransfer(_itmTransfer);

            if (!_itemTransferManager.IsExceptionthrown())
            {
                this.appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Item Transfer has been  updated Successfully", Severity.Success);
                trnOpReq.TrnKy = _itmTransfer.FromTransactionKey;
                await LoadTransfer(trnOpReq);
                this.appStateService.IsLoaded = false;
            }
            else
            {
                this.appStateService.IsLoaded = false;
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Error Occured", Severity.Error);
            }


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

        #region hold

        //private async Task<int> saveTrnHeaderFrom()
        //{
        //    _afterSaveHeaderResponse = await _itemTransferManager.SaveTrnHeaderFromLoc(_itmTransfer);
        //    TrnkyForOut = _afterSaveHeaderResponse.FromTransactionKey;
        //    await Task.CompletedTask;
        //    UIStateChanged();

        //    return TrnkyForOut;
        //}
        //private async Task<int> saveTrnHeaderTo()
        //{
        //    _itmTransfer.HdrTrfLnkKy = TrnkyForOut;
        //    _afterSaveHeaderResponse = await _itemTransferManager.SaveTrnHeaderToLoc(_itmTransfer);
        //    TrnKyForIn = _afterSaveHeaderResponse.ToTransactionKey;
        //    await Task.CompletedTask;
        //    UIStateChanged();

        //    return TrnKyForIn;
        //}
        //private async Task ItmSave()
        //{
        //    foreach (var list_itm in _itmTransfer.ScanItemTransferLineItem)
        //    {
        //        list_itm.ObjectKey = (int)elementKey;
        //        list_itm.LineNumber = _itmTransfer.ScanItemTransferLineItem.IndexOf(list_itm) + 1;
        //        list_itm.TransactionKey = TrnkyForOut;
        //        list_itm.LocationKey = _itmTransfer.FromLocation.CodeKey;
        //        list_itm.PreItmTrnKy = list_itm.OriItmTrnKy;
        //        list_itm.OurCode = "TRFOUTSCN3";


        //        list_itm.ReturnVal = await _itemTransferManager.SaveLine(list_itm);

        //        list_itm.TransactionKey = TrnKyForIn;
        //        list_itm.LocationKey = _itmTransfer.ToLocation.CodeKey;
        //        list_itm.OurCode = "TRFINSCN3";
        //        list_itm.PreItmTrnKy = list_itm.ReturnVal;
        //        list_itm.ItmTrnTrfLnkKy = list_itm.ReturnVal;

        //        await _itemTransferManager.SaveLine(list_itm);
        //    }

        //    await FifoPosting();
        //    await Task.CompletedTask;
        //}
        //private async Task<int> UpdateHeaderofItmTransferOut()
        //{
        //    _itmTransfer.FromTransactionKey = _find_itm_trnsfer.FromTransactionKey;
        //    _itmTransfer.ElementKey = elementKey;
        //    _itmTransfer.FromTmStmp = _find_itm_trnsfer.FromTmStmp;

        //    await _itemTransferManager.UpdateHeaderForOut(_itmTransfer);

        //    return _itmTransfer.FromTransactionKey;
        //}

        //private async Task UpdateHeaderofItmTransferIn()
        //{
        //    _itmTransfer.ToTransactionKey = _find_itm_trnsfer.ToTransactionKey;
        //    _itmTransfer.ElementKey = elementKey;
        //    _itmTransfer.ToTmStmp = _find_itm_trnsfer.ToTmStmp;

        //    await _itemTransferManager.UpdateHeaderForIn(_itmTransfer);
        //}

        //private async Task UpdateItemsofItmTransfer()
        //{

        //    if (_itmTransfer.ScanItemTransferLineItem.Count() > 0)
        //    {
        //        foreach (var list_itm in _itmTransfer.ScanItemTransferLineItem)
        //        {
        //            list_itm.ObjectKey = (int)elementKey;

        //            if (list_itm.FromItemTrnKey > 1 && list_itm.ToItemTrnKey > 1)
        //            {
        //                list_itm.LineNumber = _itmTransfer.ScanItemTransferLineItem.IndexOf(list_itm) + 1;
        //                list_itm.TransactionKey = _find_itm_trnsfer.FromTransactionKey;
        //                list_itm.LocationKey = _itmTransfer.FromLocation.CodeKey;
        //                list_itm.OurCode = "TRFOUTSCN3";

        //                await _itemTransferManager.UpdateLineForOut(list_itm);

        //                list_itm.TransactionKey = _find_itm_trnsfer.ToTransactionKey;
        //                list_itm.LocationKey = _itmTransfer.ToLocation.CodeKey;
        //                list_itm.OurCode = "TRFINSCN3";

        //                await _itemTransferManager.UpdateLineForIn(list_itm);
        //            }
        //            else
        //            {
        //                list_itm.LineNumber = _itmTransfer.ScanItemTransferLineItem.IndexOf(list_itm) + 1;
        //                list_itm.TransactionKey = _find_itm_trnsfer.FromTransactionKey;
        //                list_itm.LocationKey = _itmTransfer.FromLocation.CodeKey;
        //                list_itm.PreItmTrnKy = list_itm.OriItmTrnKy;
        //                list_itm.OurCode = "TRFOUTSCN3";

        //                list_itm.ReturnVal = await _itemTransferManager.SaveLine(list_itm);

        //                list_itm.TransactionKey = _find_itm_trnsfer.ToTransactionKey;
        //                list_itm.LocationKey = _itmTransfer.ToLocation.CodeKey;
        //                list_itm.OurCode = "TRFINSCN3";
        //                list_itm.PreItmTrnKy = list_itm.ReturnVal;
        //                list_itm.ItmTrnTrfLnkKy = list_itm.ReturnVal;

        //                await _itemTransferManager.SaveLine(list_itm);
        //            }

        //        }

        //    }

        //    await Task.CompletedTask;
        //}

        //private async Task FifoPosting()
        //{
        //    _itmTransfer.FromTransactionKey = TrnkyForOut;
        //    _itmTransfer.ElementKey = elementKey;
        //    await _itemTransferManager.FIFOPosting(_itmTransfer);

        //}
        #endregion
    }
}
