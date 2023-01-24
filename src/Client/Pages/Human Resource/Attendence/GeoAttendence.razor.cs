using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Shared.Components;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using System;
using MudBlazor;
using BrowserInterop.Geolocation;
using BrowserInterop.Extensions;
using System.Linq;
using BlueLotus360.Com.Client.Shared.Popups.HR;
using BlueLotus360.CleanArchitecture.Application.Validators.HR;
using BlueLotus360.Com.Client.Settings;

namespace BlueLotus360.Com.Client.Pages.Human_Resource.Attendence
{
    public partial class GeoAttendence 
    {
        #region parameter

        private BLUIElement formDefinition;
        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;
        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;

        private UserDetails _user;
        private MultiAtnAnlysis _multiAtnAnlysis;
       
        private MultiAtnAnlysis_Response _User_multiAtnAnlysis_Response;
        private IList<MultiAtnAnlysis_Response> _multiAtnAnlysis_Responses;
        private InShift _shift;
        private ManualAttendence _manualAttendance;
        private AddManualAdt _addManualAdt;
        private UserPermission _userPermission;

        private  bool fixedheader = true;
        private string headerName;

        private WindowNavigatorGeolocation geolocationWrapper;
        private GeolocationResult currentPosition;
        private List<GeolocationPosition> positioHistory = new List<GeolocationPosition>();

        private IHRValidator validator;
        private IHREditFormValidator validatorEdit;
        string alertMessage;
        private TimeSpan _totalHoursFromAttendence;
        private bool IsProcessing;
        private bool IsDataLoading;
        private bool showAlert;
        private bool hasUserPermissionForAdd;
        private bool hasUserPermissionForEdit;

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

            InitializeNewAttendence();

            HookInteractions();

            GetEmployee();

            _userPermission.ObjKy = Convert.ToInt32(elementKey);
            CheckUserPermissionForUsers(_userPermission);

            var window = await _jsRuntime.Window();
            var navigator = await window.Navigator();
            geolocationWrapper = navigator.Geolocation;

        }

        private void InitializeNewAttendence()
        {
           
            _user = new UserDetails();
            _userPermission=new UserPermission();   
            _multiAtnAnlysis =new MultiAtnAnlysis();
            _shift = new InShift();
            _manualAttendance = new ManualAttendence();
            _User_multiAtnAnlysis_Response = new MultiAtnAnlysis_Response();
            _manualAttendance.selectedAttendence = new UpdateAttendence();
            _addManualAdt = new AddManualAdt();
            _multiAtnAnlysis_Responses = new List<MultiAtnAnlysis_Response>();
        }

        private void HookInteractions()
        {
            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 
            AppSettings.RefreshTopBar("Geo Attendence");
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #endregion

        #region ui events
        private  async void DateClick(UIInterectionArgs<DateTime?> args)
        {
            _manualAttendance.Date = ((DateTime)args.DataObject).ToString("yyyy/MM/dd");
            _multiAtnAnlysis.FDt = _manualAttendance.Date;
            _multiAtnAnlysis.TDt = _manualAttendance.Date;

            if (_multiAtnAnlysis.FDt!=null&& DateTime.Compare(DateTime.Parse(_multiAtnAnlysis.FDt), DateTime.Now) != 0)
            {
                    ToggleEditability("InTime",false);
                    ToggleEditability("OutTime", false);
            }
            

            if (_user != null)
            {
                IsDataLoading = true;
                UIStateChanged();

                await GetExistingRecord();

                IsDataLoading = false;
                UIStateChanged();
            }

           
            UIStateChanged();
        }
        private void OnLocationChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            _manualAttendance.Location=args.DataObject;
            UIStateChanged();
        }

        private void OnInTimeClick(UIInterectionArgs<TimeSpan?> args)
        {

            _manualAttendance.selectedAttendence.InDtm = DateTime.Parse(DateTime.Now.Year+"/"+ DateTime.Now.Month+"/"+ DateTime.Now.Day +" "+ args.DataObject.Value.Hours+":"+ args.DataObject.Value.Minutes+":"+ args.DataObject.Value.Seconds);
            UIStateChanged();
        }
        private void OnOutTimeClick(UIInterectionArgs<TimeSpan?> args)
        {
            _manualAttendance.selectedAttendence.OutDtm = DateTime.Parse(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + args.DataObject.Value.Hours + ":" + args.DataObject.Value.Minutes + ":" + args.DataObject.Value.Seconds);
            
            UIStateChanged();
        }

        private void OnClickPutIn(UIInterectionArgs<TimeSpan?> args)
        {
            _addManualAdt.InDtm= DateTime.Parse(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + args.DataObject.Value.Hours + ":" + args.DataObject.Value.Minutes + ":" + args.DataObject.Value.Seconds);
            
            UIStateChanged();
        }
        private void OnClickPutOut(UIInterectionArgs<TimeSpan?> args)
        {
            _addManualAdt.OutDtm = DateTime.Parse(DateTime.Now.Year + "/" + DateTime.Now.Month + "/" + DateTime.Now.Day + " " + args.DataObject.Value.Hours + ":" + args.DataObject.Value.Minutes + ":" + args.DataObject.Value.Seconds);
            
            UIStateChanged();
        }
        private async void InClick(UIInterectionArgs<object> args)
        {
                IsProcessing = true;
                UIStateChanged();

                await PutIn();

                IsProcessing = false;
                UIStateChanged();

                if (_user != null)
                {
                    IsDataLoading = true;
                    UIStateChanged();

                    await GetExistingRecord();

                    IsDataLoading = false;
                    UIStateChanged();
                }
            

            
            UIStateChanged();
        }

        private async void OutClick(UIInterectionArgs<object> args)
        {
            IsProcessing = true;
            await PutOut();

            if (_user != null)
            {
                IsDataLoading = true;
                UIStateChanged();

                await GetExistingRecord();

                IsDataLoading = false;
                UIStateChanged();
            }


            IsProcessing = false;
            UIStateChanged();
        }

        private async void AddClick(UIInterectionArgs<object> args)
        {
            await ShowCreateAttendence();
            UIStateChanged();

        }

        #endregion

        #region attendence 
        private async void GetEmployee()
        {

            _user = await _hrManager.GetUserAsync();
            headerName = DateTime.Now.ToString("dd/MM/yyyyy") + "_" + _user.AdrFullNm;

            if(_user!=null)
            {
                IsDataLoading = true;
                UIStateChanged();

                await GetExistingRecord();

                IsDataLoading = false;
                UIStateChanged();
            }
                
        }

        private async void CheckUserPermissionForUsers(UserPermission permission)
        {
            _userPermission =await _hrManager.GetUserPermission(permission);

            if (_userPermission.IsAllowUpdate==1)
            {
                hasUserPermissionForEdit = true;
            }
            if (_userPermission.IsAllowAdd == 1)
            {
                hasUserPermissionForAdd = true;
            }
            UIStateChanged();
        }

        private async Task GetExistingRecord()
        {
            _totalHoursFromAttendence = TimeSpan.Zero;
            _multiAtnAnlysis.EmpKy = _user.AdrKy;
            _multiAtnAnlysis_Responses = await _hrManager.GetExistingRecordForDay(_multiAtnAnlysis);

            foreach (var itm in _multiAtnAnlysis_Responses)
            {
                //if (DateTime.Compare(new DateTime(itm.InDtm.Value.Year, itm.InDtm.Value.Month, itm.InDtm.Value.Day),new DateTime(1901,1,1)) ==0 || DateTime.Compare(new DateTime(itm.InDtm.Value.Year, itm.InDtm.Value.Month, itm.InDtm.Value.Day), new DateTime(0001, 1, 1)) == 0)
                //{
                //    itm.InDtm = null;
                //}
                //if (DateTime.Compare(new DateTime(itm.OutDtm.Value.Year, itm.OutDtm.Value.Month, itm.OutDtm.Value.Day), new DateTime(1901, 1, 1)) == 0 || DateTime.Compare(new DateTime(itm.OutDtm.Value.Year, itm.OutDtm.Value.Month, itm.OutDtm.Value.Day), new DateTime(0001, 1, 1)) == 0)
                //{
                //    itm.OutDtm = null;
                //}

                _totalHoursFromAttendence = _totalHoursFromAttendence.Add(itm.GetWorkHours());
            }
            UIStateChanged();
        }

        private async Task PutIn()
        {
            try
            {
                await GetGeolocation();
                _manualAttendance.EmpKy = _user.AdrKy;

                //after refresh can't use 
                _shift = await _hrManager.GetShift(_manualAttendance);

                if (!_hrManager.IsExceptionthrown())
                {
                    _manualAttendance.ShiftKy = _shift.ShiftKy;
                    _manualAttendance.IsHoliday = _shift.isHoliday;
                    _manualAttendance.Longitude = currentPosition.Location.Coords.Longitude;
                    _manualAttendance.Latitude = currentPosition.Location.Coords.Latitude;
                }



                if (_manualAttendance.ShiftKy != 0 && _manualAttendance.Latitude != 0 && _manualAttendance.Longitude != 0)
                {
                    _manualAttendance.IsIn = 1;
                    _manualAttendance.IsOut = 0;
                    _User_multiAtnAnlysis_Response = await _hrManager.InOut(_manualAttendance);   //after refresh can't use

                    if (!_hrManager.IsExceptionthrown() && _User_multiAtnAnlysis_Response.MultiAtnDetKy != 0)
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.BottomRight;
                        _snackBar.Add("Attendence is submited Successfully", Severity.Success);
                    }

                }
                else
                {
                    _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                    _snackBar.Add("An Error Occured", Severity.Error);
                }
            }
            catch(Exception ex)
            {

            }
             
           

             
        }

        private async Task PutOut()
        {
            try
            {
                _manualAttendance = new ManualAttendence();
                _shift = new InShift();
                _User_multiAtnAnlysis_Response = new MultiAtnAnlysis_Response();

                _manualAttendance.EmpKy = _user.AdrKy;


                if (_multiAtnAnlysis_Responses.Count() > 0)
                {

                    _User_multiAtnAnlysis_Response = _multiAtnAnlysis_Responses.LastOrDefault();

                    await GetGeolocation();

                    _manualAttendance.Location = _User_multiAtnAnlysis_Response.Location;
                    _manualAttendance.Longitude = currentPosition.Location.Coords.Longitude;
                    _manualAttendance.Latitude = currentPosition.Location.Coords.Latitude;
                    _manualAttendance.IsIn = 0;

                    //in time available & out time is null
                    if (_User_multiAtnAnlysis_Response.InDtm != null && _User_multiAtnAnlysis_Response.OutDtm == null)
                    {
                        _manualAttendance.IsHoliday = 0;
                        _manualAttendance.MultiAtnDetKy = _User_multiAtnAnlysis_Response.MultiAtnDetKy;
                        _manualAttendance.IsOut = 1;
                    }
                    
                    //in time available & out time is available 
                    else if (_User_multiAtnAnlysis_Response.InDtm != null && _User_multiAtnAnlysis_Response.OutDtm != null)
                    {
                        _shift = await _hrManager.GetShift(_manualAttendance);

                        if (!_hrManager.IsExceptionthrown())
                        {
                            _manualAttendance.ShiftKy = _shift.ShiftKy;
                            _manualAttendance.IsHoliday = _shift.isHoliday;

                        }

                        _manualAttendance.MultiAtnDetKy = 1;
                        _manualAttendance.IsOut = 0;
                        _manualAttendance.IsoutWithoutIn = 1;


                    }
                    else
                    {

                    }
                }
                else
                {

                }

                await _hrManager.InOut(_manualAttendance);
                 
            }
            catch (Exception ex)
            {

            }
           


        }

        private async Task ShowCreateAttendence()
        {
            try
            {
                BLUIElement modalUIElement;

                await GetGeolocation();
                _manualAttendance.EmpKy = _user.AdrKy;
                _addManualAdt.EmpKy = _user.AdrKy;
                _addManualAdt.Location = _manualAttendance.Location;
                _addManualAdt.Latitude = currentPosition.Location.Coords.Latitude;
                _addManualAdt.Longitude = currentPosition.Location.Coords.Longitude;
                //_addManualAdt.InDtm = DateTime.Now;
                //_addManualAdt.OutDtm = DateTime.Now;

                _shift = await _hrManager.GetShift(_manualAttendance);

                if (!_hrManager.IsExceptionthrown())
                {

                    _addManualAdt.ShiftKy = _shift.ShiftKy;
                    _addManualAdt.IsHoliday = _shift.isHoliday;

                }


                if (!_modalDefinitions.TryGetValue("CreateAttendencePopUp", out modalUIElement))
                {
                    modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("CreateAttendencePopUp")).FirstOrDefault();
                    if (modalUIElement != null && modalUIElement.Children.Count > 0)
                    {
                        _modalDefinitions.Add("CreateAttendencePopUp", modalUIElement);

                    }
                }

                if (modalUIElement != null)
                {
                    validator = new HRValidator(_addManualAdt);
                    var parameters = new DialogParameters
                    {
                        ["AttendenceRequest"] = _addManualAdt,
                        ["ModalUIElement"] = modalUIElement,
                        ["InteractionLogics"] = _interactionLogic,
                        ["ObjectHelpers"] = _objectHelpers,
                        ["ButtonName"] = "Save",
                        ["HeadingPopUp"] = "Add",
                        ["Validaor"] = validator,
                    };
                    DialogOptions options = new DialogOptions();
                    var dialog = _dialogService.Show<CreateAttendence>("Create", parameters, options);

                    var result = await dialog.Result;


                    if (!result.Cancelled)
                    {

                        await _hrManager.CreateManualAttendence(_addManualAdt);

                        if (!_hrManager.IsExceptionthrown())
                        {
                            IsDataLoading = true;
                            UIStateChanged();

                            await GetExistingRecord();

                            IsDataLoading = false;
                            UIStateChanged();

                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Added Successfully", Severity.Success);
                        }
                        else
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Can't add, Please retry", Severity.Error);
                        }


                    }

                }


                UIStateChanged();
            }
            catch (Exception ex)
            {

            }
            



        }

        private async Task ShowEditAttendence(MultiAtnAnlysis_Response att)
        {
            try
            {
                BLUIElement modalUIElement;

                _manualAttendance.selectedAttendence.MultiAtnDetKy = att.MultiAtnDetKy;
                _manualAttendance.selectedAttendence.AtnDetKy = att.AtnDetKy;

                if (!_modalDefinitions.TryGetValue("AddEditAttendencePopUp", out modalUIElement))
                {
                    modalUIElement = formDefinition.Children.Where(x => x._internalElementName != null && x._internalElementName.Equals("AddEditAttendencePopUp")).FirstOrDefault();
                    if (modalUIElement != null && modalUIElement.Children.Count > 0)
                    {
                        _modalDefinitions.Add("AddEditAttendencePopUp", modalUIElement);

                    }
                }



                if (modalUIElement != null)
                {
                    validatorEdit = new EditAttendenceValidator(_manualAttendance.selectedAttendence);
                    var parameters = new DialogParameters
                    {
                        ["AttendenceDetails"] = att,
                        ["ModalUIElement"] = modalUIElement,
                        ["InteractionLogics"] = _interactionLogic,
                        ["ObjectHelpers"] = _objectHelpers,
                        ["ButtonName"] = "Save",
                        ["HeadingPopUp"] = "Edit",
                        ["Validaor"] = validatorEdit,
                    };
                    DialogOptions options = new DialogOptions();
                    var dialog = _dialogService.Show<AddEditAttendence>("Edit", parameters, options);

                    var result = await dialog.Result;
                    if (!result.Cancelled)
                    {


                        await _hrManager.UpdateRecord(_manualAttendance.selectedAttendence);

                        if (!_hrManager.IsExceptionthrown())
                        {
                            IsDataLoading = true;
                            UIStateChanged();

                            await GetExistingRecord();

                            IsDataLoading = false;
                            UIStateChanged();

                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Updated Attendence Successfully", Severity.Success);
                        }
                        else
                        {
                            _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                            _snackBar.Add("Can't update, Please retry", Severity.Error);
                        }


                    }

                }


                UIStateChanged();
            }
            catch (Exception ex)
            {

            }
            



        }


        #endregion

        #region geo location

        private async Task GetGeolocation()
        {
            try
            {
                currentPosition = await geolocationWrapper.GetCurrentPosition(new PositionOptions()
                {
                    EnableHighAccuracy = true,
                    MaximumAgeTimeSpan = TimeSpan.FromHours(1),
                    TimeoutTimeSpan = TimeSpan.FromMinutes(1)
                });


            }
            catch(Exception ex)
            {
                alertMessage = "Can't detect your current location,Please check it and retry";
                showAlert = true;

                UIStateChanged();
            }
            
        }

        #endregion

        #region object-helpers
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
