using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using InfoManager.Maui.Services;
using InfoManager.Shared.Requests;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfoManager.Maui.Pages
{
    internal partial class RegisterPageModel : ObservableObject
    {
        InfoManagerServices infoManagerServices;
        public RegisterPageModel(InfoManagerServices infoManagerServices)
        {
            this.infoManagerServices = infoManagerServices;
        }
        [ObservableProperty] string name;
        [ObservableProperty] string username;
        [ObservableProperty] string password;
        [ObservableProperty] string passwordRepeat;
        [RelayCommand]
        public async Task Register()
        {
            if (Password == PasswordRepeat)
            {
                try
                {
                    RegisterRequest request = new RegisterRequest()
                    {
                        Name = Name,
                        Username = Username,
                        Password = Password,
                    };
                    var result = await infoManagerServices.RegisterAsync(request);
                }
                catch (Exception ex)
                {

                }
            }
        }
        [RelayCommand]
        public void GoToLoginPage()
        {
            Shell.Current.GoToAsync(new ShellNavigationState("///Login"));
        }
    }
}
