using MySqlConnector;
using Portafolio.Data;
using Portafolio.Modelos;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Portafolio.ViewModels
{
    public class SolucionesPageViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<grupo> Grupos { get; set; }
        private grupo selectedGroup;

        private ObservableCollection<main> contenidos;

        public ObservableCollection<CardDataModel> imagenes;
        public ObservableCollection<main> Contenidos
        {
            get { return contenidos; }
            set
            {
                contenidos = value;
                OnPropertyChanged(nameof(Contenidos));
            }
        }

        public ObservableCollection<CardDataModel> Imagenes
        {
            get { return imagenes; }
            set
            {
                imagenes = value;
                OnPropertyChanged(nameof(Imagenes));
            }
        }

        public SolucionesPageViewModel()
        {
            Grupos = new ObservableCollection<grupo>();
            CargarProductos();

        }

        public void SelectGroup(grupo groupToSelect)
        {
            if (selectedGroup != null)
                selectedGroup.IsSelected = false;

            groupToSelect.IsSelected = true;
            selectedGroup = groupToSelect;

            OnPropertyChanged(nameof(Grupos));  // Notifica que la lista de grupos ha cambiado
        }

        private bool _isGroupSelected;
        public bool IsGroupSelected
        {
            get { return _isGroupSelected; }
            set
            {
                _isGroupSelected = value;
                OnPropertyChanged(nameof(IsGroupSelected));
            }
        }

        private void CargarProductos()
        {
            using MySqlConnection connection = DataConexion.ObtenerConexion();
            connection.Open();

            // Modifica la consulta para seleccionar también el id_grupo y pt_otracolumna
            MySqlCommand command = new MySqlCommand("SELECT id_grupo, pt_nombre, pt_url FROM pt_grupos", connection);

            using MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                Grupos.Add(new grupo
                {
                    Id_grupo = Convert.ToInt32(reader["id_grupo"]),
                    Nombre = reader["pt_nombre"].ToString(),
                    Url = reader["pt_url"].ToString()
                });
            }
        }
        public async Task<string> ObtenerContenidoDesdeUrl(string url)
        {
            using HttpClient client = new HttpClient();
            return await client.GetStringAsync(url);
        }

        public async Task<List<main>> ObtenerContenidoPorGrupo(grupo grupo)
        {
            List<main> contenidos = new List<main>();

            using MySqlConnection connection = DataConexion.ObtenerConexion();
            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT * FROM pt_main WHERE Id_gr = @id", connection);
            command.Parameters.AddWithValue("@id", grupo.Id_grupo);

            using MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())  // Usa "while" en lugar de "if" para procesar todos los registros
            {
                var descripcion = reader["pt_desc"].ToString();

                // Verifica si "descripcion" es una URL y, de ser así, obtiene el contenido
                if (Uri.IsWellFormedUriString(descripcion, UriKind.Absolute))
                {
                    descripcion = await ObtenerContenidoDesdeUrl(descripcion);
                }

                contenidos.Add(new main
                {
                    Id_pt = Convert.ToInt32(reader["id_pt"]),
                    Nombre = reader["pt_nombre"].ToString(),
                    GrupoId = Convert.ToInt32(reader["Id_gr"]),
                    Url = reader["pt_url"].ToString(),
                    Descripcion = descripcion
                });
            }

            return contenidos; // Devuelve la lista completa de registros
        }


        public async Task<List<CardDataModel>> ObtenerContenidoPorid(int idContenido)
        {
            List<CardDataModel> imagenes = new List<CardDataModel>();

            using MySqlConnection connection = DataConexion.ObtenerConexion();
            connection.Open();

            MySqlCommand command = new MySqlCommand("SELECT * FROM pt_conten WHERE id_pt = @id", connection);
            command.Parameters.AddWithValue("@id", idContenido);

            using MySqlDataReader reader = command.ExecuteReader();
            while (reader.Read())  
            {
               // var descripcion = reader["pt_desc"].ToString();

                // Verifica si "descripcion" es una URL y, de ser así, obtiene el contenido
                //if (Uri.IsWellFormedUriString(descripcion, UriKind.Absolute))
                //{
                //    descripcion = await ObtenerContenidoDesdeUrl(descripcion);
                //}

                imagenes.Add(new CardDataModel
                {
                    //Id_pt = Convert.ToInt32(reader["id_pt"]),
                    //GrupoId = Convert.ToInt32(reader["Id_gr"]),
                    Url = reader["pt_conte"].ToString(),
                   // Descripcion = descripcion
                });
            }

            return imagenes; // Devuelve la lista completa de registros
        }



        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
