using InfoManager.Maui.Pages;
using InfoManager.Maui.Services;

using Microsoft.Extensions.Logging;

namespace InfoManager.Maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<LoginPageModel>();
            
            builder.Services.AddTransient<LoginPage>(serviceProvider =>
                new LoginPage(serviceProvider.GetRequiredService<LoginPageModel>()));

            builder.Services.AddTransient<RegisterPageModel>();
            builder.Services.AddTransient<RegisterPage>(serviceProvider =>
                new RegisterPage(serviceProvider.GetRequiredService<RegisterPageModel>()));

            builder.Services.AddSingleton<InfoManagerServices>();



#if DEBUG
		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}