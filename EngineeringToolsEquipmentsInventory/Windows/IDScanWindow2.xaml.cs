using EngineeringToolsEquipmentsInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace EngineeringToolsEquipmentsInventory.Windows
{
    /// <summary>
    /// Interaction logic for IDScanWindow2.xaml
    /// </summary>
    public partial class IDScanWindow2 : Window
    {
        List<User> userList = new List<User>();

        public IDScanWindow2()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
           
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 0, 0, 500) };
            timer.Tick += new EventHandler(IDRead);
            using (var context = new DatabaseContext())
            {
                var user = context.Users.ToList();
                userList = user;
                timer.Start();
            }
            
        }

        private void IDRead (object sender, EventArgs e)
        {
            imageSignal.Visibility = imageSignal.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private static Random random = new Random();
        public static string RandomString (int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string charTemp = Regex.Replace(e.Key.ToString(), "[^0-9.]", "").Replace(".", "");
            UserSession.idScanTemp = UserSession.idScanTemp + charTemp;
            if (e.Key == Key.Enter)
            {
                var check = userList.FirstOrDefault(c => c.UserID == UserSession.idScanTemp);
                if (check != null)
                {
                    if (check.HasAccess)
                    {
                        DialogResult = true;
                    }
                    else
                    {
                        MessageBox.Show("You have no permission to access this.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("User does not exist.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                UserSession.idScanTemp = "";
            }
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }
        }
    }
}
