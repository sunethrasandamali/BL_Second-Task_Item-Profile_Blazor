using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Shared.Components
{
    public partial class MiniSideMenu
    {
        [Parameter] public MenuItem Menu { get; set; }
        public string IconSvgCode { get; private set; }
        [Parameter]public IDictionary<int, string> IconDictionary { get; set; }
        protected override Task OnParametersSetAsync()
        {
           
            //GetIconByStringName(this.Menu.MenuIcon, typeof(Icons));
            return base.OnParametersSetAsync();
        }


        //string GetIconByStringName(string PropertyName, Type t)
        //{

        //    if (PropertyName is null)
        //    {
        //        PropertyName = "Filled.SpaceDashboard";
        //    }
        //    Type type = null;
        //    string[] path = PropertyName.Split('.');
        //    string IconName = null;
        //    object iconObject = Icons.Material.Filled;
        //    if (path.Length == 2)
        //    {
        //        //This will assume the Filled section
        //        if (path[0].Equals("Filled"))
        //        {
        //            type = Icons.Material.Filled.GetType();
        //            iconObject = Icons.Material.Filled;
        //        }
        //        //This will assume the Filled section
        //        if (path[0].Equals("Outlined"))
        //        {
        //            type = Icons.Material.Outlined.GetType();
        //            iconObject = Icons.Material.Outlined;
        //        }

        //        if (path[0].Equals("TwoTone"))
        //        {
        //            type = Icons.Material.TwoTone.GetType();
        //            iconObject = Icons.Material.TwoTone;
        //        }

        //        if (path[0].Equals("Sharp"))
        //        {
        //            type = Icons.Material.Sharp.GetType();
        //            iconObject = Icons.Material.Sharp;
        //        }


        //        if (path[0].Equals("Rounded"))
        //        {
        //            type = Icons.Material.Rounded.GetType();
        //            iconObject = Icons.Material.Rounded;
        //        }

        //        IconName = path[1];

        //    }
        //    else
        //    {
        //        type = Icons.Material.Filled.GetType();
        //        iconObject = Icons.Material.Filled;
        //        IconName = PropertyName;
        //    }


        //    if (type != null)
        //    {
        //        PropertyInfo info = type.GetProperty(IconName);
        //        if (info != null)
        //        {
        //            string value = info.GetValue(iconObject) as string;
        //            IconSvgCode = value;
        //        }
        //    }

            

        //}
    }
}
