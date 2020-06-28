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
    /// Interaction logic for ConsumablesView.xaml
    /// </summary>
    public partial class ConsumablesView : UserControl
    {
        public ConsumablesView()
        {
            InitializeComponent();
        }


        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        List<TransactionItem> TransactionItem = new List<TransactionItem>();
        //Binding ItemBinding = new Binding();
        System.Windows.Forms.BindingSource ItemBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource consumableItemBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource spareItemBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource spareOrderedItemBinding = new System.Windows.Forms.BindingSource();
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

        private void LoadConsumables(string Group)
        {
            try
            {
                if (Group != "ALL")
                {

                    using (var context = new DatabaseContext())
                    {
                        var consumable = context.Consumables.Where(br => br.Group == Group);

                        dgConsumables.ItemsSource = consumable.ToList();
                    }
                }
                else
                {
                    using (var context = new DatabaseContext())
                    {
                        dgGridItems.Visibility = Visibility.Visible;
                        dgSpareItems.Visibility = Visibility.Collapsed;
                        dgSpareParts.Visibility = Visibility.Collapsed;
                        dgConsumables.Visibility = Visibility.Visible;
                        dgTableView.Header = "Consumables";
                        var consumable = context.Consumables;
                        dgConsumables.ItemsSource = consumable.ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGridItems.SelectedItem as TransactionItem;
            if (selectedItem != null)
            {
                int cnt = 0;
                foreach (var item in ConsumableSession.TransItemList)
                {
                    if (item.ItemCode == selectedItem.ItemCode)
                    {
                        ConsumableSession.TransItemList.RemoveAt(cnt);
                        break;
                    }
                    cnt++;
                }
                //LoadItems();
                DeleteItem();
            }
        }

        private void BtnBarrower_Click(object sender, RoutedEventArgs e)
        {
            BarrowerLookupWindow barrowerLookupWindow = new BarrowerLookupWindow();
            barrowerLookupWindow.ShowDialog();
        }

        private void BtnBMG_Click(object sender, RoutedEventArgs e)
        {
            LoadConsumables("BMG");
        }

        private void BtnCKT_Click(object sender, RoutedEventArgs e)
        {
            LoadConsumables("CKT/CPG");
        }

        private void BtnSewing_Click(object sender, RoutedEventArgs e)
        {
            LoadConsumables("SEWING");
        }

        private void BtnFab_Click(object sender, RoutedEventArgs e)
        {
            LoadConsumables("FAB");
        }

        private void BtnCommon_Click(object sender, RoutedEventArgs e)
        {
            LoadConsumables("Common");
        }

        private void BtnElect_Click(object sender, RoutedEventArgs e)
        {
            LoadConsumables("Electrical/Air ");
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {

            dgTableView.Header = "Consumables";
            string Temp_TransID = "";

            while (true)
            {
                Temp_TransID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                using (var context = new DatabaseContext())
                {
                    var check = context.Transactions.FirstOrDefault(br => br.TransactionID == Temp_TransID);
                    if (check == null)
                    {
                        break;
                    }
                }
            }
            ConsumableSession.NewTransaction.TransactionID = Temp_TransID;
            ConsumableSession.NewTransaction.TransactionDate = DateTime.Now;
            ConsumableSession.NewTransaction.LoginName = UserSession.UserName;
            TransactionSession.consumable = true;
            TransactionSession.sparepart = false;
            txtTransID.Text = Temp_TransID;
            LoadConsumables("ALL");

            btnAll.IsEnabled = false;
            btnSpareParts.IsEnabled = false;

            dgConsumables.IsEnabled = true;

        }

        private void NewTransaction()
        {
            dgSpareItems.IsEnabled = true;
            dgSpareParts.IsEnabled = true;


            cmbLineCode.IsEnabled = false;
            btnCancelTrans.IsEnabled = true;
            btnFinish.IsEnabled = true;
            btnBarrower.IsEnabled = true;
            dgConsumables.IsEnabled = false;
            dgGridItems.IsEnabled = false;
            pnlGroups.IsEnabled = true;
            btnNewtrans.IsEnabled = false;


        }
        private void EndTransaction()
        {
            btnAll.IsEnabled = true;
            btnSpareParts.IsEnabled = true;

            dgSpareItems.IsEnabled = false;
            dgSpareParts.IsEnabled = false;
            cmbLineCode.Text = "";
            cmbLineCode.IsEnabled = true;
            txtLineName.Text = "";
            btnCancelTrans.IsEnabled = false;
            btnFinish.IsEnabled = false;
            btnBarrower.IsEnabled = false;
            dgConsumables.IsEnabled = false;
            pnlGroups.IsEnabled = false;
            dgGridItems.IsEnabled = false;
            btnNewtrans.IsEnabled = true;
            ConsumableSession.NewTransaction.TransactionID = "";
            ConsumableSession.TransItemList.Clear();

            SparePartSession.NewTransactionSparePart.TransactionID = "";
            SparePartSession.TransSparePartItemList.Clear();

            dgGridItems.ItemsSource = null;
            txtTotal.Text = "0";
            txtTransID.Text = "0";
            LoadConsumables("ALL");
        }

        private void DeleteItem()
        {
            dgGridItems.ItemsSource = null;
            dgGridItems.RefreshData();
            dgGridItems.ItemsSource = ConsumableSession.TransItemList;
            dgGridItems.RefreshData();
            txtTotal.Text = ConsumableSession.TransItemList.Count.ToString();
        }

        private void BtnNewtrans_Click(object sender, RoutedEventArgs e)
        {
            if (cmbLineCode.Text != "")
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
                                MessageBox.Show("Invalid Linecode", "Inventory System!", MessageBoxButton.OK, MessageBoxImage.Error);
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
            }
            else
            {
                MessageBox.Show("Specify Linecode!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                cmbLineCode.Focus();
            }
        }

        private void BtnCancelTrans_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this transaction?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                EndTransaction();
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionSession.consumable == true && TransactionSession.sparepart == false)
            {
                if (ConsumableSession.TransItemList.Count <= 0)
                {
                    MessageBox.Show("This transaction does not contain any item/s!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int total = 0;
                foreach (var item in ConsumableSession.TransItemList)
                {
                    total = total + item.Quantity;

                }
                ConsumableSession.NewTransaction.NumberofItem = ConsumableSession.TransItemList.Count;
                ConsumableSession.NewTransaction.TotalQuantity = total;
                ConsumableSession.NewTransaction.LineCode = cmbLineCode.Text;
                ConsumableSession.NewTransaction.LineName = txtLineName.Text;
                ConsumableSession.TransactionScan = true;
                IDScanWindow iDScanWindow = new IDScanWindow();
                iDScanWindow.ShowDialog();
                if (iDScanWindow.DialogResult == true)
                {
                    //try
                    //{
                        using (var context = new DatabaseContext())
                        {
                            context.Transactions.Add(ConsumableSession.NewTransaction);
                            context.SaveChanges();
                        }
                        foreach (var item in ConsumableSession.TransItemList)
                        {
                            using (var context2 = new DatabaseContext())
                            {
                                context2.TransactionItems.Add(item);
                                context2.SaveChanges();
                            }
                        }

                        foreach (var item in ConsumableSession.TransItemList)
                        {
                            using (var context = new DatabaseContext())
                            {
                                var consumable = context.Consumables.FirstOrDefault(br => br.ItemCode == item.ItemCode);
                                if (consumable != null)
                                {
                                    consumable.RemainingQuantity = consumable.RemainingQuantity - item.Quantity;
                                    context.SaveChanges();
                                }
                            }
                        }
                        EndTransaction();
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    //}
                }
            }
            else if (TransactionSession.consumable == false && TransactionSession.sparepart == true)
            {
                if (SparePartSession.TransSparePartItemList.Count <= 0)
                {
                    MessageBox.Show("This transaction does not contain any item/s!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int total = 0;
                float totalCost = 0;
                foreach (var item in SparePartSession.TransSparePartItemList)
                {
                    total = total + item.Quantity;
                    totalCost = totalCost + item.TotalCost;
                }
                SparePartSession.NewTransactionSparePart.TotalItems = SparePartSession.TransSparePartItemList.Count;
                SparePartSession.NewTransactionSparePart.TotalQuantity = total;
                SparePartSession.NewTransactionSparePart.LineCode = cmbLineCode.Text;
                SparePartSession.NewTransactionSparePart.LineName = txtLineName.Text;
                SparePartSession.NewTransactionSparePart.TotalCost = totalCost;
                SparePartSession.TransactionSparePartScan = true;

                IDScanWindow iDScanWindow = new IDScanWindow();
                iDScanWindow.ShowDialog();
                if (iDScanWindow.DialogResult == true)
                {
                    //try
                    //{
                        using (var context = new DatabaseContext())
                        {
                            context.SparePartTransactions.Add(SparePartSession.NewTransactionSparePart);
                            context.SaveChanges();
                        }
                        foreach (var item in SparePartSession.TransSparePartItemList)
                        {
                            using (var context2 = new DatabaseContext())
                            {
                                context2.SparePartTransactionItems.Add(item);
                                context2.SaveChanges();
                            }
                        }

                        foreach (var item in SparePartSession.TransSparePartItemList)
                        {
                            using (var context = new DatabaseContext())
                            {
                                var sparepart = context.SpareParts.FirstOrDefault(br => br.PartID == item.PartID);
                                if (sparepart != null)
                                {
                                    sparepart.AvailableQuantity = sparepart.AvailableQuantity - item.Quantity;
                                    context.SaveChanges();
                                }
                            }
                        }
                        EndTransaction();
                    //}
                    //catch (Exception ex)
                    //{
                    //    //MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    //    throw;
                    //}
                }

            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            LoadConsumables("ALL");
            try
            {
                using (var context = new GMCSDatabaseContext())
                {
                    var lines = context.line_Msts;
                    if (lines.Count() > 0)
                    {
                        foreach (var item in lines)
                        {
                            cmbLineCode.Items.Add(item.line_nm);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Cannot retrieve linecodes from line_mst", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Cannot retrieve linecodes from line_mst", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                MessageBox.Show(ex.ToString());
            }

        }

        private void LoadSpareParts()
        {
            dgGridItems.Visibility = Visibility.Collapsed;
            dgSpareItems.Visibility = Visibility.Visible;
            dgConsumables.Visibility = Visibility.Collapsed;
            dgSpareParts.Visibility = Visibility.Visible;
            using (var context = new DatabaseContext())
            {
                var parts = context.SpareParts;
                if (parts.Count() > 0)
                {
                    spareItemBinding.DataSource = parts.ToList();
                    dgSpareParts.ItemsSource = spareItemBinding;
                    dgSpareParts.RefreshData();
                }
            }
        }

        private void LoadItems()
        {
            dgConsumables.Visibility = Visibility.Visible;
            dgSpareParts.Visibility = Visibility.Collapsed;
            ItemBinding.DataSource = ConsumableSession.TransItemList;
            dgGridItems.ItemsSource = ItemBinding;
            dgGridItems.RefreshData();
            txtTotal.Text = ItemBinding.Count.ToString();
        }

        private void LoadSparePartsItem()
        {
            spareOrderedItemBinding.DataSource = SparePartSession.TransSparePartItemList;
            dgSpareItems.ItemsSource = spareOrderedItemBinding;
            dgSpareItems.RefreshData();
            txtTotal.Text = spareOrderedItemBinding.Count.ToString();

        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (ConsumableSession.NewTransaction.TransactionID != null)
            {
                var selectedItems = dgConsumables.SelectedItem as Consumable;
                if (selectedItems != null)
                { 
                    var check = ConsumableSession.TransItemList.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                    if (check != null)
                    {
                        using (var context = new DatabaseContext())
                        {
                            var item = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                            if (item != null)
                            {
                                if (item.RemainingQuantity < (check.Quantity + 1))
                                {
                                    MessageBox.Show("Not enough stocks available!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                check.Quantity = check.Quantity + 1;
                                if ((item.RemainingQuantity - check.Quantity) < item.MaintainingQuantity)
                                {
                                    MessageBox.Show("This item is in critical level! reorder is required", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                
                                LoadItems();
                                return;
                            }
                        }

                    }


                    if (selectedItems.MaintainingQuantity >= selectedItems.RemainingQuantity)
                    {
                        MessageBox.Show("This item is in critical level! reorder is required", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (selectedItems.RemainingQuantity <= 0)
                    {
                        MessageBox.Show("This item is Out of Stock!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    using (var context = new DatabaseContext())
                    {
                        var item = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                        if (item != null)
                        {
                            TransactionItem transactionItems = new TransactionItem();
                            string tempID = "";
                            while (true)
                            {
                                using (var context2 = new DatabaseContext())
                                {
                                    tempID = "TRANSIT" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                                    var idCheck = context.TransactionItems.FirstOrDefault(br => br.TransactionItemID == tempID);
                                    if (idCheck == null)
                                    {
                                        break;
                                    }
                                }
                            }
                            transactionItems.TransactionItemID = tempID;
                            transactionItems.TransactionID = ConsumableSession.NewTransaction.TransactionID;
                            transactionItems.ItemCode = selectedItems.ItemCode;
                            transactionItems.Group = selectedItems.Group;
                            transactionItems.ItemName = selectedItems.ItemName;
                            transactionItems.UOM = selectedItems.UOM;
                            transactionItems.Quantity = 1;
                            transactionItems.Date = DateTime.Now;
                            transactionItems.LoginName = UserSession.UserName;
                            transactionItems.UserName = "";

                            ConsumableSession.TransItemList.Add(transactionItems);
                            LoadItems();

                        }
                    }
                }
            }
        }

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgGridItems.SelectedItem as TransactionItem;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var consumable = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                    if (consumable != null)
                    {
                        if (selectedItem.Quantity > consumable.RemainingQuantity)
                        {
                            MessageBox.Show("Invalid Quantity! not enough stocks.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            selectedItem.Quantity = 1;
                            return;
                        }

                        if ((consumable.RemainingQuantity - selectedItem.Quantity) < consumable.MaintainingQuantity)
                        {
                            MessageBox.Show("Invalid Quantity! not enough stocks.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            selectedItem.Quantity = 1;
                            return;
                        }
                    }

                    
                }
            }
        }

        private void CmbLineCode_DropDownClosed(object sender, EventArgs e)
        {
            try
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
                        MessageBox.Show("Invalid Linecode", "Inventory System!", MessageBoxButton.OK, MessageBoxImage.Error);
                        txtLineName.Text = "";
                        cmbLineCode.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot retrieve linecodes from line_mst", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbLineCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
                            //MessageBox.Show("Invalid Linecode", "Inventory System!", MessageBoxButton.OK, MessageBoxImage.Error);
                            //cmbLineCode.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot retrieve linecodes from line_mst", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbLineCode_LostFocus(object sender, RoutedEventArgs e)
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
                            //MessageBox.Show("Invalid Linecode", "Inventory System!", MessageBoxButton.OK, MessageBoxImage.Error);
                            //cmbLineCode.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Cannot retrieve linecodes from line_mst", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSpareParts_Click(object sender, RoutedEventArgs e)
        {

            dgTableView.Header = "Spare Parts";

            string Temp_TransID = "";

            while (true)
            {
                Temp_TransID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                using (var context = new DatabaseContext())
                {
                    var check = context.SparePartTransactions.FirstOrDefault(br => br.TransactionID == Temp_TransID);
                    if (check == null)
                    {
                        break;
                    }
                }
            }
            SparePartSession.NewTransactionSparePart.TransactionID = Temp_TransID;
            SparePartSession.NewTransactionSparePart.Date = DateTime.Now;
            SparePartSession.NewTransactionSparePart.PIC = UserSession.UserName;
            TransactionSession.consumable = false;
            TransactionSession.sparepart = true;

            txtTransID.Text = Temp_TransID;
            LoadSpareParts();
            dgdgSparePartsTableView.Header = "Spare Parts";

            btnSpareParts.IsEnabled = false;
            btnAll.IsEnabled = false;
            dgSpareParts.IsEnabled = true;

        }

        private void BtnSpareDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgSpareItems.SelectedItem as SparePartTransactionItem;
            if (selectedItem != null)
            {
                var part = SparePartSession.TransSparePartItemList.FirstOrDefault(br => br.TransactionItemID == selectedItem.TransactionItemID);
                if (part != null)
                {
                    SparePartSession.TransSparePartItemList.Remove(part);
                    LoadSparePartsItem();
                }
            }
        }

        private void BtnSparePartAdd_Click(object sender, RoutedEventArgs e)
        {
            if (SparePartSession.NewTransactionSparePart.TransactionID != null)
            {
                var selectedItems = dgSpareParts.SelectedItem as SparePart;
                if (selectedItems != null)
                { 
                    if (selectedItems.MaintainingQuantity >= selectedItems.AvailableQuantity)
                    {
                        MessageBox.Show("This item is in critical level! reorder is required", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (selectedItems.AvailableQuantity <= 0)
                    {
                        MessageBox.Show("This item is Out of Stock!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var test =SparePartSession.TransSparePartItemList.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                    if (test != null)
                    {
                        test.Quantity = test.Quantity + 1;
                        test.TotalCost = test.Cost * test.Quantity;
                        LoadSparePartsItem();
                        return;
                    } 

                    using (var context = new DatabaseContext())
                    {
                        var item = context.SpareParts.FirstOrDefault(br => br.PartID == selectedItems.PartID);
                        if (item != null)
                        {
                            SparePartTransactionItem transactionItems = new SparePartTransactionItem();
                            string tempID = "";
                            while (true)
                            {
                                using (var context2 = new DatabaseContext())
                                {
                                    tempID = "TRANSIT" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                                    var check = context.SparePartTransactionItems.FirstOrDefault(br => br.TransactionItemID == tempID);
                                    if (check == null)
                                    {
                                        break;
                                    }
                                }
                            }
                            transactionItems.TransactionItemID = tempID;
                            transactionItems.TransactionID = SparePartSession.NewTransactionSparePart.TransactionID;
                            transactionItems.ItemCode = selectedItems.ItemCode;
                            transactionItems.PartID = selectedItems.PartID;
                            transactionItems.Location = selectedItems.Location;
                            transactionItems.Type = selectedItems.Type;
                            transactionItems.ItemName = selectedItems.ItemName;
                            transactionItems.Cost = selectedItems.Cost;
                            transactionItems.Date = DateTime.Now;
                            transactionItems.PIC = UserSession.UserName;
                            transactionItems.UserName = "";
                            transactionItems.Date = DateTime.Now;
                            transactionItems.Quantity = 1;
                            transactionItems.TotalCost = transactionItems.Cost * transactionItems.Quantity;

                            SparePartSession.TransSparePartItemList.Add(transactionItems);
                            LoadSparePartsItem();
                        }
                    }
                }
            }
        }

        private void BtnAll_Click_1(object sender, RoutedEventArgs e)
        {
            dgTableView.Header = "Consumables";

            string Temp_TransID = "";

            while (true)
            {
                Temp_TransID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                using (var context = new DatabaseContext())
                {
                    var check = context.Transactions.FirstOrDefault(br => br.TransactionID == Temp_TransID);
                    if (check == null)
                    {
                        break;
                    }
                }
            }
            ConsumableSession.NewTransaction.TransactionID = Temp_TransID;
            ConsumableSession.NewTransaction.TransactionDate = DateTime.Now;
            ConsumableSession.NewTransaction.LoginName = UserSession.UserName;

            txtTransID.Text = Temp_TransID;

            dgConsumables.IsEnabled = false;
            btnAll.IsEnabled = false;
            btnSpareParts.IsEnabled = false;
            dgConsumables.IsEnabled = true;

            dgGridItems.IsEnabled = true;
            dgConsumables.Visibility = Visibility.Visible;

            TransactionSession.consumable = true;
            TransactionSession.sparepart = false;

            //string Temp_TransID = "";

            //while (true)
            //{
            //    Temp_TransID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
            //    using (var context = new DatabaseContext())
            //    {
            //        var check = context.Transactions.FirstOrDefault(br => br.TransactionID == Temp_TransID);
            //        if (check == null)
            //        {
            //            break;
            //        }
            //    }
            //}
            //ConsumableSession.NewTransaction.TransactionID = Temp_TransID;
            //ConsumableSession.NewTransaction.TransactionDate = DateTime.Now;
            //ConsumableSession.NewTransaction.LoginName = UserSession.UserName;

            //txtTransID.Text = Temp_TransID;
        }


        private void LoadOrderedConsumableItems()
        {
            dgGridItems.Visibility = Visibility.Visible;
            dgSpareItems.Visibility = Visibility.Collapsed;
            dgConsumables.Visibility = Visibility.Visible;
            dgSpareParts.Visibility = Visibility.Collapsed;
            using (var context = new DatabaseContext())
            {
                var consumable = context.Consumables;
                if (consumable.Count() > 0)
                {
                    consumableItemBinding.DataSource = consumable.ToList();
                    dgGridItems.ItemsSource = consumableItemBinding;
                    dgGridItems.RefreshData();
                    txtTotal.Text = consumableItemBinding.Count.ToString();
                }
            }


        }

        private void DgSpareItemsTableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgSpareItems.SelectedItem as SparePartTransactionItem;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var consumable = context.SpareParts.FirstOrDefault(br => br.PartID == selectedItem.PartID);
                    if (consumable != null)
                    {
                        if (selectedItem.Quantity > consumable.AvailableQuantity)
                        {
                            MessageBox.Show("Invalid Quantity! not enough stocks.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            selectedItem.Quantity = 1;
                            selectedItem.TotalCost = selectedItem.Quantity * consumable.Cost;
                            return;
                        }

                        if ((consumable.AvailableQuantity - selectedItem.Quantity) < consumable.MaintainingQuantity)
                        {
                            MessageBox.Show("Invalid Quantity! not enough stocks.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            selectedItem.Quantity = 1;
                            selectedItem.TotalCost = selectedItem.Quantity * consumable.Cost;
                            return;
                        }
                    } 
                    selectedItem.TotalCost = selectedItem.Quantity * consumable.Cost;
                }
            }
        }
    }
}
