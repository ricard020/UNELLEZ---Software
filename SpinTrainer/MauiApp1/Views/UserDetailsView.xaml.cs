using SpinningTrainer.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace SpinningTrainer.Views
{
    public partial class UserDetailsView : ContentPage
    {
        private readonly IServiceProvider _serviceProvider;
        
        // Constructor sin par�metros para la edici�n del perfil de usuario
        public UserDetailsView(IServiceProvider serviceProvider)
        {
            InitializeComponent();
            _serviceProvider = serviceProvider;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (BindingContext is UsersViewModel viewModel)
            {
                viewModel.Clean();
            }
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            if (this.BindingContext.GetType() != typeof(UsersViewModel))
            {
                var viewModel = _serviceProvider.GetService<UsersViewModel>();
                viewModel.SetEditingCurrentUserProfileValue(true);

                this.BindingContext = viewModel;
            }

            rbtPrincipalDataPanel.IsChecked = true;
        }

        private async void OnTabCheckedChanged(object sender, CheckedChangedEventArgs e)
        {
            if (!(sender is RadioButton radioButton) || !e.Value)
                return;

            // Obtiene el �ndice de la pesta�a seleccionada
            int tabIndex = int.Parse(radioButton.Value.ToString());

            // Calcula la nueva posici�n de la l�nea resaltada
            double targetPosition = tabIndex * (HighlightLine.Width + 10); // Ajusta el valor de 10 para el espaciado si es necesario

            // Ejecuta la animaci�n de desplazamiento de la l�nea resaltada
            await HighlightLine.TranslateTo(targetPosition, 0, 250, Easing.CubicInOut);

            // Cambia el contenido visible en funci�n de la pesta�a seleccionada
            PrincipalDataPanel.IsVisible = tabIndex == 0;
            AditionalsDetailsPanel.IsVisible = tabIndex == 1;
        }
    }
}