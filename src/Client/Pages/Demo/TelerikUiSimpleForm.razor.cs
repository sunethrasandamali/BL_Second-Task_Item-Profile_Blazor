using BlueLotus360.CleanArchitecture.Client.Infrastructure.Helpers;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.Com.Client.Extensions;
using BlueLotus360.Com.Client.Shared.Components;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static BlueLotus360.CleanArchitecture.Domain.Entities.Demo.TelerikSimpleFormText;

namespace BlueLotus360.Com.Client.Pages.Demo
{
    public partial class TelerikUiSimpleForm
    {
        #region parameter

        private BLUIElement formDefinition;
        private SimpleTelerikFormRequest simpleForm;

        private IDictionary<string, EventCallback> _interactionLogic;
        private IDictionary<string, BLUIElement> _modalDefinitions;

        private IDictionary<string, IBLUIOperationHelper> _objectHelpers;

        private UIBuilder _refBuilder;

        #endregion

        #region General

        protected override void OnParametersSet()
        {

            base.OnParametersSet();
        }

        protected override async Task OnInitializedAsync()
        {
            long elementKey = 1;
            _navigationManager.TryGetQueryString<long>("ElementKey", out elementKey);// get element key from url 
            if (elementKey > 10)
            {
                var formrequest = new ObjectFormRequest();
                formrequest.MenuKey = elementKey;
                formDefinition = await _navManger.GetMenuUIElement(formrequest);//get ui elements
            }

            formDefinition.IsDebugMode = true;
            _interactionLogic = new Dictionary<string, EventCallback>();
            _modalDefinitions = new Dictionary<string, BLUIElement>();
            _objectHelpers = new Dictionary<string, IBLUIOperationHelper>();

            RefreshGrid();
            HookInteractions();
            UIStateChanged();

            simpleForm.ElementKey = elementKey;
        }

        private async void RefreshGrid()
        {
            simpleForm = new SimpleTelerikFormRequest();
            simpleForm.ElementKey = formDefinition.ElementKey;
            await Task.CompletedTask;
        }

        private void HookInteractions()
        {

            InteractionHelper helper = new InteractionHelper(this, formDefinition);//formdefinition has all form objects 
            _interactionLogic = helper.GenerateEventCallbacks();// generate all event callbacks 

        }

        private void UIStateChanged()
        {
            this.StateHasChanged();
        }

        #endregion


        #region ui events

        private void OnNameChange(UIInterectionArgs<string> args)
        {
            simpleForm.Name = args.DataObject;
            UIStateChanged();
        }

        private void OnItemCodeChange(UIInterectionArgs<CodeBaseResponse> args)
        {
            simpleForm.Code = args.DataObject;
            UIStateChanged();
        }

        private void OnDescriptionChange(UIInterectionArgs<string> args)
        {
            simpleForm.Description = args.DataObject;
            UIStateChanged();
        }

        private void OnDateChange(UIInterectionArgs<DateTime?> args)
        {
            simpleForm.Date = args.DataObject;
            UIStateChanged();
        }

        private void OnDiscountChange(UIInterectionArgs<decimal> args)
        {
            simpleForm.Discount = args.DataObject;
            UIStateChanged();
        }

        private async void OnSubmit(UIInterectionArgs<object> args)
        {
            UIStateChanged();
        }

        private async void OnCancel(UIInterectionArgs<object> args)
        {
            UIStateChanged();
        }
        #endregion
    }
}
