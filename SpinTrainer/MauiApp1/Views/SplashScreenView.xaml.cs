using SpinTrainer.ViewModels;

namespace SpinningTrainer.Views
{
    public partial class SplashScreenView : ContentPage
    {
        private readonly SplashScreenViewModel _viewModel;

        public SplashScreenView(SplashScreenViewModel viewModel)
        {
            InitializeComponent();

            _viewModel = viewModel;
            this.BindingContext = _viewModel;
        }

        private async void ContentPage_Loaded(object sender, EventArgs e)
        {
            await _viewModel.LoadData();
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            while (true)
            {
                await LoadingIcon.RotateTo(360, 6000);
                LoadingIcon.Rotation = 0; 
            }

        }
    }
}