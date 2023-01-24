using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Shared.Constants.Application;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;


namespace BlueLotus360.Com.UI.BlazorServer.Shared
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
           
            _rightToLeft = false;

        
        }

        public async Task ToggleDarkMode()
        {
            await OnDarkModeToggle.InvokeAsync();
        }

        protected override async Task OnInitializedAsync()
        {
           // _rightToLeft = await _clientPreferenceManager.IsRTL();
           // _interceptor.RegisterEvent();
            //hubConnection = hubConnection.TryInitialize(_navigationManager, _localStorage);
            _snackBar.Add(string.Format(_localizer["Welcome {0}"], FirstName), Severity.Success);
            //AppSettings._miniDrawer = this;
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
   
            };

            var options = new DialogOptions { CloseButton = true, MaxWidth = MaxWidth.Small, FullWidth = true };

           
        }


     

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
