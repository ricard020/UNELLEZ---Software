using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;

namespace SpinningTrainer.Views
{
    public partial class AdminMenuView : ContentPage
    {
        private readonly INavigationServices _navigationServices;
        private readonly IServiceProvider _serviceProvider;

        public AdminMenuView(INavigationServices navigationServices, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _navigationServices = navigationServices;
            _serviceProvider = serviceProvider;
        }

        private async void btnOpenDataManagment_Clicked(object sender, EventArgs e)
        {
            var viewModel = _serviceProvider.GetService<CompanyDataViewModel>();

            await _navigationServices.NavigateToAsync<CompanyDataView>(viewModel);
        }
    }
}