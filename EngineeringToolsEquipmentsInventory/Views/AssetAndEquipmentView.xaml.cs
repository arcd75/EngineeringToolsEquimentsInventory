using DevExpress.Xpf.Core;
using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Windows;
using System;
using System.Collections.Generic;
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

namespace EngineeringToolsEquipmentsInventory.Views
{
    /// <summary>
    /// Interaction logic for AssetAndEquipmentView.xaml
    /// </summary>
    /// 

    public partial class AssetAndEquipmentView : UserControl
    {
        public AssetAndEquipmentView()
        {
            InitializeComponent();
        }
        System.Windows.Forms.BindingSource withdrawItemsBinding = new System.Windows.Forms.BindingSource();

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            dgGridItems.ItemsSource = withdrawItemsBinding;
            LoadAssets();


        }

        private void LoadAssets()
        {
            dgAssets.ShowLoadingPanel = true;
            var task = Task.Run(() =>
            {
                try
                {
                    using (var context = new DatabaseContext())
                    {
                        var items = context.AssetAndEquipments;
                        Dispatcher.Invoke(() =>
                        {
                            dgAssets.ItemsSource = items.ToList();
                        });
                    }

                    using (var context = new GMCSDatabaseContext())
                    {
                        var lineNames = context.line_Msts.Select(br => br.line_nm);
                        Dispatcher.Invoke(() =>
                        {
                            foreach (var item in lineNames)
                            {
                                cmbLineCode.Items.Add(item);
                            }
                        });
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    dgAssets.ShowLoadingPanel = false;
                    task.Dispose();
                });
            });
        }

        private void DgJigWithdrawTableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgGridItems.SelectedItem as AssetTransactionItem;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var checkOrdered = context.AssetAndEquipments.FirstOrDefault(br => br.AssetID == selectedItem.AssetID);
                    if (checkOrdered != null)
                    {
                        if (selectedItem.Quantity > checkOrdered.Qty_Available)
                        {
                            DXMessageBox.Show("Not enough stock available", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            selectedItem.Quantity = 1;
                        }
                    }
                }
            }
        }

        private void CmbLineCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (AssetsSession.NewAssetTransaction.TransactionID != null)
            {
                var selectedItems = dgAssets.SelectedItem as AssetAndEquipment;
                if (selectedItems != null)
                {
                    var orderJigs = AssetsSession.AssetTransItemList.FirstOrDefault(br => br.AssetID == selectedItems.AssetID);
                    if (orderJigs != null)
                    {
                        using (var context = new DatabaseContext())
                        {
                            var item = context.AssetAndEquipments.FirstOrDefault(br => br.AssetID == selectedItems.AssetID);
                            if (item != null)
                            {
                                if (item.Qty_Available <= item.MaintainingQty)
                                {
                                    MessageBox.Show("This item is in critical level! reorder is required", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                if ((item.Qty_Available - (orderJigs.Quantity + 1)) < item.MaintainingQty)
                                {
                                    MessageBox.Show("This item is in critical level! reorder is required", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                if (item.Qty_Available < orderJigs.Quantity + 1)
                                {
                                    DXMessageBox.Show("Not enough stocks available!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                orderJigs.Quantity = orderJigs.Quantity + 1;
                                orderJigs.TotalCost = orderJigs.Quantity * orderJigs.UnitCost;
                                LoadItems();
                                return;
                            }
                        }

                    }

                    //if (int.Parse(selectedItems.Quantity) >= int.Parse(selectedItems.Balance))
                    //{
                    //    MessageBox.Show("This item is in critical level! reorder is required", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    return;
                    //}
                    if (selectedItems.Qty_Available <= 0)
                    {
                        MessageBox.Show("This item is Out of Stock!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (selectedItems.Qty_Available <= selectedItems.MaintainingQty)
                    {
                        MessageBox.Show("This item is in critical level! reorder is required", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    using (var context = new DatabaseContext())
                    {
                        var item = context.AssetAndEquipments.FirstOrDefault(br => br.AssetID == selectedItems.AssetID);
                        if (item != null)
                        {
                            AssetTransactionItem jigTransactionItem = new AssetTransactionItem();
                            string tempID = "";
                            while (true)
                            {
                                using (var context2 = new DatabaseContext())
                                {
                                    tempID = "TRANSIT" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                                    var check = context.AssetTransactionItems.FirstOrDefault(br => br.AssetTransactionItemID == tempID);
                                    if (check == null)
                                    {
                                        break;
                                    }
                                }
                            }

                            jigTransactionItem.AssetTransactionItemID = tempID;
                            jigTransactionItem.ItemCode = item.ItemCode;
                            jigTransactionItem.TransactionID = AssetsSession.NewAssetTransaction.TransactionID;
                            jigTransactionItem.AssetID = item.AssetID;
                            jigTransactionItem.LineName = txtLineName.Text;
                            jigTransactionItem.ItemDescription = item.ItemDescription;
                            jigTransactionItem.UOM = item.UOM;
                            jigTransactionItem.SerialNo = item.SerialNo;
                            jigTransactionItem.AssetNo = item.AssetNo;
                            jigTransactionItem.Condition = item.Condition;
                            jigTransactionItem.Remarks = "N/A";
                            jigTransactionItem.Quantity = 1;
                            jigTransactionItem.Date = DateTime.Now;
                            jigTransactionItem.DateWithdrawn = DateTime.Now;
                            jigTransactionItem.UnitCost = selectedItems.UnitCost;
                            jigTransactionItem.TotalCost = selectedItems.UnitCost * 1;
                            AssetsSession.AssetTransItemList.Add(jigTransactionItem);
                            LoadItems();
                        }
                    }
                }
            }

        }

        private void LoadItems()
        {
            withdrawItemsBinding.DataSource = AssetsSession.AssetTransItemList;
            dgGridItems.RefreshData();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void BtnNewtrans_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbLineCode.Text != "")
                {
                    using (var context = new GMCSDatabaseContext())
                    {
                        var line = context.line_Msts.FirstOrDefault(br => br.line_nm == cmbLineCode.Text);
                        if (line != null)
                        {
                            txtLineName.Text = line.line_nm;
                        }
                        else
                        {
                            txtLineName.Text = "";
                            MessageBox.Show("Invalid Line", "Inventory System!", MessageBoxButton.OK, MessageBoxImage.Error);
                            cmbLineCode.Focus();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot retrieve linecodes from line_mst", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewTransaction();
            //}
            //else
            //{
            //    //MessageBox.Show("Specify Linecode!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            //    //cmbLineCode.Focus();
            //}
        }

        private void NewTransaction()
        {
            dgAssets.IsEnabled = false;

            rbWithdraw.IsEnabled = true;
            rbReserve.IsEnabled = true;
            btnPrint.IsEnabled = true;
            cmbLineCode.IsEnabled = true;
            btnCancelTrans.IsEnabled = true;
            btnFinish.IsEnabled = true;
            btnBarrower.IsEnabled = true;
            dgGridItems.IsEnabled = false;
            pnlGroups.IsEnabled = true;
            btnNewtrans.IsEnabled = false;
            txtTransID.Text = "0";
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to go back to main menu?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
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
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (txtLineName.Text != "" || cmbLineCode.Text != "")
            {
                if (AssetsSession.AssetTransactionMode == "Withdraw" || AssetsSession.AssetTransactionMode == "Reserve")
                {
                    if (AssetsSession.AssetTransItemList.Count <= 0)
                    {
                        MessageBox.Show("This transaction does not contain any item/s!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    int total = 0;
                    foreach (var item in AssetsSession.AssetTransItemList)
                    {
                        total = total + item.Quantity;
                        item.LineName = cmbLineCode.Text;
                    }
                    float totalCost = 0;
                    foreach (var item in AssetsSession.AssetTransItemList)
                    {
                        totalCost = totalCost + item.TotalCost;
                        item.LineName = cmbLineCode.Text;
                        if (AssetsSession.AssetTransactionMode == "Withdraw")
                        {
                            item.Date = DateTime.Now;
                            item.DateWithdrawn = DateTime.Now;
                        }
                        else
                        {
                            item.Date = DateTime.Now;
                            item.DateWithdrawn = DateTime.MinValue;
                        }
                    }

                    AssetsSession.NewAssetTransaction.TotalItem = total;
                    AssetsSession.NewAssetTransaction.TotalCost = totalCost;
                    AssetsSession.NewAssetTransaction.Remarks = "N/A";
                    AssetsSession.NewAssetTransaction.Date = DateTime.Now;
                    AssetsSession.NewAssetTransaction.LineName = cmbLineCode.Text;
                    
                    AssetsSession.NewAssetTransaction.DateWithdrawn = DateTime.MinValue;
                    

                    AssetsSession.AssetTransactionScan = true;
                    IDScanWindow iDScanWindow = new IDScanWindow();
                    iDScanWindow.ShowDialog();
                    if (iDScanWindow.DialogResult == true)
                    {
                        using (var context = new DatabaseContext())
                        {
                            context.AssetAndEquipmentTransactions.Add(AssetsSession.NewAssetTransaction);
                            context.SaveChanges();
                        }
                        foreach (var item in AssetsSession.AssetTransItemList)
                        {
                            using (var context2 = new DatabaseContext())
                            {
                                context2.AssetTransactionItems.Add(item);
                                context2.SaveChanges();
                            }
                        }

                        foreach (var item in AssetsSession.AssetTransItemList)
                        {
                            using (var context = new DatabaseContext())
                            {
                                var jigs = context.AssetAndEquipments.FirstOrDefault(br => br.AssetID == item.AssetID);
                                if (jigs != null)
                                {
                                    jigs.Qty_Available = jigs.Qty_Available - item.Quantity;
                                    jigs.TotalCost = jigs.UnitCost * jigs.Qty_Available;
                                    context.SaveChanges();
                                }
                            }
                        }
                    }
                }
                //else if (TransactionSession.consumable == false && TransactionSession.sparepart == true)
                //{
                //    if (SparePartSession.TransSparePartItemList.Count <= 0)
                //    {
                //        MessageBox.Show("This transaction does not contain any item/s!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                //        return;
                //    }
                //    int total = 0;
                //    foreach (var item in SparePartSession.TransSparePartItemList)
                //    {
                //        total = total + item.Quantity;
                //    }
                //    SparePartSession.NewTransactionSparePart.TotalItems = ConsumableSession.TransItemList.Count;
                //    SparePartSession.NewTransactionSparePart.TotalQuantity = total;
                //    SparePartSession.NewTransactionSparePart.LineCode = cmbLineCode.Text;
                //    SparePartSession.NewTransactionSparePart.LineName = txtLineName.Text;
                //    SparePartSession.TransactionSparePartScan = true;

                //    IDScanWindow iDScanWindow = new IDScanWindow();
                //    iDScanWindow.ShowDialog();
                //    if (iDScanWindow.DialogResult == true)
                //    {
                //        //try
                //        //{
                //        using (var context = new DatabaseContext())
                //        {
                //            context.SparePartTransactions.Add(SparePartSession.NewTransactionSparePart);
                //            context.SaveChanges();
                //        }
                //        foreach (var item in SparePartSession.TransSparePartItemList)
                //        {
                //            using (var context2 = new DatabaseContext())
                //            {
                //                context2.SparePartTransactionItems.Add(item);
                //                context2.SaveChanges();
                //            }
                //        }

                //        foreach (var item in SparePartSession.TransSparePartItemList)
                //        {
                //            using (var context = new DatabaseContext())
                //            {
                //                var sparepart = context.SpareParts.FirstOrDefault(br => br.PartID == item.PartID);
                //                if (sparepart != null)
                //                {
                //                    sparepart.AvailableQuantity = sparepart.AvailableQuantity - item.Quantity;
                //                    context.SaveChanges();
                //                }
                //            }
                //        }
                EndTransaction();
                //    }

                //}
            }
            else
            {
                MessageBox.Show("Specify Linecode!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbLineCode.Focus();
            }
        }

        private void BtnCancelTrans_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure you want to cancel this transaction?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {

                EndTransaction();
            }
        }

        private void EndTransaction()
        {
            cmbLineCode.Text = "";
            cmbLineCode.IsEnabled = true;
            txtLineName.Text = "";
            btnCancelTrans.IsEnabled = false;
            btnFinish.IsEnabled = false;
            btnBarrower.IsEnabled = false;
            dgAssets.IsEnabled = false;
            pnlGroups.IsEnabled = false;
            dgGridItems.IsEnabled = false;
            btnNewtrans.IsEnabled = true;
            btnPrint.IsEnabled = false;

            AssetsSession.AssetTransItemList.Clear();
            AssetsSession.NewAssetTransaction.TransactionID = "";
            LoadItems();
            LoadAssets();

            rbWithdraw.IsChecked = false;
            rbReserve.IsChecked = false;


            txtTotal.Text = "0";
            txtTransID.Text = "0";
        }

        private void BtnBarrower_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnPrint_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void RbWithdraw_Click(object sender, RoutedEventArgs e)
        {
            AssetsSession.AssetTransactionMode = "Withdraw";
            //dgFieldLocation.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            rbWithdraw.IsEnabled = false;
            rbReserve.IsEnabled = false;
            dgAssets.IsEnabled = true;
            dgGridItems.IsEnabled = true;

            string tempID = "";

            while (true)
            {
                using (var context = new DatabaseContext())
                {
                    tempID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                    var test = context.AssetAndEquipmentTransactions.FirstOrDefault(br => br.TransactionID == tempID);
                    if (test == null)
                    {
                        break;
                    }
                }
            }
            txtTransID.Text = tempID;
            AssetsSession.NewAssetTransaction.Type = "Withdraw";
            AssetsSession.NewAssetTransaction.TransactionID = tempID;
            //JigsSession.NewJigTransaction.LineCode = cmbLineCode.Text;
            AssetsSession.NewAssetTransaction.LineName = txtLineName.Text;
            AssetsSession.NewAssetTransaction.Remarks = "N/A";
            AssetsSession.NewAssetTransaction.DateWithdrawn = DateTime.Now;
            AssetsSession.NewAssetTransaction.Date = DateTime.Now;
            AssetsSession.NewAssetTransaction.PIC = UserSession.UserName;
        }

        private void RbReserve_Click(object sender, RoutedEventArgs e)
        {
            AssetsSession.AssetTransactionMode = "Reserve";
            //dgFieldLocation.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            rbWithdraw.IsEnabled = false;
            rbReserve.IsEnabled = false;
            dgAssets.IsEnabled = true;
            dgGridItems.IsEnabled = true;

            string tempID = "";

            while (true)
            {
                using (var context = new DatabaseContext())
                {
                    tempID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                    var test = context.AssetAndEquipmentTransactions.FirstOrDefault(br => br.TransactionID == tempID);
                    if (test == null)
                    {
                        break;
                    }
                }
            }
            txtTransID.Text = tempID;
            AssetsSession.NewAssetTransaction.Type = "Reserve";
            AssetsSession.NewAssetTransaction.TransactionID = tempID;
            //JigsSession.NewJigTransaction.LineCode = cmbLineCode.Text;
            AssetsSession.NewAssetTransaction.LineName = txtLineName.Text;
            AssetsSession.NewAssetTransaction.Remarks = "Reserved to {name}";
            AssetsSession.NewAssetTransaction.DateWithdrawn = DateTime.MinValue;
            AssetsSession.NewAssetTransaction.Date = DateTime.Now;
            AssetsSession.NewAssetTransaction.PIC = UserSession.UserName;
        }

        private void BtnItemDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DgJigWithdrawTableView_CellValueChanged_1(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }
    }
}
