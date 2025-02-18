using SpinningTrainer.ViewModels;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows.Input;
using ENTITYS;
using SERVICES.UserServices;
using UTILITIES.CryptographyDataUtility;
using UTILITIES.ToastMessagesUtility;
using System.Timers;
using System.Text.RegularExpressions;
using SpinTrainer.Resources.Charts;
using SERVICES.ExerciseTemplateServices;
using Azure;
using System;


#if ANDROID
using Android.Content;
#endif

namespace SpinTrainer.ViewModels
{
    internal class RemoteControlViewModel : ViewModelBase
    {
        private List<DataPoint> _dataPoints;
        private double _graphProgress;
        private int _currentDataPointIndex;

        private bool _initialized = false;

        private UserEntity _currentUser;
        private SessionEntity _session;
        private List<SessionExercisesEntity> _exerciseList;
        private SessionExercisesEntity _currentExercise;
        private SessionExercisesEntity _restingExercise;
        private ImageSource _currentHandsPositionImage;
        private ImageSource _currentExerciseImage;

        private double _currentRPMValue;
        private double _currentResistanceValue;
        private double _currentTargetRPM;
        private double _currentResistancePercentageTarget = 0;
        private string _sessionName;
        private int _currentExerciseIndex = 0;

        private DateTime _startTime;
        private TimeSpan _elapsedExerciseTime;
        private TimeSpan _elapsedTime;
        private TimeSpan _targetTime;
        private TimeSpan _changeExerciseCountdown;

        private string _ipOfTheConnectedTV;

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
        public SessionExercisesEntity CurrentExercise
        {
            get => _currentExercise;
            set
            {
                _currentExercise = value;
                OnPropertyChanged(nameof(CurrentExercise));
                ((ViewModelCommand)NextExerciseCommand).RaiseCanExecuteChanged();
                ((ViewModelCommand)PreviousExerciseCommand).RaiseCanExecuteChanged();
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
                ((ViewModelCommand)IncreaseResistanceCommand).RaiseCanExecuteChanged();
                ((ViewModelCommand)ReduceResistanceCommand).RaiseCanExecuteChanged();
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
        public string SessionName
        {
            get => _sessionName;
            set
            {
                _sessionName = value;
                OnPropertyChanged(nameof(SessionName));
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
        public TimeSpan ElapsedTime
        {
            get => _elapsedTime;
            set
            {
                _elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }
        public SessionExercisesEntity RestingExercise 
        {
            get => _restingExercise; 
            set 
            {
                _restingExercise = value;
                OnPropertyChanged(nameof(RestingExercise));
                ((ViewModelCommand)InsertRestingExerciseCommand).RaiseCanExecuteChanged();
            }
        }

        private readonly ICryptographyDataUtility _cryptographyDataUtility;
        private readonly IToastMessagesUtility _toastMessagesUtility;

#if ANDROID
        private Context _context;
        private TimerService _timerService;
#endif

        public event Action? StartGraphAnimation;
        public event Action? NextExercise;
        public event Action? PreviousExercise;
        public event Action? PauseOrPlayGraphAnimation;
        public event Action? InsertRestingExercise;
        public event Action<int>? ModifyExerciseIntensity;

        public ICommand PlayOrPauseCommand { get; }
        public ICommand NextExerciseCommand { get; }
        public ICommand PreviousExerciseCommand { get; }
        public ICommand InsertRestingExerciseCommand { get; }
        public ICommand IncreaseResistanceCommand { get; }
        public ICommand ReduceResistanceCommand { get; }

        public RemoteControlViewModel(IUserServices userServices,
                                      ICryptographyDataUtility cryptographyDataUtility,
                                      IToastMessagesUtility toastMessagesUtility,
                                      IExerciseTemplateServices exerciseTemplateServices)
        {
            _cryptographyDataUtility = cryptographyDataUtility;
            _toastMessagesUtility = toastMessagesUtility;

            PlayOrPauseCommand = new ViewModelCommand(ExecutePlayOrPauseCommand);
            NextExerciseCommand = new ViewModelCommand(ExecuteNextExerciseCommand, CanExecuteNextExerciseCommand);
            PreviousExerciseCommand = new ViewModelCommand(ExecutePreviousExerciseCommand, CanExecutePreviousExerciseCommand);
            InsertRestingExerciseCommand = new ViewModelCommand(ExecuteInsertRestingExerciseCommand, CanExecuteInsertRestingExerciseCommand);
            IncreaseResistanceCommand = new ViewModelCommand(ExecuteIncreaseResistanceCommand, CanExecuteIncreaseResistanceCommand);
            ReduceResistanceCommand = new ViewModelCommand(ExecuteReduceResistanceCommand, CanExecuteReduceResistanceCommand);

            _currentUser = userServices.GetCurrentUser();
            
            ElapsedTime = TimeSpan.Zero;

            LoadRestingExercise(exerciseTemplateServices);

#if ANDROID
            _context = Android.App.Application.Context;
            _timerService = new TimerService();
#endif
        }

        private bool CanExecuteReduceResistanceCommand(object obj)
        {
            int newIntensity = (int)Math.Round((0.3 * (double)CurrentTargetRPM) + (0.7 * (double)CurrentResistancePercentageTarget) + 20);
            if (newIntensity > 0)
                return true;
            else
                return false;
        }

        private bool CanExecuteIncreaseResistanceCommand(object obj)
        {
            int newIntensity = (int)Math.Round((0.3 * (double)CurrentTargetRPM) + (0.7 * (double)CurrentResistancePercentageTarget) + 20);
            if (newIntensity < 100)
                return true;
            else
                return false;
        }

        private bool CanExecuteInsertRestingExerciseCommand(object obj)
        {
            if (RestingExercise != null)
                return true;
            else
                return false;
        }

        private bool CanExecutePreviousExerciseCommand(object obj)
        {
            if (_currentExerciseIndex == 0)
                return false;
            else
                return true;
        }

        private bool CanExecuteNextExerciseCommand(object obj)
        {
            if(_currentExerciseIndex == ExerciseList.Count - 1)
                return false;
            else
                return true;
        }

        private async void LoadRestingExercise(IExerciseTemplateServices exerciseTemplateServices)
        {
            var (restingExercise, operationComplete, errorMessage) = await exerciseTemplateServices.GetRestingExercise();

            if (restingExercise != null)
            {
                RestingExercise = new SessionExercisesEntity()
                {
                    DurationMin = restingExercise.DurationMin,
                    ExerciseID = restingExercise.ExerciseID,
                    DescripMov = restingExercise.DescripMov,
                    HandsPosition = restingExercise.HandsPosition,
                    RPMMed = restingExercise.RPMMed,
                    RPMFin = restingExercise.RPMFin,
                    ResistancePercentage = restingExercise.ResistancePercentage,
                };
            }
            else
                restingExercise = null;
        }

        public (List<DataPoint>, double, int, DateTime) LoadExercisesInChart()
        {
            if (_dataPoints == null)
            {
                List<DataPoint> dataPoints = new List<DataPoint> { new DataPoint(0, 50, 0, Color.FromHex("#E18417")) };

                foreach (var exercise in ExerciseList)
                {
                    int intensityPercentageFrom = (int)Math.Round((0.3 * (double)exercise.RPMMed) + (0.7 * (double)exercise.ResistancePercentage) + 20);

                    var time = (float)dataPoints[^1].Time;

                    float valueToIncrement = (float)exercise.DurationMin / 2;

                    time = time + valueToIncrement;

                    dataPoints.Add(new DataPoint(time, intensityPercentageFrom, (double)exercise.DurationMin / 2, Color.FromHex("#E18417")));

                    int intensityPercentageTo = (int)Math.Round((0.3 * (double)exercise.RPMFin) + (0.7 * (double)exercise.ResistancePercentage) + 20);

                    time += valueToIncrement;

                    dataPoints.Add(new DataPoint(time, intensityPercentageTo, (double)exercise.DurationMin / 2, Color.FromHex("#E18417")));
                }

                return (dataPoints, -1, 0, DateTime.Now);
            }
            else
                return (_dataPoints, _graphProgress, _currentDataPointIndex, _startTime);
        }

        private async void ExecuteReduceResistanceCommand(object obj)
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"REDUCE_RESISTANCE|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                {
                    CurrentResistancePercentageTarget -= 1;
                    ExerciseList[_currentExerciseIndex].ResistancePercentage -= 1;

                    int newIntensity = (int)Math.Round((0.3 * (double)CurrentTargetRPM) + (0.7 * (double)CurrentResistancePercentageTarget) + 20);

                    ModifyExerciseIntensity?.Invoke(newIntensity);
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Parece que ocurrió un error al intentar controlar la sesión", "OK");
            }
        }

        private async void ExecuteIncreaseResistanceCommand(object obj)
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"INCREASE_RESISTANCE|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                {
                    CurrentResistancePercentageTarget += 1;
                    ExerciseList[_currentExerciseIndex].ResistancePercentage += 1;

                    int newIntensity = (int)Math.Round((0.3 * (double)CurrentTargetRPM) + (0.7 * (double)CurrentResistancePercentageTarget) + 20);

                    ModifyExerciseIntensity?.Invoke(newIntensity);
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Parece que ocurrió un error al intentar controlar la sesión", "OK");
            }
        }

        private async void ExecuteInsertRestingExerciseCommand(object obj)
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"RESTING_EXECISE|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                {
                    ExerciseList.Insert(_currentExerciseIndex + 1, RestingExercise);
                    InsertRestingExercise?.Invoke();
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Parece que ocurrió un error al intentar controlar la sesión", "OK");
            }
        }

        private async void ExecutePreviousExerciseCommand(object obj)
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"PREVIOUS_EXECISE|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                {
                    ChangeExercise(false);
                    PreviousExercise?.Invoke();
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Parece que ocurrió un error al intentar controlar la sesión", "OK");
            }
        }

        private async void ExecuteNextExerciseCommand(object obj)
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"NEXT_EXECISE|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                {
                    ChangeExercise(true);
                    NextExercise?.Invoke();
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Parece que ocurrió un error al intentar controlar la sesión", "OK");
            }
        }

        private async void ExecutePlayOrPauseCommand(object obj)
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"CONTROL_SESSION|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                {
#if ANDROID
                    if (_timerService.GetEnabled() == false)
                    {
                        if (!_initialized)
                            ExecuteStartIncrement();
                        else
                        {
                            _timerService.StartTimer();
                            PauseOrPlayGraphAnimation?.Invoke();
                        }

                        _timerService.SetEnabled(true);
                    }
                    else
                    {
                        _timerService.StopTimer();
                        PauseOrPlayGraphAnimation?.Invoke();
                        _timerService.SetEnabled(false);
                    }
#endif
                }
                else
                    await Application.Current.MainPage.DisplayAlert("Error", "Parece que ocurrió un error al intentar controlar la sesión", "OK");
            }
        }

        private void ExecuteStartIncrement()
        {
#if ANDROID
            InitiliceReproduction();
            _initialized = true;
            _startTime = DateTime.Now;
           
            var intent = new Intent(_context, typeof(TimerService));
           
            _context.StartService(intent);
        
            _timerService.SetTimerElapsedEvent(TimerElapsed);
    
            StartGraphAnimation?.Invoke();
#endif
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
                CurrentExerciseImage = ImageSource.FromFile($"icono{Regex.Replace(Regex.Replace(CurrentExercise.DescripMov.ToLower(), @"\s", ""), "/", "")}.png");
                CurrentHandsPositionImage = ImageSource.FromFile($"hp{CurrentExercise.HandsPosition}.jpeg");
            }
        }
        
        private async void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            // Calcular cuánto tiempo ha pasado desde que comenzó
            TimeSpan elapsedTime = DateTime.Now - _startTime;
            ElapsedTime = ElapsedTime.Add(TimeSpan.FromMilliseconds(100));
            ElapsedExerciseTime = ElapsedExerciseTime.Add(TimeSpan.FromMilliseconds(-100));
            _changeExerciseCountdown = _changeExerciseCountdown.Add(TimeSpan.FromMilliseconds(-100)); ;

            // Calcular el progreso basado en el tiempo transcurrido
            double progress = elapsedTime.TotalMilliseconds / _targetTime.TotalMilliseconds;

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
            if (Math.Abs(ElapsedExerciseTime.TotalMilliseconds) < 0.01)
            {
                if (Math.Abs(_changeExerciseCountdown.TotalMilliseconds) < 0.01)
                {
                    if (_currentExerciseIndex == ExerciseList.Count - 1)
                    {
                        return;
                    }
                    else
                    {
                        ChangeExercise(true);
                    }
                }
                else
                {
                    _startTime = DateTime.Now;
                    ElapsedExerciseTime = TimeSpan.FromMilliseconds(_targetTime.TotalMilliseconds / 2);
                    CurrentTargetRPM = CurrentExercise.RPMFin;
                    CurrentResistancePercentageTarget = CurrentExercise.ResistancePercentage;
                }
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
            CurrentExerciseImage = ImageSource.FromFile($"icono{Regex.Replace(Regex.Replace(CurrentExercise.DescripMov.ToLower().Replace('ñ','n'), @"\s", ""),"/","")}.png");
            CurrentHandsPositionImage = ImageSource.FromFile($"hp{CurrentExercise.HandsPosition}.jpeg");
        }

        public async void CancelSession()
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"CANCEL_SESSION|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                    _toastMessagesUtility.ShowMessage("El evento se controlo exitosamente.");
            }
        }

        private async Task<string> SendCommandAsync(string command)
        {
            string userId = _currentUser.Id.ToString();

            using var udpClient = new UdpClient();
            var discoveryMessage = Encoding.UTF8.GetBytes(_cryptographyDataUtility.Encrypt($"{command}"));
            var broadcastEndpoint = new IPEndPoint(IPAddress.Parse(_ipOfTheConnectedTV), 5000);

            udpClient.EnableBroadcast = true;
            await udpClient.SendAsync(discoveryMessage, discoveryMessage.Length, broadcastEndpoint);

            var buffer = new byte[1024];

            var result = await udpClient.ReceiveAsync();
            var response = _cryptographyDataUtility.Decrypt(Encoding.UTF8.GetString(result.Buffer));
            
            return response;
        }

        public void SetIpOfTheConnectedTV(string ipOfTheConnectedTV) => _ipOfTheConnectedTV = ipOfTheConnectedTV;

        public SessionExercisesEntity GetRestingExercise() => RestingExercise;

        public void SetSession(SessionEntity session)
        {
            Session = session;
            this.SessionName = Session.Descrip;
            this.ExerciseList = Session.SessionExercises.ToList();
        }

        public void SetCurrentSessionInPlayData(SessionEntity session,
                                                List<SessionExercisesEntity> sessionExercises,
                                                int currentExerciseIndex,
                                                List<DataPoint> dataPoints,
                                                int currentDataPointIndex,
                                                double graphProgress,
                                                double currentRPMValue,
                                                double currentResistanceValue,
                                                double currentTargetRPM,
                                                DateTime startTime,
                                                TimeSpan elapsedExerciseTime,
                                                TimeSpan elapsedTime,
                                                TimeSpan changeExerciseCountdown)
        {
            _graphProgress = graphProgress;
            _dataPoints = dataPoints;
            _currentDataPointIndex = currentDataPointIndex;

            Session = session;
            ExerciseList = sessionExercises;
            CurrentExercise = sessionExercises[currentExerciseIndex];
            CurrentHandsPositionImage = ImageSource.FromFile($"hp{CurrentExercise.HandsPosition}.jpeg");
            CurrentExerciseImage = ImageSource.FromFile($"icono{Regex.Replace(Regex.Replace(CurrentExercise.DescripMov.ToLower().Replace('ñ', 'n'), @"\s", ""), "/", "")}.png");

            CurrentRPMValue = currentRPMValue;
            CurrentResistanceValue = currentResistanceValue;
            CurrentTargetRPM = currentTargetRPM;
            _currentResistancePercentageTarget = CurrentExercise.ResistancePercentage;
            SessionName = Session.Descrip;
            _currentExerciseIndex = currentExerciseIndex;

            ElapsedExerciseTime = elapsedExerciseTime;
            ElapsedTime = elapsedTime;
            _targetTime = TimeSpan.FromMilliseconds(TimeSpan.FromMinutes(CurrentExercise.DurationMin).TotalMilliseconds);
            _changeExerciseCountdown = changeExerciseCountdown;

            _startTime = startTime;
        }

        public async Task<bool> SendFinishSessionMessage()
        {
            string userId = _currentUser.Id.ToString();

            var response = await SendCommandAsync($"FINISH_SESSION|{userId}");

            var responseSep = response.Split('|');

            if (responseSep[1] == userId)
            {
                if (responseSep[0] == "1")
                {
                    _toastMessagesUtility.ShowMessage("El evento se controlo exitosamente.");
                    return true;
                }
                else                    
                {
                    await Application.Current.MainPage.DisplayAlert("Error", "Parece que ocurrió un error al intentar controlar la sesión", "OK");
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}
