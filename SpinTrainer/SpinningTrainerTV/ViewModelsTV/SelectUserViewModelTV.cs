using ENTITYS;
using SERVICES.NavigationServices;
using SERVICES.UserServices;
using SpinningTrainerTV.ViewTV;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UTILITIES.ToastMessagesUtility;

namespace SpinningTrainerTV.ViewModelsTV
{
    public class SelectUserViewModelTV : ViewModelBaseTV
    {
        private ObservableCollection<UserEntity> _usersList = new ObservableCollection<UserEntity>();
        private UserEntity _selectedUser;

        public ObservableCollection<UserEntity> UsersList 
        {
            get => _usersList; 
            set 
            {
                _usersList = value; 
                OnPropertyChanged(nameof(UsersList));
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

        private readonly IUserServices _userServices;
        private readonly IToastMessagesUtility _toastMessagesUtility;
        private readonly IServiceProvider _serviceProvider;
        private readonly INavigationServices _navigationServices;

        public SelectUserViewModelTV(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _userServices = _serviceProvider.GetService<IUserServices>();
            _toastMessagesUtility = _serviceProvider.GetService<IToastMessagesUtility>();
            _navigationServices = _serviceProvider.GetRequiredService<INavigationServices>();

            LoadUserList();
        }

        public async Task SelectedUserChanged(UserEntity selectedUser)
        {
            var viewmodel = _serviceProvider.GetService<RequestUserPINViewModelTV>();

            viewmodel.SetCurrentUser(selectedUser);

            await _navigationServices.NavigateToAsync<RequestUserPINViewTV>(viewmodel);
         }

        private async void LoadUserList()
        {
            UsersList.Clear();

            var (users, operationComplete, errorMessage) = await _userServices.GetAllTrainers();

            if (operationComplete)
            {
                foreach (var item in users)
                {
                    UsersList.Add(item);
                }
            }
            else
            {
                await _toastMessagesUtility.ShowMessage(errorMessage);
            }
        }
    }
}
