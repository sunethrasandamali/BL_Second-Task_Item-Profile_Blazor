using BlueLotus360.Com.Shared.Managers;
using MudBlazor;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Infrastructure.Managers.Preferences
{
    public interface IClientPreferenceManager : IPreferenceManager
    {
        Task<MudTheme> GetCurrentThemeAsync();

        Task<bool> ToggleDarkModeAsync();
    }
}