using SERVICES.UserServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using UTILITIES.MailUtility;

namespace SpinningTrainer.ViewModels
{
    public class RecoveryLoginDataViewModel : ViewModelBase
    {
        private int _recoveryType;
        private string _email;       
        private string _username;
        private string _newPassword;
        private string _confirmNewPassword;
        private string _recoveryCodeSent;
        private string _recoveryCodeInput;
        private string _recoveryData;
        private string _errorMessage;
        private string _helpMessage;
        private string _placeholderHelpMessage;
        private bool _showVslRecoveryType = true;
        private bool _showVslRecoveryData;
        private bool _showVslVerifyCode;
        private bool _showVslNewPassword;

        public int RecoveryType 
        { 
            get => _recoveryType; 
            set 
            { 
                _recoveryType = value; 
                OnPropertyChanged(nameof(RecoveryType));
                ChangedRecoveryType();
            }
        }
        public string Email 
        { 
            get => _email; 
            set 
            { 
                _email = value; 
                OnPropertyChanged(nameof(Email));
            }
        }
        public string RecoveryCodeSent 
        {
            get => _recoveryCodeSent; 
            set 
            {
                _recoveryCodeSent = value; 
                OnPropertyChanged(nameof(RecoveryCodeSent));
            }
        }
        public string RecoveryCodeInput
        {
            get => _recoveryCodeInput;
            set
            {
                _recoveryCodeInput = value;
                OnPropertyChanged(nameof(RecoveryCodeInput));
                ((ViewModelCommand)VerifyCodeInput).RaiseCanExecuteChanged();
            }
        }
        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
                ((ViewModelCommand)UpdateUserPassword).RaiseCanExecuteChanged();
            }

        }
        public string ConfirmNewPassword
        {
            get => _confirmNewPassword;
            set
            {
                _confirmNewPassword = value;
                OnPropertyChanged(nameof(ConfirmNewPassword));
                ((ViewModelCommand)UpdateUserPassword).RaiseCanExecuteChanged();
            }
        }
        public string RecoveryData 
        {
            get => _recoveryData; 
            set 
            {
                _recoveryData = value; 
                OnPropertyChanged(nameof(RecoveryData));
                ((ViewModelCommand)VerifyRecoveryData).RaiseCanExecuteChanged();
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
        public string Username 
        {
            get => _username; 
            set 
            {
                _username = value; 
                OnPropertyChanged(nameof(Username));
            } 
        }
        public string HelpMessage 
        { 
            get => _helpMessage; 
            set 
            {
                _helpMessage = value;
                OnPropertyChanged(nameof(HelpMessage));
            }
        }
        public string PlaceholderHelpMessage 
        {
            get => _placeholderHelpMessage; 
            set 
            {
                _placeholderHelpMessage = value;
                OnPropertyChanged(nameof(PlaceholderHelpMessage));
            } 
        }
        public bool ShowVslRecoveryType 
        { 
            get => _showVslRecoveryType; 
            set 
            {
                _showVslRecoveryType = value;
                OnPropertyChanged(nameof(ShowVslRecoveryType));
            }
        }
        public bool ShowVslRecoveryData 
        { 
            get => _showVslRecoveryData; 
            set 
            {
                _showVslRecoveryData = value;
                OnPropertyChanged(nameof(ShowVslRecoveryData));
            } 
        }
        public bool ShowVslVerifyCode 
        {
            get => _showVslVerifyCode; 
            set 
            {
                _showVslVerifyCode = value;
                OnPropertyChanged(nameof(ShowVslVerifyCode));
            }
        }
        public bool ShowVslNewPassword 
        {
            get => _showVslNewPassword; 
            set 
            {
                _showVslNewPassword = value;
                OnPropertyChanged(nameof(ShowVslNewPassword));
            } 
        }

        private readonly IUserServices _userServices;
        private readonly IMailUtility _mailUtility;

        public ICommand VerifyRecoveryData { get; }
        public ICommand VerifyCodeInput { get; }
        public ICommand UpdateUserPassword { get; }
        public ICommand ResendRecoveryCode { get; }

        public RecoveryLoginDataViewModel(IUserServices userServices, IMailUtility mailUtility)
        {
            _userServices = userServices;
            _mailUtility = mailUtility;

            VerifyRecoveryData = new ViewModelCommand(ExecuteVerifyRecoveryDataCommand, CanExecuteVerifyRecoveryDataCommand);
            VerifyCodeInput = new ViewModelCommand(ExecuteVerifyCodeInputCommand, CanExecuteVerifyCodeInputCommand);
            UpdateUserPassword = new ViewModelCommand(ExecuteUpdateUserPasswordCommand, CanExecuteUpdateUserPasswordCommand);
            ResendRecoveryCode = new ViewModelCommand(ExecuteResendRecoveryCodeCommand);
        }        

        private bool CanExecuteUpdateUserPasswordCommand(object obj)
        {
            if (NewPassword != ConfirmNewPassword)
                return false;
            else
                return true;
        }

        private bool CanExecuteVerifyCodeInputCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(RecoveryCodeInput) || RecoveryCodeInput.Length < 8)
                return false;
            else 
                return true;
        }

        private bool CanExecuteVerifyRecoveryDataCommand(object obj)
        {
            if (string.IsNullOrWhiteSpace(RecoveryData))
                return false;
            else
                return true;
        }

        private async void ExecuteResendRecoveryCodeCommand(object obj)
        {           
            var (randomNumber, errorMessage) = _mailUtility.SendMailRecoveryCode(Email);

            if (randomNumber == "0")
            {
                ErrorMessage = errorMessage;
            }
            else
            {
                RecoveryCodeSent = randomNumber;                
            }
        }

        /// <summary>
        /// Actualiza la contraseña del usuario.
        /// </summary>
        /// <param name="obj">Objeto</param>        
        private async void ExecuteUpdateUserPasswordCommand(object obj)
        {
            var(succesfulyUpdate, errorMessage) = await _userServices.UpdatePassword(Username, NewPassword);

            if (succesfulyUpdate)
            {
                await Application.Current.MainPage.DisplayAlert("Proceso terminado!", "Su contraseña se ha actualizado!", "Aceptar");
                await App.Current.MainPage.Navigation.PopAsync();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Fallo de Actualización", errorMessage, "Aceptar");
            }
        }

        /// <summary>
        /// Verifica el código de verificación ingresado.
        /// </summary>
        /// <param name="obj">Objeto</param>        
        private async void ExecuteVerifyCodeInputCommand(object obj)
        {
            if (RecoveryCodeInput == RecoveryCodeSent)
            {
                if (RecoveryType == 0)
                {                    
                    _mailUtility.SendMail(this.Email, "Recuperación de código usuario", "Su código de usuario es:" + Username);
                    await Application.Current.MainPage.DisplayAlert("Proceso terminado!", "Se le ha enviado un correo electrónico en el cual se encuentra su código de usuario.", "Aceptar");
                    await App.Current.MainPage.Navigation.PopAsync();
                }
                else if (RecoveryType == 1)
                {
                    ErrorMessage = "";
                    ShowVslVerifyCode = false;
                    ShowVslNewPassword = true;
                }
            }
            else
            {
                ErrorMessage = "El código ingresado no coincide con el enviado.";
            }
        }

        /// <summary>
        /// Esta clase verifica el dato de recuperación ingresado en base al tipo de recuperación seleccionada previamente y posteriormente envía el email con el numero aleatorio
        /// </summary>
        /// <param name="obj">Objeto que ejecuta el comando.</param>
        private async void ExecuteVerifyRecoveryDataCommand(object obj)
        {
            if (RecoveryType == 0)
            {                
                var (codUsua, operationComplete, errorMessage) = await _userServices.ValidateUserEmailforUsernameRecovery(RecoveryData);

                if (operationComplete)
                {
                    if (!string.IsNullOrEmpty(codUsua))
                    {
                        Username = codUsua;

                        var (numeroAleatorio, mensajeError) = _mailUtility.SendMailRecoveryCode(RecoveryData);

                        if (numeroAleatorio == "0")
                        {
                            ErrorMessage = mensajeError;
                        }
                        else
                        {
                            RecoveryCodeSent = numeroAleatorio;
                            Email = RecoveryData;

                            ShowVslRecoveryType = false;
                            ShowVslRecoveryData = false;

                            ErrorMessage = "";
                            ShowVslVerifyCode = true;
                        }
                    }
                    else
                    {
                        ErrorMessage = "El correo electrónico ingresado no es valido.";
                    }
                }
                else
                {
                    ErrorMessage = errorMessage;
                }
            }
            else
            {
                var (user, operationComplete, errorMessage) = await _userServices.GetByUserName(RecoveryData);

                if (operationComplete)
                {
                    string email = user.Email;

                    if (!string.IsNullOrEmpty(email))
                    {
                        Username = RecoveryData;

                        var (randomNumber, errorMessageMail) = _mailUtility.SendMailRecoveryCode(email);

                        if (randomNumber == "0")
                        {
                            ErrorMessage = errorMessageMail;
                        }
                        else
                        {
                            RecoveryCodeSent = randomNumber;
                            this.Email = email;

                            ShowVslRecoveryType = false;
                            ShowVslRecoveryData = false;

                            ErrorMessage = "";
                            ShowVslVerifyCode = true;
                        }
                    }
                    else
                    {
                        ErrorMessage = "El usuario ingresado no es valido.";
                    }
                }
                else
                {
                    ErrorMessage = errorMessage;
                }
            }
        }

        /// <summary>
        /// Evento que se dispara al cambiar el tipo de recuperación para alternar entre los valores de ayuda que se le muestran al usuario en pantalla.
        /// </summary>
        private void ChangedRecoveryType()
        {
            if (RecoveryType == 0)
            {
                HelpMessage = "Correo electrónico";
                PlaceholderHelpMessage = "Correo electrónico de la cuenta";
            }
            else
            {
                HelpMessage = "Código de usuario";
                PlaceholderHelpMessage = "Código de usuario";
            }

            ShowVslRecoveryData = true;
        }
    }
}