using EngineeringToolsEquipmentsInventory.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EngineeringToolsEquipmentsInventory.Windows
{
    /// <summary>
    /// Interaction logic for IDScanWindow.xaml
    /// </summary>
    public partial class IDScanWindow : Window
    {
        public IDScanWindow()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(500);
            timer.Tick += IDRead;
            timer.Start();
        }

        private void IDRead(object sender, EventArgs e)
        {
            if (imageSignal.Visibility == Visibility.Visible)
            {
                imageSignal.Visibility = Visibility.Hidden;
            }
            else
            {
                imageSignal.Visibility = Visibility.Visible;

            }
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            string charTemp = Regex.Replace(e.Key.ToString(), "[^0-9.]", "").Replace(".", "");
            Models.UserSession.idScanTemp = Models.UserSession.idScanTemp + charTemp;
            if (e.Key == Key.Enter)
            {
                if (ReturningSession.returnScanID == false && HistorySession.historyIDScan == false && ConsumableSession.TransactionScan == false && SparePartSession.TransactionSparePartScan == false && JigsSession.JgsTransactionScan ==false)
                {
                    if (Models.UserSession.idScanTemp != "")
                    {
                        using (var context = new DatabaseContext())
                        {
                            var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp);
                            if (user != null)
                            {
                                TransactionSession.newTransaction.UserID = user.UserID;
                                TransactionSession.newTransaction.UserName = user.Name;

                                foreach (var item in TransactionSession.barrowTools)
                                {
                                    using (var toolContext = new DatabaseContext())
                                    {
                                        var SearchTool = toolContext.Tools.FirstOrDefault(br => br.ItemCode == item.ItemCode);
                                        if (SearchTool != null)
                                        {
                                            SearchTool.Status = "Loaned";
                                            toolContext.SaveChanges();
                                        }

                                        LoanedTool loanedTool = new LoanedTool();
                                        loanedTool.LoanToolID = "TLOAN-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6); ;
                                        loanedTool.LoanID = TransactionSession.newTransaction.LoanID;
                                        loanedTool.ItemCode = item.ItemCode;
                                        loanedTool.PECode = item.PECode;
                                        loanedTool.DateBorrowed = DateTime.Now;
                                        loanedTool.DateReturned = DateTime.MinValue;
                                        loanedTool.Item = item.Item;
                                        loanedTool.Status = "Active";
                                        loanedTool.Condition = "GOOD";
                                        loanedTool.LoginName = UserSession.UserName;
                                        loanedTool.ReturnLoginName = "";
                                        loanedTool.BorrowerName = user.Name;
                                        toolContext.LoanedTools.Add(loanedTool);
                                        toolContext.SaveChanges();
                                    }

                                }

                                using (var LoanContext = new DatabaseContext())
                                {
                                    LoanContext.Loans.Add(TransactionSession.newTransaction);
                                    LoanContext.SaveChanges();
                                }
                                //MessageBox.Show("Loan Added!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                                Models.UserSession.idScanTemp = "";
                                DialogResult = true;

                            }
                            else
                            {
                                Models.UserSession.idScanTemp = "";
                                MessageBox.Show("Borrower is not registered! Do you want to register this borrower?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Error);
                                Models.UserSession.idScanTemp = "";
                                DialogResult = false;
                            }
                        }
                    }
                }
                else if (ReturningSession.returnScanID == true && HistorySession.historyIDScan == false && ConsumableSession.TransactionScan == false && JigsSession.JgsTransactionScan == false)
                {
                    using (var context = new DatabaseContext())
                    {
                        var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp);
                        if (user != null)
                        {
                            ReturningSession.borrowerID = Models.UserSession.idScanTemp;
                            var loan = context.Loans.Where(br => br.UserID == ReturningSession.borrowerID && br.Status == "Active");
                            if (loan.Count() <= 0)
                            {
                                MessageBox.Show("This user has no existing loans", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                                ReturningSession.borrowerID = "";
                                Models.UserSession.idScanTemp = "";
                                ReturningSession.returnScanID = false;
                                DialogResult = true;
                            }
                            else
                            {
                                //MessageBox.Show("This user has existing loans", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                                Models.UserSession.idScanTemp = "";
                                ReturningSession.returnScanID = false;
                                DialogResult = true;

                            }
                        }
                        else
                        {
                            Models.UserSession.idScanTemp = "";
                            ReturningSession.returnScanID = false;
                            MessageBox.Show("Borrower is not registered! Do you want to add as new borrower?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Error);
                        }
                    }
                }
                else if (HistorySession.historyIDScan == true && ConsumableSession.TransactionScan == false && ReturningSession.returnScanID == false && SparePartSession.TransactionSparePartScan == false && JigsSession.JgsTransactionScan == false)
                {
                    using (var context = new DatabaseContext())
                    {
                        var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp);
                        if (user != null)
                        {
                            HistorySession.queryBorrowerID = Models.UserSession.idScanTemp;
                            Models.UserSession.idScanTemp = "";
                            HistorySession.historyIDScan = false;
                            DialogResult = true;
                        }
                        else
                        {
                            HistorySession.queryBorrowerID = "";
                            Models.UserSession.idScanTemp = "";
                            HistorySession.historyIDScan = false;
                            MessageBox.Show("Borrower is not registered! Do you want to add as new borrower?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Error);
                        }
                    }
                }
                else if (ReturningSession.returnScanID == false && HistorySession.historyIDScan == false && ConsumableSession.TransactionScan == true && SparePartSession.TransactionSparePartScan == false && JigsSession.JgsTransactionScan == false)
                {
                    using (var context = new DatabaseContext())
                    {
                        var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp);
                        if (user != null)
                        {
                            ConsumableSession.NewTransaction.UserID = user.UserID;
                            ConsumableSession.NewTransaction.UserName = user.Name;

                            foreach (var item in ConsumableSession.TransItemList)
                            {
                                item.UserName = user.Name;
                            }

                            ConsumableSession.TransactionScan = false;
                            Models.UserSession.idScanTemp = "";
                            DialogResult = true;
                        }
                        else
                        {
                            ConsumableSession.TransactionScan = false;
                            Models.UserSession.idScanTemp = "";
                            
                            if (MessageBox.Show("Borrower is not registered! Do you want to add as new borrower?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                            {
                                DialogResult = false;
                                BarrowerLookupWindow barrowerLookupWindow = new BarrowerLookupWindow(); 
                            }
                        }
                    }
                }

                else if (ReturningSession.returnScanID == false && HistorySession.historyIDScan == false && ConsumableSession.TransactionScan == false && SparePartSession.TransactionSparePartScan == true && JigsSession.JgsTransactionScan == false)
                {
                    using (var context = new DatabaseContext())
                    {
                        var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp);
                        if (user != null)
                        {
                            SparePartSession.NewTransactionSparePart.UserID = user.UserID;
                            SparePartSession.NewTransactionSparePart.UserName = user.Name;

                            foreach (var item in SparePartSession.TransSparePartItemList)
                            {
                                item.UserName = user.Name;
                                item.UserID = user.UserID;
                            }

                            SparePartSession.TransactionSparePartScan = false;
                            Models.UserSession.idScanTemp = "";
                            DialogResult = true;
                        }
                        else
                        {
                            SparePartSession.TransactionSparePartScan = false;
                            Models.UserSession.idScanTemp = "";

                            if (MessageBox.Show("Borrower is not registered! Do you want to add as new borrower?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                            {
                                DialogResult = false;
                                BarrowerLookupWindow barrowerLookupWindow = new BarrowerLookupWindow();
                            }
                        }
                    }
                }
                else if (ReturningSession.returnScanID == false && HistorySession.historyIDScan == false && ConsumableSession.TransactionScan == false && SparePartSession.TransactionSparePartScan == false && JigsSession.JgsTransactionScan == true)
                {
                    using (var context = new DatabaseContext())
                    {
                        var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp);
                        if (user != null)
                        {
                            JigsSession.NewJigTransaction.UserID = user.UserID;
                            JigsSession.NewJigTransaction.UserName = user.Name;

                            foreach (var item in JigsSession.JigTransItemList)
                            {
                                item.UserName = user.Name;
                                item.UserID = user.UserID;
                            }

                            JigsSession.JgsTransactionScan = false;
                            Models.UserSession.idScanTemp = "";
                            DialogResult = true;
                        }
                        else
                        {
                            JigsSession.JgsTransactionScan = false;
                            Models.UserSession.idScanTemp = "";

                            if (MessageBox.Show("Borrower is not registered! Do you want to add as new borrower?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                            {
                                DialogResult = false;
                                BarrowerLookupWindow barrowerLookupWindow = new BarrowerLookupWindow();
                            }
                        }
                    }
                }
            }


            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }
        }
    }
}
