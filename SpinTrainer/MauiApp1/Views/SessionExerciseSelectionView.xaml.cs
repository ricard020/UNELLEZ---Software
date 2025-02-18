using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;
using SpinTrainer.Views;
using UTILITIES.ToastMessagesUtility;

namespace SpinningTrainer.Views;

public partial class SessionExerciseSelectionView : ContentPage
{
	private readonly INavigationServices _navigationServices;
	private readonly IToastMessagesUtility _toastMessagesUtility;
	public SessionExerciseSelectionView(INavigationServices navigationServices, IToastMessagesUtility toastMessagesUtility)
	{
		InitializeComponent();
        _navigationServices = navigationServices;
        _toastMessagesUtility = toastMessagesUtility;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {
		var viewmodel = (NewSessionExerciseViewModel)this.BindingContext;

		if (viewmodel.SelectedExercise != null)
		{
			await _navigationServices.NavigateToAsync<SessionExerciseHandsPositionSelectionView>(viewmodel);
		}
		else
			await _toastMessagesUtility.ShowMessage("Seleccione un ejercicio");
    }
}