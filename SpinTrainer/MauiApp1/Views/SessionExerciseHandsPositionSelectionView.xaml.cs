using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;
using UTILITIES.ToastMessagesUtility;

namespace SpinTrainer.Views;

public partial class SessionExerciseHandsPositionSelectionView : ContentPage
{
    private readonly INavigationServices _navigationServices;
    private readonly IToastMessagesUtility _toastMessagesUtility;
    public SessionExerciseHandsPositionSelectionView(INavigationServices navigationServices, IToastMessagesUtility toastMessagesUtility)
	{
		InitializeComponent();
        _navigationServices = navigationServices;
        _toastMessagesUtility = toastMessagesUtility;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
        var viewmodel = (NewSessionExerciseViewModel)this.BindingContext;

        if (viewmodel.SelectedHandsPosition?.Number == null)
        {
            await _toastMessagesUtility.ShowMessage("Seleccione una posición de manos.");
            return;
        }

        await _navigationServices.NavigateToAsync<SessionExerciseRpmAndEnergyZoneView>(viewmodel);
    }

}