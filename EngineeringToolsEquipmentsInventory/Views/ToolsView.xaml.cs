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
    public partial class ToolsView : UserControl
    {
        public ToolsView()
        {
            InitializeComponent();
        }
        private static Random random = new Random();
        System.Windows.Forms.BindingSource toolsBindingSource = new System.Windows.Forms.BindingSource();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        private void BtnNewtrans_Click(object sender, RoutedEventArgs e)
        {
            string transID = "LOAN-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
            TransactionSession.newTransaction.LoanID = transID;
            TransactionSession.newTransaction.LoanDate = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt"));

            txtTransID.Text = transID;
            NewTrans();
        }

        private void NewTrans()
        {
            btnNewtrans.IsEnabled = false;
            btnCancelTrans.IsEnabled = true;
            btnBarrower.IsEnabled = true;
            btnTool.IsEnabled = true;
            btnFinish.IsEnabled = true;
        }
        private void CancelTrans()
        {
            btnNewtrans.IsEnabled = true;
            btnCancelTrans.IsEnabled = false;
            btnBarrower.IsEnabled = false;
            btnTool.IsEnabled = false;
            btnFinish.IsEnabled = false;
            TransactionSession.barrowTools.Clear();
            LoadItems();
            dgGridItems.RefreshData();
            txtTotal.Text = "0";
            txtTransID.Text = "";

        }

        private void BtnCancelTrans_Click(object sender, RoutedEventArgs e)
        {
            CancelTrans();
        }

        private void BtnBarrower_Click(object sender, RoutedEventArgs e)
        {
            BarrowerLookupWindow barrowerLookupWindow = new BarrowerLookupWindow();
            barrowerLookupWindow.ShowDialog();
        }

        private void BtnTool_Click(object sender, RoutedEventArgs e)
        {
            ToolLookupWindow lookupWindow = new ToolLookupWindow();
            lookupWindow.ShowDialog();

            if (lookupWindow.DialogResult == true)
            {
                LoadItems();
            }
        }
        private void LoadItems()
        {
            if (TransactionSession.barrowTools.Count > 0)
            {
                dgGridItems.ItemsSource = TransactionSession.barrowTools;
                txtTotal.Text = TransactionSession.barrowTools.Count.ToString();
                dgGridItems.RefreshData();
            }
        }

        private void BtnItemDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGridItems.SelectedItem as Tool;
            if (selectedItem != null)
            {
                int cnt = 0;
                foreach (var item in TransactionSession.barrowTools)
                {
                    if (selectedItem.ItemCode == item.ItemCode)
                    {
                        //TransactionSession.barrowTools.Remove(item);
                        TransactionSession.barrowTools.RemoveAt(cnt);
                        break;
                    }
                    cnt++;
                }
                dgGridItems.ItemsSource = null;
                dgGridItems.ItemsSource = TransactionSession.barrowTools;
                dgGridItems.RefreshData();
                txtTotal.Text = TransactionSession.barrowTools.Count.ToString();
                dgGridItems.RefreshData(); 
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (TransactionSession.barrowTools.Count <= 0)
            {
                MessageBox.Show("This loan does not contain any item/s!","Inventory System",MessageBoxButton.OK,MessageBoxImage.Error);
                return;
            }
            else
            {
                TransactionSession.newTransaction.LoanQuantity = txtTotal.Text;
                TransactionSession.newTransaction.Status = "Active";
                TransactionSession.newTransaction.Remarks = "N/A";
                TransactionSession.newTransaction.LoginName = UserSession.UserName;
                TransactionSession.newTransaction.ReturnDate = DateTime.MinValue;

                IDScanWindow iDScanWindow = new IDScanWindow();
                iDScanWindow.ShowDialog();

                if (iDScanWindow.DialogResult == true)
                {
                    CancelTrans();
                    GetData();
                }
            }
        }

        private void BtnReturningScanID_Click(object sender, RoutedEventArgs e)
        {


        }

        private void LoadLoans()
        {
            var loanID = new List<Loan>();
            var loanedItems = new List<LoanedTool>();
            using (var context = new DatabaseContext())
            {
                loanID = context.Loans.Where(br => br.UserID == ReturningSession.borrowerID && br.Status == "Active").ToList();
                if (loanID.Count() >0)
                {
                    txtTotalLoan.Text = loanID.Count().ToString();
                }

            }
            if (loanID.Count() >0)
            {
                foreach (var item in loanID)
                {
                    using (var context = new DatabaseContext())
                    {
                        var loan = context.LoanedTools.Where(br => br.LoanID == item.LoanID && br.Status == "Active");
                        if (loan != null)
                        {
                            foreach (var LoanedItem in loan)
                            {
                                loanedItems.Add(LoanedItem);
                            }
                        }
                    }
                } 
            }
            toolsBindingSource.DataSource = loanedItems.ToList();
            dgGridLoans.ItemsSource = toolsBindingSource;
        }

        private void BtnReturnLoan_Click(object sender, RoutedEventArgs e)
        {
            #region Return Whole Loan 
            //var selectedItem = dgGridLoans.SelectedItem as Loan;
            //if (selectedItem != null)
            //{

            //    if (MessageBox.Show("Are you sure you want to return this loan?", "Inventory Sytem", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //    {
            //        var selectedItems = dgGridLoans.SelectedItem as Loan;
            //        if (selectedItems != null)
            //        {
            //            using (var context = new DatabaseContext())
            //            {
            //                var loan = context.Loans.FirstOrDefault(br => br.LoanID == selectedItems.LoanID);
            //                if (loan != null)
            //                {
            //                    loan.Status = "Returned";
            //                    loan.ReturnDate = DateTime.Now;
            //                    context.SaveChanges();
            //                    //LoadLoans();
            //                    var loanItem = context.LoanedTools.Where(br => br.LoanID == selectedItems.LoanID && br.Status == "Active");
            //                    if (loanItem != null)
            //                    {
            //                        foreach (var item in loanItem)
            //                        {
            //                            using (var context2 = new DatabaseContext())
            //                            {
            //                                var Loanitem = context2.LoanedTools.FirstOrDefault(br => br.LoanToolID == item.LoanToolID && br.LoanID == item.LoanID);
            //                                if (Loanitem != null)
            //                                {
            //                                    Loanitem.Status = "Returned";
            //                                    Loanitem.DateReturned = DateTime.Now;
            //                                    Loanitem.ReturnLoginName = UserSession.UserName;
            //                                    context2.SaveChanges();
            //                                }
            //                            }

            //                            using (var context3 = new DatabaseContext())
            //                            {
            //                                var updateTool = context3.Tools.FirstOrDefault(br => br.ItemCode == item.ItemCode);
            //                                if (updateTool != null)
            //                                {
            //                                    updateTool.Status = "In-Stock";
            //                                    updateTool.LastUpdate = DateTime.Now;
            //                                    context3.SaveChanges();
            //                                }
            //                            }
            //                        }
            //                    }
            //                    LoadLoans();
            //                }
            //            }
            //        }
            //    }
            //} 
            #endregion

            ClearReturning();

        }

        private void BtnLoanItems_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgGridLoans.SelectedItem as Loan;
            if (selectedItems != null)
            {
                ReturningSession.LoanItemLoanID = selectedItems.LoanID;
                LoanItemsWindow loanItemsWindow = new LoanItemsWindow();
                loanItemsWindow.ShowDialog();
                if (loanItemsWindow.DialogResult == true)
                {
                    LoadLoans();
                }
            }

        }

        private void BtnBorrowScanID_Click(object sender, RoutedEventArgs e)
        {
            ReturningSession.returnScanID = true;
            IDScanWindow iDScanWindow = new IDScanWindow();
            iDScanWindow.ShowDialog();

            if (iDScanWindow.DialogResult == true)
            {
                LoadLoans();
                using (var context = new DatabaseContext())
                {
                    var user = context.Users.FirstOrDefault(br => br.UserID == ReturningSession.borrowerID);
                    if (user != null)
                    {
                        txtBorrowerName.Text = user.Name;
                    }
                }
            }
            else
            {
                txtBorrowerName.Text = "";
            }
        }

        private void ClearReturning()
        {
            txtTotalLoan.Text = "";
            txtBorrowerName.Text = "";
            toolsBindingSource.DataSource = null;
            dgGridLoans.ItemsSource = toolsBindingSource;
            dgGridLoans.RefreshData();
        }

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to go back to main menu?","Inventory System",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
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

        private void BtnReturnGood_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGridLoans.SelectedItem as LoanedTool;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.LoanedTools.FirstOrDefault(br => br.LoanToolID == selectedItem.LoanToolID);
                    if (tool != null)
                    {
                        tool.Status = "Returned";
                        tool.Condition = "GOOD";
                        tool.DateReturned = DateTime.Now;
                        tool.ReturnLoginName = UserSession.UserName;
                        context.SaveChanges();

                        using (var inventoryContext = new DatabaseContext())
                        {
                            var inventory = inventoryContext.Tools.FirstOrDefault(br => br.ItemCode == tool.ItemCode);
                            if (inventory != null)
                            {
                                inventory.Status = "In-Stock";
                                inventory.Condition = "GOOD";
                                inventory.LastUpdate = DateTime.Now;
                                inventoryContext.SaveChanges();
                            }
                        }

                        using (var loanContext = new DatabaseContext())
                        {
                            var loanTools = loanContext.LoanedTools.Where(br => br.LoanID == tool.LoanID && br.Status == "Active");
                            if (loanTools.Count() <=0)
                            {
                                using (var ReturnLoanContext = new DatabaseContext())
                                {
                                    var motherLoan = ReturnLoanContext.Loans.FirstOrDefault(br => br.LoanID == tool.LoanID);
                                    if (motherLoan != null)
                                    {
                                        motherLoan.Status = "Returned";
                                        motherLoan.ReturnDate = DateTime.Now;
                                        motherLoan.LoginName = UserSession.UserName;
                                        ReturnLoanContext.SaveChanges();
                                        MessageBox.Show("Full loan returned", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        } 
                        LoadLoans();
                        GetData();
                    }
                }
            }
        }

        private void BtnReturnNoGood_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGridLoans.SelectedItem as LoanedTool;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.LoanedTools.FirstOrDefault(br => br.LoanToolID == selectedItem.LoanToolID);
                    if (tool != null)
                    {
                        tool.Status = "Returned";
                        tool.Condition = "NOGOOD";
                        tool.DateReturned = DateTime.Now;
                        tool.ReturnLoginName = UserSession.UserName;
                        context.SaveChanges();

                        using (var inventoryContext = new DatabaseContext())
                        {
                            var inventory = inventoryContext.Tools.FirstOrDefault(br => br.ItemCode == tool.ItemCode);
                            if (inventory != null)
                            {
                                inventory.Status = "In-Stock";
                                inventory.Condition = "NOGOOD";
                                inventory.LastUpdate = DateTime.Now;
                                inventoryContext.SaveChanges();
                            }
                        }


                        using (var loanContext = new DatabaseContext())
                        {
                            var loanTools = loanContext.LoanedTools.Where(br => br.LoanID == tool.LoanID && br.Status == "Active");
                            if (loanTools.Count() <= 0)
                            {
                                using (var ReturnLoanContext = new DatabaseContext())
                                {
                                    var motherLoan = ReturnLoanContext.Loans.FirstOrDefault(br => br.LoanID == tool.LoanID);
                                    if (motherLoan != null)
                                    {
                                        motherLoan.Status = "Returned";
                                        motherLoan.ReturnDate = DateTime.Now;
                                        motherLoan.LoginName = UserSession.UserName;
                                        ReturnLoanContext.SaveChanges();
                                        MessageBox.Show("Full loan returned", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        }
                        LoadLoans();
                        GetData();
                    }
                }
            }
        }

        private void BtnReturnMissing_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGridLoans.SelectedItem as LoanedTool;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.LoanedTools.FirstOrDefault(br => br.LoanToolID == selectedItem.LoanToolID);
                    if (tool != null)
                    {
                        tool.Status = "Active";
                        tool.Condition = "MISSING";
                        tool.DateReturned = DateTime.Now;
                        tool.ReturnLoginName = UserSession.UserName;
                        context.SaveChanges();

                        using (var inventoryContext = new DatabaseContext())
                        {
                            var inventory = inventoryContext.Tools.FirstOrDefault(br => br.ItemCode == tool.ItemCode);
                            if (inventory != null)
                            {
                                inventory.Status = "In-Stock";
                                inventory.Condition = "LOST";
                                inventory.LastUpdate = DateTime.Now;
                                inventoryContext.SaveChanges();
                            }
                        }

                        using (var loanContext = new DatabaseContext())
                        {
                            var loanTools = loanContext.LoanedTools.Where(br => br.LoanID == tool.LoanID && br.Status == "Active");
                            if (loanTools.Count() <= 0)
                            {
                                using (var ReturnLoanContext = new DatabaseContext())
                                {
                                    var motherLoan = ReturnLoanContext.Loans.FirstOrDefault(br => br.LoanID == tool.LoanID);
                                    if (motherLoan != null)
                                    {
                                        motherLoan.Status = "Returned";
                                        motherLoan.ReturnDate = DateTime.Now;
                                        motherLoan.LoginName = UserSession.UserName;
                                        ReturnLoanContext.SaveChanges();
                                        MessageBox.Show("Full loan returned", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                                    }
                                }
                            }
                        }
                        LoadLoans();
                        GetData();
                    }
                }
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
        }

        private void GetData()
        {
            using (var context = new DatabaseContext())
            {
                var data = context.Consumables;
                if (data.Count() > 0)
                { 
                    #region getActiveLoans
                    using (var activeCtx = new DatabaseContext())
                    {
                        var activeloans = activeCtx.Loans.Where(br => br.Status == "Active");
                        if (activeloans.Count() > 0)
                        {
                            txtActiveLoans.Text = activeloans.Count().ToString();
                        }
                    }
                    #endregion 
                    #region getLoanedTools
                    using (var activeCtx = new DatabaseContext())
                    {
                        var activeloans = activeCtx.Tools.Where(br => br.Status == "Loaned");
                        if (activeloans.Count() > 0)
                        {
                            txtLoanedTool.Text = activeloans.Count().ToString();
                        }
                    }
                    #endregion  
                    #region getLostTools
                    using (var activeCtx = new DatabaseContext())
                    {
                        var activeloans = activeCtx.Tools.Where(br => br.Condition == "LOST");
                        if (activeloans.Count() > 0)
                        {
                            txtMissing.Text = activeloans.Count().ToString();
                        }
                    }
                    #endregion 
                }
            }
        }

        private void TileActiveLoans_Click(object sender, EventArgs e)
        {
            #region getActiveLoans
            using (var activeCtx = new DatabaseContext())
            {
                var activeloans = activeCtx.Loans.Where(br => br.Status == "Active");
                if (activeloans.Count() > 0)
                {
                    dgPopUpGrid.ItemsSource = activeloans.ToList();
                    dgTableView.Header = "All Active Loans";
                    pnlPopUp.Visibility = Visibility.Visible;
                }
            }
            #endregion
        }

        private void TileLoanedTools_Click(object sender, EventArgs e)
        {
            #region getLoanedTools
            using (var activeCtx = new DatabaseContext())
            {
                var activeloans = activeCtx.Tools.Where(br => br.Status == "Loaned");
                if (activeloans.Count() > 0)
                {
                    dgPopUpGrid.ItemsSource = activeloans.ToList();
                    dgTableView.Header = "All Loaned Tools";
                    pnlPopUp.Visibility = Visibility.Visible;
                }
            }
            #endregion
        }

        private void TileMissing_Click(object sender, EventArgs e)
        {
            #region getLostTools
            using (var activeCtx = new DatabaseContext())
            {
                var activeloans = activeCtx.Tools.Where(br => br.Condition == "LOST");
                if (activeloans.Count() > 0)
                {
                    dgPopUpGrid.ItemsSource = activeloans.ToList();
                    dgTableView.Header = "All Missing Tools";
                    pnlPopUp.Visibility = Visibility.Visible;
                }
            }
            #endregion
        }

        private void BtnClosePopUp_Click(object sender, RoutedEventArgs e)
        {
            pnlPopUp.Visibility = Visibility.Collapsed;

        }
    }

}
