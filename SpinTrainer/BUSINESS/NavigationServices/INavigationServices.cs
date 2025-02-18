namespace SERVICES.NavigationServices
{
    public interface INavigationServices
    {
        Task NavigateToAsync<TPage>() where TPage : Page;
        Task NavigateToAsync<TPage>(object viewModel) where TPage : Page;
        Task GoBackAsync();
    }
}
