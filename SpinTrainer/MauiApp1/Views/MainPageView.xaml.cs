using ENTITYS;
using SpinningTrainer.ViewModels;
using System.Collections.ObjectModel;

namespace SpinningTrainer.Views;

public partial class MainPageView : ContentPage
{
    ObservableCollection<SessionEntity> infoSesiones = new ObservableCollection<SessionEntity>();
    private readonly MainPageViewModel _mainPageViewModel;

	public MainPageView(IServiceProvider serviceProvider)
	{
		InitializeComponent();

        _mainPageViewModel = serviceProvider.GetService<MainPageViewModel>();
        this.BindingContext = _mainPageViewModel;

        lvInfoSesiones.ItemsSource = _mainPageViewModel.SessionsObservableCollection;
        sbTextFilter.SearchCommand = _mainPageViewModel.SearchBySessionDescripCommand;
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        var imageButton = (ImageButton)sender;

        var sessionID = (int)imageButton.CommandParameter; // Obtén el ID del parámetro del comando
        await ShowPopupMenu(sessionID);
    }

    private async Task ShowPopupMenu(int sessionID)
    {
        List<string> actions;
        if (_mainPageViewModel.AlreadyConnectedToTV)
        {
            actions = new List<string> { "Duplicar", "Modificar", "Eliminar", "Reproducir" };
        }
        else
        {
            actions = new List<string> { "Duplicar", "Modificar", "Eliminar" };
        }

        var selectedAction = await DisplayActionSheet("Seleccionar opción", "Cancelar", null, actions.ToArray());

        // Aquí puedes manejar la opción seleccionada
        switch (selectedAction)
        {
            case "Duplicar":
                _mainPageViewModel.DuplicateSession(sessionID);
                break;
            case "Modificar":
                _mainPageViewModel.ModifySession(sessionID);
                break;
            case "Eliminar":
                _mainPageViewModel.DeleteSession(sessionID);
                break;
            case "Reproducir":
                _mainPageViewModel.PlaySessionInTV(sessionID);
                break;
            default:
                // Cancelar o ningún botón seleccionado
                break;
        }
    }

    private async void btnCreateNewSession_Clicked(object sender, EventArgs e)
    {
        _mainPageViewModel.CreateNewSession();
    }

    private async void btnViewReports_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new MenuReportView());
    }

    private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        SearchBar searchBar = (SearchBar)sender;

        searchBar.SearchCommand.Execute(null);
    }

    private void sbTextFilter_TextChanged(object sender, TextChangedEventArgs e)
    {
        _mainPageViewModel.TextFilter = sbTextFilter.Text;

        if (sbTextFilter.Text == "")
            sbTextFilter.SearchCommand.Execute(null);
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        await _mainPageViewModel.SetCurrentUser();
    }
}