using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.CleanArchitecture.Domain.Entities.Transaction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlueLotus360.Com.Client.Shared.Dialogs
{
    public partial class ScanSerial
    {

        [CascadingParameter] MudDialogInstance MudDialog { get; set; }

        [Parameter] public TransactionLineItem LineItem { get; set; }
        void Submit()
        {
            DialogResult result = DialogResult.Ok<TransactionLineItem>(LineItem);
            MudDialog.Close(result);
        }
        void Cancel() => MudDialog.Cancel();

    }
}
