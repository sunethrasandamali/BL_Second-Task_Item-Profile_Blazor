using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Client.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Shared.Components
{
    public partial class TelerikUIRendere
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }



        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }


        public BLUIElement LinkedUIObject { get; private set; }
        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }

        public void OnSectionOK()
        {

        }

        public async Task Refresh()
        {
            await Task.CompletedTask;
        }

        public void ResetToInitialValue()
        {
            throw new System.NotImplementedException();
        }

        public Task SetValue(object value)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnInitialized()
        {

            base.OnInitialized();
        }
    }
}
