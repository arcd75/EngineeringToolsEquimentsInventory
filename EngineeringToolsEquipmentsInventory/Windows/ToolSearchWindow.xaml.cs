using EngineeringToolsEquipmentsInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EngineeringToolsEquipmentsInventory.Windows
{
    /// <summary>
    /// Interaction logic for ToolSearchWindow.xaml
    /// </summary>
    public partial class ToolSearchWindow : Window
    {
        public ToolSearchWindow()
        {
            InitializeComponent();
        }

        private void BtnFind_Click(object sender, RoutedEventArgs e)
        {
            HistorySession.toolFind = txtItemName.Text;
            DialogResult = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            { 
                DialogResult = false;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtItemName.Focus();
        }
    }
}
