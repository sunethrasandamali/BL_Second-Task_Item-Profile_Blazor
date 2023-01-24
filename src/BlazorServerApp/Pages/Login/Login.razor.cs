using Blazored.FluentValidation;
using BlueLotus360.Com.Application.Requests.Identity;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using System;
using System.Threading;
using Microsoft.JSInterop;

namespace BlueLotus360.Pages.Login
{
    public partial class Login : ComponentBase
    {
        
        private TokenRequest _tokenModel = new();
        private bool IsLoginSuccessFull = false;
        private string Message = "Login with your Credentials.";
        private string className = "";
        protected override async Task OnInitializedAsync()
        {
            var state = await _stateProvider.GetAuthenticationStateAsync();
            if (state != new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())))
            {
               //_navigationManager.NavigateTo("/home");
                //await _jsRuntime.InvokeVoidAsync("SetDomTitle", "Blue Lotus 360");
            }
        }


        private async Task SubmitAsync()
        {
            Message = "Login with your Credentials.";
            className = "";
            StateHasChanged();

            var result = await _authenticationManager.Login(_tokenModel);
            

            StateHasChanged();
        }

        private bool _passwordVisibility;
        private InputType _passwordInput = InputType.Password;
        private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

        void TogglePasswordVisibility()
        {
            if (_passwordVisibility)
            {
                _passwordVisibility = false;
                _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
                _passwordInput = InputType.Password;
            }
            else
            {
                _passwordVisibility = true;
                _passwordInputIcon = Icons.Material.Filled.Visibility;
                _passwordInput = InputType.Text;

            }
        }


    }
}