using Portafolio.Modelos;
using Portafolio.ViewModels;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace Portafolio.Views;

public partial class SolucionesPage : ContentPage
{
	public SolucionesPage()
    {
        InitializeComponent();
        BindingContext = new SolucionesPageViewModel();
    }

    private void ProductoTapped(object sender, EventArgs e)
    {
        if (sender is ContentView contentView && contentView.BindingContext is grupo grupo)
        {
            var viewModel = BindingContext as SolucionesPageViewModel;
            var listaContenidos = viewModel.ObtenerContenidoPorGrupo(grupo);
            viewModel.Contenidos = new ObservableCollection<main>(listaContenidos);

            // Cambia el valor de IsGroupSelected a 'true'
            viewModel.IsGroupSelected = true;
        }
    }


}