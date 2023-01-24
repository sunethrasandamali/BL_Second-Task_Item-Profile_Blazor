﻿using BlueLotus360.Com.Client.Extensions;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using BlueLotus360.Com.Shared.Constants.Storage;

namespace BlueLotus360.Com.Client.Shared.Components
{
    public partial class UserCard
    {
        [Parameter] public string Class { get; set; }
        private string FirstName { get; set; }
        private string SecondName { get; set; }
        private string Email { get; set; }
        private char FirstLetterOfName { get; set; }
        private string CompanyName { get; set; }

        [Parameter]
        public string ImageDataUrl { get; set; }

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

            this.Email = user.GetFirstName();
            this.FirstName = user.GetFirstName();
            this.SecondName = user.GetLastName();
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

            CompanyName= await _localStorage.GetItemAsync<string>(StorageConstants.Local.CompanyName);
            StateHasChanged();
        }
    }
}