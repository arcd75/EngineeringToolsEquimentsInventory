using DevExpress.Xpf.Core;
using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Windows;
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
    /// Interaction logic for StockinView.xaml
    /// </summary>
    public partial class StockinView : UserControl
    {
        System.Windows.Forms.BindingSource consumableBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource toolBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource spareBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource assetBinding = new System.Windows.Forms.BindingSource();

        List<DeliveryItems> consumableDeliveryList = new List<DeliveryItems>();
        List<DeliveryAsset> assetDeliveryList = new List<DeliveryAsset>();
        List<DeliveryItemSparePart> spareDeliveryList = new List<DeliveryItemSparePart>();
        List<CreateToolDelivery> toolDeliveryList = new List<CreateToolDelivery>();

        public StockinView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            dgAssetStockIn.ItemsSource = assetBinding;
            dgConsumableStockIn.ItemsSource = consumableBinding;
            dgSparepartsStockIn.ItemsSource = spareBinding;
            dgToolStockIn.ItemsSource = toolBinding;
            txtDeliveryID.Text = GenerateID();
            txtSparePartTotal.Text = "0";
            txtToolTotal.Text = "0";
            txtConsumableTotal.Text = "0";
            RefreshBinding();
        }


        private void RefreshBinding()
        {
            bsyPanel.Visibility = Visibility.Visible;
            var task = Task.Run(() =>
            {
                using (var context = new DatabaseContext())
                {
                    var consumables = context.ItemMsts;
                    Dispatcher.Invoke(() =>
                    {
                        cmbConsumableItemCode.ItemsSource = consumables.ToList();
                        txtToolItemCode.ItemsSource = consumables.ToList();
                        cmbSpareItemCode.ItemsSource = consumables.ToList();
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
        }
        private static string GenerateID()
        {
            string tempID = "";
            while (true)
            {
                using (var context2 = new Models.DatabaseContext())
                {
                    tempID = "DEL-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                    var checkID = context2.Deliveries.FirstOrDefault(br => br.DeliveryID == tempID);
                    if (checkID == null)
                    {
                        return tempID;
                    }
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
        public static string RandomNumbers(int length)
        {
            const string chars = "0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void CmbConsumableItemCode_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            using (var context = new DatabaseContext())
            {
                var item = context.ItemMsts.FirstOrDefault(br => br.item_cd == cmbConsumableItemCode.Text);
                if (item != null)
                {
                    txtConsumableItemName.Text = item.description;
                    txtConsumableUOM.Text = item.uom;
                }
            }
        }

        private void BtnConsumableAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtConsumableQty.Text == "" || txtConsumableQty.Text == "0" || int.Parse(txtConsumableQty.Text) <= 0)
            {
                MessageBox.Show("Quantity cannot be 0!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string tempID = "";
            while (true)
            {
                using (var context2 = new Models.DatabaseContext())
                {
                    tempID = "WHS-DLR-ITM" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                    var checkID = context2.DeliveriesItem.FirstOrDefault(br => br.DeliveryItemID == tempID);
                    if (checkID == null)
                    {
                        break;
                    }
                }
            }

            DeliveryItems deliveryItems = new DeliveryItems();
            deliveryItems.DeliveryItemID = tempID;
            deliveryItems.DRNumber = "";
            deliveryItems.ItemCode = cmbConsumableItemCode.Text;
            deliveryItems.ItemName = txtConsumableItemName.Text;
            deliveryItems.UOM = txtConsumableUOM.Text;
            deliveryItems.Quantity = int.Parse(txtConsumableQty.Text);
            deliveryItems.DeliveryID = txtDeliveryID.Text;
            deliveryItems.Date = DateTime.Now;
            consumableDeliveryList.Add(deliveryItems);
            LoadDeliveryItems();
            ClearConsumable();
            ComputeTotals();
        }

        private void ClearConsumable()
        {
            txtConsumableBarcode.Text = "";
            txtConsumableItemName.Text = "";
            txtConsumableQty.Text = "";
            txtConsumableUOM.Text = "";
            cmbConsumableItemCode.Text = "";
        }

        private void ClearAsset()
        {

            txtAssetItemName.Text = "";
            txtAssetQty.Text = "";
            txtAssetUOM.Text = "";
            cmbAssetItemCode.Text = "";
            txtAssetRTP.Text = "";
            txtAssetSerialNo.Text = "";
            txtAssetUnitCost.Text = "0";

        }

        private void ClearSpare()
        {
            txtSpareCost.Text = "";
            txtSpareItemName.Text = "";
            txtSpareQty.Text = "";
            txtSpareTotalCost.Text = "";
            cmbSpareItemCode.Text = "";
        }
        private void LoadDeliveryItems()
        {
            consumableBinding.DataSource = consumableDeliveryList;
            dgConsumableStockIn.RefreshData();

            spareBinding.DataSource = spareDeliveryList;
            dgSparepartsStockIn.RefreshData();

            toolBinding.DataSource = toolDeliveryList;
            dgToolStockIn.RefreshData();

            assetBinding.DataSource = assetDeliveryList;
            dgAssetStockIn.RefreshData();

        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgConsumableStockIn.SelectedItem as DeliveryItems;
            if (selectedItem != null)
            {
                var toDelete = consumableDeliveryList.FirstOrDefault(br => br.DeliveryItemID == selectedItem.DeliveryItemID);
                if (toDelete != null)
                {
                    consumableDeliveryList.Remove(toDelete);
                    LoadDeliveryItems();
                    ComputeTotals();
                }
            }
        }

        private void ComputeTotals()
        {
            txtConsumableTotal.Text = consumableDeliveryList.Count().ToString();
            txtSparePartTotal.Text = spareDeliveryList.Count().ToString();
            txtToolTotal.Text = toolDeliveryList.Count().ToString();
            txtAssetTotal.Text = assetDeliveryList.Count().ToString();
        }

        private void BtnSpareAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbSpareItemCode.Text == "" || txtSpareItemName.Text == "" || txtSpareCost.Text == "" || int.Parse(txtSpareQty.Text) <= 0)
                {
                    MessageBox.Show("Quantity cannot be zero!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                string tempID = "";
                while (true)
                {
                    using (var context2 = new Models.DatabaseContext())
                    {
                        tempID = "WHS-DLR-ITM" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                        var checkID = context2.DeliveryItemSpareParts.FirstOrDefault(br => br.DeliveryItemID == tempID);
                        if (checkID == null)
                        {
                            break;
                        }
                    }
                }

                DeliveryItemSparePart deliveryItemSparePart = new DeliveryItemSparePart();
                deliveryItemSparePart.DeliveryItemID = tempID;
                deliveryItemSparePart.ItemName = txtSpareItemName.Text.Trim();
                deliveryItemSparePart.Cost = txtSpareCost.Text;
                deliveryItemSparePart.Qty = txtSpareQty.Text;
                deliveryItemSparePart.TotalCost = txtSpareTotalCost.Text;
                deliveryItemSparePart.ItemCode = cmbSpareItemCode.Text;
                deliveryItemSparePart.DeliveryID = txtDeliveryID.Text;
                spareDeliveryList.Add(deliveryItemSparePart);
                LoadDeliveryItems();
                ClearSpare();
                ComputeTotals();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Quantity cannot be zero! " + ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);

            }



        }

        private void CmbSpareItemCode_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            using (var context = new DatabaseContext())
            {
                var spareItem = context.ItemMsts.FirstOrDefault(br => br.item_cd == cmbSpareItemCode.Text);
                if (spareItem != null)
                {
                    txtSpareItemName.Text = spareItem.description;
                    txtSpareCost.Text = spareItem.UnitCost.ToString();
                    txtSpareQty.Text = "0";
                    float temp = float.Parse(txtSpareQty.Text) * float.Parse(txtSpareCost.Text);
                    txtSpareTotalCost.Text = temp.ToString();
                }
            }
        }


        private void TxtSpareQty_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            if (txtSpareQty.Text == "")
            {
                txtSpareQty.Text = "0";
            }
            if (txtSpareCost.Text != "")
            {
                float res = float.Parse(txtSpareQty.Text) * float.Parse(txtSpareCost.Text);
                txtSpareTotalCost.Text = res.ToString();
            }

        }

        private void BtnSpareDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgSparepartsStockIn.SelectedItem as DeliveryItemSparePart;

            var item = spareDeliveryList.FirstOrDefault(br => br.DeliveryItemID == selectedItems.DeliveryItemID);
            if (item != null)
            {
                spareDeliveryList.Remove(item);
                LoadDeliveryItems();
                ComputeTotals();
            }
        }

        private void BtnConsumableAddNew_Click(object sender, RoutedEventArgs e)
        {
            AddNewConsumable addNewConsumable = new AddNewConsumable();
            addNewConsumable.ShowDialog();
            RefreshBinding();
        }

        private void LookUpEdit_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            if (txtToolItemCode.Text != "")
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.ItemMsts.FirstOrDefault(br => br.item_cd == txtToolItemCode.Text);
                    if (tool != null)
                    {
                        txtToolItemname.Text = tool.description;
                        txtToolUnitCost.Text = tool.UnitCost.ToString();
                    }
                }
            }
        }

        private void BtnToolAdd_Click(object sender, RoutedEventArgs e)
        {
            AddToolWindow addToolWindow = new AddToolWindow();
            addToolWindow.ShowDialog();
            RefreshBinding();
        }

        private void BtnAddTool_Click(object sender, RoutedEventArgs e)
        {
            if (txtToolUnitCost.Text == "" || txtToolPECode.Text == "" || txtToolItemCode.Text == "")
            {
                MessageBox.Show("Please complete all information!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string tempID = "";
            while (true)
            {
                using (var context2 = new Models.DatabaseContext())
                {
                    tempID = "WHS-DLR-ITM" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                    var checkID = context2.DeliveriesToolItem.FirstOrDefault(br => br.DeliveryItemID == tempID);
                    if (checkID == null)
                    {
                        break;
                    }
                }
            }
            ItemMst tool = new ItemMst();
            using (var context = new DatabaseContext())
            {
                var getTool = context.ItemMsts.FirstOrDefault(br => br.item_cd == txtToolItemCode.Text);
                if (getTool != null)
                {
                    tool = getTool;
                }
            }

            CreateToolDelivery deliveryToolItems = new CreateToolDelivery();
            deliveryToolItems.DeliveryItemID = tempID;
            deliveryToolItems.DRNumber = "";
            deliveryToolItems.ItemName = tool.description;
            deliveryToolItems.ItemDescription = tool.description;
            deliveryToolItems.Brand = tool.brand;
            deliveryToolItems.PECode = txtToolPECode.Text;
            deliveryToolItems.Date = DateTime.Now;
            deliveryToolItems.ItemCode = txtToolItemCode.Text;
            deliveryToolItems.UnitCost = float.Parse(txtToolUnitCost.Text);
            deliveryToolItems.DeliveryID = txtDeliveryID.Text;
            toolDeliveryList.Add(deliveryToolItems);
            LoadDeliveryItems();
            ClearToolFields();
            ComputeTotals();
        }

        private void ClearToolFields()
        {
            txtToolItemCode.Text = "";
            txtToolItemname.Text = "";
            txtToolPECode.Text = "";
            txtToolUnitCost.Text = "0";
        }

        private void BtnToolDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgToolStockIn.SelectedItem as CreateToolDelivery;
            if (selectedItems != null)
            {
                var deleteItem = toolDeliveryList.FirstOrDefault(br => br.DeliveryItemID == selectedItems.DeliveryItemID);
                if (deleteItem != null)
                {
                    toolDeliveryList.Remove(deleteItem);
                    LoadDeliveryItems();
                    ComputeTotals();
                }
            }
        }

        private void BtnToolGeneratePECode_Click(object sender, RoutedEventArgs e)
        {
            if (txtToolPECode.Text == "")
            {
                string tempID = "";
                while (true)
                {
                    using (var context2 = new Models.DatabaseContext())
                    {
                        tempID = "PE-" + RandomNumbers(4);
                        var checkID = context2.Tools.FirstOrDefault(br => br.PECode == tempID && br.ProductCode == txtToolItemCode.Text);
                        if (checkID == null)
                        {
                            txtToolPECode.Text = tempID;
                            break;
                        }
                    }
                }
            }
        }

        private void TableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgSparepartsStockIn.SelectedItem as DeliveryItemSparePart;
            if (selectedItem != null)
            {
                if (selectedItem.Qty == "0" || int.Parse(selectedItem.Qty.ToString()) <= 0)
                {
                    MessageBox.Show("Quantity cannot be zero!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                    selectedItem.Qty = "1";
                }
                selectedItem.TotalCost = (float.Parse(selectedItem.Cost.ToString()) * float.Parse(selectedItem.Qty.ToString())).ToString();
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure you want to cancel this delivery?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                CancelDelivery();
            }
        }

        private void BtnFinish_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure you want to add this delivery?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.No)
            {
                bsyPanel.Visibility = Visibility.Collapsed;
                return;
            }

            bsyPanel.Visibility = Visibility.Visible;
            if (consumableDeliveryList.Count() <= 0 && spareDeliveryList.Count() <= 0 && toolDeliveryList.Count() <= 0 && assetDeliveryList.Count() <= 0)
            {
                MessageBox.Show("Delivery contains no item!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                bsyPanel.Visibility = Visibility.Collapsed;
                return;
            }

            if (dtDeliveryDate.DisplayDate == null || dtDeliveryDate.SelectedDate == null)
            {
                MessageBox.Show("Please set date of delivery!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                bsyPanel.Visibility = Visibility.Collapsed;
                return;
            }

            try
            {
                DateTime.Parse(dtDeliveryDate.SelectedDate.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Please set date of delivery!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                bsyPanel.Visibility = Visibility.Collapsed;
                return;

            }

            Delivery delivery = new Delivery();
            delivery.DeliveryID = txtDeliveryID.Text;
            delivery.DRNumber = "N/A";
            delivery.DeliveryDate = DateTime.Parse(dtDeliveryDate.SelectedDate.ToString());
            delivery.Date = DateTime.Now;
            delivery.PONumber = "N/A";
            delivery.TotalItem = (int.Parse(txtToolTotal.Text) + int.Parse(txtSparePartTotal.Text) + int.Parse(txtConsumableTotal.Text) + int.Parse(txtAssetTotal.Text)).ToString();
            delivery.ConsumableTotal = int.Parse(txtConsumableTotal.Text);
            delivery.ToolTotal = int.Parse(txtToolTotal.Text);
            delivery.SpareTotal = int.Parse(txtSparePartTotal.Text);
            delivery.AssetTotal = int.Parse(txtAssetTotal.Text);
            using (var context = new DatabaseContext())
            {
                context.Deliveries.Add(delivery);
                context.SaveChanges();
            }

            #region Updating Consumable
            if (consumableDeliveryList.Count() > 0)
            {
                foreach (var item in consumableDeliveryList)
                {
                    DeliveryItems newDeliveryItems = new DeliveryItems();
                    newDeliveryItems.DeliveryItemID = item.DeliveryItemID;
                    newDeliveryItems.DeliveryID = item.DeliveryID;
                    newDeliveryItems.DRNumber = "N/A";
                    newDeliveryItems.ItemCode = item.ItemCode;
                    newDeliveryItems.ItemName = item.ItemName;
                    newDeliveryItems.UOM = item.UOM;
                    newDeliveryItems.Quantity = item.Quantity;
                    newDeliveryItems.Date = DateTime.Now;

                    var itemRef = new ItemMst();
                    using (var itemContext = new DatabaseContext())
                    {
                        itemRef = itemContext.ItemMsts.FirstOrDefault(br => br.item_cd == item.ItemCode);
                    }
                    using (var context = new DatabaseContext())
                    {
                        context.DeliveriesItem.Add(newDeliveryItems);
                        context.SaveChanges();
                    }
                    using (var context = new DatabaseContext())
                    {
                        var check = context.Consumables.FirstOrDefault(br => br.ItemCode == item.ItemCode);
                        if (check != null)
                        {
                            check.RemainingQuantity = check.RemainingQuantity + item.Quantity;
                            context.SaveChanges();
                        }
                        else
                        {
                            Consumable newConsumable = new Consumable();
                            newConsumable.ItemCode = item.ItemCode;
                            newConsumable.Group = "Unassigned";
                            newConsumable.ItemDescription = itemRef.description;
                            newConsumable.ItemName = itemRef.description;
                            newConsumable.UOM = itemRef.uom;
                            newConsumable.RemainingQuantity = item.Quantity;
                            newConsumable.MaintainingQuantity = 0;
                            newConsumable.DateAdd = DateTime.Now;
                            newConsumable.ProductCode = item.ItemCode;
                            context.Consumables.Add(newConsumable);
                            context.SaveChanges();
                        }
                    }
                }
            }
            #endregion

            #region UpdatingTool
            if (toolDeliveryList.Count() > 0)
            {
                foreach (var item in toolDeliveryList)
                {
                    DeliveryToolItems deliveryTool = new DeliveryToolItems();
                    deliveryTool.DeliveryItemID = item.DeliveryItemID;
                    deliveryTool.DRNumber = "N/A";
                    deliveryTool.DeliveryID = item.DeliveryID;
                    deliveryTool.ItemCode = item.ItemCode;
                    deliveryTool.ItemName = item.ItemName;
                    deliveryTool.ItemDescription = item.ItemDescription;
                    deliveryTool.Brand = item.Brand;
                    deliveryTool.PECode = item.PECode;
                    deliveryTool.UnitCost = item.UnitCost;
                    using (var context = new DatabaseContext())
                    {
                        context.DeliveriesToolItem.Add(deliveryTool);
                        context.SaveChanges();
                    }
                    string tempID = "";
                    while (true)
                    {
                        using (var context2 = new Models.DatabaseContext())
                        {
                            tempID = "PE-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                            var checkID = context2.Tools.FirstOrDefault(br => br.ItemCode == tempID);
                            if (checkID == null)
                            {
                                break;
                            }
                        }
                    }
                    Tool newTool = new Tool();
                    newTool.ItemCode = tempID;
                    newTool.Type = "";
                    newTool.Item = item.ItemName;
                    newTool.ItemDescription = item.ItemDescription;
                    newTool.Brand = item.Brand;
                    newTool.Condition = "GOOD";
                    newTool.Remarks = "N/A";
                    newTool.Status = "In-Stock";
                    newTool.PECode = item.PECode;
                    newTool.DateDelivered = item.Date;
                    newTool.LastUpdate = DateTime.Now;
                    newTool.UnitCost = item.UnitCost;
                    newTool.ProductCode = item.ItemCode;
                    using (var context = new DatabaseContext())
                    {
                        context.Tools.Add(newTool);
                        context.SaveChanges();
                    }
                }
            }
            #endregion

            #region Updating Spare Part

            if (spareDeliveryList.Count() > 0)
            {
                foreach (var item in spareDeliveryList)
                {
                    DeliveryItemSparePart deliveryItemSparePart = new DeliveryItemSparePart();
                    deliveryItemSparePart.DeliveryItemID = item.DeliveryItemID;
                    deliveryItemSparePart.DeliveryID = item.DeliveryID;
                    deliveryItemSparePart.ItemName = item.ItemName;
                    deliveryItemSparePart.Cost = item.Cost;
                    deliveryItemSparePart.Qty = item.Qty;
                    deliveryItemSparePart.TotalCost = item.TotalCost;
                    deliveryItemSparePart.ItemCode = item.ItemCode;
                    using (var context = new DatabaseContext())
                    {
                        context.DeliveryItemSpareParts.Add(deliveryItemSparePart);
                        context.SaveChanges();
                    }

                    var itemRef = new ItemMst();
                    using (var itemContext = new DatabaseContext())
                    {
                        itemRef = itemContext.ItemMsts.FirstOrDefault(br => br.item_cd == item.ItemCode);
                    }

                    string tempID = "";
                    while (true)
                    {
                        using (var context2 = new Models.DatabaseContext())
                        {
                            tempID = "SPR-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                            var checkID = context2.SpareParts.FirstOrDefault(br => br.PartID == tempID);
                            if (checkID == null)
                            {
                                break;
                            }
                        }
                    }


                    using (var context = new DatabaseContext())
                    {
                        var check = context.SpareParts.FirstOrDefault(br => br.ItemCode == item.ItemCode);
                        if (check != null)
                        {
                            check.AvailableQuantity = check.AvailableQuantity + int.Parse(item.Qty);
                            context.SaveChanges();
                        }
                        else
                        {
                            SparePart newSparePart = new SparePart();
                            newSparePart.PartID = tempID;
                            newSparePart.Location = "UNASSIGNED";
                            newSparePart.Type = "OTHERS";
                            newSparePart.ItemCode = item.ItemCode;
                            newSparePart.ItemName = item.ItemName;
                            newSparePart.Cost = float.Parse(item.Cost);
                            newSparePart.TotalCost = float.Parse(item.TotalCost);
                            newSparePart.MaintainingQuantity = 0;
                            newSparePart.AvailableQuantity = int.Parse(item.Qty);
                            context.SpareParts.Add(newSparePart);
                            context.SaveChanges();
                        }
                    }



                }
            }
            #endregion 
            #region Updating Asset
            if (assetDeliveryList.Count() > 0)
            {
                foreach (var item in assetDeliveryList)
                {
                    DeliveryAsset newDeliveryItems = new DeliveryAsset();
                    newDeliveryItems.DeliveryItemID = item.DeliveryItemID;
                    newDeliveryItems.DeliveryID = item.DeliveryID;
                    newDeliveryItems.ItemCode = item.ItemCode;
                    newDeliveryItems.ItemName = item.ItemName;
                    newDeliveryItems.UOM = item.UOM;
                    newDeliveryItems.Quantity = item.Quantity;
                    newDeliveryItems.Date = DateTime.Now;
                    newDeliveryItems.RtpNo = item.RtpNo;
                    newDeliveryItems.SerialNo = item.SerialNo;
                    newDeliveryItems.UnitCost = item.UnitCost;
                    newDeliveryItems.TotalCost = item.TotalCost;

                    var itemRef = new ItemMst();
                    using (var itemContext = new DatabaseContext())
                    {
                        itemRef = itemContext.ItemMsts.FirstOrDefault(br => br.item_cd == item.ItemCode);
                    }
                    using (var context = new DatabaseContext())
                    {
                        context.DeliveryAssets.Add(newDeliveryItems);
                        context.SaveChanges();
                    }
                    using (var context = new DatabaseContext())
                    {
                        var check = context.AssetAndEquipments.FirstOrDefault(br => br.ItemCode == item.ItemCode);
                        if (check != null && item.SerialNo == "")
                        {
                            check.Qty_Available = check.Qty_Available + item.Quantity;
                            check.TotalCost = check.Qty_Available * item.UnitCost;
                            context.SaveChanges();
                        }
                        else
                        {
                            string tempID = "";
                            while (true)
                            {
                                using (var context2 = new Models.DatabaseContext())
                                {
                                    tempID = "ASST" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                                    var checkID = context2.AssetAndEquipments.FirstOrDefault(br => br.AssetID == tempID);
                                    if (checkID == null)
                                    {
                                        break;
                                    }
                                }
                            }
                            AssetAndEquipment newAsset = new AssetAndEquipment();
                            newAsset.AssetID = tempID;
                            newAsset.RTPNo = item.RtpNo;
                            newAsset.LineName = "";
                            newAsset.ItemCode = item.ItemCode;
                            newAsset.ItemDescription = item.ItemName;
                            newAsset.UOM = item.UOM;
                            newAsset.SerialNo = item.SerialNo;
                            newAsset.AssetNo = "";
                            newAsset.Qty_Available = item.Quantity;
                            newAsset.Qty_Reserved = 0;
                            newAsset.MaintainingQty = 0;
                            newAsset.Condition = "GOOD";
                            newAsset.UnitCost = item.UnitCost;
                            newAsset.TotalCost = item.TotalCost;
                            context.AssetAndEquipments.Add(newAsset);
                            context.SaveChanges();
                            
                        }
                    }
                }
            }
            #endregion

            bsyPanel.Visibility = Visibility.Collapsed;
            DXMessageBox.Show("Delivery Added!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
            CancelDelivery();
        }

        private void CancelDelivery()
        {
            txtDeliveryID.Text = GenerateID();
            dtDeliveryDate.SelectedDate = null;
            txtConsumableTotal.Text = "0";
            txtToolTotal.Text = "0";
            txtSparePartTotal.Text = "0";
            spareDeliveryList.Clear();
            toolDeliveryList.Clear();
            consumableDeliveryList.Clear();
            assetDeliveryList.Clear();
            LoadDeliveryItems();
            ClearAsset();
            ClearSpare();
            ClearConsumable();
            ClearToolFields();
            txtAssetTotal.Text = "0";

        }

        private void CmbAssetItemCode_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            using (var context = new DatabaseContext())
            {
                var item = context.ItemMsts.FirstOrDefault(br => br.item_cd == cmbAssetItemCode.Text);
                if (item != null)
                {
                    txtAssetItemName.Text = item.description;
                    txtAssetUOM.Text = item.uom;
                }
            }
        }

        private void BtnAssetAdd_Click(object sender, RoutedEventArgs e)
        {
            if (txtAssetQty.Text == "" || txtAssetQty.Text == "0" || int.Parse(txtAssetQty.Text) <= 0)
            {
                DXMessageBox.Show("Quantity cannot be 0!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (txtAssetUnitCost.Text == "" || float.Parse(txtAssetUnitCost.Text) < 0)
            {
                DXMessageBox.Show("Unitcost can not be less than zero or empty!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //if (txtAssetRTP.Text == "" || txtAssetSerialNo.Text == "")
            //{
            //    DXMessageBox.Show("Please complete all information!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            string tempID = "";
            while (true)
            {
                using (var context2 = new Models.DatabaseContext())
                {
                    tempID = "WHS-DLR-ITM" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                    var checkID = context2.DeliveriesItem.FirstOrDefault(br => br.DeliveryItemID == tempID);
                    if (checkID == null)
                    {
                        break;
                    }
                }
            }

            DeliveryAsset deliveryItems = new DeliveryAsset();
            deliveryItems.DeliveryItemID = tempID;
            deliveryItems.ItemCode = cmbAssetItemCode.Text;
            deliveryItems.ItemName = txtAssetItemName.Text;
            deliveryItems.UOM = txtAssetUOM.Text;
            deliveryItems.Quantity = int.Parse(txtAssetQty.Text);
            deliveryItems.DeliveryID = txtDeliveryID.Text;
            deliveryItems.Date = DateTime.Now;
            deliveryItems.UnitCost = float.Parse(txtAssetUnitCost.Text);
            deliveryItems.TotalCost = int.Parse(txtAssetQty.Text) * float.Parse(txtAssetUnitCost.Text);
            deliveryItems.RtpNo = txtAssetRTP.Text.Trim();
            deliveryItems.SerialNo = txtAssetSerialNo.Text.Trim();
            assetDeliveryList.Add(deliveryItems);
            LoadDeliveryItems();
            ClearAsset();
            ComputeTotals();
        }

        private void BtnAssetDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgAssetStockIn.SelectedItem as DeliveryAsset;
            if (selectedItem != null)
            {
                var toDelete = assetDeliveryList.FirstOrDefault(br => br.DeliveryItemID == selectedItem.DeliveryItemID);
                if (toDelete != null)
                {
                    assetDeliveryList.Remove(toDelete);
                    LoadDeliveryItems();
                    ComputeTotals();
                }
            }
        }

        private void DgAssetTableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgAssetStockIn.SelectedItem as DeliveryAsset;
            if (selectedItem != null)
            {
                var asset = assetDeliveryList.FirstOrDefault(br => br.DeliveryItemID == selectedItem.DeliveryItemID);
                if (asset != null)
                {
                    if (selectedItem.Quantity == null || selectedItem.Quantity == 0 || selectedItem.Quantity <= 0)
                    {
                        MessageBox.Show("Quantity cannot be 0!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                        selectedItem.Quantity = 1;
                        //return;
                    }

                    if (selectedItem.UnitCost == null || selectedItem.UnitCost < 0)
                    {
                        MessageBox.Show("Unitcost can not be less than zero or empty!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                        selectedItem.UnitCost = 0;
                        //return;
                    }

                    asset.Quantity = selectedItem.Quantity;
                    asset.TotalCost = selectedItem.Quantity * selectedItem.UnitCost;
                    asset.UnitCost = selectedItem.UnitCost;
                }
            }
        }
    }
}
