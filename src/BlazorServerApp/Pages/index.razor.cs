using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlueLotus360.Pages
{
    
    public partial class Index
    {
        [CascadingParameter(Name = "PinnedMenus")]
        protected MenuItem ChildMenu { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }
        private string CompanyName { get; set; }
        public string ImageDataUrl { get; set; }

        private Transition transition = Transition.Slide;

        List<MenuItem> tiles = new List<MenuItem>();
        string greetings = "";
        long elementKey=1;
        private bool showReminderAlert = true;
        protected override async Task OnInitializedAsync()
        {
            //_navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);
            greetings = await _jsRuntime.InvokeAsync<string>("GerGreetings", null);
            
            HookInteractions();

            //await _jsRuntime.InvokeVoidAsync("SetDomTitle", "Blue Lotus 360");
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await LoadDataAsync();
            }
            
            
        }

        private void HookInteractions()
        {
            
            appStateService._AppBarName = "Home";
        }

        private async Task LoadDataAsync()
        {

            var state = await _stateProvider.GetAuthenticationStateAsync();
            ClaimsPrincipal user = new ClaimsPrincipal();
            if (state!=null)
            {
                user = state.User;

                ClaimsPrincipal principal = state.User as ClaimsPrincipal;
                if (principal!=null)
                {
                    Claim? claim = principal.FindFirst("FirstName");
                    if (claim != null && claim.Value != null)
                    {
                        FirstName = claim.Value.Split(".")[0];
                    }
                }


                if (this.FirstName.Length > 0)
                {
                    FirstLetterOfName = FirstName[0];
                }
                var UserId = FirstName;
                var imageResponse = await _localStorage.GetItemAsync<string>(StorageConstants.Local.UserImageURL);
                if (!string.IsNullOrEmpty(imageResponse))
                {
                    ImageDataUrl = imageResponse;
                }

                CompanyName = await _localStorage.GetItemAsync<string>(StorageConstants.Local.CompanyName);
                StateHasChanged();
            }
        }

        private void CloseMe(bool value)
        {
            if (value)
            {
                showReminderAlert = false;
            }
            
        }

    }
}
