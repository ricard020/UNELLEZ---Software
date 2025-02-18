using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;
using ENTITYS;
using SERVICES.NavigationServices;
using SERVICES.SessionServices;
using System.Windows.Input;
using UTILITIES.ToastMessagesUtility;

namespace SpinningTrainer.ViewModels
{
    public class InsertNewDuplicateSessionDataViewModel : ViewModelBase
    {
        private int _sessionIdToDuplicate;
        private string _descrip;
        private DateTime _dateI = DateTime.Now;
        private TimeSpan _timeI;

        public string Descrip 
        { 
            get => _descrip; 
            set 
            {
                _descrip = value; 
                OnPropertyChanged(nameof(Descrip));
                ((ViewModelCommand)DuplicateSessionCommand).RaiseCanExecuteChanged();
            }
        }
        public DateTime DateI 
        { 
            get => _dateI;
            set 
            {
                _dateI = value;
                OnPropertyChanged(nameof(DateI));
            }
        }
        public TimeSpan TimeI
        {
            get => _timeI;
            set
            {
                _timeI = value;
                OnPropertyChanged(nameof(TimeI));               
            }
        }

        private readonly INavigationServices _navigationServices;
        private readonly ISessionServices _sessionServices;
        private readonly IToastMessagesUtility _toastMessagesUtility;

        public ICommand DuplicateSessionCommand { get; }
        
        public InsertNewDuplicateSessionDataViewModel(ISessionServices sessionServices, IToastMessagesUtility toastMessagesUtility, INavigationServices navigationServices)
        {
            _sessionServices = sessionServices;
            _toastMessagesUtility = toastMessagesUtility;
            _navigationServices = navigationServices;

            DuplicateSessionCommand = new ViewModelCommand(ExecuteDuplicateSessionCommand, CanExecuteDuplicateSessionCommand);            
        }

        private bool CanExecuteDuplicateSessionCommand(object obj)
        {
            return !string.IsNullOrWhiteSpace(Descrip);
        }

        private async void ExecuteDuplicateSessionCommand(object obj)
        {
            try
            {
                var (sessionToDuplicate, operationComplete, errorMessage) = await _sessionServices.GetByID(_sessionIdToDuplicate);

                if (operationComplete) 
                {
                    var newSession = new SessionEntity()
                    {
                        Descrip = this.Descrip,
                        DateC = DateTime.Now,
                        DateI = new DateTime(DateI.Year, DateI.Month, DateI.Day, TimeI.Hours, TimeI.Minutes, TimeI.Seconds),
                        Duration = sessionToDuplicate.Duration,
                        SessionExercises = new List<SessionExercisesEntity>(),
                        TrainerID = sessionToDuplicate.TrainerID
                    };

                    foreach (var exercise in sessionToDuplicate.SessionExercises)
                    {
                        newSession.SessionExercises.Add(new SessionExercisesEntity()
                        {
                            DescripMov = exercise.DescripMov,
                            DurationMin = exercise.DurationMin,
                            ResistancePercentage = exercise.ResistancePercentage,
                            ExerciseID = exercise.ExerciseID,
                            HandsPosition = exercise.HandsPosition,
                            RPMFin = exercise.RPMFin,
                            RPMMed = exercise.RPMMed
                        });
                    }

                    var (operationCompleteInsert, errorMessageInsert) = await _sessionServices.Add(newSession);

                    if (operationCompleteInsert)
                    {
                        await _toastMessagesUtility.ShowMessage("Operación completada satisfactoriamente.");
                        await _navigationServices.GoBackAsync();
                    }
                    else
                    {
                        await _toastMessagesUtility.ShowMessage(errorMessage);
                    }
                }
                else
                {
                    await _toastMessagesUtility.ShowMessage(errorMessage);
                }
            }
            catch (Exception ex)
            {
                await _toastMessagesUtility.ShowMessage(ex.Message);
            }            
        }

        public void SetSessionIDToDuplicate(int idSession)
        {
            _sessionIdToDuplicate = idSession;
        }
    }
}
