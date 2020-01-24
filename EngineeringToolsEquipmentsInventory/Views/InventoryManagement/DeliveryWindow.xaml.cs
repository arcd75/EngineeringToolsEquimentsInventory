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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngineeringToolsEquipmentsInventory.Views.InventoryManagement
{
    /// <summary>
    /// Interaction logic for DeliveryWindow.xaml
    /// </summary>
    public partial class DeliveryWindow : UserControl
    {
        public DeliveryWindow()
        {
            InitializeComponent();
        }

        private void BtnNewDelivery_Click(object sender, RoutedEventArgs e)
        {
            mainNavigation.Navigate(new NewDeliveryView());
        }
    }
}
