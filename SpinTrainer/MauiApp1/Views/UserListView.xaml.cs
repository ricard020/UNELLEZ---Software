using ENTITYS;
using SpinningTrainer.ViewModels;

namespace SpinningTrainer.Views
{
    public partial class UserListView : ContentPage
    {
        public UserListView()
        {
            InitializeComponent();            
        }     

        private void ltvUserListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is UserEntity selectedUser)
            {
                // Obtener el ViewModel del BindingContext
                if (this.BindingContext is UsersViewModel viewModel)
                {
                    viewModel.EditUser(selectedUser);
                }
            }
        }
    }
}
