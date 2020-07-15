using EngineeringToolsEquipmentsInventory.Models;
using System.Linq;
using System.Windows;

namespace EngineeringToolsEquipmentsInventory.Windows
{
    /// <summary>
    /// Interaction logic for UserWindow.xaml
    /// </summary>
    public partial class UserWindow : Window
    {
        User userInfo = new User();

        public UserWindow()
        {
            InitializeComponent();
        }

        public UserWindow(User user)
        {
            userInfo = user;
            InitializeComponent();
            txtName.Text = userInfo.Name;
            cbDept.Text = userInfo.Department;
            cbType.Text = userInfo.Type;
            chkHasAccess.IsChecked = userInfo.HasAccess;
            txtUserID.Password = userInfo.UserID;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtName.Text) || string.IsNullOrEmpty(cbDept.Text) || string.IsNullOrEmpty(cbType.Text) || string.IsNullOrEmpty(txtUserID.Password))
            {
                MessageBox.Show("Please fill in all fields", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (MessageBox.Show("Are you sure all user information are correct?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var context = new DatabaseContext())
                    {
                        if (userInfo == null)
                        {
                            userInfo.Name = txtName.Text;
                            userInfo.Department = cbDept.Text;
                            userInfo.Type = cbType.Text;
                            userInfo.UserID = txtUserID.Password;
                            userInfo.HasAccess = (bool)chkHasAccess.IsChecked;
                            context.Users.Add(userInfo);
                            context.SaveChanges();
                            MessageBox.Show("User added successfully.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            var user = context.Users.FirstOrDefault(u => u.UserKey == userInfo.UserKey);
                            user.Name = txtName.Text;
                            user.Department = cbDept.Text;
                            user.Type = cbType.Text;
                            user.UserID = txtUserID.Password;
                            user.HasAccess = (bool)chkHasAccess.IsChecked;
                            context.SaveChanges();
                            MessageBox.Show("User information edited successfully.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        DialogResult = true;
                    }
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void CbType_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            chkHasAccess.IsEnabled = cbType.SelectedIndex == 1 ? true : false;
            if (cbType.SelectedIndex == 0) chkHasAccess.IsChecked = false;
        }
    }
}
