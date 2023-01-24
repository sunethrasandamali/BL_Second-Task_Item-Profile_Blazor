using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Collections.Generic;
using BlueLotus360.CleanArchitecture.Domain;
using BlueLotus360.Com.Client.Extensions;
using System.Threading.Tasks;
using System;
using System.Reflection;
using Telerik.Blazor.Components;
using Telerik.DataSource;

namespace BlueLotus360.Com.Client.Shared.Components.TelerikComponents.Grid
{
    public partial class BLTelerikKendoGrid<TItem> : ComponentBase, IBLUIOperationHelper
    {
        [Parameter]
        public IList<TItem> DataObject { get; set; }

        [Parameter]
        public BLUIElement FormObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public string Height { get; set; }
        private int filterDebounceDelay { get; set; } = 200;
        TelerikGrid<TItem> GridRef { get; set; }
        public BLUIElement LinkedUIObject => throw new System.NotImplementedException();

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
            //ReArrangeElements();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
        }

        protected override Task OnAfterRenderAsync(bool firstRender)
        {

            EventCallback callBack;

            if (InteractionLogics.TryGetValue("OnGridInitilize_" + FormObject._internalElementName, out callBack))
            {
                if (callBack.HasDelegate)
                {
                    callBack.InvokeAsync();
                }
            }

            return base.OnAfterRenderAsync(firstRender);


        }

        private string SetGridValues(Object obj, string name)
        {
            string column = "";
            if (name == null || name.Trim().Length < 2)
            {
                column = String.Empty;

            }

            Type type = obj.GetType();

            foreach (string part in name.Split('.'))
            {
                if (obj == null) { column = String.Empty; }


                PropertyInfo info = type.GetProperty(part);

                if (info == null)
                {
                    column = String.Empty;
                }

                else if ((info?.GetValue(obj, null)).IsNumericType())
                {
                    column = decimal.Parse(info?.GetValue(obj, null).ToString()).ToString("N2");
                }
                else
                {
                    column = info?.GetValue(obj, null).ToString();
                }


                type = info?.PropertyType;
                obj = info?.GetValue(obj, null);
            }
            return column;
        }

        private void ReArrangeElements()
        {
            var childsHash = FormObject.Children.ToLookup(elem => elem.ParentKey);
            foreach (var child in FormObject.Children)
            {
                child.Children = childsHash[child.ElementKey].ToList();
            }
            BLUIElement form = FormObject.Children.Where(x => x.ElementKey == FormObject.ElementKey).FirstOrDefault();
            FormObject = form;

        }

        private void OnStateInit(GridStateEventArgs<TItem> args)
        {
            //    args.GridState.GroupDescriptors = new List<TItem>()
            //{
            //    new GroupDescriptor()
            //    {
            //        Member = nameof(ProductDto.CategoryId),
            //        MemberType = typeof(int)
            //    }
            //};
        }


        public void ResetToInitialValue()
        {
            throw new System.NotImplementedException();
        }

        public void UpdateVisibility(bool IsVisible)
        {
            throw new System.NotImplementedException();
        }

        public void ToggleEditable(bool IsEditable)
        {
            throw new System.NotImplementedException();
        }

        public Task Refresh()
        {
            throw new System.NotImplementedException();
        }

        public Task FocusComponentAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task SetValue(object value)
        {
            throw new System.NotImplementedException();
        }
    }
}
