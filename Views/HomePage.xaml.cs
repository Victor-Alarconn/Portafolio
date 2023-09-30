using Portafolio.ViewModels;

namespace Portafolio.Views;

public partial class HomePage : ContentPage
{
	public HomePage()
	{
		InitializeComponent();
        BindingContext = new HomePageViewModel();
    }
}