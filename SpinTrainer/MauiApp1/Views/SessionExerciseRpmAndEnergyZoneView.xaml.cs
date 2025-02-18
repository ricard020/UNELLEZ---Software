using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;
using UTILITIES.ToastMessagesUtility;
using Timer = System.Timers.Timer;

namespace SpinTrainer.Views;

public partial class SessionExerciseRpmAndEnergyZoneView : ContentPage
{
    private readonly INavigationServices _navigationServices;
    private readonly IToastMessagesUtility _toastMessagesUtility;
    private Timer _repeatTimer;
    private Action _repeatAction;
    private NewSessionExerciseViewModel _viewModel;

    public SessionExerciseRpmAndEnergyZoneView(INavigationServices navigationServices, IToastMessagesUtility toastMessagesUtility)
    {
        InitializeComponent();
        _navigationServices = navigationServices;
        _toastMessagesUtility = toastMessagesUtility;
        _repeatTimer = new Timer(50);
        _repeatTimer.Elapsed += (s, e) => MainThread.BeginInvokeOnMainThread(() => _repeatAction?.Invoke());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel = (NewSessionExerciseViewModel)this.BindingContext;
    }

    private async void Button_Clicked(object sender, EventArgs e)
    {

        int intensityPercentageFrom = (int)Math.Round((0.3 * (double)_viewModel.RPMMed) + (0.7 * (double)_viewModel.ResistancePercentage) + 20);
        int intensityPercentageTo = (int)Math.Round((0.3 * (double)_viewModel.RPMFin) + (0.7 * (double)_viewModel.ResistancePercentage) + 20);

        if (!(_viewModel.RPMMed >= _viewModel.SelectedExercise.RPMMin))
        {
            await _toastMessagesUtility.ShowMessage("Ingresa un valor de RPM Med. válido");
            return;
        }

        if (!(_viewModel.RPMFin <= _viewModel.SelectedExercise.RPMMax))
        {
            await _toastMessagesUtility.ShowMessage("Ingresa un valor de RPM Final válido");
            return;
        }

        if (!(_viewModel.ResistancePercentage >= 20 && _viewModel.ResistancePercentage <= 100))
        {
            await _toastMessagesUtility.ShowMessage("Ingresa un valor de porcentaje de Resistencia válido");
            return;
        }

        if (!(_viewModel.RPMMed <= _viewModel.RPMFin))
        {
            await _toastMessagesUtility.ShowMessage("RPM Med no puede ser mayor que RPM Fin");
            return;
        }



        if(intensityPercentageFrom < 50)
        {
            await _toastMessagesUtility.ShowMessage("La intensidad percibida de la zona de energía de inicio es muy baja.");
            return;

        }

        if (intensityPercentageTo > 100)
        {
            await _toastMessagesUtility.ShowMessage("La intensidad percibida de la zona de energía final es muy alta.");
            return;

        }

        await _navigationServices.NavigateToAsync<SessionExerciseResistanceAndTimeView>(_viewModel);
    }

    private void StartIncreaseRPMMed(object sender, EventArgs e)
    {
        _repeatAction = IncreaseRPMMed;
        _repeatTimer.Start();
        IncreaseRPMMed();
    }

    private void StartDecreaseRPMMed(object sender, EventArgs e)
    {
        _repeatAction = DecreaseRPMMed;
        _repeatTimer.Start();
        DecreaseRPMMed();
    }

    private void IncreaseRPMMed()
    {
        if (_viewModel.RPMMed < _viewModel.RPMFin)
        {
            _viewModel.RPMMed++;
        }
    }

    private void DecreaseRPMMed()
    {
        if (_viewModel.RPMMed > _viewModel.SelectedExercise.RPMMin)
        {
            _viewModel.RPMMed--;
        }
    }

    private void StartIncreaseRPMFin(object sender, EventArgs e)
    {
        _repeatAction = IncreaseRPMFin;
        _repeatTimer.Start();
        IncreaseRPMFin();
    }

    private void StartDecreaseRPMFin(object sender, EventArgs e)
    {
        _repeatAction = DecreaseRPMFin;
        _repeatTimer.Start();
        DecreaseRPMFin();
    }

    private void IncreaseRPMFin()
    {
        if (_viewModel.RPMFin < _viewModel.SelectedExercise.RPMMax)
        {
            _viewModel.RPMFin++;
        }
    }

    private void DecreaseRPMFin()
    {
        if (_viewModel.RPMFin > _viewModel.RPMMed)
        {
            _viewModel.RPMFin--;
        }
    }

    private void StartIncreaseResistance(object sender, EventArgs e)
    {
        _repeatAction = IncreaseResistance;
        _repeatTimer.Start();
        IncreaseResistance();
    }

    private void StartDecreaseResistance(object sender, EventArgs e)
    {
        _repeatAction = DecreaseResistance;
        _repeatTimer.Start();
        DecreaseResistance();
    }

    private void IncreaseResistance()
    {
        if (_viewModel.ResistancePercentage < 100)
        {
            _viewModel.ResistancePercentage++;
        }
    }

    private void DecreaseResistance()
    {
        if (_viewModel.ResistancePercentage > 0)
        {
            _viewModel.ResistancePercentage--;
        }
    }

    private void StopRepeatAction(object sender, EventArgs e)
    {
        _repeatTimer.Stop();
    }
}
