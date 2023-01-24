﻿using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.UI.Definitions.Extensions;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlueLotus360.Com.UI.Definitions.Pages.SetUp.Master_Data.Item_Profile_Simple_V2.Components
{
    public partial class BLItemProfileSimpleInsertDetails
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
