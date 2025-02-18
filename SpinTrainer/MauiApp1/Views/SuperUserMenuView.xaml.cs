using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;

namespace SpinningTrainer.Views
{
    public partial class SuperUserMenuView : ContentPage
    {
        private readonly INavigationServices _navigationServices;
        private readonly IServiceProvider _serviceProvider;

        public SuperUserMenuView(INavigationServices navigationServices, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _navigationServices = navigationServices;
            _serviceProvider = serviceProvider;
        }
        
        private async void btnOpenSelectionUser_Clicked(object sender, EventArgs e)
        { 
            var viewModel = _serviceProvider.GetService<UsersViewModel>();

            await _navigationServices.NavigateToAsync<UserListView>(viewModel);
        }
    }
}