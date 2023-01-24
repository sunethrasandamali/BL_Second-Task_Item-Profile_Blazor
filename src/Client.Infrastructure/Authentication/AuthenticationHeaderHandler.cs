using Blazored.LocalStorage;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.Infrastructure.Managers.NavMenuManager;
using Microsoft.AspNetCore.Components;
using System;

namespace BlueLotus360.Com.Client.Infrastructure.Authentication
{
    public class AuthenticationHeaderHandler : DelegatingHandler
    {
        private readonly ILocalStorageService localStorage;
        private readonly NavigationManager _navigationManager;
        public AuthenticationHeaderHandler(ILocalStorageService localStorage, NavigationManager _navigationManager)
        {
            this.localStorage = localStorage;
            this._navigationManager = _navigationManager;

        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            HttpResponseMessage httpResponseMessage;
            if (request.Headers.Authorization?.Scheme != "Bearer")
            {
                //var savedToken = await this.localStorage.GetItemAsync<string>(StorageConstants.Local.AuthToken);

                //if (!string.IsNullOrWhiteSpace(savedToken))
                //{
                //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", savedToken);
                //}
            }
            httpResponseMessage = await base.SendAsync(request, cancellationToken);
            if (httpResponseMessage.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                try
                {
                    //await localStorage.RemoveItemAsync(StorageConstants.Local.AuthToken);
                    //await localStorage.RemoveItemAsync(StorageConstants.Local.RefreshToken);
                    //await localStorage.RemoveItemAsync(StorageConstants.Local.UserImageURL);
                    //await localStorage.RemoveItemAsync(StorageConstants.Local.CompanyName);
                    //_navigationManager.NavigateTo("/login");
                }
                catch (Exception exception)
                {

                }
            }
            return httpResponseMessage;


        }
    }
}