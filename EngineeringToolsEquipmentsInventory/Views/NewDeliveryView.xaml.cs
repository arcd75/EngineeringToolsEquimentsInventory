using DevExpress.Xpf.Grid;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EngineeringToolsEquipmentsInventory.Views
{
    /// <summary>
    /// Interaction logic for NewDeliveryView.xaml
    /// </summary>
    public partial class NewDeliveryView : UserControl
    {
        string deliveryID = "";
        public NewDeliveryView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            while (true)
            {
                deliveryID = "WHS-DLR-" + DateTime.Today.ToString("ddMMyy") + RandomString(4);
                using (var context = new DatabaseContext())
                {
                    var check = context.Deliveries.FirstOrDefault(br => br.DeliveryID == deliveryID);
                    if (check == null)
                    {
                        txtDeliveryID.Text = deliveryID;
                        break;
                    }
                }
            }
            NewDeliveryItems();
            NewDeliveryToolItems();
        }

        private void NewDeliveryItems()
        {
            DeliveryReceiving.ItemBinding.DataSource = DeliveryReceiving.GetDeliveryItems.ToList();
            dgItems.ItemsSource = DeliveryReceiving.ItemBinding;
            //dgItems.ItemsSource = DeliveryReceiving.GetDeliveryItems.ToList();
            dgItems.RefreshData();
        }
        private void NewDeliveryToolItems()
        {
            DeliveryReceiving.ItemToolBinding.DataSource = DeliveryReceiving.GetDeliveryToolItems.ToList();
            dgTools.ItemsSource = DeliveryReceiving.ItemToolBinding;
            dgTools.RefreshData();
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItems = dgItems.SelectedItem as DeliveryItems;
            if (selectedItems != null)
            {
                using (var context = new DatabaseContext())
                {
                    var consumable = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                    if (consumable != null)
                    {
                        selectedItems.ItemName = consumable.ItemName;
                        selectedItems.UOM = consumable.UOM;
                    }
                }
            }
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            int cnt = 0;

       
            if (dgItems.VisibleRowCount > 0)
            {
                for (int i = 0; i < dgItems.VisibleRowCount; i++)
                {
                    string deliveryID = "";
                    while (true)
                    {
                        deliveryID = "WHS-DLR-ITM" + DateTime.Today.ToString("ddMMyy") + RandomString(4); ;
                        using (var context = new DatabaseContext())
                        {
                            var checking = context.DeliveriesItem.FirstOrDefault(br => br.DeliveryItemID == deliveryID);
                            if (checking == null)
                            {
                                break;
                            }
                        }
                    }

                    DeliveryItems createDelivery = new DeliveryItems();
                    createDelivery.DeliveryItemID = deliveryID;
                    createDelivery.DRNumber = txtDRN.Text;
                    createDelivery.ItemCode = dgItems.GetCellValue(i, "ItemCode").ToString();
                    
                    createDelivery.ItemName = dgItems.GetCellValue(i, "ItemName").ToString();
                    createDelivery.UOM = dgItems.GetCellValue(i, "UOM").ToString();
                    createDelivery.Quantity = Convert.ToInt32(dgItems.GetCellValue(i, "Quantity").ToString());
                    createDelivery.Date = DateTime.Now;
                    DeliveryReceiving.GetDeliveryItems.Add(createDelivery);

                    using (var context = new DatabaseContext())
                    {
                        context.DeliveriesItem.Add(createDelivery);
                        context.SaveChanges();
                    } 
                    using (var context = new DatabaseContext())
                    {
                        string code = dgItems.GetCellValue(i, "ItemCode").ToString();
                        var consumable = context.Consumables.FirstOrDefault(br => br.ItemCode == code);
                        if (consumable != null)
                        {
                            consumable.RemainingQuantity = consumable.RemainingQuantity + Convert.ToInt32(dgItems.GetCellValue(i, "Quantity").ToString());
                            context.SaveChanges();
                            cnt++;
                        }
                    }

                } 
            }

            if (dgTools.VisibleRowCount > 0)
            {
                for (int i = 0; i < dgTools.VisibleRowCount; i++)
                {
                    string deliveryID = "";
                    string itemCode = "";
                    while (true)
                    {
                        deliveryID = "WHS-DLR-ITM" + DateTime.Today.ToString("ddMMyy") + RandomString(4); ;
                        using (var context = new DatabaseContext())
                        {
                            var checking = context.DeliveriesToolItem.FirstOrDefault(br => br.DeliveryItemID == deliveryID);
                            if (checking == null)
                            {
                                break;
                            }
                        }
                    }

                    DeliveryToolItems deliveryTool = new DeliveryToolItems();
                    deliveryTool.DeliveryItemID = deliveryID;
                    deliveryTool.DRNumber = txtDRN.Text;
                    deliveryTool.ItemName = dgTools.GetCellValue(i, "ItemName").ToString();
                    deliveryTool.ItemDescription = dgTools.GetCellValue(i, "ItemDescription").ToString();
                    deliveryTool.Brand = dgTools.GetCellValue(i, "Brand").ToString();
                    deliveryTool.PECode = dgTools.GetCellValue(i, "PECode").ToString();
                    deliveryTool.Date = DateTime.Now;

                    using (var context = new DatabaseContext())
                    {
                        context.DeliveriesToolItem.Add(deliveryTool);
                        context.SaveChanges();
                    }
                    while (true)
                    {
                        itemCode = "PE-" + DateTime.Today.ToString("ddMMyy") + RandomString(6); ;
                        using (var context = new DatabaseContext())
                        {
                            var checking = context.Tools.FirstOrDefault(br => br.ItemCode == itemCode);
                            if (checking == null)
                            {
                                break;
                            }
                        }
                    } 
                    Tool tool = new Tool();
                    tool.ItemCode = itemCode;
                    tool.Type = "N/A";
                    tool.Item = deliveryTool.ItemName;
                    tool.ItemDescription = deliveryTool.ItemDescription;
                    tool.Brand = deliveryTool.Brand;
                    tool.Condition = "GOOD";
                    tool.Remarks = "N/A";
                    tool.DateDelivered = DeliveryReceiving.newDelivery.DeliveryDate;
                    tool.LastUpdate = DateTime.Now;
                    tool.Status = "In-Stock";
                    tool.PECode = deliveryTool.PECode;
                    using (var context = new DatabaseContext())
                    {
                        context.Tools.Add(tool);
                        context.SaveChanges();
                    }
                    cnt++; 
                } 
            }

            if (dgItems.VisibleRowCount <= 0 && dgTools.VisibleRowCount <= 0)
            {
                MessageBox.Show("Delivery is Empty!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    Delivery delivery = new Delivery();
                    delivery.DeliveryID = DeliveryReceiving.newDelivery.DeliveryID;
                    delivery.DRNumber = DeliveryReceiving.newDelivery.DRNumber;
                    delivery.PONumber = DeliveryReceiving.newDelivery.PONumber;
                    delivery.DeliveryDate = DeliveryReceiving.newDelivery.DeliveryDate;
                    delivery.Date = DateTime.Now;
                    delivery.TotalItem = cnt.ToString();
                    context.Deliveries.Add(delivery);
                    context.SaveChanges();
                    MessageBox.Show("Delivery saved!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                } 
            }
        }

        private void ClearFields()
        {
            txtDRN.Text = "";
            txtPON.Text = "";
            dgItems.ItemsSource = null;
            dgTools.ItemsSource = null;
            DeliveryReceiving.newDelivery.DeliveryID ="";
        }
          

        private void TxtDRN_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void TxtPON_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void BtnFile_Click(object sender, RoutedEventArgs e)
        {
            if (DeliveryReceiving.newDelivery.DeliveryID == "" || DeliveryReceiving.newDelivery.DeliveryID == null)
            {
                if (txtDRN.Text == "" || txtPON.Text == "")
                {
                    MessageBox.Show("Incomplete information!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                DeliveryReceiving.newDelivery.DeliveryID = txtDeliveryID.Text;
                DeliveryReceiving.newDelivery.DRNumber = txtDRN.Text.Trim();
                DeliveryReceiving.newDelivery.PONumber = txtPON.Text.Trim();
                DeliveryReceiving.newDelivery.Date = DateTime.Now;
                DeliveryReceiving.newDelivery.DeliveryDate = dtDateDelivered.DateTime;

                txtPON.IsEnabled = false;
                txtDRN.IsEnabled = false;
                txtDeliveryID.IsEnabled = false;
                dtDateDelivered.IsEnabled = false;
                dgItems.IsEnabled = true;
                dgTools.IsEnabled = true;
                btnFile.IsEnabled = false;
                NewDeliveryItems();
                NewDeliveryToolItems(); 
            }
        }
        private void ResetTransaction()
        {
            DeliveryReceiving.newDelivery.DeliveryID = "";
            txtDeliveryID.IsEnabled = true;
            dtDateDelivered.IsEnabled = true;
            txtPON.IsEnabled = true;
            txtDRN.IsEnabled = true;
            btnFile.IsEnabled = true;
            dgItems.IsEnabled = false;
            dgTools.IsEnabled = false;
            txtDRN.Text = "";
            txtPON.Text = "";
            dgItems.ItemsSource = null;
            dgItems.RefreshData();
            dgTools.ItemsSource = null;
            dgTools.RefreshData();
            DeliveryReceiving.GetDeliveryItems.Clear();
            while (true)
            {
                deliveryID = "WHS-DLR-" + DateTime.Today.ToString("ddMMyy") + RandomString(4);
                using (var context = new DatabaseContext())
                {
                    var check = context.Deliveries.FirstOrDefault(br => br.DeliveryID == deliveryID);
                    if (check == null)
                    {
                        txtDeliveryID.Text = deliveryID;
                        break;
                    }
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            ResetTransaction();
        }

        private void TableView_CellValueChanged_1(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            string deliveryToolID = "";
            while (true)
            {
                deliveryToolID = "WHS-DLR-ITM" + DateTime.Today.ToString("ddMMyy") + RandomString(4);
                using (var context = new DatabaseContext())
                {
                    var check = context.DeliveriesToolItem.FirstOrDefault(br => br.DeliveryItemID == deliveryID);
                    if (check == null)
                    { 
                        break;
                    }
                }
            }
            
            var selectedItem = dgTools.SelectedItem as CreateToolDelivery; 
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.Tools.FirstOrDefault(br => br.Item == selectedItem.ItemName);
                    if (tool != null)
                    {
                        selectedItem.Brand = tool.Brand;
                        selectedItem.ItemDescription = tool.ItemDescription;
                    }

                }
            }

            if (selectedItem.PECode != "" || selectedItem.PECode != null)
            {
                using (var context = new DatabaseContext())
                {
                    var checkPECode = context.Tools.FirstOrDefault(br => br.Item == selectedItem.ItemName && br.PECode == selectedItem.PECode);
                    if (checkPECode != null)
                    {
                        if (MessageBox.Show("A tool with the same name and PE Code is found - " + checkPECode.Item + ", " + checkPECode.PECode + "\nDo you still want to use this code?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Error) == MessageBoxResult.Yes)
                        {

                        }
                        else
                        {
                            selectedItem.PECode = "";
                        }
                    }
                }
            }


        }
    }
}
