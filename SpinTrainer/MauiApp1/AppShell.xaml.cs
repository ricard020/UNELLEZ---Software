using SERVICES.DatabaseServices;
using SERVICES.NavigationServices;
using SpinningTrainer.Views;

namespace SpinningTrainer
{
    public partial class AppShell : Shell
    {
        private readonly IDatabaseServices _databaseServices;
        private readonly INavigationServices _navigationServices;

        public AppShell(INavigationServices navigationServices, IDatabaseServices databaseServices)
        {
            InitializeComponent();
            _navigationServices = navigationServices;
            _databaseServices = databaseServices;
            Routing.RegisterRoute(nameof(MainPageView), typeof(MainPageView));
        }

        public void SetUserType(int userType)
        {
            if (userType == 0) // Super Usuario
            {
                SuperUserMenu.IsVisible = true;
                AdminMenu.IsVisible = false;
                TrainerMenu.IsVisible = false;
            }
            else if (userType == 1) // Administrador
            {
                SuperUserMenu.IsVisible = false;
                AdminMenu.IsVisible = true;
                TrainerMenu.IsVisible = false;
            }
            else if (userType == 2) // Entrenador
            {
                var isOnline = _databaseServices.GetIsOnlineValue();

                SuperUserMenu.IsVisible = false;
                AdminMenu.IsVisible = false;
                TrainerMenu.IsVisible = isOnline ? true : false;
                TrainerMenuOffline.IsVisible = !isOnline ? true : false;
            }
        }
    }
}