using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EngineeringToolsEquipmentsInventory
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Timer timer = new Timer();
        Timer timer2 = new Timer();
        public DispatcherTimer dTimer = new DispatcherTimer() { Interval = new TimeSpan(0, 30, 0) };
        public static bool IsUpdateDone = false;
        public MainWindow()
        {
            try
            {
                InitializeComponent(); 
                timer.Interval = 1000;
                timer.Elapsed += Timer_Elapsed;
                timer.Start(); 
                timer2.Interval = 1000;
                timer2.Elapsed += Timer2_Elapsed;
                //timer2.Start(); 
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }

        private void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                txtInteractionTime.Text = GetLastInputTime().ToString();

                if (Convert.ToInt32(txtInteractionTime.Text) >= 60)
                {
                    UserSession.UserID = "";
                    UserSession.UserName = "";
                    UserSession.idScanTemp = "";
                    UserSession.UserIDTemp = "";
                    txtUserName.Text = "";
                    txtDepartment.Text = "";
                    timer2.Stop();
                    dTimer.Stop();
                    mainNavigation.Navigate(new LoginView()); 
                }
            });
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            try
            {
                this.Dispatcher.Invoke(() =>
                {
                    txtDateTime.Text = DateTime.Now.ToLongDateString() + " - " + DateTime.Now.ToLongTimeString();
                });
            }
            catch (Exception ex)
            {

                //throw;
            }
            
        }

        private void dTimer_Tick(object sender, EventArgs e)
        {
            UpdateLocalDB();
        }

        public void UpdateLocalDB()
        {
            //bsyPanel.Visibility = Visibility.Visible;
            dTimer.Stop();

            var task = Task.Run(() =>
            {
                using (var context = new iJosDatabaseContext())
                {
                    int cnt = 0;

                    //Dispatcher.Invoke(() =>
                    //{
                    //    waitIndicator.Content = "Gathering Receiving Data from iJOS. Please wait...";
                    //});

                    var receivables = context.Database.SqlQuery<Receiving>("SELECT * FROM Receiving").ToList();
                    var DeliveryNos = receivables.Select(d => d.PONo).Distinct().ToList();

                    using (var tranContext = new DatabaseContext())
                    {
                        var dbDel = tranContext.JOSDeliveries.ToList();

                        foreach (var item in DeliveryNos)
                        {
                            cnt++;
                            //Dispatcher.Invoke(() =>
                            //{
                            //    waitIndicator.Content = "Transfering Receiving Data from iJOS " + cnt.ToString() + " / " + DeliveryNos.Count().ToString();
                            //});

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
                    /*bsyPanel.Visibility = Visibility.Collapsed;
                    LoadDeliveries();*/
                });
            });
        }

        public void LoadPurchaseOrder()
        {
            //bsyPanel.Visibility = Visibility.Visible;

            var task = Task.Run(() =>
            {
                using (var context = new iJosDatabaseContext())
                {
                    int cnt = 0;

                    //Dispatcher.Invoke(() =>
                    //{
                    //    waitIndicator.Content = "Gathering PO Data from iJOS. Please wait...";
                    //});

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
                            //Dispatcher.Invoke(() =>
                            //{
                            //    waitIndicator.Content = "Transfering PO Data from iJOS " + cnt.ToString() + " / " + OrderNumber.Count().ToString();
                            //});

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

                        //var purchaseOrders = tranContext.JigPurchaseOrders;
                        //Dispatcher.Invoke(() =>
                        //{
                        //    dgJOSOrders.ItemsSource = purchaseOrders.ToList();
                        //});

                        var lastUpdate = tranContext.JOSUpdates.ToList();

                        if (lastUpdate.Count > 0)
                        {
                            var update = lastUpdate.FirstOrDefault();
                            update.LastUpdate = DateTime.Now;
                        }
                        else
                        {
                            var update = new JOSUpdate()
                            {
                                LastUpdate = DateTime.Now
                            };
                            tranContext.JOSUpdates.Add(update);
                        }
                        tranContext.SaveChanges();
                    }
                }
            });

            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    IsUpdateDone = true;
                    dTimer.Start();
                });
            });
        }

        public void UpdateMissing()
        {
            var task = Task.Run(() =>
            {
                using (var context = new DatabaseContext())
                {
                    var loanedTools = context.LoanedTools.ToList();
                    var loans = context.Loans.ToList();
                    var tools = context.Tools.ToList();
                    var loanedItems = loanedTools.Where(l => l.Status == "Active" && l.Condition == "GOOD").OrderByDescending(l => l.DateBorrowed).ToList();
                    var dateNow = DateTime.Now;
                    foreach (var loan in loanedItems)
                    {
                        var hrsBorrow = dateNow.Subtract(loan.DateBorrowed);
                        if (Math.Round(hrsBorrow.TotalHours) > 24)
                        {
                            loan.Status = "Active";
                            loan.Condition = "MISSING";
                            loan.DateReturned = DateTime.Now;
                            loan.ReturnLoginName = "SYSTEM";

                            var inv = tools.FirstOrDefault(i => i.ItemCode == loan.ItemCode);
                            if (inv != null)
                            {
                                inv.Status = "In-Stock";
                                inv.Condition = "LOST";
                                inv.LastUpdate = DateTime.Now;
                            }

                            var loanTools = loanedTools.Where(l => l.LoanID == loan.LoanID && l.Status == "Active");
                            if (loanTools.Count() <= 0)
                            {
                                var motherLoan = loans.FirstOrDefault(m => m.LoanID == loan.LoanID);
                                if (motherLoan != null)
                                {
                                    motherLoan.Status = "Returned";
                                    motherLoan.ReturnDate = DateTime.Now;
                                    motherLoan.LoginName = "SYSTEM";
                                }
                            }

                            context.SaveChanges();
                        }
                    }
                }
            });
            task.ContinueWith((t) =>
            {
                UpdateLocalDB();
            });
        }

        public void NavigateView(string viewName)
        {
            mainNavigation.Navigate(new ToolsView());
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            if (UserSession.UserID != "")
            {
                if (MessageBox.Show("Are you sure you want to logout?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    UserSession.UserID = "";
                    UserSession.UserName = "";
                    UserSession.UserIDTemp = "";
                    txtUserName.Text = "";
                    txtDepartment.Text = "";
                    mainNavigation.Navigate(new LoginView());
                    timer2.Stop();
                    dTimer.Stop();
                    mainNavigation.Focus();
                }
            }
            mainNavigation.Focus();
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                mainNavigation.Navigate(new LoginView());
                dTimer.Tick += dTimer_Tick;
                //dTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e) 
        {
            if (Models.UserSession.UserID == "")
            {
                string test = e.Key.ToString();
                string charTemp = Regex.Replace(e.Key.ToString(), "[^0-9.]", "").Replace(".", "");
                Models.UserSession.idScanTemp = Models.UserSession.idScanTemp + charTemp;
                if (e.Key == Key.Enter)
                {
                    if (Models.UserSession.idScanTemp != "")
                    {
                        using (var context = new DatabaseContext())
                        {
                            var user = context.Users.FirstOrDefault(br => br.UserID == Models.UserSession.idScanTemp && br.Type == "Administrator");
                            if (user != null)
                            {
                                Models.UserSession.idScanTemp = "";
                                Models.UserSession.UserID = user.UserID;
                                txtUserName.Text = user.Name;
                                txtDepartment.Text = user.Department;
                                UserSession.UserName = user.Name;
                                timer2.Start();
                                //UpdateLocalDB();
                                UpdateMissing();
                                mainNavigation.Navigate(new MainMenuView());
                            }
                            else
                            {
                                Models.UserSession.idScanTemp = "";
                                MessageBox.Show("Unrecognized ID Card", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                    }
                }
            }
            if (e.Key == Key.F1 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new ToolsView());
            }
            if (e.Key == Key.F7 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new MainMenuView());
            }
            if (e.Key == Key.F4 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new HistoryView());
            }
            if (e.Key == Key.F2 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new ConsumablesView());
            }
            if (e.Key == Key.F6 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new InventoryManagementView());
            }
            if (e.Key == Key.F3 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new JigsView());
            }
            if (e.Key == Key.F5 && Models.UserSession.UserID != "")
            {
                mainNavigation.Navigate(new AssetAndEquipmentView());
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        struct LASTINPUTINFO
        {
            public static readonly int SizeOf = Marshal.SizeOf(typeof(LASTINPUTINFO));

            [MarshalAs(UnmanagedType.U4)]
            public UInt32 cbSize;
            [MarshalAs(UnmanagedType.U4)]
            public UInt32 dwTime;
        }

        [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        static uint GetLastInputTime()
        {
            uint idleTime = 0;
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = (uint)Marshal.SizeOf(lastInputInfo);
            lastInputInfo.dwTime = 0;

            uint envTicks = (uint)Environment.TickCount;

            if (GetLastInputInfo(ref lastInputInfo))
            {
                uint lastInputTick = lastInputInfo.dwTime;

                idleTime = envTicks - lastInputTick;
            }

            return ((idleTime > 0) ? (idleTime / 1000) : 0);
        }
    }
}

