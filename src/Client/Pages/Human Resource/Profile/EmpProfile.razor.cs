using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.Client.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Pages.Human_Resource.Profile
{
    public partial class EmpProfile
    {
        [Parameter]
        public EmployeeModel Employee { get; set; }=new EmployeeModel();

        [Parameter]
        public string ImageDataUrl { get; set; } = "";

        [Parameter]
        public char FirstLetterOfName { get; set; }

        [Parameter]
        public string FirstName { get; set; } = "";
         
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

    }
}
