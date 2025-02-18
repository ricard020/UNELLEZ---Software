using SERVICES.NavigationServices;
using SERVICES.UserServices;
using SpinningTrainerTV.Socket;

namespace SpinningTrainerTV.ViewModelsTV
{
    internal class SessionFinalResultsViewModelTV : ViewModelBaseTV
    {
        private SessionListViewModelTV _sessionListViewModelTV;

        private string _totalSessionDuration = "0";

        private string _estimatedDistanceTraveled = "0";

        private string _averageSpeed = "0";

        private string _averageResistancePercentage = "0";

        private string _top1EnergyZoneName;

        private string _top2EnergyZoneName;

        private string _top3EnergyZoneName;
        
        private string _top4EnergyZoneName;

        private string _top5EnergyZoneName;

        private string _top1EnergyZoneDuration;

        private string _top2EnergyZoneDuration;

        private string _top3EnergyZoneDuration;
        
        private string _top4EnergyZoneDuration;
        
        private string _top5EnergyZoneDuration;

        public string TotalSessionDuration 
        { 
            get => _totalSessionDuration + " Min."; 
            set 
            {
                _totalSessionDuration = value;
                OnPropertyChanged(nameof(TotalSessionDuration));
            }
        }
        public string EstimatedDistanceTraveled 
        {
            get => _estimatedDistanceTraveled + " Km."; 
            set 
            {
                _estimatedDistanceTraveled = value; 
                OnPropertyChanged(nameof(EstimatedDistanceTraveled));
            }
        }
        public string AverageSpeed 
        {
            get => _averageSpeed + " RPM"; 
            set 
            {
                _averageSpeed = value; 
                OnPropertyChanged(nameof(AverageSpeed));
            }
        }
        public string AverageResistancePercentage 
        {
            get => _averageResistancePercentage + "%"; 
            set 
            {
                _averageResistancePercentage = value; 
                OnPropertyChanged(nameof(AverageResistancePercentage));
            }
        }
        public string Top1EnergyZoneName 
        {
            get => _top1EnergyZoneName; 
            set 
            {
                _top1EnergyZoneName = value; 
                OnPropertyChanged(nameof(Top1EnergyZoneName));
            }
        }
        public string Top2EnergyZoneName 
        { 
            get => _top2EnergyZoneName; 
            set 
            {
                _top2EnergyZoneName = value;
                OnPropertyChanged(nameof(Top2EnergyZoneName));
            }
        }
        public string Top3EnergyZoneName 
        {
            get => _top3EnergyZoneName; 
            set 
            {
                _top3EnergyZoneName = value;
                OnPropertyChanged(nameof(Top3EnergyZoneName));
            }
        }
        public string Top1EnergyZoneDuration 
        {
            get => _top1EnergyZoneDuration; 
            set 
            {
                _top1EnergyZoneDuration = value;
                OnPropertyChanged(nameof(Top1EnergyZoneDuration));
            }
        }
        public string Top2EnergyZoneDuration 
        {
            get => _top2EnergyZoneDuration; 
            set 
            {
                _top2EnergyZoneDuration = value;
                OnPropertyChanged(nameof(Top2EnergyZoneDuration));
            }
        }
        public string Top3EnergyZoneDuration 
        {
            get => _top3EnergyZoneDuration; 
            set 
            {
                _top3EnergyZoneDuration = value;
                OnPropertyChanged(nameof(Top3EnergyZoneDuration));
            }
        }

        public string Top4EnergyZoneDuration 
        {
            get => _top4EnergyZoneDuration; 
            set 
            {
                _top4EnergyZoneDuration = value;
                OnPropertyChanged(nameof(Top4EnergyZoneDuration));
            }
        }
        public string Top5EnergyZoneDuration 
        {
            get => _top5EnergyZoneDuration; 
            set 
            {
                _top5EnergyZoneDuration = value;
                OnPropertyChanged(nameof(Top5EnergyZoneDuration));
            }
        }
        public string Top4EnergyZoneName 
        {
            get => _top4EnergyZoneName; 
            set 
            {
                _top4EnergyZoneName = value;
                OnPropertyChanged(nameof(Top4EnergyZoneName));
            }
        }
        public string Top5EnergyZoneName 
        {
            get => _top5EnergyZoneName; 
            set 
            {
                _top5EnergyZoneName = value;
                OnPropertyChanged(nameof(Top5EnergyZoneDuration));
            }
        }

        private readonly IUserServices _userServices;
        private readonly INavigationServices _navigationService;
        private readonly DeviceResponder _deviceResponder;

        public SessionFinalResultsViewModelTV(DeviceResponder deviceResponder, IUserServices userServices, INavigationServices navigationService)
        {
            _deviceResponder = deviceResponder;
            _userServices = userServices;
            _navigationService = navigationService;
        }

        public async Task<bool> WaitingForTheFinishedSessionMessage()
        {
            while (true)
            {
                var action = await _deviceResponder.WaitingForTheFinishedSessionMessage(_userServices.GetCurrentUser().Id.ToString());
                
                if (action == 0)
                {
                    _navigationService.GoBackAsync();
                    _navigationService.GoBackAsync();

                    _sessionListViewModelTV.WaitForTheSessionToPlay();

                    break;
                }
            }

            return false;
        }

        public void SetSessionListViewModelTV(SessionListViewModelTV sessionListViewModelTV)
        {
            _sessionListViewModelTV = sessionListViewModelTV;
        }
    }
}
