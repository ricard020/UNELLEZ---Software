using SpinTrainer.ViewModels;

namespace SpinningTrainerTV.ViewTV
{
    public partial class ConnectionViewTV : ContentPage
    {        
        public ConnectionViewTV(ConnectionViewModelTV viewmodel)
        {
            InitializeComponent();

            this.BindingContext = viewmodel;
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            entNombreServidor.Focus();
        }

        private void btnVerificarConexion_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Visibility")
            {
                btnGuardarConexion.Focus();
            }
        }
    }
}