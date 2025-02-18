using ENTITYS;
using Microsoft.Data.SqlClient;
using SpinningTrainerTV.ViewTV;
using SERVICES.UserServices;

namespace SpinningTrainerTV.ViewModelsTV
{
    public class RequestUserPINViewModelTV : ViewModelBaseTV
    {
        private UserEntity _userSelected;

        private string _charPin1;
        private string _charPin2;
        private string _charPin3;
        private string _charPin4;
        private string _messageError;

        public UserEntity UserSelected
        {
            get => _userSelected; 
            set 
            {
                _userSelected = value;
                OnPropertyChanged(nameof(UserSelected));
            }
        }
        public string CharPin1
        {
            get => _charPin1; 
            set 
            {
                _charPin1 = value; 
                OnPropertyChanged(nameof(CharPin1));
            }
        }
        public string CharPin2
        {
            get => _charPin2;
            set
            {
                _charPin2 = value;
                OnPropertyChanged(nameof(CharPin2));
            }
        }
        public string CharPin3 
        {
            get => _charPin3;
            set
            {
                _charPin3 = value;
                OnPropertyChanged(nameof(CharPin3));
            }
        }
        public string CharPin4 
        {
            get => _charPin4;
            set
            {
                _charPin4 = value;
                OnPropertyChanged(nameof(CharPin4));
            }
        }
        public string MessageError 
        {
            get => _messageError; 
            set 
            {
                _messageError = value;
                OnPropertyChanged(nameof(MessageError));
            }
        }

        private readonly IUserServices _userServices;

        public RequestUserPINViewModelTV(IUserServices userServices)
        {
            _userServices = userServices;
        }

        public async void ValidatePIN()
        {
            string pin = $"{CharPin1}{CharPin2}{CharPin3}{CharPin4}";

            if (string.IsNullOrWhiteSpace(pin) || pin.Length != 4)
            {
                MessageError = "El PIN debe tener exactamente 4 dígitos.";
                return;
            }

            try
            {
                var (logginSuccesully, state) = await _userServices.AuthenticateUserPIN(UserSelected.Id, pin);

                if (logginSuccesully)
                {
                    _userServices.SetCurrentUser(UserSelected);
                    await Shell.Current.GoToAsync($"//{nameof(SessionListViewTV)}");
                }
                else
                {
                    if(state == 1)
                    {
                        MessageError = "PIN incorrecto.";
                    }
                    else if (state == 2)
                    {
                        MessageError = "Membresía vencida.";
                    }
                    else if (state == 3)
                    {
                        MessageError = "Error desconocido.";
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageError = $"Error de conexión a la base de datos: {ex.Message}";
            }
            catch (Exception ex)
            {
                MessageError = $"Error: {ex.Message}";
            }
        }

        public void SetCurrentUser(UserEntity user)
        {
            UserSelected = user;
        }
    }
}
