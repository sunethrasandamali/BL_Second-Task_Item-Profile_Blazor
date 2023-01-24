using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Client.Infrastructure.Settings;
using MudBlazor;
using System;
using System.Threading.Tasks;

namespace BlueLotus360.Shared
{
    public partial class MainLayout : IDisposable
    {
        private MudTheme _currentTheme;
        private bool _rightToLeft = false;
        
        private bool IsLoginSuccess = false;
        private async Task RightToLeftToggle(bool value)
        {
            _rightToLeft = value;
            await Task.CompletedTask;
        }

        protected override async Task OnInitializedAsync()
        {
            _currentTheme = BL10LookAndFeel.DefaultTheme;
            //_currentTheme = await _clientPreferenceManager.GetCurrentThemeAsync();

            

            await base.OnInitializedAsync();

        }


        public void Dispose()
        {
            // _interceptor.DisposeEvent();
        }
    }
}
