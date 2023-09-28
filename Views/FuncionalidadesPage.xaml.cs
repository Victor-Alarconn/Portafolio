using Portafolio.ViewModels;

namespace Portafolio.Views;

public partial class FuncionalidadesPage : ContentPage
{
	public FuncionalidadesPage()
	{
		InitializeComponent();
        BindingContext = new FuncionalidadesPageViewModel();
    }
}