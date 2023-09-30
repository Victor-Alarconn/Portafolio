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

    private async void ProductoTapped(object sender, EventArgs e)
    {
        if (sender is Frame frame && frame.BindingContext is grupo selectedGrupo)
        {
            var viewModel = BindingContext as SolucionesPageViewModel;
            var listaContenidos = await viewModel.ObtenerContenidoPorGrupo(selectedGrupo);
            viewModel.SelectGroup(selectedGrupo);
            viewModel.Contenidos = new ObservableCollection<main>(listaContenidos);

            // Cambia el valor de IsGroupSelected a 'true'
            viewModel.IsGroupSelected = true;
        }
    }


    private async void OnCardTapped(object sender, EventArgs e)
    {
        //var cardData = (sender as Frame).BindingContext as CardDataModel;
        var cardData = (sender as Frame).BindingContext as main;

        var modalPage = new ContentPage
        {
            Content = new StackLayout
            {
                Children = {
                new Image { Source = cardData.Url },
                new Label { Text = cardData.Descripcion }
            }
            }
        };

        await Navigation.PushModalAsync(modalPage);
    }



}