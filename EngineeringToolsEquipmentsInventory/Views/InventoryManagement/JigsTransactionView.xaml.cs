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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngineeringToolsEquipmentsInventory.Views.InventoryManagement
{
    /// <summary>
    /// Interaction logic for JigsTransactionView.xaml
    /// </summary>
    public partial class JigsTransactionView : UserControl
    {
        public JigsTransactionView()
        {
            InitializeComponent();
        }

        System.Windows.Forms.BindingSource withdrawBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource reserveBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource inventoryBinding = new System.Windows.Forms.BindingSource();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgWithdrawHistory.ItemsSource = withdrawBinding;
            dgReserveJigs.ItemsSource = reserveBinding;
            dgJigInventory.ItemsSource = inventoryBinding;
            LoadWithdrawn();
            LoadReserve();
            LoadInventory();
        }

        private void LoadInventory()
        {
            using (var context = new DatabaseContext())
            {
                var inventory = context.Jigs;

                inventoryBinding.DataSource = inventory.ToList();
                dgJigInventory.ItemsSource = inventoryBinding;
                dgJigInventory.RefreshData();
            }
        }

        private void LoadWithdrawn()
        {
            using (var context = new DatabaseContext())
            {
                var history = context.JigTransactionItems.Where(br => br.Status == "Withdraw"); 
                    withdrawBinding.DataSource = history.ToList();
                    dgWithdrawHistory.ItemsSource = withdrawBinding;
                    dgWithdrawHistory.RefreshData(); 
            }
        }

        private void LoadReserve()
        {
            using (var context = new DatabaseContext())
            {
                var history = context.JigTransactionItems.Where(br => br.Status == "Reserve");

                reserveBinding.DataSource = history.ToList();

            }
            dgReserveJigs.ItemsSource = reserveBinding;
            dgReserveJigs.RefreshData();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to update this record?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedItems = dgReserveJigs.SelectedItem as JigTransactionItem;
                if (selectedItems != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var itemToUpdate = context.JigTransactionItems.FirstOrDefault(br => br.JigTransactionItemID == selectedItems.JigTransactionItemID);

                        if (itemToUpdate != null)
                        {
                            if (selectedItems.Status == "Reserve")
                            {
                                itemToUpdate.Status = "Reserve";
                                itemToUpdate.DateWithdrawn = DateTime.MinValue;
                            }
                            else
                            {
                                itemToUpdate.Status = "Withdraw";
                                itemToUpdate.DateWithdrawn = DateTime.Now;
                            }
                            itemToUpdate.Location = selectedItems.Location;
                            context.SaveChanges();
                            LoadReserve();
                            LoadWithdrawn();
                            LoadInventory();
                        }
                    }
                }
            }
        }

        private void BtnUpdateInventory_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
