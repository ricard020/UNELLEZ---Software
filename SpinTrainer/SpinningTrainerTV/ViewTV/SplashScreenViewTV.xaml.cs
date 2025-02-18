using SpinningTrainerTV.ViewModelsTV;

namespace SpinningTrainerTV.ViewTV
{
    public partial class SplashScreenViewTV : ContentPage
    {
        private readonly SplashScreenViewModelTV _viewModel;

        public SplashScreenViewTV(SplashScreenViewModelTV viewModel)
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