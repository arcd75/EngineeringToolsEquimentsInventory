using EngineeringToolsEquipmentsInventory.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EngineeringToolsEquipmentsInventory.Views
{
    /// <summary>
    /// Interaction logic for MainMenuView.xaml
    /// </summary>
    public partial class MainMenuView : System.Windows.Controls.UserControl
    { 
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        public MainMenuView()
        {
            InitializeComponent();
            UserSession.idScanTemp = "";
        }  

        private void BtnTools_Click(object sender, RoutedEventArgs e)
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
                SendKeys.SendWait("{F1}");
                SendKeys.Flush();
            }
        }

        private void BtnHistory_Click(object sender, RoutedEventArgs e)
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
                SendKeys.SendWait("{F4}");
                SendKeys.Flush();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnConsumables_Click(object sender, RoutedEventArgs e)
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
                SendKeys.SendWait("{F2}");
                SendKeys.Flush();
            }
        }

        private void BtnInventoryManagement_Click(object sender, RoutedEventArgs e)
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
                SendKeys.SendWait("{F6}");
                SendKeys.Flush();
            }
        }

        private void BtnJigs_Click(object sender, RoutedEventArgs e)
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
                SendKeys.SendWait("{F3}");
                SendKeys.Flush();
            }
        }

        private void BtnAssets_Click(object sender, RoutedEventArgs e)
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
                SendKeys.SendWait("{F5}");
                SendKeys.Flush();
            }
        }
    }
}

