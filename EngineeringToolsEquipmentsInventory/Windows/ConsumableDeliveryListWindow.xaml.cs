using DevExpress.Xpf.Core;
using EngineeringToolsEquipmentsInventory.Models;
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
using System.Windows.Shapes;

namespace EngineeringToolsEquipmentsInventory.Windows
{
    /// <summary>
    /// Interaction logic for ConsumableDeliveryListWindow.xaml
    /// </summary>
    public partial class ConsumableDeliveryListWindow : Window
    {
        public ConsumableDeliveryListWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (StockInHistorySession.ItemCode != "")
            {
                dgSpareParts.Visibility = Visibility.Collapsed;
                dgConsumables.Visibility = Visibility.Visible;
                using (var context = new DatabaseContext())
                {
                    var history = context.DeliveriesItem.Where(br => br.ItemCode == StockInHistorySession.ItemCode);
                    if (history.Count() > 0)
                    {
                        dgConsumables.ItemsSource = history.ToList();
                        pnlConsumable.Visibility = Visibility.Visible;
                        pnlSpareParts.Visibility = Visibility.Collapsed;
                        dgConsumables.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        DXMessageBox.Show("No stockin history found", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                } 
            }

            if (StockInHistorySession.SpareItemCode != "")
            {
                dgSpareParts.Visibility = Visibility.Visible;
                dgConsumables.Visibility = Visibility.Collapsed;
                using (var context = new DatabaseContext())
                {
                    var history = context.DeliveryItemSpareParts.Where(br => br.ItemCode == StockInHistorySession.SpareItemCode);
                    if (history.Count() > 0)
                    {
                        dgSpareParts.ItemsSource = history.ToList();
                        pnlConsumable.Visibility = Visibility.Collapsed;
                        pnlSpareParts.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        DXMessageBox.Show("No stockin history found", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }

        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                StockInHistorySession.SpareItemCode = "";
                StockInHistorySession.ItemCode = "";
                this.Close();
            }
        }

        private void BtnSparePartDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure yo want to delete this item?","Inventory System",MessageBoxButton.YesNo,MessageBoxImage.Question)  == MessageBoxResult.Yes)
            {
                var selectedItem = dgSpareParts.SelectedItem as DeliveryItemSparePart;
                if (selectedItem != null)
                { 
                    using (var context = new DatabaseContext())
                    {

                        var check = context.SpareParts.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                        if (check != null)
                        {
                            if ((check.AvailableQuantity - Int32.Parse(selectedItem.Qty)) > 0)
                            {
                                //// Do nothing
                            }
                            else
                            {
                                DXMessageBox.Show("Not enough quantity to return", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }

                    using (var context = new DatabaseContext())
                    {
                        var update = context.SpareParts.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                        if (update != null)
                        {
                            update.AvailableQuantity = update.AvailableQuantity - Int32.Parse(selectedItem.Qty);
                            context.SaveChanges();
                            var delete = context.DeliveryItemSpareParts.FirstOrDefault(br => br.DeliveryItemID == selectedItem.DeliveryItemID);
                            if (delete != null)
                            {
                                context.DeliveryItemSpareParts.Remove(delete);
                                context.SaveChanges();
                            }
                        }
                    }

                    if (StockInHistorySession.SpareItemCode != "")
                    {
                        dgSpareParts.Visibility = Visibility.Visible;
                        dgConsumables.Visibility = Visibility.Collapsed;
                        using (var context = new DatabaseContext())
                        {
                            var history = context.DeliveryItemSpareParts.Where(br => br.ItemCode == StockInHistorySession.SpareItemCode);
                            if (history.Count() > 0)
                            {
                                dgSpareParts.ItemsSource = history.ToList();
                                pnlConsumable.Visibility = Visibility.Collapsed;
                                pnlSpareParts.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                dgSpareParts.ItemsSource = history.ToList();
                                pnlConsumable.Visibility = Visibility.Collapsed;
                                pnlSpareParts.Visibility = Visibility.Visible;
                                DXMessageBox.Show("No stockin history found", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }
                } 
            }
        }

        private void BtnConsumableDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure yo want to delete this item?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedItem = dgConsumables.SelectedItem as DeliveryItems;
                if (selectedItem != null)
                {
                    using (var context = new DatabaseContext())
                    {

                        var check = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                        if (check != null)
                        {
                            if ((check.RemainingQuantity - selectedItem.Quantity) > 0)
                            {
                                //// Do nothing
                            }
                            else
                            {
                                DXMessageBox.Show("Not enough quantity to return", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                return;
                            }
                        }
                    }

                    using (var context = new DatabaseContext())
                    {
                        var update = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                        if (update != null)
                        {
                            update.RemainingQuantity = update.RemainingQuantity - selectedItem.Quantity;
                            context.SaveChanges();
                            var delete = context.DeliveriesItem.FirstOrDefault(br => br.DeliveryItemID == selectedItem.DeliveryItemID);
                            if (delete != null)
                            {
                                context.DeliveriesItem.Remove(delete);
                                context.SaveChanges();
                            }
                        }
                    }

                    if (StockInHistorySession.ItemCode != "")
                    {
                        dgSpareParts.Visibility = Visibility.Collapsed;
                        dgConsumables.Visibility = Visibility.Visible;
                        using (var context = new DatabaseContext())
                        {
                            var history = context.DeliveriesItem.Where(br => br.ItemCode == StockInHistorySession.ItemCode);
                            if (history.Count() > 0)
                            {
                                dgConsumables.ItemsSource = history.ToList();
                                pnlConsumable.Visibility = Visibility.Visible;
                                pnlSpareParts.Visibility = Visibility.Collapsed;
                                dgConsumables.Visibility = Visibility.Visible;
                            }
                            else
                            {
                                dgConsumables.ItemsSource = history.ToList();
                                pnlConsumable.Visibility = Visibility.Visible;
                                pnlSpareParts.Visibility = Visibility.Collapsed;
                                dgConsumables.Visibility = Visibility.Visible;
                                DXMessageBox.Show("No stock in history found", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Warning);
                            }
                        }
                    }


                }
            }
        }
    }
}
