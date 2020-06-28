using DevExpress.Xpf.Printing;
using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Reports;
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
    /// Interaction logic for TransactionHistoryView.xaml
    /// </summary>
    /// 
    public partial class TransactionHistoryView : UserControl
    {
        public TransactionHistoryView()
        {
            InitializeComponent();
        }

        System.Windows.Forms.BindingSource transactionBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource spareBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource assetBinding = new System.Windows.Forms.BindingSource();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgTransaction.ItemsSource = transactionBinding;
            dgSpareTransaction.ItemsSource = spareBinding;
            dgAssetTransaction.ItemsSource = assetBinding;
            LoadAllTransactions();
        }

        private void LoadAllTransactions()
        {
            using (var context = new DatabaseContext())
            {
                var transactions = context.Transactions;
                if (transactions.Count() > 0)
                {
                    transactionBinding.DataSource = transactions.ToList();
                    
                    dgTransaction.RefreshData();
                }
            }

            using (var context = new DatabaseContext())
            {
                var transactions = context.SparePartTransactions;
                if (transactions.Count() > 0)
                {
                    spareBinding.DataSource = transactions.ToList();
                    
                    dgSpareTransaction.RefreshData();
                }
            }

            using (var context = new DatabaseContext())
            {
                var transactions = context.AssetAndEquipmentTransactions;
                if (transactions.Count() > 0)
                {
                    assetBinding.DataSource = transactions.ToList();
                    
                    dgAssetTransaction.RefreshData();
                }
            }

        }

        private void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.busyIndicator.Visibility = Visibility.Visible;
            var selectedItem = dgTransaction.SelectedItem as Transaction;
            var task = Task.Run(() =>
            {
                if (selectedItem != null)
                {
                    AssetAndEquipmentTransactionReport transactionItemsReport = new AssetAndEquipmentTransactionReport();
                    using (var context = new DatabaseContext())
                    {
                        var data = context.Transactions.FirstOrDefault(br => br.TransactionID == selectedItem.TransactionID);
                        if (data != null)
                        {
                            transactionItemsReport.FindControl("xrTransactionID", false).Text = data.TransactionID;
                            transactionItemsReport.FindControl("xrUserName", false).Text = data.UserName;
                            transactionItemsReport.FindControl("xrTotalItems", false).Text = data.NumberofItem.ToString();
                            transactionItemsReport.FindControl("xrTransactionDate", false).Text = data.TransactionDate.ToString();
                            transactionItemsReport.FindControl("xrTotalQuantity", false).Text = data.TotalQuantity.ToString(); 
                            transactionItemsReport.FindControl("xrPIC", false).Text = data.LoginName;
                            transactionItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;
                            //transactionItemsReport.FindControl("xrLineCode", false).Text = data.LineCode;
                            transactionItemsReport.FindControl("xrLineName", false).Text = data.LineName;
                            var transactionItems = context.TransactionItems.Where(br => br.TransactionID == selectedItem.TransactionID);
                            Dispatcher.Invoke(() =>
                            {
                                transactionItemsReport.DataSource = transactionItems.ToList();
                                PrintHelper.ShowPrintPreviewDialog(null, transactionItemsReport);
                            });
                        }
                        else
                        {
                            MessageBox.Show("This transaction is not existing!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BtnViewSpareDetails_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.busyIndicator.Visibility = Visibility.Visible;
            var selectedItem = dgSpareTransaction.SelectedItem as SparePartTransaction;
            var task = Task.Run(() =>
            {
                if (selectedItem != null)
                {
                    SparePartTransactionReport transactionItemsReport = new SparePartTransactionReport();
                    using (var context = new DatabaseContext())
                    {
                        var data = context.SparePartTransactions.FirstOrDefault(br => br.TransactionID == selectedItem.TransactionID);
                        if (data != null)
                        {
                            transactionItemsReport.FindControl("xrTransactionID", false).Text = data.TransactionID;
                            transactionItemsReport.FindControl("xrUserName", false).Text = data.UserName;
                            transactionItemsReport.FindControl("xrTotalItems", false).Text = data.TotalItems.ToString();
                            transactionItemsReport.FindControl("xrTransactionDate", false).Text = data.Date.ToString();
                            transactionItemsReport.FindControl("xrTotalQuantity", false).Text = data.TotalQuantity.ToString();
                            transactionItemsReport.FindControl("xrPIC", false).Text = data.PIC;
                            transactionItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;
                            //transactionItemsReport.FindControl("xrLineCode", false).Text = data.LineCode;
                            transactionItemsReport.FindControl("xrLineName", false).Text = data.LineName;
                            transactionItemsReport.FindControl("xrTotalUnitCost", false).Text = data.TotalCost.ToString();

                            var transactionItems = context.SparePartTransactionItems.Where(br => br.TransactionID == selectedItem.TransactionID);
                            Dispatcher.Invoke(() =>
                            {
                                transactionItemsReport.DataSource = transactionItems.ToList();
                                PrintHelper.ShowPrintPreviewDialog(null, transactionItemsReport);
                            });
                        }
                        else
                        {
                            MessageBox.Show("This transaction is not existing!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
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

        private void BtnViewAssetTransaction_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.busyIndicator.Visibility = Visibility.Visible;
            var selectedItem = dgAssetTransaction.SelectedItem as Models.AssetAndEquipmentTransaction;
            var task = Task.Run(() =>
            {
                if (selectedItem != null)
                {
                    AssetTransactionReport transactionItemsReport = new AssetTransactionReport();
                    using (var context = new DatabaseContext())
                    {
                        var data = context.AssetAndEquipmentTransactions.FirstOrDefault(br => br.TransactionID == selectedItem.TransactionID);
                        if (data != null)
                        {
                            transactionItemsReport.FindControl("xrTransactionID", false).Text = data.TransactionID;
                            transactionItemsReport.FindControl("xrUserName", false).Text = data.UserName;
                            transactionItemsReport.FindControl("xrTotalItems", false).Text = data.TotalItem.ToString();
                            transactionItemsReport.FindControl("xrTransactionDate", false).Text = data.Date.ToString();
                            transactionItemsReport.FindControl("xrTotalQuantity", false).Text = data.TotalCost.ToString();
                            transactionItemsReport.FindControl("xrPIC", false).Text = data.PIC;
                            transactionItemsReport.FindControl("xrLineName", false).Text = data.LineName;
                            transactionItemsReport.FindControl("xrDateWithdrawn", false).Text = data.DateWithdrawn.ToString();
                            transactionItemsReport.FindControl("xrTransType", false).Text = data.Type;
                            transactionItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;
                            transactionItemsReport.FindControl("xrTotalUnitCost", false).Text = data.TotalCost.ToString();
                            var transactionItems = context.AssetTransactionItems.Where(br => br.TransactionID == selectedItem.TransactionID);
                            Dispatcher.Invoke(() =>
                            {
                                transactionItemsReport.DataSource = transactionItems.ToList();
                                PrintHelper.ShowPrintPreviewDialog(null, transactionItemsReport);
                            });
                        }
                        else
                        {
                            MessageBox.Show("This transaction is not existing!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
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
}
