using SpinningTrainer.ViewModels;
using Timer = System.Timers.Timer;

namespace SpinningTrainer.Views;

public partial class ExerciseConfiguratorView : ContentPage
{
    private ExerciseConfiguratorViewModel _exerciseConfiguratorViewModel;
    private Timer _repeatTimer;
    private Action _repeatAction;
    public ExerciseConfiguratorView()
    {
        InitializeComponent();

        _repeatTimer = new Timer(50); // Velocidad de incremento/decremento continuo
        _repeatTimer.Elapsed += (s, e) => MainThread.BeginInvokeOnMainThread(() => _repeatAction?.Invoke());

    }
    protected override void OnAppearing()
    {
        base.OnAppearing();
        _exerciseConfiguratorViewModel = (ExerciseConfiguratorViewModel)this.BindingContext;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _exerciseConfiguratorViewModel.ClearSelection();
    }
    private void OnPickerButtonClicked(object sender, EventArgs e)
    {
        exercisePicker.Focus();
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

    private void StartDecreaseDurationMin(object sender, EventArgs e)
    {
        _repeatAction = DecreaseDurationMin;
        _repeatTimer.Start();
        DecreaseDurationMin();
    }

    private void StartIncreaseDurationMin(object sender, EventArgs e)
    {
        _repeatAction = IncreaseDurationMin;
        _repeatTimer.Start();
        IncreaseDurationMin();
    }

    private void IncreaseDurationMin()
    {
        _exerciseConfiguratorViewModel.DurationMin++;
    }

    private void DecreaseDurationMin()
    {
        if(_exerciseConfiguratorViewModel.DurationMin <= 0)
        {
            return;
        }

        _exerciseConfiguratorViewModel.DurationMin--;

    }

    private void IncreaseRPMMed()
    {
        if (_exerciseConfiguratorViewModel.RPMMed < _exerciseConfiguratorViewModel.RPMFin)
        {
            _exerciseConfiguratorViewModel.RPMMed++;
        }
    }

    private void DecreaseRPMMed()
    {
        if (_exerciseConfiguratorViewModel.RPMMed > _exerciseConfiguratorViewModel.SelectedExercise.RPMMin)
        {
            _exerciseConfiguratorViewModel.RPMMed--;
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
        if (_exerciseConfiguratorViewModel.RPMFin < _exerciseConfiguratorViewModel.SelectedExercise.RPMMax)
        {
            _exerciseConfiguratorViewModel.RPMFin++;
        }
    }

    private void DecreaseRPMFin()
    {
        if (_exerciseConfiguratorViewModel.RPMFin > _exerciseConfiguratorViewModel.RPMMed)
        {
            _exerciseConfiguratorViewModel.RPMFin--;
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
        if (_exerciseConfiguratorViewModel.ResistancePercentage < 100)
        {
            _exerciseConfiguratorViewModel.ResistancePercentage++;
        }
    }

    private void DecreaseResistance()
    {
        if (_exerciseConfiguratorViewModel.ResistancePercentage > 0)
        {
            _exerciseConfiguratorViewModel.ResistancePercentage--;
        }
    }

    private void StopRepeatAction(object sender, EventArgs e)
    {
        _repeatTimer.Stop();
    }
   
}
