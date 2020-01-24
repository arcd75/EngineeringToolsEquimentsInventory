using EngineeringToolsEquipmentsInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EngineeringToolsEquipmentsInventory.Windows
{
    /// <summary>
    /// Interaction logic for BarrowerLookupWindow.xaml
    /// </summary>
    /// 
    
    public partial class BarrowerLookupWindow : Window
    {
        bool isEdit = false;
        bool isAdd = false;
        bool isBeingUse = false;
        public BarrowerLookupWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (UserSession.UserIDTemp == "" && isBeingUse == false)
            {
                string charTemp = Regex.Replace(e.Key.ToString(), "[^0-9.]", "").Replace(".", "");
                Models.UserSession.idScanTemp = Models.UserSession.idScanTemp + charTemp;
                if (e.Key == Key.Enter)
                {
                    if (Models.UserSession.idScanTemp != "")
                    {
                        using (var context = new DatabaseContext())
                        {
                            var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp);
                            if (user != null)
                            {
                                stckNotFound.Visibility = Visibility.Collapsed;
                                barrowerForm.Visibility = Visibility.Visible;
                                UserSession.UserIDTemp = charTemp;
                                txtUserID.Text = user.UserID;
                                txtName.Text = user.Name;
                                cmbDepartment.Text = user.Department;
                                cmbType.Text = user.Type;
                                Models.UserSession.idScanTemp = "";
                            }
                            else
                            {
                                Models.UserSession.idScanTemp = "";
                                barrowerForm.Visibility = Visibility.Collapsed;
                                stckNotFound.Visibility = Visibility.Visible;
                            }
                        }
                    }
                } 
            }
            if (e.Key == Key.Escape)
            {
                DialogResult = true;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += IDRead;
            timer.Start();
        }
        private void IDRead(object sender, EventArgs e)
        {
            if (imageSignal.IsVisible == true)
            {
                imageSignal.Visibility = Visibility.Hidden;
            }
            else
            {
                imageSignal.Visibility = Visibility.Visible;
            }
        }

        private void BtnRepeat_Click(object sender, RoutedEventArgs e)
        {
            isEdit = false;
            isAdd = false;
            ClearFields();
            DeActivateForm();
            UserSession.UserIDTemp = "";
            UserSession.idScanTemp = "";
            stckNotFound.Visibility = Visibility.Collapsed;
            barrowerForm.Visibility = Visibility.Collapsed;
            isBeingUse = false;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            ActivateForm();
            isAdd = true;
            isBeingUse = true;
        }

        private void ClearFields()
        {
            txtUserID.Text = "";
            txtName.Text = "";
            cmbDepartment.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
        }
        private void ActivateForm()
        {
            txtUserID.IsReadOnly = false;
            txtName.IsReadOnly = false;
        }

        private void DeActivateForm()
        {
            txtUserID.IsReadOnly = true;
            txtName.IsReadOnly = true;
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (isAdd == true)
            {
                if (txtUserID.Text != "" && txtName.Text != "")
                {
                    using (var context = new DatabaseContext())
                    {
                        var checkUser = context.Users.FirstOrDefault(br => br.UserID == txtUserID.Text);
                        if (checkUser == null)
                        {
                            User user = new User();
                            string userKey = "USR-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                            user.UserKey = userKey;
                            user.UserID = txtUserID.Text.Trim();
                            user.Name = txtName.Text;
                            user.Department = cmbDepartment.Text;
                            user.Type = cmbType.Text;
                            context.Users.Add(user);
                            context.SaveChanges();
                            MessageBox.Show("User is Added!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearFields();
                            DeActivateForm();
                            barrowerForm.Visibility = Visibility.Collapsed;
                            isAdd = false;
                            isBeingUse = false;
                        }
                        else
                        {
                            MessageBox.Show("This ID is already registered!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            txtUserID.SelectAll();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please complete all infromation!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                } 
            }

            if (isEdit == true)
            {
                if (txtUserID.Text != "" && txtName.Text != "")
                {
                    using (var context = new DatabaseContext())
                    {
                        var userEdit = context.Users.FirstOrDefault(br => br.UserID == txtUserID.Text);
                        if (userEdit != null)
                        {
                            userEdit.Name = txtName.Text;
                            userEdit.Department = cmbDepartment.Text;
                            userEdit.Type = cmbType.Text;
                            context.SaveChanges();
                            MessageBox.Show("User Updated!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                            DeActivateForm();
                            isEdit = false;
                            
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please complete all infromation!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ActivateForm();
            isEdit = true;
            txtUserID.IsReadOnly = true;
            isBeingUse = true;
        }

        private void BtnYes_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            ActivateForm();
            isAdd = true;
            stckNotFound.Visibility = Visibility.Collapsed;
            barrowerForm.Visibility = Visibility.Visible;
            isBeingUse = true;
        }

        private void BtnNo_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            DeActivateForm();
            stckNotFound.Visibility = Visibility.Collapsed;
            isBeingUse = false;
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this User?","Inventory System",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                if (UserSession.UserID != txtUserID.Text)
                {

                    if (isEdit == false || isAdd == false)
                    {
                        using (var context = new DatabaseContext())
                        {
                            var delete = context.Users.FirstOrDefault(br => br.UserID == txtUserID.Text);
                            if (delete != null)
                            {
                                context.Users.Remove(delete);
                                context.SaveChanges();
                                MessageBox.Show("User Removed!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                                ClearFields();
                                DeActivateForm();
                                barrowerForm.Visibility = Visibility.Collapsed;
                            }
                        }
                    }  
                }
                else
                {
                    MessageBox.Show("You can't delete an active user!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
