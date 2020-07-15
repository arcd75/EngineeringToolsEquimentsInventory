using EngineeringToolsEquipmentsInventory.Views.InventoryManagement;
using EngineeringToolsEquipmentsInventory.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EngineeringToolsEquipmentsInventory.Views
{
    /// <summary>
    /// Interaction logic for InventoryManagementView.xaml
    /// </summary>
    public partial class InventoryManagementView : UserControl
    {
        public InventoryManagementView()
        {
            InitializeComponent();

        }

        public double width = 0;
        public DispatcherTimer timer = new DispatcherTimer();

        private void BtnDashBoard_Click(object sender, RoutedEventArgs e)
        {
            mainNavigation.Navigate(new IMDashboard());
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            mainNavigation.Navigate(new IMDashboard());
        }

        private void BtnMainMenu_Click(object sender, RoutedEventArgs e)
        {
            IntPtr zero = IntPtr.Zero;
            for (int i = 0; (i < 60) && (zero == IntPtr.Zero); i++)
            {
                Thread.Sleep(500);
                zero = FindWindow(null, "Mainwindow");
            }
            if (zero != IntPtr.Zero)
            {
                SetForegroundWindow(zero);
                System.Windows.Forms.SendKeys.SendWait("{F7}");
                System.Windows.Forms.SendKeys.Flush();
            }
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            mainNavigation.Navigate(new IMInventoryView());
        }

        private void BtnDelivery_Click(object sender, RoutedEventArgs e)
        {
            mainNavigation.Navigate(new DeliveryWindow());
        }

        private void BtnTransHistory_Click(object sender, RoutedEventArgs e)
        {
            mainNavigation.Navigate(new TransactionHistoryView());
        }

        private void BtnJigs_Click(object sender, RoutedEventArgs e)
        {
            var window = new IDScanWindow2();
            if ((bool)window.ShowDialog())
            {
                mainNavigation.Navigate(new JigsTransactionView());
            }
        }

        private void BtnExpand_Click(object sender, RoutedEventArgs e)
        {
             
        }

        private void HamburgerMenu_ContextMenuClosing(object sender, ContextMenuEventArgs e)
        {
          
        }

        private void Menu_ViewStateChanged(object sender, DevExpress.Xpf.WindowsUI.HamburgerMenuViewStateChangedEventArgs e)
        {
       
        }

        private void BtnPersonnel_Click(object sender, RoutedEventArgs e)
        {
            var window = new IDScanWindow2();
            if ((bool)window.ShowDialog())
            {
                mainNavigation.Navigate(new PersonnelView());
            }
        }
    }
}
