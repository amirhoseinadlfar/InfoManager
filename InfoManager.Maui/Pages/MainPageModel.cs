using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoManager.Maui.Pages
{
    internal partial class MainPageModel : ObservableObject
    {
        [ObservableProperty] string name;
        [ObservableProperty] string username;
    }
}
