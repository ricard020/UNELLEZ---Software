using ENTITYS;
using SERVICES.NavigationServices;
using SERVICES.SessionServices;
using SERVICES.UserServices;
using SpinningTrainer.ViewModel;
using SpinningTrainer.Views;
using System.Collections.ObjectModel;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Input;
using UTILITIES.ToastMessagesUtility;
using UTILITIES.CryptographyDataUtility;
using SpinTrainer.ViewModels;
using Application = Microsoft.Maui.Controls.Application;
using SERVICES.DatabaseServices;
using System.Text.Json;
using SpinTrainer.Resources.Charts;

#if ANDROID
using Android.Runtime;
using Android.Content;
using Android.Net.Wifi;
#endif

namespace SpinningTrainer.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private bool _isOnline;

        private string _textFilter = "";
        private DateTime? _searchDate = null;

        private ObservableCollection<SessionEntity> _sessionsObservableCollection = new ObservableCollection<SessionEntity>();

        private UserEntity _currentUser;

        private int _currentPageIndex = 0;
        private int _recordsFound;
        private string _ipOfTheConnectedTV;

        private string _pageTitle;

        public bool AlreadyConnectedToTV
        {
            get => !string.IsNullOrEmpty(IpOfTheConnectedTV);
        }
        private string IpOfTheConnectedTV
        {
            get => _ipOfTheConnectedTV;
            set
            {
                _ipOfTheConnectedTV = value;
                OnPropertyChanged(nameof(IpOfTheConnectedTV));
                OnPropertyChanged(nameof(AlreadyConnectedToTV));
                ((ViewModelCommand)ConnectTVCommand).RaiseCanExecuteChanged();
            }
        }
        public string TextFilter
        {
            get => _textFilter;
            set
            {
                if (_textFilter != value)
                {
                    _textFilter = value;
                    OnPropertyChanged(nameof(TextFilter));
                }
            }
        }
        public DateTime? SearchDate
        {
            get => _searchDate;
            set
            {
                if (value != _searchDate)
                {
                    _searchDate = value;
                    OnPropertyChanged(nameof(SearchDate));
                    ExecuteSearchBySessionDescripCommand(null);
                }
            }
        }
        public ObservableCollection<SessionEntity> SessionsObservableCollection
        {
            get => _sessionsObservableCollection;
            set
            {
                _sessionsObservableCollection = value;
                OnPropertyChanged(nameof(SessionsObservableCollection));
            }
        }

        public int CurrentPageIndex
        {
            get => _currentPageIndex;
            set
            {
                if (_currentPageIndex != value)
                {
                    _currentPageIndex = value;
                    ((ViewModelCommand)PreviousPageCommand).RaiseCanExecuteChanged();
                    OnPropertyChanged(nameof(CurrentPageIndex));
                }
            }
        }

        public int RecordsFound
        {
            get => _recordsFound;
            set
            {
                _recordsFound = value;
                ((ViewModelCommand)NextPageCommand).RaiseCanExecuteChanged();
            }
        }

        public string PageTitle
        {
            get => _pageTitle;
            set
            {
                _pageTitle = value;
                OnPropertyChanged(nameof(PageTitle));
            }
        }


        private readonly IServiceProvider _serviceProvider;
        private readonly ISessionServices _sessionServices;
        private readonly IUserServices _userServices;
        private readonly IToastMessagesUtility _toastMessagesUtility;
        private readonly INavigationServices _navigationServices;
        private readonly ICryptographyDataUtility _cryptographyDataUtility;

        public ICommand ConnectTVCommand { get; }
        public ICommand SearchBySessionDescripCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }

        public MainPageViewModel(
            IServiceProvider serviceProvider,
            ISessionServices sessionServices,
            IToastMessagesUtility toastMessagesUtility,
            IUserServices userServices,
            INavigationServices navigationServices,
            ICryptographyDataUtility cryptographyDataUtility,
            IDatabaseServices databaseServices)
        {            
            _serviceProvider = serviceProvider;
            _sessionServices = sessionServices;
            _toastMessagesUtility = toastMessagesUtility;
            _userServices = userServices;
            _navigationServices = navigationServices;
            _cryptographyDataUtility = cryptographyDataUtility;

            _isOnline = databaseServices.GetIsOnlineValue();

            if (_isOnline)
                PageTitle = "Mis sesiones";
            else
                PageTitle = "Mis sesiones (offline)";

            SearchBySessionDescripCommand = new ViewModelCommand(ExecuteSearchBySessionDescripCommand);
            NextPageCommand = new ViewModelCommand(ExecuteNextPageCommand, CanExecuteNextPageCommand);
            PreviousPageCommand = new ViewModelCommand(ExecutePreviousPageCommand, CanExecutePreviousPageCommand);
            ConnectTVCommand = new ViewModelCommand(ExecuteConnectTVCommand, CanExecuteConnectTVCommand);
        }

        private bool CanExecuteConnectTVCommand(object obj)
        {
            return (!AlreadyConnectedToTV && _isOnline);
        }

        private bool CanExecutePreviousPageCommand(object obj)
        {
            if (CurrentPageIndex == 0)
                return false;
            else
                return true;
        }

        private bool CanExecuteNextPageCommand(object obj)
        {
            if (RecordsFound > 4)
                return true;
            else
                return false;
        }

        private async void ExecuteConnectTVCommand(object obj)
        {
            string userId = GetCurrentUserID().ToString();
            string deviceName = DeviceInfo.Name;
            string ipAddress = GetWiFiIPAddress();

            using var udpClient = new UdpClient();
            var discoveryMessage = Encoding.UTF8.GetBytes(_cryptographyDataUtility.Encrypt($"DISCOVER_TV|{userId}|{deviceName}|{ipAddress}"));
            var broadcastEndpoint = new IPEndPoint(IPAddress.Broadcast, 5000);

            udpClient.EnableBroadcast = true;

            UdpReceiveResult result = new UdpReceiveResult();
            string? response = null;

            for (int i = 0; i < 10 && response == null; i++)
            {
                await udpClient.SendAsync(discoveryMessage, discoveryMessage.Length, broadcastEndpoint);

                Console.WriteLine($"Mensaje de descubrimiento enviado para usuario {userId}. Esperando respuestas...");

                result = await udpClient.ReceiveAsync();
                response = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
            }

            var responseSep = response.Split('|');

            if (responseSep[0] == "1" || responseSep[0] == "2" || responseSep[0] == "3")
            {
                if (responseSep[2] == userId)
                {
                    _ipOfTheConnectedTV = responseSep[1];

                    if(responseSep[0] == "1")
                        await _toastMessagesUtility.ShowMessage("Conexión con el TV realizada de manera exitosa");
                    else if (responseSep[0] == "2")
                        await _toastMessagesUtility.ShowMessage("Conexión con la TV restablecida");
                    else if (responseSep[0] == "3")
                    {
                        var sessionData = await udpClient.ReceiveAsync();
                        var sessionDataMessage = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(sessionData.Buffer));
                        var sessionDataMessageSep = sessionDataMessage.Split('|');

                        if(sessionDataMessageSep[13] == _ipOfTheConnectedTV)
                        {
                            var session = JsonSerializer.Deserialize<SessionEntity>(sessionDataMessageSep[0]);
                            var sessionExercises = JsonSerializer.Deserialize<List<SessionExercisesEntity>>(sessionDataMessageSep[1]);
                            var currentExerciseIndex = sessionDataMessageSep[2];
                            var currentDataPointIndex = int.Parse(sessionDataMessageSep[3]);
                            var dataPoints = JsonSerializer.Deserialize<List<DataPoint>>(sessionDataMessageSep[4]);
                            var graphProgress = double.Parse(sessionDataMessageSep[5]);
                            var currentRPMValue = sessionDataMessageSep[6];
                            var currentResistanceValue = sessionDataMessageSep[7];
                            var currentTargetRPM = sessionDataMessageSep[8];
                            var startTime = JsonSerializer.Deserialize<DateTime>(sessionDataMessageSep[9]);
                            var elapsedExerciseTime = JsonSerializer.Deserialize<TimeSpan>(sessionDataMessageSep[10]);
                            var elapsedTime = JsonSerializer.Deserialize<TimeSpan>(sessionDataMessageSep[11]);
                            var changeExerciseCountdown = JsonSerializer.Deserialize<TimeSpan>(sessionDataMessageSep[12]);

                            var remoteControlViewModel = _serviceProvider.GetService<RemoteControlViewModel>();

                            remoteControlViewModel.SetIpOfTheConnectedTV(_ipOfTheConnectedTV);
                            remoteControlViewModel.SetCurrentSessionInPlayData(session, sessionExercises, int.Parse(currentExerciseIndex), dataPoints, currentDataPointIndex, graphProgress, double.Parse(currentRPMValue), double.Parse(currentResistanceValue), double.Parse(currentTargetRPM), startTime, elapsedExerciseTime, elapsedTime, changeExerciseCountdown);

                            var resultResponse = _cryptographyDataUtility.Encrypt($"{GetWiFiIPAddress()}|{userId}|1");
                            var responseData = Encoding.UTF8.GetBytes(response);
                            await udpClient.SendAsync(responseData, responseData.Length, result.RemoteEndPoint);

                            await _toastMessagesUtility.ShowMessage("Conexión con la TV restablecida");

                            await _navigationServices.NavigateToAsync<RemoteControlView>(remoteControlViewModel);
                        }
                    }

                        InitialiceHeartbeat();
                }
                else
                {
                    await _toastMessagesUtility.ShowMessage("El dispositivo que respondió no posee el mismo usuario.");
                }
            }
            else
            {
                await _toastMessagesUtility.ShowMessage("Se rechazo la conexión en el módulo de TV");
            }
        }

        private async void ExecutePreviousPageCommand(object obj)
        {
            CurrentPageIndex--;
            int recordsSkip = CurrentPageIndex * 4;

            var (sessions, recordsFound, operationComplete, errorMessage) = await _sessionServices.GetAllByFilters(SearchDate, _currentUser.Id, TextFilter, recordsSkip);

            if (!operationComplete)
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
                CurrentPageIndex++;
            }
            else
            {
                SessionsObservableCollection.Clear();
                foreach (var session in sessions)
                {
                    SessionsObservableCollection.Add(session);
                }

                RecordsFound = recordsFound;
            }
        }

        private async void ExecuteNextPageCommand(object obj)
        {
            CurrentPageIndex++;
            int recordsSkip = CurrentPageIndex * 4;

            var (sessions, recordsFound, operationComplete, errorMessage) = await _sessionServices.GetAllByFilters(SearchDate, _currentUser.Id, TextFilter, recordsSkip);

            if (!operationComplete)
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
                CurrentPageIndex--;
            }
            else
            {
                SessionsObservableCollection.Clear();
                foreach (var session in sessions)
                {
                    SessionsObservableCollection.Add(session);
                }
                RecordsFound = recordsFound;
            }
        }

        private async void ExecuteSearchBySessionDescripCommand(object obj)
        {
            var (sessions, recordsFound, operationComplete, errorMessage) = await _sessionServices.GetAllByFilters(SearchDate, _currentUser.Id, TextFilter, 0);

            if (!operationComplete)
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
            }
            else
            {
                SessionsObservableCollection.Clear();
                foreach (var session in sessions)
                {
                    SessionsObservableCollection.Add(session);
                }

                RecordsFound = recordsFound;
                CurrentPageIndex = 0;
            }
        }

        public async void CreateNewSession()
        {
            var sessionViewModel = _serviceProvider.GetService<SessionViewModel>();

            sessionViewModel.SetIsEditSession(false, 0);

            await _navigationServices.NavigateToAsync<NewSessionView>(sessionViewModel);

            ExecuteSearchBySessionDescripCommand(null);
        }

        public async void PlaySessionInTV(int sessionID)
        {
            string userId = GetCurrentUserID().ToString();

            using var udpClient = new UdpClient();
            var discoveryMessage = Encoding.UTF8.GetBytes(_cryptographyDataUtility.Encrypt($"PLAY_SESSION|{userId}|{sessionID}"));
            var broadcastEndpoint = new IPEndPoint(IPAddress.Parse(_ipOfTheConnectedTV), 5000);

            udpClient.EnableBroadcast = true;
            await udpClient.SendAsync(discoveryMessage, discoveryMessage.Length, broadcastEndpoint);

            var result = await udpClient.ReceiveAsync();
            var response = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                var session = JsonSerializer.Deserialize<SessionEntity>(responseSep[0]);

                var remoteControlViewModel = _serviceProvider.GetService<RemoteControlViewModel>();

                remoteControlViewModel.SetSession(session);
                remoteControlViewModel.SetIpOfTheConnectedTV(_ipOfTheConnectedTV);

                await _navigationServices.NavigateToAsync<RemoteControlView>(remoteControlViewModel);
            }
        }

        public async Task DuplicateSession(int sessionID)
        {
            var insertNewDuplicateSessionDataViewModel = _serviceProvider.GetService<InsertNewDuplicateSessionDataViewModel>();

            insertNewDuplicateSessionDataViewModel.SetSessionIDToDuplicate(sessionID);

            await _navigationServices.NavigateToAsync<InsertNewDuplicateSessionDataView>(insertNewDuplicateSessionDataViewModel);

            ExecuteSearchBySessionDescripCommand(null);
        }

        public async void ModifySession(int sessionID)
        {
            var sessionViewModel = _serviceProvider.GetService<SessionViewModel>();

            sessionViewModel.SetIsEditSession(true, sessionID);

            await _navigationServices.NavigateToAsync<NewSessionView>(sessionViewModel);

            ExecuteSearchBySessionDescripCommand(null);
        }

        public async void DeleteSession(int sessionID)
        {
            bool confirm = await Application.Current.MainPage.DisplayAlert("Sesión Pendiente.", $"¿Esta seguro que desea eliminar este registro?", "Sí", "No");
            if (confirm)
            {
                string result = await Application.Current.MainPage.DisplayPromptAsync("Código de Confirmación", "Ingresa su código de usuario para confirmar la eliminación:");
                if (result == _currentUser.Username)
                {
                    try
                    {
                        var (operationComplete, errorMessage) = await _sessionServices.Delete(sessionID);

                        if (operationComplete)
                        {
                            await _toastMessagesUtility.ShowMessage("Sesión eliminada satisfactoriamente.");

                            ExecuteSearchBySessionDescripCommand(null);
                        }
                        else
                        {
                            await _toastMessagesUtility.ShowMessage("ERROR: " + errorMessage);
                        }
                    }
                    catch (Exception ex)
                    {
                        await _toastMessagesUtility.ShowMessage("ERROR: " + ex.Message);
                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Código de confirmación incorrecto.", "OK");
                }
            }
        }

        public async Task SetCurrentUser()
        {
            _currentUser = _userServices.GetCurrentUser();
            ExecuteSearchBySessionDescripCommand(null);
        }

        public int GetCurrentUserID()
        {
            return _currentUser.Id;
        }

        public async void InitialiceHeartbeat()
        {
            string userId = GetCurrentUserID().ToString();

            using var udpClient = new UdpClient();
            var discoveryMessage = Encoding.UTF8.GetBytes(_cryptographyDataUtility.Encrypt($"HEARTBEAT|{userId}"));
            var broadcastEndpoint = new IPEndPoint(IPAddress.Parse(_ipOfTheConnectedTV), 5000);

            udpClient.EnableBroadcast = true;

            while (true)
            {
                await udpClient.SendAsync(discoveryMessage, discoveryMessage.Length, broadcastEndpoint);
                await Task.Delay(10000); // Espera 10 segundos antes de enviar otro.
            }
        }

        public static string GetWiFiIPAddress()
        {
            #if ANDROID
            try
            {
                using var wifiManager = (WifiManager)Android.App.Application.Context.GetSystemService(Context.WifiService);
                if (wifiManager != null && wifiManager.ConnectionInfo != null)
                {
                    // Obtén la IP en formato entero.
                    int ip = wifiManager.ConnectionInfo.IpAddress;

                    // Convierte la IP de entero a formato legible.
                    return $"{(ip & 0xFF)}.{(ip >> 8 & 0xFF)}.{(ip >> 16 & 0xFF)}.{(ip >> 24 & 0xFF)}";
                }
                else
                {
                    return "No Wi-Fi Connected.";
                }
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
            #else
            return "";
            #endif
        }
    }
}