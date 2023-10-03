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
        var selectedContent = (sender as Frame).BindingContext as main;

        if (selectedContent == null)
            return;

        var viewModel = BindingContext as SolucionesPageViewModel;
        var relatedImages = await viewModel.ObtenerContenidoPorid(selectedContent.Id_pt);

        if (relatedImages == null || !relatedImages.Any())
            return;

        // Botón de cierre
        var closeButton = new Button
        {
            Text = "X",
            BackgroundColor = Microsoft.Maui.Graphics.Colors.Red,
            TextColor = Microsoft.Maui.Graphics.Colors.White,
            HorizontalOptions = LayoutOptions.End,
            VerticalOptions = LayoutOptions.Start
        };

        // Mostrar el modal con las imágenes relacionadas
        var modalPage = new ContentPage
        {
            Content = new Grid
            {
                Children =
        {
            new Image
            {
                Source = relatedImages.First().Url,
                Aspect = Aspect.AspectFit
            },
            new Label
            {
                Text = relatedImages.First().Descripcion,
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center
            },
            closeButton // Añadir el botón de cierre al Grid
        }
            }
        };

        // Manejar el evento Clicked del botón de cierre
        closeButton.Clicked += async (s, args) =>
        {
            await modalPage.Navigation.PopModalAsync(); // Cerrar el modal
        };

        await Navigation.PushModalAsync(modalPage);
    }






}