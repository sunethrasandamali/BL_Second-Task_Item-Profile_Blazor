﻿using BlueLotus360.Com.Client.Infrastructure.Authentication;
using BlueLotus360.Com.Infrastructure.Managers;
using Microsoft.AspNetCore.Components.Authorization;
using MudBlazor.Services;
using MudBlazor;
using System.Globalization;
using Toolbelt.Blazor.Extensions.DependencyInjection;

using Blazored.LocalStorage;
using BlueLotus360.Com.Infrastructure.Managers.Preferences;
using BL10.CleanArchitecture.Domain.ICookieAccessor;
using BlueLotus360.Com.UI.BlazorServer.Data;

namespace BlueLotus360.Com.UI.BlazorServer.Extensions
{
    public static class WebAssemblyHostBuilderExtensions
    {
        private const string ClientName = "BlueLotus360.PWA";

        public static WebApplicationBuilder AddRootComponents(this WebApplicationBuilder builder)
        {


            return builder;
        }

        public static WebApplicationBuilder AddClientServices(this WebApplicationBuilder builder)
        {
            builder
                .Services
                .AddLocalization(options =>
                {
                    options.ResourcesPath = "Resources";
                })
                .AddAuthorizationCore(options =>
                {
                   // RegisterPermissionClaims(options);
                })
                .AddBlazoredLocalStorage()
                .AddMudServices(configuration =>
                {
                    configuration.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
                    configuration.SnackbarConfiguration.HideTransitionDuration = 100;
                    configuration.SnackbarConfiguration.ShowTransitionDuration = 100;
                    configuration.SnackbarConfiguration.VisibleStateDuration = 3000;
                    configuration.SnackbarConfiguration.ShowCloseIcon = false;
                })

                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddScoped<ClientPreferenceManager>()
                .AddScoped<BL10AuthProvider>()
                .AddScoped<AuthenticationStateProvider, BL10AuthProvider>()
                .AddManagers()
                //.AddExtendedAttributeManagers()
                .AddTransient<AuthenticationHeaderHandler>()
                .AddScoped(sp => sp
                    .GetRequiredService<IHttpClientFactory>()
                    .CreateClient(ClientName).EnableIntercept(sp))
                .AddHttpClient(ClientName, client =>
                {
                    client.DefaultRequestHeaders.AcceptLanguage.Clear();
                    client.DefaultRequestHeaders.AcceptLanguage.ParseAdd(CultureInfo.DefaultThreadCurrentCulture?.TwoLetterISOLanguageName);
                    client.BaseAddress = new Uri("http://yahoo.com");
                })
              .AddHttpMessageHandler<AuthenticationHeaderHandler>();
            builder.Services.AddHttpClientInterceptor();
          //  builder.Services.AddScoped<ICookieAccessor, BLCookieAuthProvider>();
            return builder;
        }

        public static IServiceCollection AddManagers(this IServiceCollection services)
        {
            var managers = typeof(IManager);

            var types = managers
                .Assembly
                .GetExportedTypes()
                .Where(t => t.IsClass && !t.IsAbstract)
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var type in types)
            {

                if (managers.IsAssignableFrom(type.Service))
                {
                    services.AddTransient(type.Service, type.Implementation);
                }
            }

            return services;
        }

      

     
    }
}
