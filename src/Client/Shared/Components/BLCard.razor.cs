using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Client.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Shared.Components
{
    public partial class BLCard : IBLUIOperationHelper
    {
        [Parameter]
        public BLUIElement UIElement { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        public BLUIElement LinkedUIObject => throw new System.NotImplementedException();
        private string css_class = "";
        private string card_content = "";

        protected override Task OnParametersSetAsync()
        {
            if (ObjectHelpers != null)
            {
                if (ObjectHelpers.ContainsKey(UIElement.ElementName))
                {
                    ObjectHelpers.Remove(UIElement.ElementName);

                }
                ObjectHelpers.Add(UIElement.ElementName, this);
            }
            return base.OnParametersSetAsync();
        }
        protected override async Task OnInitializedAsync()
        {
            css_class = UIElement.IsVisible ? "d-flex" : "d-none" ;

            SetCardContent();
            StateHasChanged();
            await base.OnInitializedAsync();
        }

        private void SetCardContent()
        {
            try
            {
               Type t = DataObject.GetType();
               PropertyInfo p = t.GetProperty(UIElement.DefaultAccessPath, BindingFlags.Public | BindingFlags.Instance);

               card_content = p ?.GetValue(DataObject, null).ToString();
                
            }
            catch (Exception ex)
            {

            }
        }

        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task Refresh()
        {
            throw new System.NotImplementedException();
        }

        public void ResetToInitialValue()
        {
            throw new System.NotImplementedException();
        }

        public async Task SetValue(object value)
        {
            card_content=value.ToString();
            this.StateHasChanged();
            await Task.CompletedTask;
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new System.NotImplementedException();
        }


    }
}
