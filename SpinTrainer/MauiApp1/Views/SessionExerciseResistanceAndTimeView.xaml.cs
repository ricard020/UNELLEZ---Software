using SERVICES.NavigationServices;
using SpinningTrainer.ViewModels;
using UTILITIES.ToastMessagesUtility;
using Timer = System.Timers.Timer;

namespace SpinTrainer.Views;

public partial class SessionExerciseResistanceAndTimeView : ContentPage
{
    private readonly INavigationServices _navigationServices;
    private readonly IToastMessagesUtility _toastMessagesUtility;
    private Timer _repeatTimer;
    private Action _repeatAction;
    private NewSessionExerciseViewModel _viewModel;

    public SessionExerciseResistanceAndTimeView(INavigationServices navigationServices, IToastMessagesUtility toastMessagesUtility)
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
        if (_viewModel.DurationMin == 0)
        {
            await _toastMessagesUtility.ShowMessage("Ingresa una duración mayor a 0");
            return;
        }

        int totalTimeExercises = _viewModel.DurationMin;

        foreach (var exercise in _viewModel.SelectedExercisesList)
        {
            totalTimeExercises += exercise.DurationMin;
        }

        if (totalTimeExercises > _viewModel.Session.Duration)
        {
            await _toastMessagesUtility.ShowMessage("La duración del ejercicio excede el tiempo total de la sesión");
            return;
        }

        await _navigationServices.NavigateToAsync<SessionExerciseResumeView>(_viewModel);
    }

    private void StartIncreaseDurationMin(object sender, EventArgs e)
    {
        _repeatAction = IncreaseDurationMin;
        _repeatTimer.Start();
        IncreaseDurationMin();
    }

    private void StartDecreaseDurationMin(object sender, EventArgs e)
    {
        _repeatAction = DecreaseDurationMin;
        _repeatTimer.Start();
        DecreaseDurationMin();
    }

    private void IncreaseDurationMin()
    {
        if (_viewModel.DurationMin < _viewModel.Session.Duration)
        {
            _viewModel.DurationMin++;
        }
    }

    private void DecreaseDurationMin()
    {
        if (_viewModel.DurationMin > 1)
        {
            _viewModel.DurationMin--;
        }
    }

    private void StopRepeatAction(object sender, EventArgs e)
    {
        _repeatTimer.Stop();
    }
}
