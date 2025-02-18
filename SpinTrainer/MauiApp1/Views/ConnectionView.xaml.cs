using SpinTrainer.ViewModels;

namespace SpinningTrainer.Views
{
    public partial class ConnectionView : ContentPage
    {
        public ConnectionView(ConnectionViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}