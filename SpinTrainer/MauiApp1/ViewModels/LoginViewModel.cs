using SERVICES.DatabaseServices;
using SERVICES.UserServices;
using System.Windows.Input;
using UTILITIES.ToastMessagesUtility;

namespace SpinningTrainer.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _username;
        private string _password;
        private string _errorMessage;
        private bool _checkingLoggin = false;
        private bool _loginSuccessful;
        private bool _isOffline;

        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
                ((ViewModelCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
                ((ViewModelCommand)LoginCommand).RaiseCanExecuteChanged();
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public bool InicioExitoso
        {
            get => _loginSuccessful;
            set
            {
                _loginSuccessful = value;
                OnPropertyChanged(nameof(InicioExitoso));
            }
        }
        public bool IsOffline
        {
            get => _isOffline;
            set
            {
                _isOffline = value;
                OnPropertyChanged(nameof(IsOffline));
            }
        }
        public bool CheckingLoggin
        {
            get => _checkingLoggin;
            set
            {
                _checkingLoggin = value;
                OnPropertyChanged(nameof(CheckingLoggin));
                OnPropertyChanged(nameof(NotCheckingLoggin));
            }
        }
        public bool NotCheckingLoggin
        {
            get => !CheckingLoggin;
        }

        private readonly IUserServices _userServices;
        private readonly IToastMessagesUtility _toastMessagesUtility;

        public ICommand LoginCommand { get; }

        //Constructor
        public LoginViewModel(IUserServices userServices, IToastMessagesUtility toastMessagesUtility, IDatabaseServices databaseServices)
        {
            _userServices = userServices;
            _toastMessagesUtility = toastMessagesUtility;
            IsOffline = !databaseServices.GetIsOnlineValue();
            LoginCommand = new ViewModelCommand(ExecuteLoginCommand, CanExecuteLoginCommand);
        }

        private bool CanExecuteLoginCommand(object obj)
        {
            if (String.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Password))
                return false;
            else
                return true;
        }

        private async void ExecuteLoginCommand(object obj)
        {
            CheckingLoggin = true;
            var (inicioExitoso, mensaje, tipoUsuario) = await _userServices.AuthenticateUser(Username, Password);

            if (inicioExitoso)
            {
                var appShell = (AppShell)Application.Current.MainPage;
                appShell.SetUserType(tipoUsuario);

                var (currentUser, operationComplete, errorMessage) = await _userServices.GetByUserName(Username);

                if (operationComplete)
                {
                    _userServices.SetCurrentUser(currentUser);

                    Username = "";
                    Password = "";

                    // Navegación relativa desde la página actual
                    if (tipoUsuario == 0) // Super Usuario
                        await Shell.Current.GoToAsync($"///SuperUserMenuView");
                    else if (tipoUsuario == 1) // Administrador
                        await Shell.Current.GoToAsync($"///AdminMenuView");
                    else if (tipoUsuario == 2) // Entrenador
                    {
                        if (!_isOffline)
                        {
                            var (localUserAdded, localUserMessageError) = await _userServices.AddUserLoggedToLocalBD(currentUser);

                            if (!localUserAdded)
                                await _toastMessagesUtility.ShowMessage("Error al insertar usuario en la bd local");

                            await Shell.Current.GoToAsync($"///TrainerMenuView");
                        }
                        else
                        {
                            await Shell.Current.GoToAsync($"///TrainerMenuOffline");
                        }
                    } 
                }
                else
                {
                    ErrorMessage = "* " + errorMessage + " *";
                }
            }
            else
            {
                ErrorMessage = "* " + mensaje + " *";
            }

            CheckingLoggin = false;
        }
    }
}