using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using BlueLotus360.CleanArchitecture.Application.Validators.SalesOrder;
using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.Order;
using BlueLotus360.Com.Client.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlueLotus360.Com.Client.Shared.Dialogs
{
    public partial class OrderItemDialog
    {
        [CascadingParameter]
        MudDialogInstance MudDialog { get; set; }

        [Parameter]
        public OrderItem OrderItem { get; set; }

        [Parameter]
        public BLUIElement ModalUIElement { get; set; }

        [Parameter]
        public IDictionary<string, EventCallback> InteractionLogics { get; set; }

        [Parameter]
        public IDictionary<string, IBLUIOperationHelper> ObjectHelpers { get; set; }

        [Parameter]
        public CodeBaseResponse ParentLocation { get; set; }


        [Parameter]
        public IOrderValidator Validaor { get; set; }

        [Parameter]
        public string ButtonName { get; set; }

        [Parameter]
        public string HeadingPopUp { get; set; }


        

        private OrderItem _orderCopy { get; set; }

        protected override Task OnInitializedAsync()
        {
            Validaor.UserMessages.UserMessages = new List<UserMessage>();

            _orderCopy = OrderItem;

            return base.OnInitializedAsync();
        }


        MudMessageBox mbox;

        public void Cancel()
        {
            OrderItem = _orderCopy;
            MudDialog.Cancel();
        }

        public async void AddItem()
        {
            if (Validaor != null)
            {
                if (Validaor.CanAddItemToGrid())
                {
                    MudDialog.Close(DialogResult.Ok(OrderItem));
                }
                else
                {
                  
                   
                }
            }

        }

        public async void Refresh()
        {
            await Task.CompletedTask;
        }



    }
}
