using System.Collections.ObjectModel;
using System.Windows.Input;
using SpinningTrainer.Views;
using ENTITYS;
using SERVICES.UserServices;
using UTILITIES.CryptographyDataUtility;
using UTILITIES.ToastMessagesUtility;
using SERVICES.NavigationServices;

namespace SpinningTrainer.ViewModels
{
    public class UsersViewModel : ViewModelBase
    {
        private string _newCodUsua;
        private string _newDescrip;
        private string _newContra;
        private string _newConfirmContra;
        private string _newPIN;
        private string _newEmail;
        private string _newTelef;        
        private DateTime _newFechaR = DateTime.Now;
        private DateTime _newFechaV = DateTime.Now.AddMonths(1);
        private int _newTipoUsuario = 2;
        private bool _editionEnable;
        private bool _codExists;
        private bool _editingCurrentUserProfile;
        private bool _reloadingUsersList = false;
        private ObservableCollection<UserEntity> _users = new ObservableCollection<UserEntity>();

        private UserEntity _selectedUser;
        
        public ObservableCollection<UserEntity> Users
        {
            get => _users;
            set
            {
                _users = value;
                OnPropertyChanged(nameof(Users));
            }
        }

        public string NewCodUsua
        {
            get => _newCodUsua;
            set
            {
                _newCodUsua = value;
                OnPropertyChanged(nameof(NewCodUsua));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
                VerifyCodUsuaExists();
            }
        }
        public string NewDescrip 
        {
            get => _newDescrip;
            set 
            {
                _newDescrip = value;
                OnPropertyChanged(nameof(NewDescrip));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }
        public string NewContra 
        {
            get => _newContra;
            set 
            {
                _newContra = value; 
                OnPropertyChanged(nameof(NewContra));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }
        public string NewConfirmContra
        {
            get => _newConfirmContra;
            set
            {
                _newConfirmContra = value;
                OnPropertyChanged(nameof(NewConfirmContra));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }
        public string NewPIN 
        {
            get => _newPIN;
            set 
            {
                _newPIN = value;
                OnPropertyChanged(nameof(NewPIN));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }
        public string NewEmail 
        {
            get => _newEmail; 
            set 
            {
                _newEmail = value;
                OnPropertyChanged(nameof(NewEmail));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }
        public string NewTelef 
        {
            get => _newTelef;
            set 
            {
                _newTelef = value;
                OnPropertyChanged(nameof(NewTelef));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }
        public DateTime NewFechaR 
        {
            get => _newFechaR; 
            set 
            {
                _newFechaR = value; 
                OnPropertyChanged(nameof(NewFechaR));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }
        public DateTime NewFechaV 
        {
            get => _newFechaV;
            set 
            {
                _newFechaV = value; 
                OnPropertyChanged(nameof(NewFechaV));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            }
        }        
        public int NewTipoUsuario 
        {
            get => _newTipoUsuario; 
            set 
            {
                _newTipoUsuario = value;
                OnPropertyChanged(nameof(NewTipoUsuario));
                ((ViewModelCommand)SaveUserCommand).RaiseCanExecuteChanged();
            } 
        }
        public bool EditionEnable
        {
            get => _editionEnable;
            set
            {
                _editionEnable = value;
                OnPropertyChanged(nameof(EditionEnable));
            }
        }
        public bool ReloadingUsersList
        {
            get => _reloadingUsersList;
            set
            {
                _reloadingUsersList = value;
                OnPropertyChanged(nameof(ReloadingUsersList));
            }
        }
        public UserEntity SelectedUser 
        {
            get => _selectedUser; 
            set 
            {
                _selectedUser = value; 
                OnPropertyChanged(nameof(SelectedUser));
            }
        }
        public bool CodExists 
        {
            get => _codExists; 
            set 
            {
                _codExists = value; 
                OnPropertyChanged(nameof(CodExists));
            }
        }
        public bool EditingCurrentUserProfile 
        {
            get => _editingCurrentUserProfile;
            set 
            {
                _editingCurrentUserProfile = value; 
                OnPropertyChanged(nameof(EditingCurrentUserProfile));
                OnPropertyChanged(nameof(NotEditingCurrentUserProfile));
            }
        }
        public bool NotEditingCurrentUserProfile
        {
            get => !_editingCurrentUserProfile;
        }

        private readonly IUserServices _userServices;
        private readonly ICryptographyDataUtility _cryptographyDataUtility;
        private readonly IToastMessagesUtility _toastMessagesUtility;
        private readonly INavigationServices _navigationServices;

        public ICommand AddNewUserCommand { get; }
        public ICommand SaveUserCommand { get; }
        public ICommand IncrementMembershipCommand { get; }
        public ICommand DeleteUserCommand { get; }     
        public ICommand RefreshUserListCommand { get; }

        public UsersViewModel(IUserServices userServices, ICryptographyDataUtility cryptographyDataUtility, IToastMessagesUtility toastMessagesUtility, INavigationServices navigationServices)
        {            
            _userServices = userServices;
            _cryptographyDataUtility = cryptographyDataUtility;
            _toastMessagesUtility = toastMessagesUtility;
            _navigationServices = navigationServices;

            AddNewUserCommand = new ViewModelCommand(ExecuteAddNewUserCommand);
            DeleteUserCommand = new ViewModelCommand(ExecuteDeleteUserCommand);
            SaveUserCommand = new ViewModelCommand(ExecuteSaveUserCommand, CanExecuteSaveUserCommand);
            IncrementMembershipCommand = new ViewModelCommand(ExecuteIncrementMembershipCommand);
            RefreshUserListCommand = new ViewModelCommand(ExecuteRefreshUserListCommand);

            LoadUsers();
        }

        private bool CanExecuteSaveUserCommand(object obj)
        {

            if (!string.IsNullOrEmpty(NewCodUsua) && !string.IsNullOrEmpty(NewDescrip) && !string.IsNullOrEmpty(NewContra) && !string.IsNullOrEmpty(NewConfirmContra))
            {
                if (string.IsNullOrEmpty(NewPIN) && NewTipoUsuario == 2)
                    return false;
                else if (CodExists || (NewContra != NewConfirmContra))
                    return false;
                else
                    return true;
            }
            else
            {
                return false;
            }
            
        }

        private async void ExecuteSaveUserCommand(object obj)
        {
            UserEntity user = new UserEntity()
            {
                Username = NewCodUsua,
                Descrip = NewDescrip,
                Password = _cryptographyDataUtility.Encrypt(NewContra),
                Email = NewEmail,
                PIN = _cryptographyDataUtility.Encrypt(NewPIN),
                NumberPhone = NewTelef,
                DateR = NewFechaR,
                DateV = NewFechaV,
                UserType = (short)NewTipoUsuario
            };

            bool operationCompleted;
            string errorMessage;

            if (EditionEnable || EditingCurrentUserProfile)
            {
                user.Id = SelectedUser.Id;
                (operationCompleted, errorMessage) = await _userServices.Update(user);
            }
            else
            {
                user.DateC = DateTime.Now;
                (operationCompleted, errorMessage) = await _userServices.Add(user);
            }

            if (operationCompleted)
            {
                await _toastMessagesUtility.ShowMessage("Operación completada.");
                await _navigationServices.GoBackAsync();
            }
            else
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
            }

            LoadUsers();
        }

        private async void ExecuteIncrementMembershipCommand(object obj)
        {
            var (updateSuccessfully, errorMessage) = await _userServices.IncrementMembership(SelectedUser.Id);

            if (updateSuccessfully)
            {
                await _toastMessagesUtility.ShowMessage("Incremento Exitoso");

                NewFechaR = DateTime.Now;
                NewFechaV = NewFechaV.AddMonths(1);
            }
            else
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
            }

            LoadUsers();
        }

        private async void ExecuteDeleteUserCommand(object obj)
        {
            try
            {
                var (memmembershipAvailability, operationComplete, errorMessageVerify) = await _userServices.VerifyMembershipValidity(SelectedUser.Id);

                if (operationComplete)
                {
                    if (memmembershipAvailability)
                    {
                        bool confirm = await Application.Current.MainPage.DisplayAlert("Membresía Activa", $"El usuario {SelectedUser.Username} tiene una membresía activa. ¿Deseas eliminar este usuario?", "Sí", "No");
                        if (confirm)
                        {
                            string result = await Application.Current.MainPage.DisplayPromptAsync("Código de Confirmación", "Ingresa el código del usuario a eliminar:");
                            if (result == SelectedUser.Username)
                            {
                                var (deleteSuccessfully, errorMessage) = await _userServices.Delete(SelectedUser.Id);
                                if (!deleteSuccessfully)
                                {
                                    await Application.Current.MainPage.DisplayAlert("Error", "El usuario no se pudo eliminar de la base de datos: " + errorMessage, "OK");
                                }
                                else
                                {
                                    await _toastMessagesUtility.ShowMessage("Usuario eliminado satisfactoriamente!");
                                    await _navigationServices.GoBackAsync();
                                    LoadUsers();
                                }
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Código de confirmación incorrecto.", "OK");
                            }
                        }
                    }
                    else
                    {
                        bool confirm = await Application.Current.MainPage.DisplayAlert("Eliminar Usuario", $"¿Seguro que desea eliminar al usuario {SelectedUser.Username}?", "Sí", "No");
                        if (confirm)
                        {
                            string result = await Application.Current.MainPage.DisplayPromptAsync("Código de Confirmación", "Ingresa el código del usuario a eliminar:");
                            if (result == SelectedUser.Username)
                            {
                                var (deleteSuccessfully, errorMessage) = await _userServices.Delete(SelectedUser.Id);
                                if (!deleteSuccessfully)
                                {
                                    await Application.Current.MainPage.DisplayAlert("Error", "El usuario no se pudo eliminar de la base de datos.", "OK");
                                }
                                else
                                {
                                    await _toastMessagesUtility.ShowMessage(errorMessage);
                                    await _navigationServices.GoBackAsync();
                                    LoadUsers();
                                }
                            }
                            else
                            {
                                await Application.Current.MainPage.DisplayAlert("Error", "Código de confirmación incorrecto.", "OK");
                            }
                        }
                    }
                }
                else
                {
                    await _toastMessagesUtility.ShowMessage(errorMessageVerify);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                await Application.Current.MainPage.DisplayAlert("Error", "No se pudo verificar la membresía del usuario: " + ex.Message, "OK");
            }       
        }

        private void ExecuteRefreshUserListCommand(object obj)
        {
            LoadUsers();
        }

        private void ExecuteAddNewUserCommand(object obj)
        {
            SetEditingCurrentUserProfileValue(false);
            _navigationServices.NavigateToAsync<UserDetailsView>(this);
        }

        public void EditUser(UserEntity selectedUser)
        {
            // Llamar al método SeleccionarCliente del ViewModel
            Edit(selectedUser);
            _navigationServices.NavigateToAsync<UserDetailsView>(this);
        }

        public void SetEditingCurrentUserProfileValue(bool editingCurrentUserProfile)
        {
            EditingCurrentUserProfile = editingCurrentUserProfile;
            if (EditingCurrentUserProfile)
            {
                Edit(_userServices.GetCurrentUser());
            }
        }

        private void VerifyCodUsuaExists()
        {
            if ((EditionEnable || EditingCurrentUserProfile) && SelectedUser != null && NewCodUsua == SelectedUser.Username)
            {
                CodExists = false;
            }
            else
            {
                CodExists = Users.Any(c => c.Username == NewCodUsua);
            }
        }

        private async void LoadUsers()
        {
            ReloadingUsersList = true;
            var (usersFromDatabase, operationComplete, errorMessage) = await _userServices.GetAll();

            if (operationComplete)
            {
                //Users = null;
                //Users = new ObservableCollection<UserEntity>(usersFromDatabase);
                Users.Clear();
                foreach (var user in usersFromDatabase)
                {
                    Users.Add(user);
                }
            }
            else
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
            }

            ReloadingUsersList = false;
        }

        public void Clean()
        {
            NewCodUsua = string.Empty;
            NewDescrip = string.Empty;
            NewTelef = string.Empty;
            NewEmail = string.Empty;
            NewContra = string.Empty;
            NewConfirmContra = string.Empty;
            NewFechaR = DateTime.Now;
            NewFechaV = DateTime.Now.AddMonths(1);
            NewTipoUsuario = 2;
            NewPIN = string.Empty;
            EditionEnable = false;
            EditingCurrentUserProfile = false;
        }

        private void Edit(UserEntity selectedUser)
        {                
            NewCodUsua = selectedUser.Username;
            NewDescrip = selectedUser.Descrip;
            NewContra = _cryptographyDataUtility.Decrypt(selectedUser.Password);
            NewConfirmContra = _cryptographyDataUtility.Decrypt(selectedUser.Password);
            NewPIN = _cryptographyDataUtility.Decrypt(selectedUser.PIN);
            NewEmail = selectedUser.Email;
            NewTelef = selectedUser.NumberPhone;
            NewFechaR = selectedUser.DateR;
            NewFechaV = selectedUser.DateV;
            NewTipoUsuario = selectedUser.UserType;
            SelectedUser = selectedUser;
            EditionEnable = EditingCurrentUserProfile ? false : true;
        }
    }
}
