using ENTITYS;
using System.Timers;
using Timer = System.Timers.Timer;
using SERVICES.SessionServices;
using UTILITIES.ToastMessagesUtility;
using SpinningTrainerTV.Socket;
using SERVICES.UserServices;
using Plugin.Maui.Audio;
using AudioManager = Plugin.Maui.Audio.AudioManager;
using SERVICES.NavigationServices;
using System.Net;
using SpinningTrainerTV.Resources.Charts;
using SERVICES.ExerciseTemplateServices;

namespace SpinningTrainerTV.ViewModelsTV;

public class PlaySessionViewModelTV : ViewModelBaseTV
{
    private UserEntity _currentUser;
    private bool _initialized = false;

    private SessionEntity _session;
    private string _sessionDescrip;
    private List<SessionExercisesEntity> _exerciseList;
    private SessionExercisesEntity _restingExercise;
    private DateTime _dateInitializeSession;
    private DateTime _dateFinishedSession;
    private string[,] _rankedEnergyZone;

    private double _currentRPMValue;
    private double _currentResistanceValue;
    private double _currentTargetRPM;
    private double _currentResistancePercentageTarget = 0;
    private string _currentRangeFCM;
    private string _currentExerciseName;
    private string _currentRPMRange;
    private ImageSource _currentHandsPositionImage;
    private ImageSource _currentExerciseImage;
    private string _currentExerciseImageName;
    private int _currentExerciseIndex = 0;
    private Timer _timer;
    private SessionExercisesEntity _currentExercise;
    private DateTime _startTime;
    private TimeSpan _targetTime;
    private TimeSpan _changeExerciseCountdown;
    private TimeSpan _elapsedTime;
    private TimeSpan _elapsedExerciseTime;

    public double CurrentTargetRPM
    {
        get => _currentTargetRPM;
        set
        {
            if (_currentTargetRPM != value)
            {
                _currentTargetRPM = value;
                OnPropertyChanged(nameof(CurrentTargetRPM));
            }
        }
    }
    public double CurrentResistanceValue
    {
        get => _currentResistanceValue;
        set
        {
            if (_currentResistanceValue != value)
            {
                _currentResistanceValue = value;
                OnPropertyChanged(nameof(CurrentResistanceValue));
            }
        }
    }

    public double CurrentResistancePercentageTarget
    {
        get => _currentResistancePercentageTarget;
        set
        {
            _currentResistancePercentageTarget = value;
            OnPropertyChanged(nameof(CurrentResistancePercentageTarget));
        }
    }
    public double CurrentRPMValue
    {
        get => _currentRPMValue;
        set
        {
            if (_currentRPMValue != value)
            {
                _currentRPMValue = value;
                OnPropertyChanged(nameof(CurrentRPMValue));
            }
        }
    }
    public SessionEntity Session 
    { 
        get => _session; 
        set 
        {
            _session = value;
            OnPropertyChanged(nameof(Session));
        }
    }
    public List<SessionExercisesEntity> ExerciseList
    {
        get => _exerciseList;
        set
        {
            _exerciseList = value;
            OnPropertyChanged(nameof(ExerciseList));
        }
    }
    public DateTime DateInitializeSession
    {
        get => _dateInitializeSession;
        set
        {
            _dateInitializeSession = value;
            OnPropertyChanged(nameof(DateInitializeSession));
        }
    }
    public DateTime DateFinishedSession
    {
        get => _dateFinishedSession;
        set
        {
            _dateFinishedSession = value;
            OnPropertyChanged(nameof(DateFinishedSession));
        }
    }
    public string[,] RankedEnergyZone
    {
        get => _rankedEnergyZone;
        set
        {
            _rankedEnergyZone = value;
            OnPropertyChanged(nameof(RankedEnergyZone));
        }
    }
    public SessionExercisesEntity CurrentExercise
    {
        get => _currentExercise; 
        set 
        {
            _currentExercise = value;
            OnPropertyChanged(nameof(CurrentExercise));
        }
    }
    public TimeSpan ElapsedTime 
    {
        get => _elapsedTime; 
        set
        {
            _elapsedTime = value;
            OnPropertyChanged(nameof(ElapsedTime));
        }
    }
    public TimeSpan ElapsedExerciseTime 
    { 
        get => _elapsedExerciseTime; 
        set 
        {
            _elapsedExerciseTime = value; 
            OnPropertyChanged(nameof(ElapsedExerciseTime));
        }
    }
    public string CurrentExerciseName 
    { 
        get => _currentExerciseName; 
        set 
        {
            _currentExerciseName = value;
            OnPropertyChanged(nameof(CurrentExerciseName));
        }
    }
    public string CurrentRPMRange
    {
        get => _currentRPMRange;
        set
        {
            _currentRPMRange = value;
            OnPropertyChanged(nameof(CurrentRPMRange));
        }
    }
    public string SessionDescrip 
    {
        get => _sessionDescrip; 
        set 
        {
            _sessionDescrip = value; 
            OnPropertyChanged(nameof(SessionDescrip));
        }
    }
    public string CurrentRangeFCM
    { 
        get => _currentRangeFCM;
        set 
        {
            _currentRangeFCM = value;
            OnPropertyChanged(nameof(CurrentRangeFCM));
        }
    }

    public ImageSource CurrentHandsPositionImage 
    {
        get => _currentHandsPositionImage; 
        set 
        {
            _currentHandsPositionImage = value;
            OnPropertyChanged(nameof(CurrentHandsPositionImage));
        }
    }
    public ImageSource CurrentExerciseImage
    {
        get => _currentExerciseImage;
        set
        {
            _currentExerciseImage = value; 
            OnPropertyChanged(nameof(CurrentExerciseImage));
        }
    }

    private SessionListViewModelTV _sessionListViewModelTV;
    
    private IAudioManager _audioManager;
    private readonly DeviceResponder _deviceResponder;
    private readonly ISessionServices _sessionServices;
    private readonly INavigationServices _navigationServices;
    private readonly IServiceProvider _serviceProvider;
    private readonly IToastMessagesUtility _toastMessagesUtility;
    private readonly IExerciseTemplateServices _exerciseTemplateServices;

    public event Action<IPEndPoint>? GetCurrentGraphData;
    public event Action? StartGraphAnimation;
    public event Action? NextExercise;
    public event Action? PreviousExercise;
    public event Action? PauseOrPlayGraphAnimation;
    public event Action? InsertRestingExercise;
    public event Action<int>? ModifyExerciseIntensity;

    public PlaySessionViewModelTV(ISessionServices sessionServices,
                                  IToastMessagesUtility toastMessagesUtility,
                                  DeviceResponder deviceResponder,
                                  IUserServices userServices,
                                  INavigationServices navigationServices,
                                  IExerciseTemplateServices exerciseTemplateServices)
    {
        _sessionServices = sessionServices;
        _toastMessagesUtility = toastMessagesUtility;
        _deviceResponder = deviceResponder;
        _currentUser = userServices.GetCurrentUser();
        _exerciseTemplateServices = exerciseTemplateServices;

        _audioManager = AudioManager.Current;

        ElapsedTime = TimeSpan.Zero;

        _timer = new Timer(100); // Intervalo del temporizador en milisegundos (0.1 segundo)
        _timer.Elapsed += TimerElapsed;

        ControlSession();
    }   

    public List<DataPoint> LoadExercisesInChart()
    {
        List<DataPoint> _dataPoints = new List<DataPoint> { new DataPoint(0, 50, 0, Colors.Black) };

        foreach (var exercise in ExerciseList)
        {
            int intensityPercentageFrom = (int)Math.Round((0.3 * (double)exercise.RPMMed) + (0.7 * (double)exercise.ResistancePercentage) + 20);

            var time = (float)_dataPoints[^1].Time;

            float valueToIncrement = (float)exercise.DurationMin / 2;
            
            time = time + valueToIncrement;

            _dataPoints.Add(new DataPoint(time, intensityPercentageFrom, (double)exercise.DurationMin/2, Colors.Black));

            int intensityPercentageTo = (int)Math.Round((0.3 * (double)exercise.RPMFin) + (0.7 * (double)exercise.ResistancePercentage) + 20);

            time += valueToIncrement;

            _dataPoints.Add(new DataPoint(time, intensityPercentageTo, (double)exercise.DurationMin/2, Colors.Black));
        }

        return _dataPoints;
    }

    private void ExecuteStartIncrement()
    {        
        InitiliceReproduction();
        _initialized = true;
        _startTime = DateTime.Now;
        _timer.Start();
        StartGraphAnimation?.Invoke();
    }

    private void InitiliceReproduction()
    {
        if (_currentExerciseIndex == 0)
        {
            this.CurrentExercise = ExerciseList[0];
            CurrentTargetRPM = this.CurrentExercise.RPMMed;
                        
            TimeSpan originalTimeSpan = TimeSpan.FromMinutes(CurrentExercise.DurationMin); // Ejemplo de 10 minutos            
            _targetTime = TimeSpan.FromMilliseconds(originalTimeSpan.TotalMilliseconds);
            _changeExerciseCountdown = _targetTime;
            ElapsedExerciseTime = TimeSpan.FromMilliseconds(_targetTime.TotalMilliseconds / 2);

            CurrentResistancePercentageTarget = CurrentExercise.ResistancePercentage;
            CurrentExerciseName = CurrentExercise.DescripMov;
            CurrentRPMRange = $"00 - {CurrentExercise.RPMMed} - {CurrentExercise.RPMFin}";
            CurrentHandsPositionImage = ImageSource.FromFile($"hp{CurrentExercise.HandsPosition}.jpeg");
            SetExerciseImage(CurrentExerciseName);

            if (CurrentExerciseName == "Plano Sentado")
                CurrentRangeFCM = "50% - 75%";
            else if (CurrentExerciseName == "Plano de Pie / Correr")
                CurrentRangeFCM = "75% - 85%";
            else if (CurrentExerciseName == "Saltos")
                CurrentRangeFCM = "75% - MAX";
            else if (CurrentExerciseName == "Escalada sentado")
                CurrentRangeFCM = "75% - 85%";
            else if (CurrentExerciseName == "Escalada de Pie")
                CurrentRangeFCM = "80% - MAX";
            else if (CurrentExerciseName == "Correr en Montaña")
                CurrentRangeFCM = "75% - 85%";
            else if (CurrentExerciseName == "Saltos en Montaña")
                CurrentRangeFCM = "85% - MAX";
            else if (CurrentExerciseName == "Sprints en Plano")
                CurrentRangeFCM = "85% - MAX";
            else if (CurrentExerciseName == "Sprints en Montaña")
                CurrentRangeFCM = "85% - MAX";
        }
        else
        {
            ChangeExercise(true);
        }
    }

    private void ChangeExercise(bool nextExercise)
    {
        string rpmINI = "";
        
        if (nextExercise)
        {
            rpmINI = CurrentExercise.RPMFin.ToString();
            _currentExerciseIndex++;
        }
        else
        {
            try
            {
                rpmINI = ExerciseList[_currentExerciseIndex - 2].RPMFin.ToString();
            }
            catch (ArgumentOutOfRangeException)
            {
                rpmINI = "00";
            }
            
            _currentExerciseIndex--;
        }

        _startTime = DateTime.Now;

        this.CurrentExercise = ExerciseList[_currentExerciseIndex];
        CurrentTargetRPM = this.CurrentExercise.RPMMed;

        TimeSpan originalTimeSpan = TimeSpan.FromMinutes(CurrentExercise.DurationMin); // Ejemplo de 10 minutos            
        _targetTime = TimeSpan.FromMilliseconds(originalTimeSpan.TotalMilliseconds);
        _changeExerciseCountdown = _targetTime;
        ElapsedExerciseTime = TimeSpan.FromMilliseconds(_targetTime.TotalMilliseconds / 2);

        CurrentResistancePercentageTarget = CurrentExercise.ResistancePercentage;
        CurrentExerciseName = CurrentExercise.DescripMov;
        CurrentRPMRange = $"{rpmINI} - {CurrentExercise.RPMMed} - {CurrentExercise.RPMFin}";
        CurrentHandsPositionImage = ImageSource.FromFile($"hp{CurrentExercise.HandsPosition}.jpeg");
        SetExerciseImage(CurrentExerciseName);

        if (CurrentExerciseName == "Plano Sentado")
            CurrentRangeFCM = "50% - 75%";
        else if (CurrentExerciseName == "Plano de Pie / Correr")
            CurrentRangeFCM = "75% - 85%";
        else if (CurrentExerciseName == "Saltos")
            CurrentRangeFCM = "75% - MAX";
        else if (CurrentExerciseName == "Escalada sentado")
            CurrentRangeFCM = "75% - 85%";
        else if (CurrentExerciseName == "Escalada de Pie")
            CurrentRangeFCM = "80% - MAX";
        else if (CurrentExerciseName == "Correr en Montaña")
            CurrentRangeFCM = "75% - 85%";
        else if (CurrentExerciseName == "Saltos en Montaña")
            CurrentRangeFCM = "85% - MAX";
        else if (CurrentExerciseName == "Sprints en Plano")
            CurrentRangeFCM = "85% - MAX";
        else if (CurrentExerciseName == "Sprints en Montaña")
            CurrentRangeFCM = "85% - MAX";
    }

    private int _alternateImageCountdown = 0;
    private bool _endingExercise = false;

    private async void TimerElapsed(object sender, ElapsedEventArgs e)
    {
        if (ElapsedExerciseTime.TotalMilliseconds == 8000)
        {
            if (_endingExercise && _currentExerciseIndex != ExerciseList.Count-1)
            {
                var audioPlayer = _audioManager.CreatePlayer(await FileSystem.OpenAppPackageFileAsync("changeexercise.mp3"));

                audioPlayer.Volume = 1;
                audioPlayer.Play();

            }
        }
        
        if (_alternateImageCountdown == 0)
            _alternateImageCountdown = 30;

        // Calcular cuánto tiempo ha pasado desde que comenzó
        TimeSpan elapsedTime = DateTime.Now - _startTime;
        ElapsedTime = ElapsedTime.Add(TimeSpan.FromMilliseconds(100));
        ElapsedExerciseTime = ElapsedExerciseTime.Add(TimeSpan.FromMilliseconds(-100));
        _changeExerciseCountdown = _changeExerciseCountdown.Add(TimeSpan.FromMilliseconds(-100)); ;

        // Calcular el progreso basado en el tiempo transcurrido
        double progress = elapsedTime.TotalMilliseconds / _targetTime.TotalMilliseconds;

        //Alternar entre imágenes de ejercicios que tengan más de una
        
        _alternateImageCountdown--;
        if (_alternateImageCountdown == 0)
        {
            if(CurrentExerciseName == "Saltos")
            {
                if (_currentExerciseImageName == "saltos1")
                {
                    CurrentExerciseImage = ImageSource.FromFile("saltos2.png");
                    _currentExerciseImageName = "saltos2";
                }
                else
                {
                    CurrentExerciseImage = ImageSource.FromFile("saltos1.png");
                    _currentExerciseImageName = "saltos1";
                }
            }
            else if(CurrentExerciseName == "Saltos en Montaña")
            {
                if (_currentExerciseImageName == "saltosenmontana1")
                {
                    CurrentExerciseImage = ImageSource.FromFile("saltosenmontana2.png");
                    _currentExerciseImageName = "saltosenmontana2";
                }
                else
                {
                    CurrentExerciseImage = ImageSource.FromFile("saltosenmontana1.png");
                    _currentExerciseImageName = "saltosenmontana1";
                }
            }
            else if (CurrentExerciseName == "Sprints en Montaña")
            {
                if (_currentExerciseImageName == "sprintsenmontana1")
                {
                    CurrentExerciseImage = ImageSource.FromFile("sprintsenmontana2.png");
                    _currentExerciseImageName = "sprintsenmontana2";
                }
                else
                {
                    CurrentExerciseImage = ImageSource.FromFile("sprintsenmontana1.png");
                    _currentExerciseImageName = "sprintsenmontana1";
                }
            }
            else if (CurrentExerciseName == "Sprints en Plano")
            {
                if (_currentExerciseImageName ==  "sprintsenplano1")
                {
                    CurrentExerciseImage = ImageSource.FromFile("sprintsenplano2.png");
                    _currentExerciseImageName = "sprintsenplano2";
                }
                else
                {
                    CurrentExerciseImage = ImageSource.FromFile("sprintsenplano1.png");
                    _currentExerciseImageName = "sprintsenplano1";
                }
            }
        }

        // Calcular el valor actual basado en el progreso
        if (CurrentTargetRPM > _currentRPMValue)
        {
            CurrentRPMValue = System.Math.Min(CurrentTargetRPM, _currentRPMValue + progress * (CurrentTargetRPM - _currentRPMValue));
        }
        else if (CurrentTargetRPM < _currentRPMValue)
        {
            CurrentRPMValue = System.Math.Max(CurrentTargetRPM, _currentRPMValue - progress * (_currentRPMValue - CurrentTargetRPM));
        }

        // Calcular el valor actual basado en el progreso
        if (CurrentResistancePercentageTarget > CurrentResistanceValue)
        {
            CurrentResistanceValue = System.Math.Min(CurrentResistancePercentageTarget, CurrentResistanceValue + progress * (CurrentResistancePercentageTarget - CurrentResistanceValue));
        }
        else if (CurrentResistancePercentageTarget < CurrentResistanceValue)
        {
            CurrentResistanceValue = System.Math.Max(CurrentResistancePercentageTarget, CurrentResistanceValue - progress * (CurrentResistanceValue - CurrentResistancePercentageTarget));
        }

        // Verificar si hemos alcanzado o superado el objetivo
        if (ElapsedExerciseTime.TotalMilliseconds == 0)
        {            
            if (_changeExerciseCountdown.TotalMilliseconds == 0)
            {
                if (_currentExerciseIndex == ExerciseList.Count - 1)
                {
                    try
                    {
                        //_timer.Stop();
                        //PauseOrPlayGraphAnimation?.Invoke();
                        //_timer.Enabled = false;

                        return;
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                }
                else
                {
                    ChangeExercise(true);
                    _endingExercise = false;
                }                               
            }
            else
            {
                _startTime = DateTime.Now;                
                ElapsedExerciseTime = TimeSpan.FromMilliseconds(_targetTime.TotalMilliseconds / 2);
                CurrentTargetRPM = CurrentExercise.RPMFin;
                CurrentResistancePercentageTarget = CurrentExercise.ResistancePercentage;
                _endingExercise = true;
            }
        }
    }

    private void SetExerciseImage(string exerciseName)
    {
        switch (exerciseName)
        {
            case "Plano Sentado":
                CurrentExerciseImage = ImageSource.FromFile($"planosentado.png");
                _currentExerciseImageName = "planosentado";
                break;
            case "Plano de Pie / Correr":
                CurrentExerciseImage = ImageSource.FromFile($"planodepie.png");
                _currentExerciseImageName = "planodepie";
                break;
            case "Saltos":
                CurrentExerciseImage = ImageSource.FromFile($"saltos1.png");
                _currentExerciseImageName = "saltos1";
                break;
            case "Escalada Sentado":
                CurrentExerciseImage = ImageSource.FromFile($"escaladasentado.png");
                _currentExerciseImageName = "escaladasentado";
                break;
            case "Escalada de Pie":
                CurrentExerciseImage = ImageSource.FromFile($"escaladadepie.png");
                _currentExerciseImageName = "escaladadepie";
                break;
            case "Correr en Montaña":
                CurrentExerciseImage = ImageSource.FromFile($"correrenmontana.png");
                _currentExerciseImageName = "correrenmontana";
                break;
            case "Saltos en Montaña":
                CurrentExerciseImage = ImageSource.FromFile($"saltosenmontana1.png");
                _currentExerciseImageName = "saltosenmontana1";
                break;
            case "Sprints en Plano":
                CurrentExerciseImage = ImageSource.FromFile($"sprintsenplano1.png");
                _currentExerciseImageName = "sprintsenplano1";
                break;
            case "Sprints en Montaña":
                CurrentExerciseImage = ImageSource.FromFile($"sprintsenmontana1.png");
                _currentExerciseImageName = "sprintsenmontana1";
                break;
            default:
                break;
        }
    }

    public async Task<SessionEntity> SetIdSession(int idSession)
    {
        var (session, operationComplete, errorMessage) = await _sessionServices.GetByID(idSession);

        if (operationComplete)
        {
            Session = session;
            this.SessionDescrip = Session.Descrip;
            this.ExerciseList = Session.SessionExercises.ToList();
        }
        else
        {
            await _toastMessagesUtility.ShowMessage(errorMessage);
        }

        return session;
    }

    private async void ControlSession()
    {
        var (restingExercise, operationComplete, errorMessage) = await _exerciseTemplateServices.GetRestingExercise();
        _restingExercise = new SessionExercisesEntity()
        {
            DurationMin = restingExercise.DurationMin,
            ExerciseID = restingExercise.ExerciseID,
            DescripMov = restingExercise.DescripMov,
            HandsPosition = restingExercise.HandsPosition,
            RPMMed = restingExercise.RPMMed,
            RPMFin = restingExercise.RPMFin,
            ResistancePercentage = restingExercise.ResistancePercentage,
        };

        var currentUserID = _currentUser.Id.ToString();
        
        while (true)
        {
            var(action, brokeConnection, ipToResponse) = await _deviceResponder.ControlSession(currentUserID);

            if (brokeConnection)
            {
                _navigationServices.GoBackAsync();

                _sessionListViewModelTV.Reload();

                break;
            }
            else
            {
                if (action == 0)
                {
                    if (_timer.Enabled == false)
                    {
                        if (!_initialized)
                            ExecuteStartIncrement();
                        else
                        {
                            _timer.Start();
                            PauseOrPlayGraphAnimation?.Invoke();
                        }

                        _timer.Enabled = true;
                    }
                    else
                    {
                        _timer.Stop();
                        PauseOrPlayGraphAnimation?.Invoke();
                        _timer.Enabled = false;
                    }
                }
                else if (action == 1)
                {
                    ChangeExercise(true);
                    NextExercise?.Invoke();
                }
                else if (action == 2)
                {
                    ChangeExercise(false);
                    PreviousExercise?.Invoke();
                }
                else if (action == 3)
                {
                    ExerciseList.Insert(_currentExerciseIndex + 1, _restingExercise);
                    InsertRestingExercise?.Invoke();
                }
                else if (action == 4)
                {
                    CurrentResistancePercentageTarget += 1;
                    ExerciseList[_currentExerciseIndex].ResistancePercentage += 1;

                    int newIntensity = (int)Math.Round((0.3 * (double)CurrentTargetRPM) + (0.7 * (double)CurrentResistancePercentageTarget) + 20);

                    ModifyExerciseIntensity?.Invoke(newIntensity);
                }
                else if (action == 5)
                {
                    CurrentResistancePercentageTarget -= 1;
                    ExerciseList[_currentExerciseIndex].ResistancePercentage -= 1;

                    int newIntensity = (int)Math.Round((0.3 * (double)CurrentTargetRPM) + (0.7 * (double)CurrentResistancePercentageTarget) + 20);

                    ModifyExerciseIntensity?.Invoke(newIntensity);
                }
                else if (action == 1035)
                {
                    if (_timer.Enabled == true)
                    {
                        _timer.Stop();
                        PauseOrPlayGraphAnimation?.Invoke();
                        _timer.Enabled = false;
                    }

                    GetCurrentGraphData?.Invoke(ipToResponse);
                }
            }
        }
    }

    public async void SendCurrentSessioninPlayData(List<DataPoint> dataPoints, GraphDrawable drawable, IPEndPoint ipToResponse, DateTime startTime)
    {
        var operationComplete = await _deviceResponder.SendCurrentSessioninPlayData(Session, ExerciseList, _currentExerciseIndex, drawable.CurrentSegmentIndex, dataPoints, drawable.Progress, CurrentRPMValue, CurrentResistanceValue, CurrentTargetRPM, startTime, ElapsedExerciseTime, ElapsedTime, _changeExerciseCountdown, _currentUser.Id, ipToResponse);

        if (!operationComplete)
            await _toastMessagesUtility.ShowMessage("Error al conectar con el dispositivo movil.");
    }

    public void StopTimer()
    {
        _timer.Stop();
    }

    public void SetSessionListViewModelTV(SessionListViewModelTV sessionListViewModelTV)
    {
        _sessionListViewModelTV = sessionListViewModelTV;
    }

    public SessionListViewModelTV GetSessionListViewModelTV()
    {
        return _sessionListViewModelTV;
    }

    public (string, string, string, string) GetSessionFinalResults()
    {
        var totalDurationInMinutes = Math.Round(ElapsedTime.TotalMinutes,2).ToString();
        int averageSpeed = 0;
        int averageResistance = 0;
        double estimatedDistanceTraveledInKm = 0;

        foreach (var exercise in ExerciseList)
        {
            averageSpeed += exercise.RPMMed + exercise.RPMFin;
            averageResistance += exercise.ResistancePercentage;
            estimatedDistanceTraveledInKm += ((double.Parse(exercise.RPMMed.ToString()) * (double.Parse(exercise.DurationMin.ToString()) / 2)) * 1.57) / 1000;
            estimatedDistanceTraveledInKm += ((double.Parse(exercise.RPMFin.ToString()) * (double.Parse(exercise.DurationMin.ToString()) / 2)) * 1.57) / 1000;
        }

        averageSpeed = averageSpeed / (ExerciseList.Count() * 2);
        averageResistance = averageResistance / ExerciseList.Count();

        return (averageSpeed.ToString(), totalDurationInMinutes, Math.Round(estimatedDistanceTraveledInKm,2).ToString(), averageResistance.ToString());
    }

    public SessionExercisesEntity GetRestingExercise() => _restingExercise;
}
