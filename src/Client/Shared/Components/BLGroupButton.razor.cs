using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Shared.Components
{
    public partial class BLGroupButton
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        private string css_class = "";

		protected override Task OnInitializedAsync()
		{
            css_class =  (FromSection.IsVisible ? "d-flex " : "d-none ") + "flex-row-reverse "+ FromSection.CssClass;
            return base.OnInitializedAsync();
		}
	}
}
