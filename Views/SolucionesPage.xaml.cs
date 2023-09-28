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
        object context = null;

        if (sender is Label label)
            context = label.BindingContext;
        else if (sender is Image image)
            context = image.BindingContext;

        if (context is grupo grupo)
        {
            var listaContenidos = (BindingContext as SolucionesPageViewModel).ObtenerContenidoPorGrupo(grupo);
            (BindingContext as SolucionesPageViewModel).Contenidos = new ObservableCollection<main>(listaContenidos);
        }
    }


}