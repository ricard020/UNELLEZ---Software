namespace SERVICES.NavigationServices
{
    public class NavigationServices : INavigationServices
    {
        private readonly IServiceProvider _serviceProvider;

        public NavigationServices(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        // Opción 1: Navegación con asignación automática del ViewModel
        public async Task NavigateToAsync<TPage>() where TPage : Page
        {
            var page = _serviceProvider.GetService<TPage>();
            var viewModelType = typeof(TPage).Name.Replace("View", "ViewModel"); // Suponiendo convención Page -> ViewModel
            var viewModel = _serviceProvider.GetService(Type.GetType(viewModelType));

            if (page != null && viewModel != null)
            {
                page.BindingContext = viewModel;
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
        }

        // Opción 2: Navegación pasando manualmente el ViewModel
        public async Task NavigateToAsync<TPage>(object viewModel) where TPage : Page
        {
            var page = _serviceProvider.GetService<TPage>();

            if (page != null && viewModel != null)
            {
                page.BindingContext = viewModel;
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
        }

        public async Task GoBackAsync()
        {
            await Application.Current.MainPage.Navigation.PopAsync();
        }
    }
}
