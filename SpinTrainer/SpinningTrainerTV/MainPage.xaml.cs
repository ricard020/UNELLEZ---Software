using System.Collections.ObjectModel;
using Microsoft.Data.SqlClient;
using SpinningTrainerTV.ViewTV;

namespace SpinningTrainerTV
{
    public partial class MainPage : ContentPage
    {
        public ObservableCollection<string> UserNames { get; set; }

        public MainPage()
        {
            InitializeComponent();
            UserNames = new ObservableCollection<string>();
            ltvUsers.ItemsSource = UserNames;
            LoadUserNamesFromDatabase();
        }

        private void LoadUserNamesFromDatabase()
        {
            string connectionString = "Server=localhost;Database=TambocaPruebas;User Id=sa;Password=200519;TrustServerCertificate=True;Persist Security Info=True;"; // Conexión con la base de datos SQL Server
            try
            {
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "SELECT CodUsua FROM Usuarios WHERE TipoUsuario = 2"; // Seleccionar usuarios con tipo entrenador
                    using (var command = new SqlCommand(query, connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string codUsua = reader.GetString(reader.GetOrdinal("CodUsua"));
                            UserNames.Add(codUsua);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error de conexión a la base de datos: {ex.Message}");
                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                
            }
        }

        async void OnItemSelected(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection.FirstOrDefault() is string selectedUser)
            {
                //await Navigation.PushAsync(new PedirPINViewTV(selectedUser)); // Almacenar y pasa al archivo "PedirPIN" el nombre del usuario que se selecciono
            }
        }
    }
}
