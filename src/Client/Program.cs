using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Infrastructure.Managers.Preferences;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BlueLotus360.Com.Client.Infrastructure.Settings;
using BlueLotus360.Com.Shared.Constants.Localization;
using Blazored.SessionStorage;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BlueLotus360.Com.Client
{
    public static class Program
    {

         public static  bool  UseKendoLibrary=true;
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder
                          .CreateDefault(args)
                          .AddRootComponents()
                          .AddClientServices();
           
            var host = builder.Build();
            var storageService = host.Services.GetRequiredService<ClientPreferenceManager>();
            if (storageService != null)
            {
                CultureInfo culture;
                var preference = await storageService.GetPreference() as ClientPreference;
                if (preference != null)
                    culture = new CultureInfo(preference.LanguageCode);
                else
                    culture = new CultureInfo(LocalizationConstants.SupportedLanguages.FirstOrDefault()?.Code ?? "en-US");
                CultureInfo.DefaultThreadCurrentCulture = culture;
            }
            builder.Services.AddBlazoredSessionStorage();
            builder.Services.AddHotKeys();
            builder.Services.AddTelerikBlazor();

           builder.Logging.SetMinimumLevel(LogLevel.None);
            await builder.Build().RunAsync();
        }
    }
}