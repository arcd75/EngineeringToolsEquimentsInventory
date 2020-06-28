using DevExpress.Xpf.Core;
using DevExpress.Xpf.Printing;
using DevExpress.XtraReports.UI;
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
    /// Interaction logic for DeliveryHistoryView.xaml
    /// </summary>
    public partial class DeliveryHistoryView : UserControl
    {
        public DeliveryHistoryView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //using (var context = new DatabaseContext())
            //{
            //    LoanedItemsReport loanedItemsReport = new LoanedItemsReport();
            //    var loanItems = context.LoanedTools.Where(br => br.LoanID == "LOAN-20200117PF7YEX");
            //    Dispatcher.Invoke(() =>
            //    {
            //        loanedItemsReport.DataSource = loanItems.ToList();

            //    });
            //    docuViewer.DocumentSource = loanedItemsReport; 
            //}
            dgDelivery.ShowLoadingPanel = true;
            var task = Task.Run(() =>
            {
                using (var context = new DatabaseContext())
                {
                    var loans = context.Deliveries;
                    if (loans.Count() <= 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DXMessageBox.Show("No deliveries available", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            dgDelivery.ItemsSource = loans.ToList();
                        });
                    }
                }
            });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    dgDelivery.ShowLoadingPanel = false;
                });
                task.Dispose();
            });

        }

        private void BtnViewReport_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgDelivery.SelectedItem as Delivery;
            var task = Task.Run(() =>
            {
                if (selectedItem != null)
                {
                    DeliveryReport loanedItemsReport = new DeliveryReport();
                    using (var context = new DatabaseContext())
                    {
                        var data = context.Deliveries.FirstOrDefault(br => br.DeliveryID == selectedItem.DeliveryID);
                        if (data != null)
                        {
                            loanedItemsReport.FindControl("xrDeliveryID", false).Text = data.DeliveryID;
                            loanedItemsReport.FindControl("xrDeliveryDate", false).Text = data.DeliveryDate.ToString();
                            loanedItemsReport.FindControl("xrDateEncoded", false).Text = data.Date.ToString();
                            loanedItemsReport.FindControl("xrEncodedBy", false).Text = "";
                            loanedItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;
                            loanedItemsReport.FindControl("xrConsumableTotal", false).Text = data.ConsumableTotal.ToString();
                            loanedItemsReport.FindControl("xrToolTotal", false).Text = data.ToolTotal.ToString();
                            loanedItemsReport.FindControl("xrSpareTotal", false).Text = data.SpareTotal.ToString();
                            loanedItemsReport.FindControl("xrAssetsCount", false).Text = data.AssetTotal.ToString();
                            loanedItemsReport.FindControl("xrBreakDownTotal", false).Text = data.TotalItem.ToString();
                            var band = loanedItemsReport.Bands;

                            var loanItems = context.DeliveriesItem.Where(br => br.DeliveryID == selectedItem.DeliveryID);
                            //loanedItemsReport.Bands[3].Report.DataSource = loanItems.ToList();
                            var toolItem = context.DeliveriesToolItem.Where(br => br.DeliveryID == selectedItem.DeliveryID);
                            //loanedItemsReport.Bands[4].Report.DataSource = toolItem.ToList();
                            var spareItem = context.DeliveryItemSpareParts.Where(br => br.DeliveryID == selectedItem.DeliveryID);
                            var assetItem = context.DeliveryAssets.Where(br => br.DeliveryID == selectedItem.DeliveryID);
                            //loanedItemsReport.Bands[5].Report.DataSource = spareItem.ToList();
                            //loanedItemsReport.DataSource = spareItem.ToList();
                            var consumable = loanedItemsReport.Bands[3] as DetailReportBand;
                            var tool = loanedItemsReport.Bands[4] as DetailReportBand;
                            var spare = loanedItemsReport.Bands[5] as DetailReportBand;
                            var asset = loanedItemsReport.Bands[8] as DetailReportBand;
                            spare.DataSource = spareItem.ToList();
                            consumable.DataSource = loanItems.ToList();
                            tool.DataSource = toolItem.ToList();
                            asset.DataSource = assetItem.ToList();

                            loanedItemsReport.Bands.Add(spare);
                            loanedItemsReport.Bands.Add(tool);
                            loanedItemsReport.Bands.Add(consumable);
                            loanedItemsReport.Bands.Add(asset);
                            Dispatcher.Invoke(() =>
                            {
                                //PrintHelper.ShowPrintPreviewDialog(null, loanedItemsReport);
                                //docuViewer.DocumentSource = loanedItemsReport;
                                docuViewer.OpenDocument(loanedItemsReport);

                                pnlDeliveryList.Visibility = Visibility.Collapsed;
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

                });
            });
        }
        public void CycleThoughBands(DetailReportBand db)
        {
            for (int i = 0; i < db.Bands.Count; i++)
                if (db.Bands[i] is DetailReportBand)
                    CycleThoughBands(db.Bands[i] as DetailReportBand);
                else
                {
                    //Perform required Action  
                }
        }


        private void BtnPrintReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnOpenDelivery_Click(object sender, RoutedEventArgs e)
        {
            pnlDeliveryList.Visibility = Visibility.Visible;
            dgDelivery.ShowLoadingPanel = true;
            var task = Task.Run(() =>
            {
                using (var context = new DatabaseContext())
                {
                    var loans = context.Deliveries;
                    if (loans.Count() <= 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            DXMessageBox.Show("No deliveries available", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                    else
                    {
                        Dispatcher.Invoke(() =>
                        {
                            dgDelivery.ItemsSource = loans.ToList();
                        });
                    }
                }
            });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    dgDelivery.ShowLoadingPanel = false;
                });
                task.Dispose();
            });
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            pnlDeliveryList.Visibility = Visibility.Collapsed;
        }
    }
}
