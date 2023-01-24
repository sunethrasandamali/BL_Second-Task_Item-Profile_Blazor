using BlueLotus360.CleanArchitecture.Domain.Entities;
using BlueLotus360.CleanArchitecture.Domain.Entities.HR;
using BlueLotus360.Com.Shared.Constants.Storage;
using BlueLotus360.Com.UI.Definitions.Extensions;
using BlueLotus360.Com.UI.Definitions.MB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MudBlazor;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BlueLotus360.Pages.HomeScreen
{
    public partial class PinSection
    {
        [Parameter]public MenuItem PinnedMenus { get; set; }
        [Parameter] public long ElementKey { get; set; }
        List<MenuItem> pincards = new List<MenuItem>();
        MenuItem pinnedrequest = new MenuItem();
        private bool opt;
        private string searchString1 = "";
        private bool IsPinPopupShown;
        int pinnedIndex = 0;

        string[] color_arr = new string[12] ;
        protected override async Task OnInitializedAsync()
        {
 
            await LoadMenus();
        
        }

        private async Task LoadMenus()
        {
            pincards.Clear();
            await Task.Delay(3000);

            if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Count() > 0)
            {
                pincards = PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList();
                this.SetPinCardColor(pincards);
                this.StateHasChanged();
            }
            pincards.Add(new MenuItem() { MenuCaption = "Add Shortcuts" });
        }

        private void SetPinCardColor(List<MenuItem> pincards)
        {
            color_arr[0] = "#35495F";
            color_arr[1] = "#15A085";
            color_arr[2] = "#F39C13";
            color_arr[3] = "#2A80B9";
            color_arr[4] = "#E74C3D";
            color_arr[5] = "#35495F";
            color_arr[6] = "#15A085";
            color_arr[7] = "#F39C13";
            color_arr[8] = "#2A80B9";
            color_arr[9] = "#E74C3D";
            color_arr[10] = "#35495F";
            color_arr[11] = "#15A085";
            //foreach (var p in pincards)
            //{
            //    int index = pincards.IndexOf(p);
            //    if (!p.MenuCaption.Equals("Add Shortcuts"))
            //    {
            //        if (index == 0 || index == 5 || index == 10)
            //        {
            //            p.PinCardColor = "#35495F";
            //        }
            //        else if (index == 1 || index == 6)
            //        {
            //            p.PinCardColor = "#15A085";
            //        }
            //        else if (index == 2 || index == 7)
            //        {
            //            p.PinCardColor = "#F39C13";
            //        }
            //        else if (index == 3 || index == 8)
            //        {
            //            p.PinCardColor = "#2A80B9";
            //        }
            //        else
            //        {
            //            p.PinCardColor = "#E74C3D";
            //        }
            //    }                
            //}
        }
        private void OpenPinpoup()
        {
            IsPinPopupShown = true;
            StateHasChanged();
        }
        private void CloseDialog()
        {
            IsPinPopupShown = false;
            StateHasChanged();
        }
        void ChangeHandler(bool value, string menucaption)
        {
            if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Count() > 0)
            {
                var item = PinnedMenus.SubMenus.Where(x => x.MenuCaption == menucaption).FirstOrDefault();
                if (item != null)
                {

                    if (value==true && PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 12)
                    {
                        item.IsChanged = true;
                        item.Ispinned = value;
                        this.StateHasChanged();
                    }
                    else if (value == false && PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
                    {
                        item.IsChanged = true;
                        item.Ispinned = value;
                        this.StateHasChanged();
                    }
                    else
                    {
                        _snackBar.Configuration.PositionClass = Defaults.Classes.Position.TopRight;
                        _snackBar.Add("Can't be added more than 12 shortcuts", Severity.Info);
                    }

            }
                
            }
            
        }
        private async Task UpdatePinSection()
        {
                if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Count() > 0)
                {
                    pinnedrequest.SubMenus = PinnedMenus.SubMenus.Where(x => x.IsChanged == true).ToList();
                    
                        if (pinnedrequest != null && pinnedrequest.SubMenus != null && pinnedrequest.SubMenus.Count()>0)
                        {
                            if (PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
                            {
                                await _navManger.UpdatePinnedMenus(pinnedrequest);
                            }
                        }
                        
                        await LoadMenus();
                        IsPinPopupShown = false;

                }

            this.StateHasChanged();
        }

        private async Task Unpin(string menucaption)
        {
            if (PinnedMenus != null && PinnedMenus.SubMenus != null && PinnedMenus.SubMenus.Where(x => x.Ispinned == true).ToList().Count() < 13)
            {
                var item = PinnedMenus.SubMenus.Where(x => x.MenuCaption == menucaption).FirstOrDefault();

                item.IsChanged = true;
                item.Ispinned = false;
                this.StateHasChanged();
            }

            await UpdatePinSection();
        }

        public async Task NavigateToNewTab(string URL)
        {
            if (!string.IsNullOrEmpty(URL))
            {
                string url = URL;
                await _jsRuntime.InvokeVoidAsync("open", url, "_blank");
            }


        }
     

    }

    
}
