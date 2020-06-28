using DevExpress.Xpf.Printing;
using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Reports;
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
    /// Interaction logic for HistoryView.xaml
    /// </summary>
    public partial class HistoryView : UserControl
    {
        public HistoryView()
        {
            InitializeComponent();
        }

        private void BtnLoanItems_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.busyIndicator.Visibility = Visibility.Visible;
            var selectedItem = dgGridLoans.SelectedItem as Loan;
            var task = Task.Run(() =>
            {
                if (selectedItem != null)
                {
                    LoanedItemsReport loanedItemsReport = new LoanedItemsReport();
                    using (var context = new DatabaseContext())
                    {
                        var data = context.Loans.FirstOrDefault(br => br.LoanID == selectedItem.LoanID);
                        if (data != null)
                        {
                            loanedItemsReport.FindControl("xrLoanID", false).Text = data.LoanID;
                            loanedItemsReport.FindControl("xrBorrowerName", false).Text = data.UserName;
                            loanedItemsReport.FindControl("xrQuantity", false).Text = data.LoanQuantity;
                            loanedItemsReport.FindControl("xrStatus", false).Text = data.Status;
                            loanedItemsReport.FindControl("xrRemarks", false).Text = data.Remarks;
                            loanedItemsReport.FindControl("xrLoanDate", false).Text = data.LoanDate.ToString();
                            loanedItemsReport.FindControl("xrReturnDate", false).Text = data.ReturnDate.ToString();
                            loanedItemsReport.FindControl("xrPIC", false).Text = data.LoginName;
                            loanedItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;
                            loanedItemsReport.FindControl("xrTotalCost", false).Text = data.TotalCost.ToString();

                            var loanItems = context.LoanedTools.Where(br => br.LoanID == selectedItem.LoanID);
                            Dispatcher.Invoke(() =>
                            {
                                loanedItemsReport.DataSource = loanItems.ToList();
                                PrintHelper.ShowPrintPreviewDialog(null, loanedItemsReport);
                            });
                        }
                        else
                        {
                            MessageBox.Show("This loan is not existing!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    mainWindow.busyIndicator.Visibility = Visibility.Hidden;
                });
            });
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            if (HistorySession.queryBorrowerID == "")
            {
                using (var context = new DatabaseContext())
                {
                    var data = context.Loans;
                    dgGridLoans.ItemsSource = data.ToList();
                }
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    var data = context.Loans.Where(br => br.UserID == HistorySession.queryBorrowerID);
                    dgGridLoans.ItemsSource = data.ToList();
                }
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            HistorySession.historyIDScan = false;
            HistorySession.queryBorrowerID = "";
            LoadData();
        }

        private void BtnScanID_Click(object sender, RoutedEventArgs e)
        {
            HistorySession.historyIDScan = true;
            IDScanWindow iDScanWindow = new IDScanWindow();
            iDScanWindow.ShowDialog();

            if (iDScanWindow.DialogResult == true)
            {
                LoadData();
                HistorySession.historyItemLookUp = false;

            }
        }

        private void BtnExport_Click(object sender, RoutedEventArgs e)
        {
            if (dgGridByItem.Visibility == Visibility.Collapsed)
            {
                try
                {
                    System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                    fbd.Description = "Save to";

                    if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string FileName = @"//LOAN" + DateTime.Now.ToString("ddMMyy") + RandomString(6) + ".xlsx";
                        string sSelectedPath = fbd.SelectedPath + FileName;
                        var task_Log = new Task(() =>
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                dgGridLoans.View.ExportToXlsx(sSelectedPath);
                            });
                        });
                        task_Log.Start();
                        MessageBox.Show("Record Exported", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                } 
            }
            else
            {
                try
                {
                    System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
                    fbd.Description = "Save to";

                    if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        string FileName = @"//ITEM" + DateTime.Now.ToString("ddMMyy") + RandomString(6) + ".xlsx";
                        string sSelectedPath = fbd.SelectedPath + FileName;
                        var task_Log = new Task(() =>
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                dgGridByItem.View.ExportToXlsx(sSelectedPath);
                            });
                        });
                        task_Log.Start();
                        MessageBox.Show("Record Exported", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                    }


                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void BtnSearchItem_Click(object sender, RoutedEventArgs e)
        {
            HistorySession.historyItemLookUp = true;
            ToolLookupWindow toolLookupWindow = new ToolLookupWindow();
            toolLookupWindow.ShowDialog();
            if (toolLookupWindow.DialogResult == true)
            {
                using (var context = new DatabaseContext())
                {
                    var data = context.LoanedTools.Where(br => br.ItemCode == HistorySession.queryTool);
                    if (data.Count() > 0)
                    {
                        dgGridByItem.ItemsSource = data.ToList();
                        HistorySession.historyItemLookUp = false;
                        dgGridLoans.Visibility = Visibility.Collapsed;
                        dgGridByItem.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        MessageBox.Show("No record found","Inventory System",MessageBoxButton.OK,MessageBoxImage.Information);
                        dgGridLoans.Visibility = Visibility.Visible;
                        dgGridByItem.Visibility = Visibility.Collapsed;
                    }
                }
            }
        }

        private void BtnViewLoan_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGridByItem.SelectedItem as LoanedTool;

            if (selectedItem != null)
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.busyIndicator.Visibility = Visibility.Visible; 
                var task = Task.Run(() =>
                {
                    if (selectedItem != null)
                    {
                        LoanedItemsReport loanedItemsReport = new LoanedItemsReport();
                        using (var context = new DatabaseContext())
                        {
                            var data = context.Loans.FirstOrDefault(br => br.LoanID == selectedItem.LoanID);
                            if (data != null)
                            {
                                loanedItemsReport.FindControl("xrLoanID", false).Text = data.LoanID;
                                loanedItemsReport.FindControl("xrBorrowerName", false).Text = data.UserName;
                                loanedItemsReport.FindControl("xrQuantity", false).Text = data.LoanQuantity;
                                loanedItemsReport.FindControl("xrStatus", false).Text = data.Status;
                                loanedItemsReport.FindControl("xrRemarks", false).Text = data.Remarks;
                                loanedItemsReport.FindControl("xrLoanDate", false).Text = data.LoanDate.ToString();
                                loanedItemsReport.FindControl("xrReturnDate", false).Text = data.ReturnDate.ToString();
                                loanedItemsReport.FindControl("xrPIC", false).Text = data.LoginName;
                                loanedItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;

                                var loanItems = context.LoanedTools.Where(br => br.LoanID == selectedItem.LoanID);
                                Dispatcher.Invoke(() =>
                                {
                                    loanedItemsReport.DataSource = loanItems.ToList();
                                    PrintHelper.ShowPrintPreviewDialog(null, loanedItemsReport);
                                });
                            }
                            else
                            {
                                MessageBox.Show("This loan is not existing!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                });
                task.ContinueWith((t) =>
                {
                    Dispatcher.Invoke(() =>
                    {
                        mainWindow.busyIndicator.Visibility = Visibility.Hidden;
                    });
                });
            }
        }

        private void BtnItemClear_Click(object sender, RoutedEventArgs e)
        {
            dgGridLoans.Visibility = Visibility.Visible;
            dgGridByItem.Visibility = Visibility.Collapsed;
            LoadData();
        }

        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        private void BtnMenu_Click(object sender, RoutedEventArgs e)
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

        private void BtnItemSearch_Click(object sender, RoutedEventArgs e)
        {
            ToolSearchWindow toolSearchWindow = new ToolSearchWindow();
            toolSearchWindow.ShowDialog();

            if (toolSearchWindow.DialogResult == true)
            {
                using (var context = new DatabaseContext())
                {
                    var historyData = context.LoanedTools.Where(br => br.Item.Contains(HistorySession.toolFind) || br.Item.Contains(HistorySession.toolFind) || br.Condition.Contains(HistorySession.toolFind) || br.LoanID.Contains(HistorySession.toolFind) || br.PECode.Contains(HistorySession.toolFind) || br.Status.Contains(HistorySession.toolFind));
                    if (historyData != null)
                    {
                        dgGridByItem.ItemsSource = historyData.ToList();
                        dgGridByItem.Visibility = Visibility.Visible;
                        dgGridLoans.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        MessageBox.Show("No record found","Inventory System",MessageBoxButton.OK,MessageBoxImage.Information);
                    }
                }
            }
        }

        private void BtnShowAll_Click(object sender, RoutedEventArgs e)
        {
            using (var context = new DatabaseContext())
            {
                var historyData = context.LoanedTools;
                if (historyData != null)
                {
                    dgGridByItem.ItemsSource = historyData.ToList();
                    dgGridByItem.Visibility = Visibility.Visible;
                    dgGridLoans.Visibility = Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("No record found", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }
    }
}
