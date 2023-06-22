namespace InfoManager.Maui.Pages;

public partial class LoginPage : ContentPage
{
	public LoginPage(object loginPageModel)
	{
		InitializeComponent();
		this.BindingContext = loginPageModel;
		
	}
}