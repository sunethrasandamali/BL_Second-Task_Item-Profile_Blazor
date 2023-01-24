using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Infrastructure.Managers.Identity.Roles;
using BlueLotus360.Com.Client.Settings;
using BlueLotus360.Com.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Telerik.Blazor.Components;

namespace BlueLotus360.Com.Client.Shared
{
    public partial class BLMiniDrawer
    {
        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public EventCallback OnDarkModeToggle { get; set; }

        [Parameter]
        public EventCallback<bool> OnRightToLeftToggle { get; set; }

        private bool _drawerOpen = false;
        [Inject] private IRoleManager RoleManager { get; set; }



      
        private string CurrentUserId { get; set; }
        private string ImageDataUrl { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }
        private bool _rightToLeft = false;
        private string Search { get; set; }
        private async Task RightToLeftToggle()
        {
            var isRtl = await _clientPreferenceManager.ToggleLayoutDirection();
            _rightToLeft = isRtl;

            await OnRightToLeftToggle.InvokeAsync(isRtl);
        }

        public async Task ToggleDarkMode()
        {
            await OnDarkModeToggle.InvokeAsync();
        }

        protected override async Task OnInitializedAsync()
        {
            _rightToLeft = await _clientPreferenceManager.IsRTL();
            _interceptor.RegisterEvent();
            hubConnection = hubConnection.TryInitialize(_navigationManager, _localStorage);
            _snackBar.Add(string.Format(_localizer["Welcome {0}"], FirstName), Severity.Success);
            AppSettings._miniDrawer = this;
            //menuItem = await _navManger.GetNavigationMenu();
            //SetMenuLevel(menuItem.SubMenus,0);
        }

        

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }
        }

        private async Task LoadDataAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            var user = state.User;
            if (user == null) return;
            if (user.Identity?.IsAuthenticated == true)
            {
                if (string.IsNullOrEmpty(CurrentUserId))
                {
                    CurrentUserId = user.GetUserId();
                    FirstName = user.GetFirstName();
                    if (FirstName.Length > 0)
                    {
                        FirstLetterOfName = FirstName[0];
                    }

                    SecondName = user.GetLastName();
                    Email = user.GetEmail();
                    //var imageResponse = await _accountManager.GetProfilePictureAsync(CurrentUserId);
                    //if (imageResponse.Succeeded)
                    //{
                    //    ImageDataUrl = imageResponse.Data;
                    //}

                    //var currentUserResult = await _userManager.GetAsync(CurrentUserId);
                    //if (!currentUserResult.Succeeded || currentUserResult.Data == null)
                    //{
                    //    _snackBar.Add(
                    //        _localizer["You are logged out because the user with your Token has been deleted."],
                    //        Severity.Error);
                    //    CurrentUserId = string.Empty;
                    //    ImageDataUrl = string.Empty;
                    //    FirstName = string.Empty;
                    //    SecondName = string.Empty;
                    //    Email = string.Empty;
                    //    FirstLetterOfName = char.MinValue;
                    //    await _authenticationManager.Logout();
                    //}
                }
            }
        }

        public void DrawerToggle()
        {
            _drawerOpen = !_drawerOpen;
        }


        public async void UpdateHeaderTitle()
        {
            StateHasChanged();
            await Task.CompletedTask;
        }
    


        private void Logout()
        {
            var parameters = new DialogParameters
            {
                {nameof(Dialogs.Logout.ContentText), $"{_localizer["Logout Confirmation"]}"},
                {nameof(Dialogs.Logout.ButtonText), $"{_localizer["Logout"]}"},
                {nameof(Dialogs.Logout.Color), Color.Error},
                {nameof(Dialogs.Logout.CurrentUserId), CurrentUserId},
                {nameof(Dialogs.Logout.HubConnection), hubConnection}
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

            _dialogService.Show<Dialogs.Logout>(_localizer["Logout"], parameters, options);
        }

        private HubConnection hubConnection;
        public bool IsConnected => hubConnection.State == HubConnectionState.Connected;

        //public void OnItemSelect(MenuItem selectedItem)
        //{
        //    SelectedItem = selectedItem;
        //    selectedItem.MenuExpanded = !selectedItem.MenuExpanded;
      
        //    foreach (var item in menuItem.SubMenus.Where(x => x.MenuLevel <= selectedItem.MenuLevel))
        //    {
        //        newData.Add(item);
        //        SetChild(item, selectedItem);
        //    }

        //    menuItem.SubMenus = newData;
        //}

        //private void SetChild(MenuItem item, MenuItem selectedItem)
        //{
        //    if (item == selectedItem && selectedItem.MenuExpanded && (item.SubMenus?.Any() ?? false))//when click root
        //    {
        //        foreach (var child in item.SubMenus)
        //        {
        //            newData.Add(child);
        //            SetChild(child, selectedItem);
        //        }
        //    }

        //    if (item != selectedItem && !(item.SubMenus?.Contains(selectedItem) ?? false))//when click non elemnt
        //    {
        //        item.MenuExpanded = false;
        //    }
        //}

        //private void SetMenuLevel(IList<MenuItem> menulist, int i)
        //{
        //    if (menulist.Count() > 0)
        //    {

        //        foreach (var itm in menulist)
        //        {
        //            itm.MenuLevel = i;
        //            SetMenuLevel(itm.SubMenus, ++(itm.MenuLevel));

        //        }
        //    }

        //}
    }
}
