using BlueLotus360.CleanArchitecture.Application.Validators.MessageSystem;
using Microsoft.AspNetCore.Components;

namespace BlueLotus360.Com.Client.Shared.Components
{
    public partial class UserMessageDialog
    {
        [Parameter]
        public UserMessageManager Messages { get; set; }

        [Parameter]
        public bool  MessageShown { get; set; }


        public void ShowUserMessageWindow()
        {
            MessageShown = true;
            StateHasChanged(); 
        }

        public void HideWindow()
        {
            MessageShown = false;
            StateHasChanged();
        }

    }
}
