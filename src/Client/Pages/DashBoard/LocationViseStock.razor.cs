using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Dashboard;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Shared.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using ApexCharts;
using BlueLotus360.Com.Client.Shared.Components.Chart;
using System.Linq;
using BlueLotus360.CleanArchitecture.Application.Validators.Dashboard.LocationWiseStockValidator;
using BlueLotus360.CleanArchitecture.Application.Validators.Dashboard.LocationWiseStock;
using BlueLotus360.Com.Client.Shared.Components.TelerikComponents.Grid;
using System.Reflection;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.Com.Client.Settings;

namespace BlueLotus360.Com.Client.Pages.DashBoard
{
    public partial class LocationViseStock
    {
        #region parameter

        private BLUIElement formDefinition;
        private LocationViseStockRequest location_vise_stocks;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;

        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;
        private ApexChart<LocationViseStockResponse> _detailsChart;
        private SelectedData<LocationViseStockResponse> selectedData;
        private IList<LocationViseStockResponse> chartDetails;
        private IList<CodeBaseResponse> locations=new List<CodeBaseResponse>();
        private IList<ItemResponse> items = new List<ItemResponse>();
        EmptyChart empty_chart;
        string[] headings = { "Location", "Unit", "Sales Price", "Vat Inclusive Price","Remarks","Special Information", "Qty" };
        private ILocationWiseStockValidator validator;
        private BLTelGrid<LocationViseStockResponse> _blTb;
        BLUIElement locWiseStockTable;
        #region flags
        private bool isChartLoading;
        private bool showAlert;
        private bool isExpansionPanelOpen;
        #endregion

        #endregion

        #region General

        protected override void OnParametersSet()
        {
            
            base.OnParametersSet();
        }
 
        protected override async Task OnInitializedAsync()
        {
            

            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
                //formDefinition.IsTelerikUI = true;

                locWiseStockTable = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("LocWiseStockTable")).FirstOrDefault();
            }
            if (locWiseStockTable != null)
            {
                locWiseStockTable.Children = formDefinition.Children.Where(x => x.ParentKey == locWiseStockTable.ElementKey).ToList();
            }

            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            HookInteractions();
            empty_chart = new();
            RefreshGrid();
            RefreshChart();
            location_vise_stocks.ElementKey = elementKey;

        }

        private async void RefreshGrid()
        {
            location_vise_stocks = new LocationViseStockRequest();
            location_vise_stocks.ElementKey = formDefinition.ElementKey;
            validator = new LocationWiseStockValidator(location_vise_stocks);
            await Task.CompletedTask;
        } 

        private async void RefreshChart()
        {
            _detailsChart = new();
            chartDetails= new List<LocationViseStockResponse>();
            chartDetails.Clear();
            await Task.CompletedTask;
        }


        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            AppSettings.RefreshTopBar("Location Wise Stock");
        }

        private void UIStateChanged()
        {

            this.StateHasChanged();
        }
        #endregion

        #region ui events

        private void OnSelectedDateChange(UIInterectionArgs<DateTime?> args)
        {
            location_vise_stocks.AsAtDate = args.DataObject;
            UIStateChanged();
        }
        private void OnVatChange(UIInterectionArgs<decimal> args)
        {
            location_vise_stocks.Vat=args.DataObject;
            UIStateChanged();
        }
        private void OnSelectedLocationChanged(UIInterectionArgs<CodeBaseResponse>args)
        {
            location_vise_stocks.Location = args.DataObject;
            UIStateChanged();
        }
        private async void SelectedLocation_AfterComboLoaded(UIInterectionArgs<IList<CodeBaseResponse>> args)
        {
            locations = args.DataObject;
            await Task.CompletedTask;
            UIStateChanged();
        }
        private void OnSelectedItemChange(UIInterectionArgs<ItemResponse> args)
        {
            location_vise_stocks.Item=args.DataObject;
            
            UIStateChanged();
        }

        private async void SelectedItem_AfterComboLoaded(UIInterectionArgs<IList<ItemResponse>> args)
        {
            items = args.DataObject;
            await Task.CompletedTask;
            UIStateChanged();
        }

        private async void LoadChart(UIInterectionArgs<object> args)
        {
            selectedData = null;

            if (validator.CanLoadChart())
            {
                isChartLoading = true;
                chartDetails.Clear();

                if (showAlert)
                {
                    showAlert = false;
                }
                if (isExpansionPanelOpen)
                {
                    isExpansionPanelOpen = false;
                }

                chartDetails = await _dashboardManager.GetLocationViseStocks(location_vise_stocks);



                if (!_dashboardManager.IsExceptionthrown())
                {
                    _detailsChart?.SetRerenderChart();
                }
                else
                {
                    chartDetails.Clear();
                }

                isChartLoading = false;
                UIStateChanged();
            }
            else
            {
                showAlert = true;
                isExpansionPanelOpen = true;
            }

        }

        private void Cancel(UIInterectionArgs<object> args)
        {
            if (showAlert)
            {
                showAlert = false;
            }
            if (isExpansionPanelOpen)
            {
                isExpansionPanelOpen = false;
            }

            selectedData = null;
            RefreshGrid();

            chartDetails.Clear();
            _detailsChart?.SetRerenderChart();
            UIStateChanged();
        }
        #endregion

        #region Chart Loading



        //private void Refresh()
        //{
        //    _detailsChart?.SetRerenderChart();
        //    UIStateChanged();
        //}
        #endregion

        #region chart reigon
        private void DataPointsSelected(SelectedData<LocationViseStockResponse> selectedData)
        {
            this.selectedData = selectedData;
            _detailsChart?.SetRerenderChart();
        }
        #endregion

        #region Table

        #endregion


    }
}
