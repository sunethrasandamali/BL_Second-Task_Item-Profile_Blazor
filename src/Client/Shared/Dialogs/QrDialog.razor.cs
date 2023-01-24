using BlueLotus360.CleanArchitecture.Domain.DTO.Object;
using BlueLotus360.Com.Client.Shared.QrBarCode;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlueLotus360.Com.Client.Shared.Dialogs
{
    public partial class QrDialog
    {
        [CascadingParameter] MudDialogInstance MudDialog { get; set; }


        private QrScanner _scanner;



        void Submit() => MudDialog.Close(DialogResult.Ok("ok"));
        void Cancel() => MudDialog.Cancel();


        public void OnQRCodeDetected(UIInterectionArgs<string> args)
        {
            MudDialog.Close(DialogResult.Ok(args.DataObject));

        }


        private void ToggleDevices()
        {
            if (_scanner != null)
            {
                _scanner.ToggleVideoInputs();
            }
        }
    }
}
