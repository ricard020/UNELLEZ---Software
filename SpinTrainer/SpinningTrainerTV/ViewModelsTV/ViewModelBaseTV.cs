using System.ComponentModel;

namespace SpinningTrainerTV.ViewModelsTV
{
    public abstract class ViewModelBaseTV : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
