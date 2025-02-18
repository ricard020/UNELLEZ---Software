using SpinningTrainer.ViewModels;
using Timer = System.Timers.Timer;

namespace SpinningTrainer.Views;

public partial class SessionExerciseFormView : ContentPage
{
    private NewSessionExerciseViewModel _sessionExerciseViewModel;
    private Timer _repeatTimer;
    private Action _repeatAction;

    public SessionExerciseFormView()
    {
        InitializeComponent();
        _repeatTimer = new Timer(50); // Velocidad de incremento/decremento continuo
        _repeatTimer.Elapsed += (s, e) => MainThread.BeginInvokeOnMainThread(() => _repeatAction?.Invoke());
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _sessionExerciseViewModel = (NewSessionExerciseViewModel)this.BindingContext;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _sessionExerciseViewModel.ClearSelection();
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
        if (_sessionExerciseViewModel.RPMMed < _sessionExerciseViewModel.RPMFin)
        {
            _sessionExerciseViewModel.RPMMed++;
        }
    }

    private void DecreaseRPMMed()
    {
        if (_sessionExerciseViewModel.RPMMed > _sessionExerciseViewModel.SelectedExercise.RPMMin)
        {
            _sessionExerciseViewModel.RPMMed--;
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
        if (_sessionExerciseViewModel.RPMFin < _sessionExerciseViewModel.SelectedExercise.RPMMax)
        {
            _sessionExerciseViewModel.RPMFin++;
        }
    }

    private void DecreaseRPMFin()
    {
        if (_sessionExerciseViewModel.RPMFin > _sessionExerciseViewModel.RPMMed)
        {
            _sessionExerciseViewModel.RPMFin--;
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
        if (_sessionExerciseViewModel.ResistancePercentage < 100)
        {
            _sessionExerciseViewModel.ResistancePercentage++;
        }
    }

    private void DecreaseResistance()
    {
        if(_sessionExerciseViewModel.ResistancePercentage > 0)
        {
            _sessionExerciseViewModel.ResistancePercentage--;
        }
    }

    private void StopRepeatAction(object sender, EventArgs e)
    {
        _repeatTimer.Stop();
    }
}
