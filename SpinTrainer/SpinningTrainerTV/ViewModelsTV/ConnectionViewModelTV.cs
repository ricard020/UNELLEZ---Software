using Microsoft.Data.SqlClient;
using SERVICES.DatabaseServices;
using SpinningTrainerTV.ViewModelsTV;
using SpinningTrainerTV.ViewTV;
using System.Data;
using System.Windows.Input;
using UTILITIES.CryptographyDataUtility;
using UTILITIES.ToastMessagesUtility;

namespace SpinTrainer.ViewModels
{
    public class ConnectionViewModelTV : ViewModelBaseTV
    {
        private string _serverName;
        private string _databaseName;
        private string _userName;
        private string _password;

        private bool _enableServerNameEntry = true;
        private bool _enableDatabaseNameEntry = true;
        private bool _enableUsernameEntry = true;
        private bool _enablePasswordEntry = true;

        private bool _checkingConnection = false;
        private bool _connectionChecked = false;

        public string ServerName
        {
            get => _serverName;
            set
            {
                _serverName = value;
                OnPropertyChanged(nameof(ServerName));
                ((ViewModelCommandTV)CheckConnectionCommand).RaiseCanExecuteChanged();
            }
        }
        public string DatabaseName
        {
            get => _databaseName;
            set
            {
                _databaseName = value;
                OnPropertyChanged(nameof(DatabaseName));
                ((ViewModelCommandTV)CheckConnectionCommand).RaiseCanExecuteChanged();
            }
        }
        public string UserName
        {
            get => _userName;
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
                ((ViewModelCommandTV)CheckConnectionCommand).RaiseCanExecuteChanged();
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ((ViewModelCommandTV)CheckConnectionCommand).RaiseCanExecuteChanged();
            }
        }

        public bool CheckingConnection
        {
            get => _checkingConnection;
            set
            {
                _checkingConnection = value;
                OnPropertyChanged(nameof(CheckingConnection));
            }
        }
        public bool EnableServerNameEntry
        {
            get => _enableServerNameEntry;
            set
            {
                _enableServerNameEntry = value;
                OnPropertyChanged(nameof(EnableServerNameEntry));
            }
        }
        public bool EnableDatabaseNameEntry
        {
            get => _enableDatabaseNameEntry;
            set
            {
                _enableDatabaseNameEntry = value;
                OnPropertyChanged(nameof(EnableDatabaseNameEntry));
            }
        }
        public bool EnableUsernameEntry
        {
            get => _enableUsernameEntry;
            set
            {
                _enableUsernameEntry = value;
                OnPropertyChanged(nameof(EnableUsernameEntry));
            }
        }
        public bool EnablePasswordEntry
        {
            get => _enablePasswordEntry;
            set
            {
                _enablePasswordEntry = value;
                OnPropertyChanged(nameof(EnablePasswordEntry));
            }
        }
        public bool ConnectionChecked
        {
            get => _connectionChecked;
            set
            {
                _connectionChecked = value;
                OnPropertyChanged(nameof(ConnectionChecked));
                OnPropertyChanged(nameof(NotConnectionCheked));
                ((ViewModelCommandTV)SaveConnectionCommand).RaiseCanExecuteChanged();
            }
        }

        public bool NotConnectionCheked
        {
            get => !ConnectionChecked;
        }

        public ICommand CheckConnectionCommand { get; }
        public ICommand SaveConnectionCommand { get; }

        private readonly IToastMessagesUtility _toastMessageUtility;
        private readonly ICryptographyDataUtility _cryptographyDataUtility;
        private readonly IDatabaseServices _databaseServices;

        public ConnectionViewModelTV(IToastMessagesUtility toastMessagesUtility, ICryptographyDataUtility cryptographyDataUtility, IDatabaseServices databaseServices)
        {
            _databaseServices = databaseServices;
            _toastMessageUtility = toastMessagesUtility;
            _cryptographyDataUtility = cryptographyDataUtility;
            CheckConnectionCommand = new ViewModelCommandTV(ExecuteCheckConnectionCommand, CanExecuteCheckConnectionCommand);
            SaveConnectionCommand = new ViewModelCommandTV(ExecuteSaveConnectionCommand, CanExecuteSaveConnectionCommand);
        }

        private bool CanExecuteCheckConnectionCommand(object obj)
        {
            if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName) || string.IsNullOrEmpty(UserName) || string.IsNullOrEmpty(Password))
                return false;
            else
                return true;
        }

        private bool CanExecuteSaveConnectionCommand(object obj)
        {
            if (ConnectionChecked)
                return true;
            else
                return false;
        }

        private async void ExecuteCheckConnectionCommand(object obj)
        {
            CheckingConnection = true;
            await Task.Delay(2000);

            try
            {

                SqlConnection connection = new SqlConnection("Data Source=" + ServerName + ";" +
                                                             "Initial Catalog=" + DatabaseName + ";" +
                                                             "TrustServerCertificate=True;" +
                                                             "Persist Security Info=True;" +
                                                             "User Id=" + UserName + ";" +
                                                             "Password=" + Password + ";");

                connection.Open();

                if ((connection.State & ConnectionState.Open) > 0)
                {
                    await _toastMessageUtility.ShowMessage("Conexión Exitosa");

                    connection.Close();

                    EnableServerNameEntry = false;
                    EnableDatabaseNameEntry = false;
                    EnableUsernameEntry = false;
                    EnablePasswordEntry = false;

                    ConnectionChecked = true;
                }
                else
                {
                    await _toastMessageUtility.ShowMessage("Conexión Falló");
                }
            }
            catch (Exception ex)
            {
                await _toastMessageUtility.ShowMessage("Conexión Falló: " + ex.Message);
            }
            finally
            {
                await Task.Delay(2000);
                CheckingConnection = false;
            }
        }

        private async void ExecuteSaveConnectionCommand(object obj)
        {
            try
            {
                string executablePath = AppDomain.CurrentDomain.BaseDirectory; // Obtiene la ruta del ejecutable
                string fileName = "Application.cfg"; // Nombre del archivo que deseas verificar o crear
                string filePath = System.IO.Path.Combine(executablePath, fileName);

                string connectionString = "Data Source=" + ServerName + ";" +
                                          "Initial Catalog=" + DatabaseName + ";" +
                                          "TrustServerCertificate=True;" +
                                          "Persist Security Info=True;" +
                                          "User Id=" + UserName + ";" +
                                          "Password=" + Password + ";";

                string connectionStringEncriptada = _cryptographyDataUtility.Encrypt(connectionString);

                File.WriteAllText(filePath, connectionStringEncriptada);

                await Shell.Current.GoToAsync($"//{nameof(SelectUserViewTV)}");
            }
            catch (Exception ex)
            {
                EnableServerNameEntry = true;
                EnableDatabaseNameEntry = true;
                EnableUsernameEntry = true;
                EnablePasswordEntry = true;

                ConnectionChecked = false;
                _toastMessageUtility.ShowMessage(ex.Message);
                throw;
            }
        }
    }
}
