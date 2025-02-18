using System.Windows.Input;
using Microsoft.Maui.Controls;
using SpinningTrainer.Views;

namespace SpinningTrainer.ViewModels
{
    internal class AppShellViewModel : ViewModelBase
    {
        public ICommand LogoutCommand { get; }
        public ICommand NavigateCommand { get; }
        public AppShellViewModel()
        {
            LogoutCommand = new Command(OnLogout);
            NavigateCommand = new Command<string>(OnNavigate);
        }

        private async void OnLogout()
        {
            // Aquí puedes realizar otras acciones como cerrar sesión antes de la navegación
            await Shell.Current.GoToAsync($"//LoginView");
        }

        private async void OnNavigate(string route)
        {
            // Navegar utilizando una ruta absoluta
            await Shell.Current.GoToAsync($"//{route}");
        }
    }
}