using SpinningTrainer.View;

namespace SpinningTrainer
{
    public partial class App : Application
    {
        public App(AppShell appShell)
        {
            InitializeComponent();

            MainPage = appShell;
            //if (!DataBaseConnection.TestConnection()) { MainPage = new ConnectionView(); }
            //else { MainPage = new LoginView(); }
        }
    }
}
