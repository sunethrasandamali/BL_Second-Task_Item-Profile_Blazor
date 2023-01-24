
using BlueLotus360.CleanArchitecture.Domain.Entities;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace BlueLotus360.Com.Client.Shared.Components;
public partial class SideMenu
{
    [Parameter]
    public MenuItem Menu { get; set; }
    [Parameter] public IDictionary<string, string> IconDictionary { get; set; }


}
