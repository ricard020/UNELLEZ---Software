#if ANDROID
using Android.Views;
#endif
using SpinningTrainerTV.DPadNavigation;
using SpinningTrainerTV.ViewModelsTV;

namespace SpinningTrainerTV.ViewTV
{
    public partial class SelectUserViewTV : ContentPage, IPageKeyDown
    {
        private readonly SelectUserViewModelTV _selectUserViewModelTV;
        int index = 0;

        public SelectUserViewTV(SelectUserViewModelTV viewModelTV)
        {
            InitializeComponent();
            _selectUserViewModelTV = viewModelTV;
            this.BindingContext = _selectUserViewModelTV;
        }

        private void ContentPage_Loaded(object sender, EventArgs e)
        {
            ltvUsers.Focus();

//#if DEBUG
            _selectUserViewModelTV.SelectedUserChanged(_selectUserViewModelTV.UsersList.FirstOrDefault(e => e.Username == "E1"));
//#endif
        }

        private void StackLayout_Focused(object sender, FocusEventArgs e)
        {
            var stackLayout = (StackLayout)sender;

            stackLayout.Background = Brush.Red;
        }


        #if ANDROID
        public bool OnPageKeyDown(Keycode keyCode, KeyEvent e)
        {
            index = 0;
            switch (keyCode)
            {
                case Keycode.DpadUp:
                    ExecuteDpadLeft();
                    break;
                case Keycode.DpadDown:
                    ExecuteDpadRight();
                    break;
                case Keycode.DpadLeft:
                    ExecuteDpadLeft();
                    break;
                case Keycode.DpadRight:
                    ExecuteDpadRight();
                    break;
                case Keycode.DpadCenter:
                    ExecuteDpadCenter();
                    break;
                default: break;
            }
            return false;
        }
        #endif

        public void ExecuteDpadLeft()
        {
            index = Math.Min(Math.Max(_selectUserViewModelTV.UsersList.IndexOf(_selectUserViewModelTV.SelectedUser) - 1, 0), _selectUserViewModelTV.UsersList.Count - 1);
            _selectUserViewModelTV.SelectedUser = _selectUserViewModelTV.UsersList[index];
        }

        public void ExecuteDpadRight()
        {
            index = Math.Min(Math.Max(_selectUserViewModelTV.UsersList.IndexOf(_selectUserViewModelTV.SelectedUser) + 1, 0), _selectUserViewModelTV.UsersList.Count - 1);
            _selectUserViewModelTV.SelectedUser = _selectUserViewModelTV.UsersList[index];
        }

        public void ExecuteDpadCenter()
        {
            OnItemSelected();
        }

        private async void OnItemSelected()
        {
            var contentModel = _selectUserViewModelTV.SelectedUser;
            if (contentModel != null)
            {
                var navigationParameters = new Dictionary<string, object>();
                await _selectUserViewModelTV.SelectedUserChanged(contentModel);
            }
        }
    }
}