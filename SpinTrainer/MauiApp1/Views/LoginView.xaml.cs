using SERVICES.NavigationServices;
using SpinningTrainer.View;
using SpinningTrainer.ViewModels;
using MauiIcons.Core;
using MauiIcons.Material;

namespace SpinningTrainer.Views
{
    public partial class LoginView : ContentPage
    {
        private readonly INavigationServices _navigationServices;
        private readonly IServiceProvider _serviceProvider;

        public LoginView(IServiceProvider serviceProvider, INavigationServices navigationServices)
        {
            InitializeComponent();
            this.BindingContext = serviceProvider.GetService<LoginViewModel>();
            _serviceProvider = serviceProvider;
            _navigationServices = navigationServices;
        }

        private async void tgrRecuperarDatos_Tapped(object sender, TappedEventArgs e)
        {
            var recoveryLoginDataViewModel = _serviceProvider.GetService<RecoveryLoginDataViewModel>();
            
            await _navigationServices.NavigateToAsync<RecoveryLoginDataView>(recoveryLoginDataViewModel);
        }

        private void imgbtnShowOrHidePassword_Clicked(object sender, EventArgs e)
        {
            if (contraIngresadaEntry.IsPassword)
            {
                contraIngresadaEntry.IsPassword = false;
                imgbtnShowOrHidePassword.Icon(MaterialIcons.Visibility);
            }
            else
            {
                contraIngresadaEntry.IsPassword = true;
                imgbtnShowOrHidePassword.Icon(MaterialIcons.VisibilityOff);
            }
        }

        private async void ImageButton_Pressed(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync($"//{nameof(ConnectionView)}");
        }
    }
}
