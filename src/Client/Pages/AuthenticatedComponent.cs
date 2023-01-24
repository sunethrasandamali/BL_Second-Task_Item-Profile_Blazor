


using BlueLotus360.Com.Client.Infrastructure.Authentication;
using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Pages;

public class AuthenticatedComponentBase: ComponentBase
{
   // BL10AuthProvider _stateProvider;
    protected override void OnInitialized()
    {

        base.OnInitialized();
     
    }

    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }
}
