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
    public partial class LoginPageModel : ObservableObject
    {
        private readonly InfoManagerServices infoManagerServices;
        public LoginPageModel(InfoManagerServices infoManagerServices)
        {
            this.infoManagerServices = infoManagerServices;
        }
        [ObservableProperty] string username;
        [ObservableProperty] string password;
        [NotifyPropertyChangedFor(nameof(IsNotBusy))]
        [ObservableProperty] bool isBusy;
        public bool IsNotBusy => !IsBusy;
        [RelayCommand]
        async Task Login()
        {
            IsBusy = true;
            try
            {
                LoginRequest loginRequest = new LoginRequest
                {
                    Username = Username,
                    Password = Password,
                };
                var sucess = await infoManagerServices.LoginAsync(loginRequest);
                if (sucess.IsT0)
                {
                    var me = await infoManagerServices.GetCurrentUserInfoAsync();
                    Shell.Current.GoToAsync(new ShellNavigationState("///Main"));
                }
                else if(sucess.IsT1)
                {
                    Shell.Current.DisplayAlert("خطا", "نام کاربری یا رمز عبور اشتباه است","تایید");
                }
            }
            catch (Exception ex)
            {

            }
            IsBusy = false;
        }
        [RelayCommand]
        void GoToRegister()
        {
            Shell.Current.GoToAsync(new ShellNavigationState("///Register"));
        }

    }
}
