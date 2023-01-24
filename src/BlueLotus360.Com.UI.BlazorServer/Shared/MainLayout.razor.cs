using BlueLotus360.Com.Client.Infrastructure.Settings;
using MudBlazor;

namespace BlueLotus360.Com.UI.BlazorServer.Shared
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
