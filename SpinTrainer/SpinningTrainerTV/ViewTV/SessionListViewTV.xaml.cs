using ENTITYS;
using SERVICES.NavigationServices;
using SpinningTrainerTV.ViewModelsTV;
using System.Collections.ObjectModel;

namespace SpinningTrainerTV.ViewTV;

public partial class SessionListViewTV : ContentPage
{
    ObservableCollection<SessionEntity> infoSesiones = new ObservableCollection<SessionEntity>();
    private readonly SessionListViewModelTV _sessionListViewModel;
    private readonly INavigationServices _navigationServices;
    private readonly IServiceProvider _serviceProvider;

    public SessionListViewTV(INavigationServices navigationServices, SessionListViewModelTV sessionListViewModelTV, IServiceProvider serviceProvider)
    {
        InitializeComponent();

        _navigationServices = navigationServices;
        _sessionListViewModel = sessionListViewModelTV;
        _serviceProvider = serviceProvider;

        this.BindingContext = _sessionListViewModel;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        var viewModel = (SessionListViewModelTV)this.BindingContext;
        viewModel.ReloadViewModel += () => MainThread.BeginInvokeOnMainThread(() => ReloadViewModel());
    }

    private void ReloadViewModel()
    {
        var newViewModel = _serviceProvider.GetService(typeof(SessionListViewModelTV));
        this.BindingContext = newViewModel;
    }
}