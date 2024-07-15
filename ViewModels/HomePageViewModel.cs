using MySqlConnector;
using Portafolio.Data;
using Portafolio.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Portafolio.ViewModels
{
    public class ImageInfo
    {
        public string ImagePath { get; set; }
        public string Link { get; set; }
    }
    public class HomePageViewModel : INotifyPropertyChanged
    {

        private System.Timers.Timer _carouselTimer;
        public ICommand OpenLinkCommand { get; private set; }
        public ObservableCollection<ImageInfo> ImagePaths { get; set; }

        private string _imagen;
        public string Imagen
        {
            get => _imagen;
            set
            {
                _imagen = value;
                OnPropertyChanged(nameof(Imagen));
            }
        }

        private int _currentPosition;
        public int CurrentPosition
        {
            get => _currentPosition;
            set
            {
                _currentPosition = value;
                OnPropertyChanged(nameof(CurrentPosition));
            }
        }

        public HomePageViewModel()
        {

            ImagePaths = new ObservableCollection<ImageInfo>();
            OpenLinkCommand = new Command(OpenLink);
            // Cargar las imágenes desde la base de datos
            CargarImagenesDesdeBD();

            _carouselTimer = new System.Timers.Timer();
            _carouselTimer.Interval = 10000; // 10 segundos
            _carouselTimer.Elapsed += (s, e) =>
            {
                CurrentPosition = (CurrentPosition + 1) % ImagePaths.Count;
            };
            _carouselTimer.Start();
        }

        private async void OpenLink()
        {
            var selectedItem = ImagePaths[CurrentPosition];
            if (selectedItem != null)
            {
                try
                {
                    bool result = await Launcher.OpenAsync(new Uri(selectedItem.Link));
                    if (!result)
                    {
                        Console.WriteLine("No se pudo abrir la URL.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al abrir la URL: {ex.Message}");
                }
            }
        }

        private void CargarImagenesDesdeBD()
        {
            using MySqlConnection connection = DataConexion.ObtenerConexion();
            connection.Open();

            string query = "SELECT ruta_img, pt_links FROM pt_imgs";
            using MySqlCommand cmd = new MySqlCommand(query, connection);
            using MySqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string rutaBase = reader.GetString("ruta_img");
                long timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                string rutaConTimestamp = $"{rutaBase}?timestamp={timestamp}";

                string link = reader.GetString("pt_links");

                // Utilizando el Dispatcher sugerido
                Application.Current.Dispatcher.Dispatch(() =>
                {
                    ImagePaths.Add(new ImageInfo { ImagePath = rutaConTimestamp, Link = link });
                });
            }
        }




        public static async Task<string> DescargarImagenYGuardar(string imageUrl)
        {
            try
            {
                using var client = new HttpClient();
                var bytes = await client.GetByteArrayAsync(imageUrl);

                // Aquí es donde cambiamos la lógica de almacenamiento
                var dataDir = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "images");
                if (!System.IO.Directory.Exists(dataDir))
                {
                    System.IO.Directory.CreateDirectory(dataDir);
                }
                var filename = System.IO.Path.Combine(dataDir, System.IO.Path.GetRandomFileName() + ".jpg");
                await System.IO.File.WriteAllBytesAsync(filename, bytes);

                return filename;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al descargar la imagen: {ex.Message}");
                return string.Empty;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
