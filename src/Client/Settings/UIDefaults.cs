using BlueLotus360.Com.Client.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
namespace BlueLotus360.Com.Client.Settings
{
    public class UIDefaults
    {
        //public static Variant DefaultControlVariants { get; set; } = Variant.Outlined;
        public static Variant DefaultControlVariants { get; set; } = Variant.Text;
    }


    public class AppSettings
    {

        public static string _AppBarName { get; set; } = "Home";

        [Inject] public static   IJSRuntime JS { get; set; }

        public static  BLMiniDrawer _miniDrawer { get; set; }

        public static void RefreshTopBar(string AppBarName)
        {
           _AppBarName = AppBarName;
            
           _miniDrawer.UpdateHeaderTitle();

        }

    }

}
