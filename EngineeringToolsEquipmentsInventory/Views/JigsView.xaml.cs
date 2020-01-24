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
    /// Interaction logic for JigsView.xaml
    /// </summary>
    public partial class JigsView : UserControl
    {
        public JigsView()
        {
            InitializeComponent();
        }

        System.Windows.Forms.BindingSource jigBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource jigItemBinding = new System.Windows.Forms.BindingSource();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new GMCSDatabaseContext())
                {
                    var lines = context.line_Msts;
                    if (lines.Count() > 0)
                    {
                        foreach (var item in lines)
                        {
                            cmbLineCode.Items.Add(item.line_c);
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
            dgJig.ItemsSource = jigBinding;
            LoadJigs();
            dgGridItems.ItemsSource = jigItemBinding;
        }

        private void LoadJigs()
        {
            
            using (var context = new DatabaseContext())
            {
                var jigs = context.Jigs;
                if (jigs.Count() > 0)
                {
                    jigBinding.DataSource = jigs.ToList();
                    dgJig.ItemsSource = jigBinding;
                    dgJig.RefreshData();
                }
            }
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RbWithdraw_Click(object sender, RoutedEventArgs e)
        {
            JigsSession.TransactionMode = "Withdraw";
            dgFieldLocation.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            rbWithdraw.IsEnabled = false;
            rbReserve.IsEnabled = false;
            dgJig.IsEnabled = true;
            dgGridItems.IsEnabled = true;

            string tempID = "";

            while (true)
            {
                using (var context = new DatabaseContext())
                {
                    tempID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                    var test = context.JigTransactions.FirstOrDefault(br => br.TransactionID == tempID);
                    if (test ==  null)
                    {
                        break;
                    }
                }
            }
            JigsSession.NewJigTransaction.Type = "Withdraw";
            JigsSession.NewJigTransaction.TransactionID = tempID;
            JigsSession.NewJigTransaction.LineCode = cmbLineCode.Text;
            JigsSession.NewJigTransaction.LineName = txtLineName.Text;
            JigsSession.NewJigTransaction.Date = DateTime.Now;
            JigsSession.NewJigTransaction.PIC = UserSession.UserName;

        }

        private void RbReserve_Click(object sender, RoutedEventArgs e)
        {
            JigsSession.TransactionMode = "Reserve";
            dgFieldLocation.AllowEditing = DevExpress.Utils.DefaultBoolean.True;
            rbWithdraw.IsEnabled = false;
            rbReserve.IsEnabled = false;
            dgJig.IsEnabled = true;
            dgGridItems.IsEnabled = true;

            string tempID = "";

            while (true)
            {
                using (var context = new DatabaseContext())
                {
                    tempID = "TRANS" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                    var test = context.JigTransactions.FirstOrDefault(br => br.TransactionID == tempID);
                    if (test == null)
                    {
                        break;
                    }
                }
            }
            JigsSession.NewJigTransaction.Type = "Reserve";
            JigsSession.NewJigTransaction.TransactionID = tempID;
            JigsSession.NewJigTransaction.LineCode = cmbLineCode.Text;
            JigsSession.NewJigTransaction.LineName = txtLineName.Text;
            JigsSession.NewJigTransaction.Date = DateTime.Now;
            JigsSession.NewJigTransaction.PIC = UserSession.UserName;


        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (JigsSession.NewJigTransaction.TransactionID != null)
            {
                var selectedItems = dgJig.SelectedItem as Jig;
                if (selectedItems != null)
                {

                    var orderJigs = JigsSession.JigTransItemList.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                    if (orderJigs != null)
                    {
                        using (var context = new DatabaseContext())
                        {
                            var item = context.Jigs.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                            if (item != null)
                            {
                                if (int.Parse(item.Balance) < (orderJigs.TotalItem+1))
                                {
                                    MessageBox.Show("Not enough stocks available!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                                orderJigs.TotalItem = orderJigs.TotalItem + 1;
                                orderJigs.TotalCost = orderJigs.TotalItem * orderJigs.Cost;
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
                    if (int.Parse(selectedItems.Balance) <= 0)
                    {
                        MessageBox.Show("This item is Out of Stock!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    using (var context = new DatabaseContext())
                    {
                        var item = context.Jigs.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                        if (item != null)
                        {
                            JigTransactionItem jigTransactionItem = new JigTransactionItem();
                            string tempID = "";
                            while (true)
                            {
                                using (var context2 = new DatabaseContext())
                                {
                                    tempID = "TRANSIT" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                                    var check = context.JigTransactionItems.FirstOrDefault(br => br.JigTransactionItemID == tempID);
                                    if (check == null)
                                    {
                                        break;
                                    }
                                }
                            }

                            jigTransactionItem.JigTransactionItemID = tempID;
                            jigTransactionItem.ItemCode = item.ItemCode;
                            jigTransactionItem.TransactionID = JigsSession.NewJigTransaction.TransactionID;
                            if (selectedItems.UnitCost != "")
                            {
                                jigTransactionItem.Cost = float.Parse(selectedItems.UnitCost);
                                jigTransactionItem.TotalCost = float.Parse(selectedItems.UnitCost) * 1;
                            }
                            else
                            {
                                jigTransactionItem.Cost = 0;
                                jigTransactionItem.TotalCost = 0;
                            }
                            jigTransactionItem.Status = JigsSession.TransactionMode;
                            jigTransactionItem.JigCode = selectedItems.JigCode;
                            jigTransactionItem.Location = selectedItems.Location;
                            jigTransactionItem.Type = selectedItems.Type ;
                            jigTransactionItem.Specification = selectedItems.Specification ;
                            jigTransactionItem.RefNo = selectedItems.RefNo ;
                            jigTransactionItem.PIC = selectedItems.PIC;
                            jigTransactionItem.WarehousePIC = UserSession.UserName ;
                            jigTransactionItem.Date = DateTime.Now;
                            jigTransactionItem.TotalItem = 1;
                            JigsSession.JigTransItemList.Add(jigTransactionItem);
                            LoadItems();
                        }
                    }
                }
            }
        }

        private void LoadItems()
        {
            jigItemBinding.DataSource = JigsSession.JigTransItemList.ToList();
            dgGridItems.ItemsSource = jigItemBinding;
            dgGridItems.RefreshData();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void BtnBarrower_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (JigsSession.TransactionMode  == "Withdraw" || JigsSession.TransactionMode == "Reserve")
            {
                if (JigsSession.JigTransItemList.Count <= 0)
                {
                    MessageBox.Show("This transaction does not contain any item/s!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                int total = 0;
                foreach (var item in JigsSession.JigTransItemList)
                {
                    total = total + item.TotalItem;

                }
                float totalCost = 0;
                foreach (var item in JigsSession.JigTransItemList)
                {
                    totalCost = totalCost + item.TotalCost;
                    if (JigsSession.TransactionMode == "Withdraw")
                    {
                        item.DateWithdrawn = DateTime.Now; 
                    }
                    else
                    {
                        item.DateWithdrawn = DateTime.MinValue;
                    }
                }
                JigsSession.NewJigTransaction.TotalItem = total ;
                JigsSession.NewJigTransaction.TotalCost = totalCost;
                JigsSession.JgsTransactionScan = true;
                IDScanWindow iDScanWindow = new IDScanWindow();
                iDScanWindow.ShowDialog();
                if (iDScanWindow.DialogResult == true)
                {
                    //try
                    //{
                    using (var context = new DatabaseContext())
                    {
                        context.JigTransactions.Add(JigsSession.NewJigTransaction);
                        context.SaveChanges();
                    }
                    foreach (var item in JigsSession.JigTransItemList)
                    {
                        using (var context2 = new DatabaseContext())
                        {
                            context2.JigTransactionItems.Add(item);
                            context2.SaveChanges();
                        }
                    }

                    foreach (var item in JigsSession.JigTransItemList)
                    {
                        using (var context = new DatabaseContext())
                        {
                            var jigs = context.Jigs.FirstOrDefault(br => br.ItemCode == item.ItemCode);
                            if (jigs != null)
                            {
                                jigs.Balance = (int.Parse(jigs.Balance) - item.TotalItem).ToString();
                                jigs.TotalCost = (float.Parse(jigs.UnitCost) * int.Parse(jigs.Balance)).ToString();
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
                foreach (var item in SparePartSession.TransSparePartItemList)
                {
                    total = total + item.Quantity;
                }
                SparePartSession.NewTransactionSparePart.TotalItems = ConsumableSession.TransItemList.Count;
                SparePartSession.NewTransactionSparePart.TotalQuantity = total;
                SparePartSession.NewTransactionSparePart.LineCode = cmbLineCode.Text;
                SparePartSession.NewTransactionSparePart.LineName = txtLineName.Text;
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
                }

            }
        }

        private void BtnCancelTrans_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to cancel this transaction?","Inventory System",MessageBoxButton.YesNo,MessageBoxImage.Question)==MessageBoxResult.Yes)
            {
                EndTransaction();
            }
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
                            var line = context.line_Msts.FirstOrDefault(br => br.line_c == cmbLineCode.Text);
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

        private void NewTransaction()
        {
            dgJig.IsEnabled = false;

            rbWithdraw.IsEnabled = true;
            rbReserve.IsEnabled = true;

            cmbLineCode.IsEnabled = false;
            btnCancelTrans.IsEnabled = true;
            btnFinish.IsEnabled = true;
            btnBarrower.IsEnabled = true;
            dgGridItems.IsEnabled = false;
            pnlGroups.IsEnabled = true;
            btnNewtrans.IsEnabled = false;


        }
        private void EndTransaction()
        { 
            cmbLineCode.Text = "";
            cmbLineCode.IsEnabled = true;
            txtLineName.Text = "";
            btnCancelTrans.IsEnabled = false;
            btnFinish.IsEnabled = false;
            btnBarrower.IsEnabled = false;
            dgJig.IsEnabled = false;
            pnlGroups.IsEnabled = false;
            dgGridItems.IsEnabled = false;
            btnNewtrans.IsEnabled = true;

            JigsSession.JigTransItemList.Clear();
            JigsSession.NewJigTransaction.TransactionID = "";
            LoadItems();
            LoadJigs();

            rbWithdraw.IsChecked = false;
            rbReserve.IsChecked = false;


            txtTotal.Text = "0";
            txtTransID.Text = "0";
        }
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

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

        private void CmbLineCode_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                using (var context = new GMCSDatabaseContext())
                {
                    var line = context.line_Msts.FirstOrDefault(br => br.line_c == cmbLineCode.Text);
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
                        var line = context.line_Msts.FirstOrDefault(br => br.line_c == cmbLineCode.Text);
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
                        var line = context.line_Msts.FirstOrDefault(br => br.line_c == cmbLineCode.Text);
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

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {

        }

        private void BtnItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGridItems.SelectedItem as JigTransactionItem;
            if (selectedItem != null)
            {
                var part = JigsSession.JigTransItemList.FirstOrDefault(br => br.JigTransactionItemID == selectedItem.JigTransactionItemID);
                if (part != null)
                {
                    JigsSession.JigTransItemList.Remove(part);
                    LoadItems();
                }
            }
        }

        private void DgJigWithdrawTableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgGridItems.SelectedItem as JigTransactionItem;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var consumable = context.Jigs.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                    if (consumable != null)
                    {
                        if (selectedItem.TotalItem > int.Parse(consumable.Balance))
                        {
                            MessageBox.Show("Invalid Quantity! not enough stocks.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            selectedItem.TotalItem = 1;
                            return;
                        }
                        //if ((consumable.AvailableQuantity - selectedItem.Quantity) < consumable.MaintainingQuantity)
                        //{
                        //    MessageBox.Show("Invalid Quantity! not enough stocks.", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        //    selectedItem.Quantity = 1;
                        //    return;
                        //}
                    }

                    selectedItem.Cost = selectedItem.TotalItem * float.Parse(consumable.UnitCost);
                }
            }
        }
    }
}
