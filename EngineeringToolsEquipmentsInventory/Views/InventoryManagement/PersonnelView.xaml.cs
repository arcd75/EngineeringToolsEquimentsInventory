using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace EngineeringToolsEquipmentsInventory.Views
{
    /// <summary>
    /// Interaction logic for PersonnelView.xaml
    /// </summary>
    public partial class PersonnelView : UserControl
    {
        List<User> userList = new List<User>();

        private void LoadUsers()
        {
            var task = Task.Run(() =>
            {
                using (var context = new DatabaseContext())
                {
                    var users = context.Users.ToList();
                    userList = users.OrderByDescending(u => u.HasAccess).ThenBy(u => u.Type).ThenByDescending(u => u.Department).ThenBy(u => u.Name).ToList();
                }
            });

            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    dgPersonnel.ItemsSource = userList;
                    dgPersonnel.RefreshData();
                    bsyPanel.Visibility = Visibility.Collapsed;
                });
            });
        }

        public PersonnelView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadUsers();
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgPersonnel.SelectedItem is User item)
            {
                var window = new UserWindow(item);
                window.txtTitle.Text = "EDIT USER";
                window.ShowDialog();
                bsyPanel.Visibility = Visibility.Visible;
                LoadUsers();
            }            
        }

        private void ViewPersonnel_AddingNewRow(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            var window = new UserWindow();
            window.txtTitle.Text = "ADD USER";
            window.ShowDialog();
            bsyPanel.Visibility = Visibility.Visible;
            LoadUsers();
        }
    }
}
