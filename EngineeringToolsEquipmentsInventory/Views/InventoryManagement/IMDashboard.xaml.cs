using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Windows;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for IMDashboard.xaml
    /// </summary>
    public partial class IMDashboard : UserControl
    {

        public IMDashboard()
        {
            InitializeComponent();
 
        }
      
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            GetData();
        } 
        private void BtnGood_Click(object sender, EventArgs e)
        {


        }

        private void GetData()
        {
            using (var context = new DatabaseContext())
            {
                var data = context.Consumables;
                if (data.Count() > 0)
                {
                    #region getNoStock
                    int cnt = 0;
                    foreach (var item in data)
                    {
                        if (item.MaintainingQuantity <= 0)
                        {
                            cnt++;
                        }
                    }
                    txtNoStock.Text = cnt.ToString();
                    #endregion
                    #region getCritical
                    cnt = 0;
                    foreach (var item in data)
                    {
                        if (item.MaintainingQuantity > item.RemainingQuantity)
                        {
                            cnt++;
                        }
                        else if ((item.RemainingQuantity - item.MaintainingQuantity) <= 30)
                        {
                            cnt++;
                        }
                    }
                    txtCritical.Text = cnt.ToString();
                    #endregion
                    #region getReorder
                    txtReorder.Text = (int.Parse(txtNoStock.Text) + int.Parse(txtCritical.Text)).ToString();
                    #endregion 
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
                    #region getLoanedTools
                    using (var activeCtx = new DatabaseContext())
                    {
                        var activeloans = activeCtx.Tools.Where(br => br.Status == "In-Stock");
                        if (activeloans.Count() > 0)
                        {
                            txtAvailableTool.Text = activeloans.Count().ToString();
                        }
                    }
                    #endregion 
                    #region getGoodTools
                    using (var activeCtx = new DatabaseContext())
                    {
                        var activeloans = activeCtx.Tools.Where(br => br.Condition == "GOOD");
                        if (activeloans.Count() > 0)
                        {
                            txtGood.Text = activeloans.Count().ToString();
                        }
                    }
                    #endregion
                    #region getNoGoodTools
                    using (var activeCtx = new DatabaseContext())
                    {
                        var activeloans = activeCtx.Tools.Where(br => br.Condition == "NO GOOD");
                        if (activeloans.Count() > 0)
                        {
                            txtNoGood.Text = activeloans.Count().ToString();
                        }
                    }
                    #endregion 
                    #region getLostTools
                    using (var activeCtx = new DatabaseContext())
                    {
                        var activeloans = activeCtx.Tools.Where(br => br.Condition == "LOST");
                        if (activeloans.Count() > 0)
                        {
                            txtLoast.Text = activeloans.Count().ToString();
                        }
                    }
                    #endregion

                    #region getRecentLoans
                    using (var loanCtx = new DatabaseContext())
                    {
                        var recent = loanCtx.Loans.Where(br => br.LoanDate >= DateTime.Today);
                        if (recent != null)
                        {
                            dgGridLoans.ItemsSource = recent.ToList();
                        }
                    }
                    #endregion

                    #region getRecentTransactions
                    using (var loanCtx = new DatabaseContext())
                    {
                        var recent = loanCtx.Transactions.Where(br => br.TransactionDate >= DateTime.Today);
                        if (recent != null)
                        {
                            dgGridTransaction.ItemsSource = recent.ToList();
                        }
                    }
                    #endregion
                }
            }

            
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

        private void LoadLoans()
        {
            using (var context = new DatabaseContext())
            {
                var loan = context.Loans.Where(br => br.UserID == ReturningSession.borrowerID && br.Status == "Active");
                dgGridLoans.ItemsSource = loan.ToList();
            }
        }
    }
}
