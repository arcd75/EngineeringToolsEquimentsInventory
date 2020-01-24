using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngineeringToolsEquipmentsInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer = new Timer();
        Timer timer2 = new Timer();
        public MainWindow()
        {
            try
            {
                InitializeComponent();


                timer.Interval = 1000;
                timer.Elapsed += Timer_Elapsed;
                timer.Start();


                timer2.Interval = 1000;
                timer2.Elapsed += Timer2_Elapsed;
                //timer2.Start();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                txtInteractionTime.Text = GetLastInputTime().ToString();

                if (Convert.ToInt32(txtInteractionTime.Text) >= 60)
                {
                    UserSession.UserID = "";
                    UserSession.UserName = "";
                    UserSession.idScanTemp = "";
                    UserSession.UserIDTemp = "";
                    txtUserName.Text = "";
                    txtDepartment.Text = "";
                    timer2.Stop();
                    mainNavigation.Navigate(new LoginView()); 
                }
            });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                txtDateTime.Text = DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToLongTimeString(); 
            });
        }

        public void NavigateView(string viewName)
        {
            mainNavigation.Navigate(new ToolsView());
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.UserID != "")
            {
                if (MessageBox.Show("Are you sure you want to logout?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    UserSession.UserID = "";
                    UserSession.UserName = "";
                    UserSession.UserIDTemp = "";
                    txtUserName.Text = "";
                    txtDepartment.Text = "";
                    mainNavigation.Navigate(new LoginView());
                    timer2.Stop();
                    mainNavigation.Focus();
                }
            }
            mainNavigation.Focus();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                mainNavigation.Navigate(new LoginView());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

            if (Models.UserSession.UserID == "")
            {
                string charTemp = Regex.Replace(e.Key.ToString(), "[^0-9.]", "").Replace(".", "");
                Models.UserSession.idScanTemp = Models.UserSession.idScanTemp + charTemp;
                if (e.Key == Key.Enter)
                {
                    if (Models.UserSession.idScanTemp != "")
                    {
                        using (var context = new DatabaseContext())
                        {
                            var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp && br.Type == "Administrator");
                            if (user != null)
                            {
                                Models.UserSession.idScanTemp = "";
                                Models.UserSession.UserID = user.UserID;
                                txtUserName.Text = user.Name;
                                txtDepartment.Text = user.Department;
                                UserSession.UserName = user.Name;
                                timer2.Start();
                                mainNavigation.Navigate(new MainMenuView());
                            }
                            else
                            {
                                Models.UserSession.idScanTemp = "";
                                MessageBox.Show("Unrecognized ID Card", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            if (e.Key == Key.F1 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new ToolsView());
            }
            if (e.Key == Key.F7 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new MainMenuView());
            }
            if (e.Key == Key.F4 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new HistoryView());
            }
            if (e.Key == Key.F2 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new ConsumablesView());
            }
            if (e.Key == Key.F6 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new InventoryManagementView());
            }
            if (e.Key == Key.F3 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new JigsView());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        static uint GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }
    }
}

