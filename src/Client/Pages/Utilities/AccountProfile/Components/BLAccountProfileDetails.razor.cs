using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Client.Extensions;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlueLotus360.Com.Client.Pages.Utilities.AccountProfile.Components
{
    public partial class BLAccountProfileDetails
    {
        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public BLUIElement FormObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }
        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public string Class { get; set; } = "default-class";

        public BLUIElement LinkedUIObject { get; private set; }
        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }
    }
}
