using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.JSInterop;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlueLotus360.Pages.HomeScreen
{
    public partial class DashboardsSection
    {
        List<string> pincards = new List<string>();
        protected override async Task OnInitializedAsync()
        {
            pincards.Add("widgets1");
            pincards.Add("widgets2");
            pincards.Add("widgets3");
        }
    }
}
