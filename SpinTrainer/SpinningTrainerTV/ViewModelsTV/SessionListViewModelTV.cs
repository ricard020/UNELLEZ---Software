using ENTITYS;
using SERVICES.NavigationServices;
using SERVICES.UserServices;
using SpinningTrainerTV.Socket;
using SpinningTrainerTV.ViewTV;

namespace SpinningTrainerTV.ViewModelsTV
{
    public class SessionListViewModelTV : ViewModelBaseTV
    {
        private UserEntity _currentUser;
        private string _textOnScreen = "Esperando conexión con el dispositivo movil...";

        public string TextOnScreen
        {
            get => _textOnScreen;
            set
            {
                _textOnScreen = value;
                OnPropertyChanged(nameof(TextOnScreen));
            }
        }

        private readonly DeviceResponder _deviceResponder;
        private readonly IUserServices _userServices;
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationServices _navigationServices;

        public event Action? ReloadViewModel;


        public SessionListViewModelTV(IUserServices userServices,
                                      IServiceProvider serviceProvider,
                                      INavigationServices navigationServices,
                                      DeviceResponder deviceResponder)
        {
            _userServices = userServices;
            _serviceProvider = serviceProvider;
            _navigationServices = navigationServices;
            _deviceResponder = deviceResponder;
             
            _currentUser = _userServices.GetCurrentUser();
            TextOnScreen = "Esperando conexión con el dispositivo movil...";
            WaitMovilConnection();
        }

        public async void WaitMovilConnection() 
        {
            await _deviceResponder.WaitForTheConnectionAsync(_currentUser.Id.ToString());

            TextOnScreen = "Conexión establecida. Por favor seleccione una sesión a reproducir.";

            WaitForTheSessionToPlay();
        }

        public async void WaitForTheSessionToPlay()
        {
            var (sessionID, ipToResponse, brokeConnection) = await _deviceResponder.WaitingForTheSessionToPlay(_currentUser.Id.ToString());

            if(brokeConnection)
                ReloadViewModel?.Invoke();
            else
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var viewmodel = scope.ServiceProvider.GetService<PlaySessionViewModelTV>();

                    var session = await viewmodel.SetIdSession(sessionID);
                    await _deviceResponder.SendSession(_currentUser.Id.ToString(), ipToResponse, session);

                    viewmodel.SetSessionListViewModelTV(this);

                    await _navigationServices.NavigateToAsync<PlaySessionViewTV>(viewmodel);
                }
            }
        }

        public void Reload()
        {
            ReloadViewModel?.Invoke();
        }
    }
}
