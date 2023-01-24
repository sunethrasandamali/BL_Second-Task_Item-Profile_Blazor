
using BlueLotus360.CleanArchitecture.Shared.Models;
using BlueLotus360.Com.Shared.Constants.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Pages.Authentication;
public class CompanySelectionModal
{
    public int CompanyKey { get; set; } = 1;
    public string ComanyName { get; set; } = "";

}
public partial class CompanySelecttion
{
   
    private IList<CompanyResponse> list { get; set; } = new List<CompanyResponse>();
    private CompanySelectionModal _companySelectionModal = new();

    private CompanyResponse selectedCompany = new();
    protected override async Task OnInitializedAsync()
    {
        var state = await _stateProvider.GetAuthenticationStateAsync();
        await LoadCompanies();


    }

    private async Task LoadCompanies()
    {
        list = await _companyManager.GetUserCompanies();
        if (list.Count > 0)
        {
            selectedCompany = list[0];
        }
        StateHasChanged();
    }

    private void NavigateCompany() { _navigationManager.NavigateTo("/"); }
    private async void ProcessCompanySelection()
    {
        CompanyResponse request = new CompanyResponse();
        request.CompanyKey = selectedCompany.CompanyKey;
        request.CompanyName = selectedCompany.CompanyName;
        await _companyManager.UpdateSelectedCompany(request);

        await Task.FromResult(0);

   
    }



    private async void CompanyChange(CompanyResponse response)
    {
        selectedCompany = response;
        await Task.CompletedTask;
    }

    private async Task<IEnumerable<CompanyResponse>> SearchAsync(string value)
    {
        // In real life use an asynchronous function for fetching data from an api.
        await Task.Delay(5);

        // if text is null or empty, show complete list
        if (string.IsNullOrEmpty(value))
        {
            return list;
        }

        return list.Where(x => x.CompanyName.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }


}
