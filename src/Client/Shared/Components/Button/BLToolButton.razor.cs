using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using System.Collections.Generic;
using BlueLotus360.CleanArchitecture.Domain;
using System;
using System.Reflection;
using System.Linq;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;

namespace BlueLotus360.Com.Client.Shared.Components.Button
{
    public partial class BLToolButton
    {
        [Parameter]
        public BLUIElement FromSection { get; set; }

        [Parameter]
        public object DataObject { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        private string btn_css = "";
        private string IconSvgCode = "";

        protected override void OnInitialized()
        {
            btn_css = (FromSection.IsVisible ? FromSection.CssClass : "d-none") ;

            string[] path = this.FromSection.IconCss.Split('.');
            GetIconByStingName(this.FromSection.IconCss,typeof(Icons));
            base.OnInitialized();
        }

        
        private void OnToolButtonClick()
        {
            UIInterectionArgs<object> args = new UIInterectionArgs<object>();

            if (InteractionLogics != null && FromSection.OnClickAction != null && FromSection.OnClickAction.Length > 3)
            {
                EventCallback callback;

                if (InteractionLogics.TryGetValue(FromSection.OnClickAction, out callback))
                {

                    if (callback.HasDelegate)
                    {

                        args.Caller = this.FromSection.OnClickAction;
                        args.ObjectPath = this.FromSection.DefaultAccessPath;
                        args.DataObject = new object();
                        args.sender = this;
                        args.InitiatorObject = FromSection;
                        callback.InvokeAsync(args);
                    }
                    else
                    {

                    }

                }
            }



        }

        void GetIconByStingName(string PropertyName,Type t)
        {

            Type type = null;
            string [] path = PropertyName.Split('.');
            string IconName = null;
            object iconObject = Icons.Material.Filled;
            if (path.Length == 2)
            {
                //This will assume the Filled section
                if (path[0].Equals("Filled"))
                {
                    type = Icons.Material.Filled.GetType();
                     iconObject = Icons.Material.Filled;
                }
                //This will assume the Filled section
                if (path[0].Equals("Outlined"))
                {
                    type = Icons.Material.Outlined.GetType();
                     iconObject = Icons.Material.Outlined;
                }

                if (path[0].Equals("TwoTone"))
                {
                    type = Icons.Material.TwoTone.GetType();
                   iconObject = Icons.Material.TwoTone;
                }

                if (path[0].Equals("Sharp"))
                {
                    type = Icons.Material.Sharp.GetType();
                    iconObject = Icons.Material.Sharp;
                }


                if (path[0].Equals("Rounded"))
                {
                    type = Icons.Material.Rounded.GetType();
                    iconObject = Icons.Material.Rounded;
                }

                IconName=path[1];

            }
            else
            {
                type = Icons.Material.Filled.GetType();
                iconObject = Icons.Material.Filled;   
                IconName = PropertyName;
            }


            if (type != null)
            {
                PropertyInfo info = type.GetProperty(IconName); 
                if (info != null)
                {
                    string value = info.GetValue(iconObject) as string;
                    IconSvgCode = value;
                }
            }



        }
    }
}
