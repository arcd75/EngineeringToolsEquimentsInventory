using DevExpress.Xpf.Core;
using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Windows;
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
            var task = Task.Run(() =>
            {
                Dispatcher.Invoke(() =>
                {
                    InitializeComponent();
                });

            });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    task.Dispose();
                });

            });

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
                //dgJigInventory.ItemsSource = inventoryBinding;
                dgJigInventory.RefreshData();
            }

            using (var context = new iJosDatabaseContext())
            {
                var jig = context.JigPriceLists;

                cmbJigCode.ItemsSource = jig.ToList();

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
            if (DevExpress.Xpf.Core.DXMessageBox.Show("Are you sure you want to delete this record?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedItem = dgJigInventory.SelectedItem as Jig;
                using (var context = new DatabaseContext())
                {
                    var jig = context.Jigs.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                    if (jig != null)
                    {
                        context.Jigs.Remove(jig);
                        context.SaveChanges();
                        LoadInventory();
                    }
                }
            }

        }

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItems = dgJigInventory.SelectedItem as Jig;
            if (selectedItems != null)
            {
                selectedItems.TotalCost = (float.Parse(selectedItems.Balance) * float.Parse(selectedItems.UnitCost)).ToString();
            }
            using (var context = new DatabaseContext())
            {
                var updateSelected = context.Jigs.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                if (updateSelected != null)
                {
                    updateSelected.Type = selectedItems.Type;
                    updateSelected.Quantity = selectedItems.Quantity;
                    updateSelected.Balance = selectedItems.Balance;
                    updateSelected.Specification = selectedItems.Specification;
                    updateSelected.RefNo = selectedItems.RefNo;
                    updateSelected.PIC = selectedItems.PIC;
                    updateSelected.PONo = selectedItems.PONo;
                    updateSelected.DateDelivered = selectedItems.DateDelivered;
                    updateSelected.WarehousePIC = selectedItems.WarehousePIC;
                    updateSelected.Location = selectedItems.Location;
                    context.SaveChanges();
                    LoadInventory();
                }
            }
            JigAddingFinish();
        }

        private void BtnAddDelivery_Click(object sender, RoutedEventArgs e)
        {
            AddNewJigWindow addNewJigWindow = new AddNewJigWindow();
            addNewJigWindow.ShowDialog();
            if (addNewJigWindow.DialogResult == true || addNewJigWindow.DialogResult == false)
            {
                LoadInventory();
            }
        }

        private void BtnManualAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TableView_EditFormShowing(object sender, DevExpress.Xpf.Grid.EditFormShowingEventArgs e)
        {

        }

        private void DgInventoryTableView_EditFormShowing(object sender, DevExpress.Xpf.Grid.EditFormShowingEventArgs e)
        {
            dgInventoryTableView.AllowEditing = true;

            tblJigName.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            tblTotalCost.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            tblUnitCost.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            tableViewItemCode.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
        }

        private void JigAddingFinish()
        {
            //dgInventoryTableView.AllowEditing = false;
            //tableViewLocation.AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            //tableViewQuantity.AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            //tableViewWarehousePIC.AllowEditing = DevExpress.Utils.DefaultBoolean.True;
        }

        private void TableViewJigCode_ContentChanged(object sender, DevExpress.Xpf.Editors.ColumnContentChangedEventArgs e)
        {

        }

        private void DgInventoryTableView_AddingNewRow(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            JigManualAdd jigManualAdd = new JigManualAdd();
            jigManualAdd.ShowDialog();
            LoadInventory();
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure you want to return this item?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedItem = dgReserveJigs.SelectedItem as JigTransactionItem;
                if (selectedItem != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var jigToReturn = context.JigTransactionItems.FirstOrDefault(br => br.JigTransactionItemID == selectedItem.JigTransactionItemID);
                        if (jigToReturn == null)
                        {
                            DXMessageBox.Show("Jig does not exist", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (jigToReturn.TotalItem > 1)
                        {
                            jigToReturn.TotalCost = jigToReturn.TotalCost - selectedItem.Cost;
                            jigToReturn.TotalItem = jigToReturn.TotalItem - 1;
                            context.SaveChanges();
                        }
                        else if (jigToReturn.TotalItem == 1)
                        {
                            context.JigTransactionItems.Remove(jigToReturn);
                            context.SaveChanges();
                        } 
                        var checkExisting = context.JigTransactionItems.Where(br => br.TransactionID == selectedItem.TransactionID);
                        if (checkExisting.Count() <= 0)
                        {
                            var deleteMainRecord = context.JigTransactions.FirstOrDefault(br => br.TransactionID == selectedItem.TransactionID);
                            if (deleteMainRecord != null)
                            {
                                context.JigTransactions.Remove(deleteMainRecord);
                                context.SaveChanges();
                            }
                        }
                        else
                        {
                            var updateMainRecord = context.JigTransactions.FirstOrDefault(br => br.TransactionID == selectedItem.TransactionID);
                            if (updateMainRecord != null)
                            {
                                updateMainRecord.TotalItem = updateMainRecord.TotalItem - 1;
                                updateMainRecord.TotalCost = updateMainRecord.TotalCost - selectedItem.Cost;
                                context.SaveChanges();
                            }
                        }
                        var returnToInventory = context.Jigs.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                        if (returnToInventory != null)
                        {
                            returnToInventory.Balance = (int.Parse(returnToInventory.Balance) + 1).ToString();
                            context.SaveChanges();
                            DXMessageBox.Show("jig returned to inventory", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadReserve();
                            LoadInventory();
                            LoadWithdrawn();
                        }
                    }
                } 
            }
        }
    }
}
