using SpinningTrainer.ViewModels;
using Microsoft.Maui.Controls;
using Microsoft.Extensions.DependencyInjection;

namespace SpinningTrainer.Views
{
    public partial class UserDetailsView : ContentPage
    {
        private readonly IServiceProvider _serviceProvider;
        
        // Constructor sin parámetros para la edición del perfil de usuario
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

            // Obtiene el índice de la pestaña seleccionada
            int tabIndex = int.Parse(radioButton.Value.ToString());

            // Calcula la nueva posición de la línea resaltada
            double targetPosition = tabIndex * (HighlightLine.Width + 10); // Ajusta el valor de 10 para el espaciado si es necesario

            // Ejecuta la animación de desplazamiento de la línea resaltada
            await HighlightLine.TranslateTo(targetPosition, 0, 250, Easing.CubicInOut);

            // Cambia el contenido visible en función de la pestaña seleccionada
            PrincipalDataPanel.IsVisible = tabIndex == 0;
            AditionalsDetailsPanel.IsVisible = tabIndex == 1;
        }
    }
}