using BL10.CleanArchitecture.Domain.Entities.ItemProfleMobile;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.MasterData;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Settings;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.Popups.MasterDetailPopup;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components.TelerikComponents.Grid;
using BlueLotus360.Com.UI.Definitions.MB.Shared.Components;
using BlueLotus360.Com.UI.Definitions.Pages.SetUp.Master_Data.Item_Profile_Simple_V2.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.Blazor.Components;
using BlueLotus360.CleanArchitecture.Client.Infrastructure.Reports.Telerik;
using MudBlazor;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using static BlueLotus360.Com.Infrastructure.OrderPlatforms.PickMe.PickmeEntity;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order; //telerik
using static System.Formats.Asn1.AsnWriter;

namespace BlueLotus360.Com.UI.Definitions.Pages.SetUp.Master_Data.Item_Profile_Simple_V2
{
    public partial class ItemProfileSimpleV2
    {
        #region parameter

        private BLUIElement formDefinition;
        private BLTransaction transaction = new();

        private ItemSelectListRequest itemSelectListRequest;  //grid
        private ItemSelectList insertRequest;  //insert

        private UIBuilder _refBuilder;
        private AddNewAddress _refNewAddressCreation;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;  //grid
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private BLItemProfileSimpleInsertDetails _bLItemProfileSimpleInsertDetails; //insert
        private BLTelGrid<ItemSelectList> _blTb;    //Gird

        private IList<ItemSelectList> gridDetails;
        private ItemSelectList currentDetails; 

        private bool ShowInsertDetails = false;  //insert 
        private bool isTableLoading = false;
        private bool showsgrid = true;

        // for telerik
        private bool ReportShown = false; 
        private TerlrikReportOptions _ShowTelReport;  
        CompletedUserAuth auth; 
       // [Parameter] public BLUIElement UIScope { get; set; } 

        BLUIElement insertmodalUIElement, gridUIElement;

        long elementKey;

        #endregion


        #region General

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            //telerik
            _ShowTelReport = new TerlrikReportOptions();
            auth = await _authenticationManager.GetUserInformation();
            _ShowTelReport.ReportParameters = new Dictionary<string, object>();

            //parameters for telerik report
            transaction.TransactionKey = 6452093; 

            elementKey = 1;

            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                insertmodalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("InsertBasicDetails")).FirstOrDefault();  //insert
                gridUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("DetailsTable")).FirstOrDefault();      //grid

                
                //grid
                if (gridUIElement != null)
                {
                    gridUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == gridUIElement.ElementKey).ToList();
                }

                //insert section
                if (insertmodalUIElement != null)
                {
                    insertmodalUIElement.Children = formDefinition.Children.Where(x => x.ParentKey == insertmodalUIElement.ElementKey).ToList();
                }

            }

            formDefinition.IsDebugMode = true;

            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();    //grid
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();


            HookInteractions();
            RefreshGrid();


            //grid
            itemSelectListRequest.ElementKey = elementKey;
          
            gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest); //request API to getItemList /(IItemProfileManager)


        }

        private async void InitNewLine()
        {
            insertRequest = new ItemSelectList();
            _objectHelpers["InputItemCode"].ResetToInitialValue();
            _objectHelpers["InputItemName"].ResetToInitialValue();
            _objectHelpers["InsertItemType"].ResetToInitialValue();
            _objectHelpers["InsertUnit"].ResetToInitialValue();
            StateHasChanged();
        }

        private void RefreshGrid()
        {
            itemSelectListRequest = new ItemSelectListRequest();
            insertRequest = new ItemSelectList();
            currentDetails = new ItemSelectList();
            _refNewAddressCreation = new AddNewAddress();
        }

        
        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);
            _interactionLogic = helper.GenerateEventCallbacks();
            appStateService._AppBarName = "Item Profile Simple V2";
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }
        #endregion

        #region Item Related Events

        //add new customer
        private async void OnAddCustomer(UIInterectionArgs<object> args)
        {
            IDictionary<string, object> ParamDictionary = new Dictionary<string, object>();
            ParamDictionary.Add("InitiatorElement", args.InitiatorObject);
            ParameterView values = ParameterView.FromDictionary(ParamDictionary);
            await _refNewAddressCreation.SetParametersAsync(values);
            _refNewAddressCreation.ShowPopUp();

            UIStateChanged();
        }

        //add new item btn
        private async void OnCreateNewItem(UIInterectionArgs<object> args)
        {
            showsgrid = false;
            ShowInsertDetails = true;

            UIStateChanged();

        }

        //Print button for display telerik report
        private async void OnReportPrint(UIInterectionArgs<object> args)
        {

            if (transaction.TransactionKey > 1) 
            {
                if (_ShowTelReport != null && _ShowTelReport.ReportParameters != null)  
                {
                    _ShowTelReport.ReportParameters.Clear();
                    _ShowTelReport.ReportName = "Invoice-Retail.trdp";
                    _ShowTelReport.ReportParameters.Add("Cky", auth.AuthenticatedCompany.CompanyKey);
                    _ShowTelReport.ReportParameters.Add("UsrKy", auth.AuthenticatedUser.UserKey);
                    _ShowTelReport.ReportParameters.Add("TrnKy", transaction.TransactionKey);
                    _ShowTelReport.ReportParameters.Add("ObjKy", elementKey); 

                    ReportShown = true;
                }

            }
            else
            {
                _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                _snackBar.Add("Invalid Request. Please select a record.", Severity.Error);
            }

            StateHasChanged();
        }

        #endregion


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

        //insert section
        #region Insert Item Events

        private async void OnInsertItemCode(UIInterectionArgs<string> args)
        {
            insertRequest.ItemCode = args.DataObject;
            UIStateChanged();
        }

        private async void OnInsertItemName(UIInterectionArgs<string> args)
        {
            insertRequest.ItemName = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertItemType(UIInterectionArgs<CodeBaseResponse> args)
        {
            insertRequest.ItemType = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertItemUnit(UIInterectionArgs<UnitResponse> args)
        {
            insertRequest.ItemUnit = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertIsActive(UIInterectionArgs<bool> args)
        {
            insertRequest.IsAct = args.DataObject;
            UIStateChanged();
        }
        private async void OnInsertIsApprove(UIInterectionArgs<bool> args)
        {
            insertRequest.IsApprove = args.DataObject;
            UIStateChanged();
        }

        //func of save btn for new item and  update item
        private async void OnInsertItemSave(UIInterectionArgs<object> args)
        {
            this.appStateService.IsLoaded = true;
            UIStateChanged();

            if (insertRequest.IsInEditMode)
            {
                await _itemProfileMobileManager.UpdateItem(insertRequest);   //request API to updateItem
                insertRequest.IsInEditMode = false;
                gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest);

                ShowInsertDetails = false;
                showsgrid = true;

                RefreshGrid();
            }
            else
            {
                await _itemProfileMobileManager.InsertItem(insertRequest);  //insert reuquest
                gridDetails = await _itemProfileMobileManager.GetItemProfileList(itemSelectListRequest); // request API to getItemList after insertion 

                InitNewLine();

                ShowInsertDetails = false;
                showsgrid = true;
                
            }
            this.appStateService.IsLoaded = false;
            UIStateChanged();

        }

        //back btn from insert section
        private async void OnInsertBack(UIInterectionArgs<object> args)
        {
            showsgrid = true;
            ShowInsertDetails = false;

            RefreshGrid();
            UIStateChanged();

        }
        #endregion

        //Delete action
        private async void DeleteHandler(UIInterectionArgs<object> args)
        {
            int index= gridDetails.ToList().IndexOf((ItemSelectList)args.DataObject);
            if (index!=-1)
            {
                gridDetails[index].IsAct = false;
                await _itemProfileMobileManager.UpdateItem((ItemSelectList)args.DataObject);  // request API for the update after deleted(isActive=false)
            }
            if (_blTb != null)
            {
                _blTb.Refresh();
            }

            UIStateChanged();
        }

        //update data using insert section
        #region Update Item Events 

        //func for "..." update btn in a table row
        private async void UpdateHandler(UIInterectionArgs<object> args)
        {
            showsgrid = false;
            ShowInsertDetails = true; //to update an item

            currentDetails = (ItemSelectList)args.DataObject;
            currentDetails.IsInEditMode = true;
            insertRequest.CopyFrom(currentDetails);
            
            //insert section(set values for update)
            await SetValue("InputItemCode", currentDetails.ItemCode);
            await SetValue("InputItemName", currentDetails.ItemName);
            await SetValue("InsertItemType", currentDetails.ItemType.CodeKey);
            await SetValue("InsertUnit", currentDetails.ItemUnit.UnitKey);
            await SetValue("InsertIsAct", currentDetails.IsAct);
            await SetValue("InsertIsApprove", currentDetails.IsApprove);

            UIStateChanged();
        }

        #endregion



        #region object helpers

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

        private void RefreshComponent(string name)
        {
            IBLUIOperationHelper helper;

            if (_objectHelpers.TryGetValue(name, out helper))
            {
                helper.Refresh();
                UIStateChanged();
            }
        }

        #endregion
    }
}
