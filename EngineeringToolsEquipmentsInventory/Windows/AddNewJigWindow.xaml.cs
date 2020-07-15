using DevExpress.Xpf.Printing;
using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Reports;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace EngineeringToolsEquipmentsInventory.Windows
{
    /// <summary>
    /// Interaction logic for AddNewJigWindow.xaml
    /// </summary>
    public partial class AddNewJigWindow : Window
    {
        DispatcherTimer timer = new DispatcherTimer() { Interval = new TimeSpan(0, 0, 1) };

        public AddNewJigWindow()
        {
            InitializeComponent();
        }
        System.Windows.Forms.BindingSource josReceivingBindingSource = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource josOrderItems = new System.Windows.Forms.BindingSource();

        public void timer_Tick (object sender, EventArgs e)
        {
            if (MainWindow.IsUpdateDone)
            {
                timer.Stop();
                LoadDeliveries();
            }
            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            dgJOSDeliveries.ItemsSource = josReceivingBindingSource;
            dgOrderItem.ItemsSource = josOrderItems;
            LoadDeliveries();
            LoadPOData();
            */
            //LoadPurchaseOrder();
            //UpdateLocalDB();
            timer.Tick += new EventHandler(timer_Tick);
            timer.Start();
        }

        public void LoadDeliveries()
        {
            using (var context = new DatabaseContext())
            {
                var lastUpdate = context.JOSUpdates.ToList();
                var receivables = context.JOSDeliveries.ToList();
                var purchaseOrders = context.JigPurchaseOrders.ToList();
                josReceivingBindingSource.DataSource = receivables;
                josOrderItems.DataSource = purchaseOrders;
                dgJOSDeliveries.ItemsSource = josReceivingBindingSource;
                dgJOSOrders.ItemsSource = josOrderItems;
                dgJOSDeliveries.RefreshData();
                dgJOSOrders.RefreshData();
                if (lastUpdate.Count() > 0)
                {
                    txtLastUpdate.Text = "Last Updated: " + lastUpdate.FirstOrDefault().LastUpdate.ToString("MMMM d, yyyy h:mm:ss tt");
                    bsyPanel.Visibility = Visibility.Collapsed;
                }
            }
        }

        #region Old LoadPurchaseOrder()
        /*public void LoadPurchaseOrder()
        {
            bsyPanel.Visibility = Visibility.Visible;
            int cnt = 0;
            var OrderNumber = new List<string>();
            var task = Task.Run(() =>
            {
                using (var context = new iJosDatabaseContext())
                {
                    //var POs = context.PurchaseOrders.Select(br => br.PONo).Distinct();
                    OrderNumber = context.Database.SqlQuery<string>("SELECT DISTINCT PONo FROM PurchaseOrder").ToList();
                }
                foreach (var item in OrderNumber)
                {
                    cnt++;
                    Dispatcher.Invoke(() =>
                    {
                        waitIndicator.Content = "Transferring Data from iJOS " + cnt.ToString() + "/" + OrderNumber.Count().ToString();
                    });
                    Int64 itemcount = 0;
                    decimal totalPrice = 0;
                    using (var POItemContext = new iJosDatabaseContext())
                    {
                        //var POItems = POItemContext.PurchaseOrders.Where(br => br.PONo == item);
                        var POItems = POItemContext.Database.SqlQuery<PurchaseOrder>("SELECT * FROM PurchaseOrder WHERE PONo = @id", new SqlParameter("@id", item)).ToList();
                        foreach (var POItem in POItems)
                        {
                            itemcount = itemcount + POItem.Quantity;
                            totalPrice = totalPrice + POItem.TotalCost;
                        }
                        JigPurchaseOrder purchaseOrder = new JigPurchaseOrder();
                        purchaseOrder.PONo = item;
                        purchaseOrder.TranNo = POItems.FirstOrDefault().TranNo.ToString();
                        purchaseOrder.PODate = POItems.FirstOrDefault().PODate;
                        purchaseOrder.ReferenceNo = POItems.FirstOrDefault().ReferenceNo;
                        purchaseOrder.IssuerName = POItems.FirstOrDefault().IssuerName;
                        purchaseOrder.Quantity = int.Parse(itemcount.ToString());
                        purchaseOrder.TotalPrice = float.Parse(totalPrice.ToString());
                        using (var transferContext = new DatabaseContext())
                        {
                            var check = transferContext.JigPurchaseOrders.FirstOrDefault(br => br.PONo == item);
                            if (check != null)
                            {
                                goto JumpLine;
                            }
                            transferContext.JigPurchaseOrders.Add(purchaseOrder);
                            transferContext.SaveChanges();
                        }
                        JumpLine:;
                    }
                }
                using (var context = new DatabaseContext())
                {
                    var purchaseOrders = context.JigPurchaseOrders;
                    Dispatcher.Invoke(() =>
                    {
                        dgJOSOrders.ItemsSource = purchaseOrders.ToList();
                    });
                }
            });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    bsyPanel.Visibility = Visibility.Collapsed;
                });
            });
        }*/
        #endregion

        #region Old UpdateLocalDB()
        /*public void UpdateLocalDB()
        {
            bsyPanel.Visibility = Visibility.Visible;
            var task = Task.Run(() =>
           {
               List<String> DeliveryNos = new List<string>();
               using (var context = new iJosDatabaseContext())
               {
                   var receivables = context.Receivings.Select(br => br.PONo).Distinct();
                   DeliveryNos = receivables.ToList();
               }
               int cnt = 1;
               foreach (var item in DeliveryNos)
               {
                   using (var context = new iJosDatabaseContext())
                   {
                       var deliveryItems = context.Receivings.Where(br => br.PONo == item);
                       if (deliveryItems.Count() > 0)
                       {
                           using (var checContext = new DatabaseContext())
                           {
                               var itemNo = deliveryItems.FirstOrDefault().PONo;
                               //string tempID = "";
                               //while (true)
                               //{
                               //    using (var context2 = new DatabaseContext())
                               //    {
                               //        tempID = "TRANSIT" + DateTime.Now.ToString("yyyyMMdd") + RandomString(6);
                               //        var checkID = context2.JOSDeliveries.FirstOrDefault(br => br.ReceivingNo == tempID);
                               //        if (checkID == null)
                               //        {
                               //            break;
                               //        }
                               //    }
                               //} 
                               var check = checContext.JOSDeliveries.FirstOrDefault(br => br.PONo == itemNo);
                               if (check != null)
                               {
                                   goto Existing;
                               }
                               JOSDelivery newDelivery = new JOSDelivery();
                               newDelivery.ReceivingNo = deliveryItems.FirstOrDefault().ReceivingNo;
                               newDelivery.ReceivedDate = deliveryItems.FirstOrDefault().ReceivedDate;
                               newDelivery.DateTransferred = DateTime.Now;
                               newDelivery.DeliveryNo = int.Parse(deliveryItems.FirstOrDefault().DeliveryNo);
                               newDelivery.PONo = deliveryItems.FirstOrDefault().PONo;
                               newDelivery.DeliveryQty = deliveryItems.Count();
                               newDelivery.TotalCost = 0;
                               using (var insertContext = new DatabaseContext())
                               {
                                   insertContext.JOSDeliveries.Add(newDelivery);
                                   insertContext.SaveChanges();
                                   cnt++;
                                   Dispatcher.Invoke(() =>
                                   {
                                       waitIndicator.Content = "Transferring Data from iJOS " + cnt.ToString() + "/" + DeliveryNos.Count.ToString();
                                   });
                               }
                           }
                       }
                   }
                   Existing:;
               }
           });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    bsyPanel.Visibility = Visibility.Collapsed;
                    LoadDeliveries();
                });
            });
        }*/
        #endregion

        #region Relocated to MainWindow

        /*public void LoadPurchaseOrder()
        {
            bsyPanel.Visibility = Visibility.Visible;

            var task = Task.Run(() =>
            {
                using (var context = new iJosDatabaseContext())
                {
                    int cnt = 0;

                    Dispatcher.Invoke(() =>
                    {
                        waitIndicator.Content = "Gathering PO Data from iJOS. Please wait...";
                    });

                    var POs = context.Database.SqlQuery<PO>("SELECT TranNo, PONo, PODate, ReferenceNo, IssuerName, Quantity, TotalCost FROM PurchaseOrder").ToList();
                    var OrderNumber = POs.Select(p => p.PONo).Distinct().ToList();                    

                    using (var tranContext = new DatabaseContext())
                    {
                        var dbPO = tranContext.JigPurchaseOrders.ToList();

                        foreach (var item in OrderNumber)
                        {
                            Int64 itemcount = 0;
                            decimal totalPrice = 0;

                            cnt++;
                            Dispatcher.Invoke(() =>
                            {
                                waitIndicator.Content = "Transfering PO Data from iJOS " + cnt.ToString() + " / " + OrderNumber.Count().ToString();
                            });

                            var check = dbPO.FirstOrDefault(c => c.PONo == item);
                            if (check == null)
                            {
                                var POItems = POs.Where(p => p.PONo == item).ToList();

                                foreach (var POItem in POItems)
                                {
                                    itemcount = itemcount + POItem.Quantity;
                                    totalPrice = totalPrice + POItem.TotalCost;
                                }

                                var POI = POItems.FirstOrDefault();
                                var purchaseOrder = new JigPurchaseOrder()
                                {
                                    PONo = item,
                                    TranNo = POI.TranNo.ToString(),
                                    PODate = POI.PODate,
                                    ReferenceNo = POI.ReferenceNo,
                                    IssuerName = POI.IssuerName,
                                    Quantity = int.Parse(itemcount.ToString()),
                                    TotalPrice = float.Parse(totalPrice.ToString())
                                };

                                tranContext.JigPurchaseOrders.Add(purchaseOrder);
                                tranContext.SaveChanges();
                            }                            
                        }

                        var purchaseOrders = tranContext.JigPurchaseOrders;
                        Dispatcher.Invoke(() =>
                        {
                            dgJOSOrders.ItemsSource = purchaseOrders.ToList();
                        });
                    }
                }
            });

            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    LoadDeliveries();
                    bsyPanel.Visibility = Visibility.Collapsed;
                });
            });
        }

        public void UpdateLocalDB()
        {
            bsyPanel.Visibility = Visibility.Visible;

            var task = Task.Run(() =>
            {
                using (var context = new iJosDatabaseContext())
                {
                    int cnt = 0;

                    Dispatcher.Invoke(() =>
                    {
                        waitIndicator.Content = "Gathering Receiving Data from iJOS. Please wait...";
                    });

                    var receivables = context.Database.SqlQuery<Receiving>("SELECT * FROM Receiving").ToList();
                    var DeliveryNos = receivables.Select(d => d.PONo).Distinct().ToList();

                    using (var tranContext = new DatabaseContext())
                    {
                        var dbDel = tranContext.JOSDeliveries.ToList();

                        foreach (var item in DeliveryNos)
                        {
                            cnt++;
                            Dispatcher.Invoke(() =>
                            {
                                waitIndicator.Content = "Transfering Receiving Data from iJOS " + cnt.ToString() + " / " + DeliveryNos.Count().ToString();
                            });

                            var deliveryItems = receivables.Where(d => d.PONo == item).ToList();
                            if (deliveryItems.Count() > 0)
                            {
                                var itemNo = deliveryItems.FirstOrDefault().PONo;
                                var check = dbDel.FirstOrDefault(c => c.PONo == itemNo);
                                if (check == null)
                                {
                                    var DI = deliveryItems.FirstOrDefault();
                                    var newDelivery = new JOSDelivery()
                                    {
                                        ReceivingNo = DI.ReceivingNo,
                                        ReceivedDate = DI.ReceivedDate,
                                        DateTransferred = DateTime.Now,
                                        DeliveryNo = int.Parse(DI.DeliveryNo),
                                        PONo = DI.PONo,
                                        DeliveryQty = deliveryItems.Count(),
                                        TotalCost = 0
                                    };
                                    tranContext.JOSDeliveries.Add(newDelivery);
                                    tranContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
            });

            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    LoadPurchaseOrder();
                    //bsyPanel.Visibility = Visibility.Collapsed;
                    //LoadDeliveries();
                });
            });
        }*/

        #endregion

        public void LoadPOData()
        {
            using (var context = new DatabaseContext())
            {
                var purchaseOrders = context.JigPurchaseOrders;
                dgJOSOrders.ItemsSource = purchaseOrders.ToList();
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
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }

        private void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.busyIndicator.Visibility = Visibility.Visible;
            var selectedItem = dgJOSDeliveries.SelectedItem as JOSDelivery;
            var task = Task.Run(() =>
            {
                if (selectedItem != null)
                {
                    JIGDeliveryReport transactionItemsReport = new JIGDeliveryReport();
                    using (var context = new DatabaseContext())
                    {
                        var data = context.JOSDeliveries.FirstOrDefault(br => br.DeliveryNo == selectedItem.DeliveryNo);
                        if (data != null)
                        {
                            transactionItemsReport.FindControl("xrReceivingNo", false).Text = data.ReceivingNo;
                            transactionItemsReport.FindControl("xrPONo", false).Text = data.PONo;
                            transactionItemsReport.FindControl("xrDeliveryNo", false).Text = data.DeliveryNo.ToString();
                            transactionItemsReport.FindControl("xrDeliveryQty", false).Text = data.DeliveryQty.ToString();
                            transactionItemsReport.FindControl("xrTotalCost", false).Text = data.TotalCost.ToString();
                            transactionItemsReport.FindControl("xrReceivedDate", false).Text = data.ReceivedDate.ToString();
                            transactionItemsReport.FindControl("xrDateTransferred", false).Text = data.DateTransferred.ToString();
                            transactionItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;

                            using (var context2 = new iJosDatabaseContext())
                            {
                                var dr = selectedItem.PONo.ToString();
                                var transactionItems = context2.Receivings.Where(br => br.PONo == dr);
                                Dispatcher.Invoke(() =>
                                {
                                    transactionItemsReport.DataSource = transactionItems.ToList();
                                    PrintHelper.ShowPrintPreviewDialog(null, transactionItemsReport);
                                });
                            }

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

        private void BtnPOViewDetails_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.busyIndicator.Visibility = Visibility.Visible;
            var selectedItem = dgJOSOrders.SelectedItem as JigPurchaseOrder;
            var task = Task.Run(() =>
            {
                if (selectedItem != null)
                {

                    PurchaseOrderReport transactionItemsReport = new PurchaseOrderReport();
                    using (var context = new DatabaseContext())
                    {
                        var data = context.JigPurchaseOrders.FirstOrDefault(br => br.PONo == selectedItem.PONo);
                        if (data != null)
                        {
                            transactionItemsReport.FindControl("xrTranNo", false).Text = data.TranNo.ToString();
                            transactionItemsReport.FindControl("xrPONo", false).Text = data.PONo;
                            transactionItemsReport.FindControl("xrRefNo", false).Text = data.ReferenceNo.ToString();
                            transactionItemsReport.FindControl("xrIssuerName", false).Text = data.IssuerName.ToString();
                            transactionItemsReport.FindControl("xrQuantity", false).Text = data.Quantity.ToString();
                            transactionItemsReport.FindControl("xrTotalPrice", false).Text = selectedItem.TotalPrice.ToString();
                            transactionItemsReport.FindControl("xrGeneratedBy", false).Text = UserSession.UserName;
                            transactionItemsReport.FindControl("xrPODate", false).Text = data.PODate.ToString();

                            using (var context2 = new iJosDatabaseContext())
                            {
                                var dr = selectedItem.PONo.ToString();
                                var transactionItems = context2.PurchaseOrders.Where(br => br.PONo == dr);
                                Dispatcher.Invoke(() =>
                                {
                                    transactionItemsReport.DataSource = transactionItems.ToList();
                                    PrintHelper.ShowPrintPreviewDialog(null, transactionItemsReport);
                                });
                            }

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

        private void BtnAddToInventory_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to transfer this delivery?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var jigsToTransfer = new List<Jig>();
                var selectedItem = dgJOSDeliveries.SelectedItem as JOSDelivery;
                using (var context = new iJosDatabaseContext())
                {
                    var deliveredJigs = context.Receivings.Where(br => br.PONo == selectedItem.PONo);
                    if (deliveredJigs != null)
                    {
                        using (var context2 = new DatabaseContext())
                        {
                            var checkPO = context2.Jigs.Where(br => br.PONo == selectedItem.PONo);
                            if (checkPO.Count() > 0)
                            {
                                MessageBox.Show("This delivery is already transferred to the system", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;

                            }
                        }

                        foreach (var jig in deliveredJigs)
                        {
                            string tempID = "";
                            while (true)
                            {
                                using (var context2 = new DatabaseContext())
                                {
                                    tempID = "JIG-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                                    var checkID = context2.Jigs.FirstOrDefault(br => br.ItemCode == tempID);
                                    if (checkID == null)
                                    {
                                        break;
                                    }
                                }
                            }
                            Jig transferJig = new Jig();
                            using (var POContext = new iJosDatabaseContext())
                            {
                                var POInfo = POContext.PurchaseOrders.FirstOrDefault(br => br.PONo == jig.PONo && br.Jig_Code == jig.Jig_Code);
                                if (POInfo != null)
                                {
                                    transferJig.ItemCode = tempID;
                                    transferJig.JigCode = jig.Jig_Code;
                                    transferJig.JigName = jig.Jig_Name;
                                    transferJig.Location = "UNASSIGNED";
                                    transferJig.Type = POInfo.OrderType;//no data
                                    transferJig.PONo = jig.PONo;
                                    transferJig.Quantity = jig.ReceivedQty.ToString();
                                    transferJig.Balance = jig.ReceivedQty.ToString();
                                    transferJig.UnitCost = POInfo.Unit_Price.ToString();//no data
                                    transferJig.TotalCost = POInfo.TotalCost.ToString();//no data
                                    transferJig.Specification = POInfo.Classification;//no data
                                    transferJig.RefNo = POInfo.ReferenceNo;//no data
                                    transferJig.WarehousePIC = UserSession.UserName;
                                    transferJig.PIC = POInfo.IssuerName;
                                    transferJig.DateDelivered = jig.ReceivedDate;

                                }
                            }
                            using (var savingContext = new DatabaseContext())
                            {
                                savingContext.Jigs.Add(transferJig);
                                savingContext.SaveChanges();
                            }


                        }
                        MessageBox.Show("Delivery transferred", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }

        private void BtnAddIndividual_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgJOSDeliveries.SelectedItem as JOSDelivery;
            if (selectedItem != null)
            {

                pnlOrderItems.Visibility = Visibility.Visible;
                LoadOrderItems(selectedItem.PONo);
            }
        }

        private void BtnAddJig_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to transfer this delivery?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var jigsToTransfer = new List<Jig>();
                var selectedItem = dgOrderItem.SelectedItem as Receiving;
                using (var context = new iJosDatabaseContext())
                {
                    var deliveredJigs = context.Receivings.FirstOrDefault(br => br.PONo == selectedItem.PONo && br.Jig_Code == selectedItem.Jig_Code);
                    if (deliveredJigs != null)
                    {
                        using (var context2 = new DatabaseContext())
                        {
                            var checkPO = context2.Jigs.Where(br => br.PONo == selectedItem.PONo && br.JigCode == selectedItem.Jig_Code);
                            if (checkPO.Count() > 0)
                            {
                                MessageBox.Show("This delivery is already transferred to the system", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;

                            }
                        }


                        string tempID = "";
                        while (true)
                        {
                            using (var context2 = new DatabaseContext())
                            {
                                tempID = "JIG-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                                var checkID = context2.Jigs.FirstOrDefault(br => br.ItemCode == tempID);
                                if (checkID == null)
                                {
                                    break;
                                }
                            }
                        }
                        Jig transferJig = new Jig();
                        using (var POContext = new iJosDatabaseContext())
                        {
                            var POInfo = POContext.PurchaseOrders.FirstOrDefault(br => br.PONo == selectedItem.PONo && br.Jig_Code == selectedItem.Jig_Code);
                            if (POInfo != null)
                            {
                                transferJig.ItemCode = tempID;
                                transferJig.JigCode = deliveredJigs.Jig_Code;
                                transferJig.JigName = deliveredJigs.Jig_Name;
                                transferJig.Location = "UNASSIGNED";
                                transferJig.Type = POInfo.OrderType;//no data
                                transferJig.PONo = deliveredJigs.PONo;
                                transferJig.Quantity = deliveredJigs.ReceivedQty.ToString();
                                transferJig.Balance = deliveredJigs.ReceivedQty.ToString();
                                transferJig.UnitCost = POInfo.Unit_Price.ToString();//no data
                                transferJig.TotalCost = POInfo.TotalCost.ToString();//no data
                                transferJig.Specification = POInfo.Classification;//no data
                                transferJig.RefNo = POInfo.ReferenceNo;//no data
                                transferJig.WarehousePIC = UserSession.UserName;
                                transferJig.PIC = POInfo.IssuerName;
                                transferJig.DateDelivered = deliveredJigs.ReceivedDate;

                            }
                        }
                        using (var savingContext = new DatabaseContext())
                        {
                            savingContext.Jigs.Add(transferJig);
                            savingContext.SaveChanges();
                        }
                        MessageBox.Show("Delivery transferred", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                        var selectedItems = dgJOSDeliveries.SelectedItem as JOSDelivery;
                        LoadOrderItems(selectedItem.PONo);
                    }
                }
            }
        }

        private void BtnCloseOrderItems_Click(object sender, RoutedEventArgs e)
        {
            pnlOrderItems.Visibility = Visibility.Collapsed;
        }

        private void LoadOrderItems(string PONo)
        {
            using (var context = new iJosDatabaseContext())
            {
                var items = context.Receivings.Where(br => br.PONo == PONo);
                josOrderItems.DataSource = items.ToList();
                //dgOrderItem.ItemsSource = josOrderItems;
                dgOrderItem.RefreshData();
            }
        }
    }
}