using DevExpress.Xpf.Core;
using EngineeringToolsEquipmentsInventory.Models;
using EngineeringToolsEquipmentsInventory.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.XtraGrid;
using DevExpress.Xpf.Printing;
using DevExpress.Xpf.Grid;
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
using EngineeringToolsEquipmentsInventory.Reports;
using DevExpress.XtraPrinting;

namespace EngineeringToolsEquipmentsInventory.Views.InventoryManagement
{
    /// <summary>
    /// Interaction logic for IMInventoryView.xaml
    /// </summary>
    public partial class IMInventoryView : UserControl
    {
        public IMInventoryView()
        {
            InitializeComponent();
        }

        bool editMode = false;
        bool addMode = true;

        System.Windows.Forms.BindingSource consumableBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource toolBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource spareBinding = new System.Windows.Forms.BindingSource();
        System.Windows.Forms.BindingSource assetBinding = new System.Windows.Forms.BindingSource();
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            dgConsumables.ItemsSource = consumableBinding;
            dgTool.ItemsSource = toolBinding;
            dgSpareParts.ItemsSource = spareBinding;
            dgAssets.ItemsSource = assetBinding;
            RefreshGRid();


            using (var context = new DatabaseContext())
            {
                var groups = context.ProductGroups;
                if (groups.Count() >= 1)
                {
                    foreach (var item in groups)
                    {
                        cmbGroup.Items.Add(item.GroupName);
                    }
                }
            }

            using (var context = new DatabaseContext())
            {
                var tooltypes = context.ToolTypes;
                if (tooltypes != null)
                {
                    foreach (var item in tooltypes)
                    {
                        cmbToolType.Items.Add(item.ToolType);
                    }
                }
            }

             

        }
        private void RefreshGRid()
        {
            using (var context = new DatabaseContext())
            {
                var consumables = context.Consumables;
                if (consumables != null)
                {
                    consumableBinding.DataSource = consumables.ToList();
                    dgConsumables.RefreshData();
                }
            }


            using (var context = new DatabaseContext())
            {
                var tools = context.Tools;
                if (tools != null)
                {
                    toolBinding.DataSource = tools.ToList();
                    dgTool.RefreshData();
                }
            }

            using (var context = new DatabaseContext())
            {
                var spare = context.SpareParts;
                if (spare != null)
                {
                    spareBinding.DataSource = spare.ToList();
                    dgSpareParts.RefreshData();
                }
            }

            using (var context = new DatabaseContext())
            {
                var assets = context.AssetAndEquipments;
                if (assets != null)
                {
                    assetBinding.DataSource = assets.ToList();
                    dgAssets.RefreshData();
                }
            }

        }

        private void DgConsumables_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                var selectedItem = dgConsumables.SelectedItem as Consumable;
                using (var context = new DatabaseContext())
                {
                    var data = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                    if (data != null)
                    {
                        txtItemCode.Text = data.ItemCode;
                        cmbGroup.Text = data.Group;
                        txtItemName.Text = data.ItemName;
                        txtItemDescription.Text = data.ItemDescription;
                        cmbUOM.Text = data.UOM;
                        txtRemaining.Text = data.RemainingQuantity.ToString();
                        txtMaintaining.Text = data.MaintainingQuantity.ToString();

                    }
                }
            }
            catch (Exception)
            {

            }
        }

        private void DXTabControl_SelectionChanged(object sender, DevExpress.Xpf.Core.TabControlSelectionChangedEventArgs e)
        {

        }

        private void BtnAddStock_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
            txtRemaining.Text = "0";
        }

        private void ClearFields()
        {
            txtItemCode.Text = "";
            cmbGroup.Text = "";
            txtItemName.Text = "";
            txtItemDescription.Text = "";
            cmbUOM.Text = "";
            txtRemaining.Text = "";
            txtMaintaining.Text = "";
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string deliveryID = "";
            while (true)
            {
                deliveryID = "WHS-CSMBL" + DateTime.Today.ToString("ddMMyy") + RandomString(4);
                using (var context = new DatabaseContext())
                {
                    var check = context.Deliveries.FirstOrDefault(br => br.DeliveryID == deliveryID);
                    if (check == null)
                    {
                        txtItemCode.Text = deliveryID;
                        break;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtItemCode.Text == "" || cmbGroup.Text == "" || txtItemName.Text == "" || txtItemDescription.Text == "" || cmbUOM.Text == "" || txtMaintaining.Text == "")
            {
                MessageBox.Show("Incmplete Information!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                using (var context = new DatabaseContext())
                {
                    Consumable consumable = new Consumable();
                    consumable.ItemCode = txtItemCode.Text;
                    consumable.Group = cmbGroup.Text;
                    consumable.ItemName = txtItemName.Text;
                    consumable.ItemDescription = txtItemDescription.Text;
                    consumable.UOM = cmbUOM.Text;
                    consumable.RemainingQuantity = Convert.ToInt32(txtRemaining.Text);
                    consumable.MaintainingQuantity = Convert.ToInt32(txtMaintaining.Text);
                    context.Consumables.Add(consumable);
                    context.SaveChanges();
                    MessageBox.Show("Stock added!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                    ClearFields();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnToolGenerate_Click(object sender, RoutedEventArgs e)
        {
            string deliveryID = "";
            while (true)
            {
                deliveryID = "PE-" + DateTime.Today.ToString("ddMMyy") + RandomString(6);
                using (var context = new DatabaseContext())
                {
                    var check = context.Tools.FirstOrDefault(br => br.ItemCode == deliveryID);
                    if (check == null)
                    {
                        txtToolItemCode.Text = deliveryID;
                        break;
                    }
                }
            }
        }

        private void DgTool_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                var selectedItems = dgTool.SelectedItem as Tool;
                if (selectedItems != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var tool = context.Tools.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                        txtToolItemCode.Text = tool.ItemCode;
                        cmbToolType.Text = tool.Type;
                        txtToolName.Text = tool.Item;
                        txtToolItemDescription.Text = tool.ItemDescription;
                        txtToolBrand.Text = tool.Brand;
                        txtToolPECode.Text = tool.PECode;
                        txtToolUnitCost.Text = tool.UnitCost.ToString();
                    }
                }
            }
            catch (Exception)
            {

            }




        }

        private void BtnToolEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgTool.SelectedItem as Tool;

            if (selectedItems != null)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.Tools.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                    if (tool != null)
                    {
                        editMode = true;
                        addMode = false;
                        txtToolItemCode.Text = tool.ItemCode;
                        cmbToolType.Text = tool.Type;
                        txtToolItemDescription.Text = tool.ItemDescription;
                        txtToolBrand.Text = tool.Brand;
                        txtToolPECode.Text = tool.PECode;
                        txtToolName.Text = tool.Item;
                    }
                }
            }
        }

        private void BtnToolAddstock_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }

        private void ToolClearFields()
        {
            cmbToolType.Text = "";
            txtToolName.Text = "";
            txtToolItemDescription.Text = "";
            txtToolBrand.Text = "";
            txtToolPECode.Text = "";
            txtToolItemCode.Text = "";
        }

        private void BtnToolSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtToolItemCode.Text == "" || cmbToolType.Text == "" || txtToolName.Text == "" || txtToolItemDescription.Text == "" || txtToolBrand.Text == "" || txtToolPECode.Text == "")
            {
                MessageBox.Show("Incomplete details!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (editMode == true && addMode == false)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.Tools.FirstOrDefault(br => br.ItemCode == txtToolItemCode.Text);
                    if (tool != null)
                    {
                        using (var checkCtx = new DatabaseContext())
                        {
                            var check = checkCtx.Tools.FirstOrDefault(br => br.Item == txtToolName.Text && br.PECode == txtToolPECode.Text);
                            if (check != null)
                            {
                                if (check.ItemCode != txtToolItemCode.Text)
                                {
                                    MessageBox.Show("Duplicate PE COde", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }
                            }
                        }
                        tool.Type = cmbToolType.Text;
                        tool.Item = txtToolName.Text;
                        tool.ItemDescription = txtToolItemDescription.Text;
                        tool.Brand = txtToolBrand.Text;
                        tool.PECode = txtToolPECode.Text;
                        context.SaveChanges();
                        MessageBox.Show("Tool Updated!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                        ToolClearFields();
                        //using (var refreshCTX = new DatabaseContext())
                        //{
                        //    var tools = refreshCTX.Tools;
                        //    if (tools != null)
                        //    {
                        //        dgTool.ItemsSource = tools.ToList();
                        //    }
                        //}
                        RefreshGRid();
                        editMode = false;
                        addMode = true;
                    }
                }
            }
            else if (editMode == false && addMode == true)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.Tools.FirstOrDefault(br => br.ItemCode == txtToolName.Text && br.PECode == txtToolPECode.Text);
                    if (tool != null)
                    {
                        MessageBox.Show("Duplicate PE COde", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    Tool newTool = new Tool();
                    newTool.ItemCode = txtToolItemCode.Text;
                    newTool.Type = cmbToolType.Text;
                    newTool.Item = txtToolName.Text;
                    newTool.ItemDescription = txtToolItemDescription.Text;
                    newTool.Brand = txtToolBrand.Text;
                    newTool.PECode = txtToolPECode.Text;
                    newTool.Condition = "GOOD";
                    newTool.Remarks = "N/A";
                    newTool.Status = "In-Stock";
                    newTool.DateDelivered = DateTime.Now;
                    newTool.LastUpdate = DateTime.Now;
                    context.Tools.Add(newTool);
                    context.SaveChanges();
                    MessageBox.Show("Tool Added!", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                    ToolClearFields();
                    using (var refreshCTX = new DatabaseContext())
                    {
                        var tools = refreshCTX.Tools;
                        if (tools != null)
                        {
                            dgTool.ItemsSource = tools.ToList();
                        }
                    }
                    editMode = false;
                    addMode = true;
                }
            }
        }

        private void BtnRemoveConsumable_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgConsumables.SelectedItem as Consumable;
            if (selectedItems != null)
            {
                if (MessageBox.Show("Are you sure you want to delete this item?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    using (var context = new DatabaseContext())
                    {
                        var toDelete = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                        if (toDelete != null)
                        {
                            context.Consumables.Remove(toDelete);
                            context.SaveChanges();
                            MessageBox.Show("Item Deleted", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                            using (var RefreshContext = new DatabaseContext())
                            {
                                var consumables = RefreshContext.Consumables;
                                if (consumables != null)
                                {
                                    dgConsumables.ItemsSource = consumables.ToList();
                                }
                            }
                        }

                    }
                }
            }
        }



        private void DgConsumableTableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgConsumables.SelectedItem as Consumable;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var consumableItem = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                    if (consumableItem != null)
                    {
                        consumableItem.Group = selectedItem.Group;
                        consumableItem.ItemDescription = selectedItem.ItemDescription;
                        consumableItem.ItemName = selectedItem.ItemName;
                        consumableItem.UOM = selectedItem.UOM;
                        consumableItem.MaintainingQuantity = selectedItem.MaintainingQuantity;
                        context.SaveChanges();
                        RefreshGRid();

                    }
                }
                dgConsumableTableView.AllowEditing = false;
                dgColumnItemCode.AllowEditing = DevExpress.Utils.DefaultBoolean.False;
            }
        }

        private void DgConsumableTableView_AddingNewRow(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            AddNewConsumable addNewConsumable = new AddNewConsumable();
            addNewConsumable.ShowDialog();

            if (addNewConsumable.DialogResult ==  true)
            {
                RefreshGRid();
            }
        }

        private void BtnToolDelete_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this item?", "Inventory Sytem", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedItem = dgTool.SelectedItem as Tool;
                if (selectedItem != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var tool = context.Tools.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                        if (tool != null)
                        {
                            context.Tools.Remove(tool);
                            context.SaveChanges();
                            RefreshGRid();
                        }
                    }
                }
            }
        }

        private void DgToolTableView_AddingNewRow(object sender, System.ComponentModel.AddingNewEventArgs e)
        {
            AddToolWindow addToolWindow = new AddToolWindow();
            addToolWindow.ShowDialog();
            RefreshGRid();
        }

        private void DgToolTableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgTool.SelectedItem as Tool;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.Tools.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                    if (tool != null)
                    {
                        tool.ItemDescription = selectedItem.ItemDescription;
                        tool.Remarks = selectedItem.Remarks;
                        tool.Status = selectedItem.Status;
                        tool.Type = selectedItem.Type;
                        tool.Brand = selectedItem.Brand;
                        tool.DateDelivered = selectedItem.DateDelivered;
                        tool.PECode = selectedItem.PECode;
                        tool.Item = selectedItem.Item;
                        tool.Condition = selectedItem.Condition;
                        tool.UnitCost = selectedItem.UnitCost;
                        context.SaveChanges();
                        RefreshGRid();
                    }
                }
            }
        }

        private void DgSpareParts_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                var selectedItem = dgSpareParts.SelectedItem as SparePart;
                if (selectedItem != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var sparePart = context.SpareParts.FirstOrDefault(br => br.PartID == selectedItem.PartID);
                        if (sparePart != null)
                        {

                            txtSparePartID.Text = sparePart.PartID;
                            txtSpareLocation.Text = sparePart.Location;
                            txtSpareType.Text = sparePart.Type;
                            txtSpareItemCode.Text = sparePart.ItemCode;
                            txtSpareItemName.Text = sparePart.ItemName;
                            txtSpareMaintaining.Text = sparePart.MaintainingQuantity.ToString();
                            txtSpareAvailableQty.Text = sparePart.AvailableQuantity.ToString();
                            txtSpareUnitCost.Text = sparePart.Cost.ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
                
            }

        }

        private void BtnSparePartDelete_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure you want to delete this item?","Inventory System",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var selectedItem = dgSpareParts.SelectedItem as SparePart;
                if (selectedItem != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var delete = context.SpareParts.FirstOrDefault(br => br.PartID == selectedItem.PartID);
                        if (delete != null)
                        {
                            context.SpareParts.Remove(delete);
                            context.SaveChanges();
                            RefreshGRid();
                        }
                    }
                } 
            }
        }

        private void DgSparePartsTableView_CellValueChanged(object sender, DevExpress.Xpf.Grid.CellValueChangedEventArgs e)
        {
            var selectedItem = dgSpareParts.SelectedItem as SparePart;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var update = context.SpareParts.FirstOrDefault(br => br.PartID == selectedItem.PartID);
                    if (update != null)
                    {
                        update.Location = selectedItem.Location;
                        update.Type = selectedItem.Type;
                        update.ItemName = selectedItem.ItemName;
                        update.Cost = selectedItem.Cost;
                        update.TotalCost = selectedItem.Cost * selectedItem.AvailableQuantity;
                        update.MaintainingQuantity = selectedItem.MaintainingQuantity;
                        context.SaveChanges();
                        RefreshGRid();
                    }
                } 
            }
        }

        private void BtnViewHistory_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgConsumables.SelectedItem as Consumable;
            ConsumableDeliveryListWindow consumableDeliveryListWindow = new ConsumableDeliveryListWindow();
            StockInHistorySession.ItemCode = selectedItem.ProductCode;
            consumableDeliveryListWindow.Show();

            if (consumableDeliveryListWindow.DialogResult == true || false)
            {
                StockInHistorySession.ItemCode = "";
                StockInHistorySession.SpareItemCode = "";
                RefreshGRid();
            }

        }

        private void BtnSpareDelivery_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgSpareParts.SelectedItem as SparePart;
            ConsumableDeliveryListWindow consumableDeliveryListWindow = new ConsumableDeliveryListWindow();
            StockInHistorySession.SpareItemCode = selectedItem.ItemCode;
            consumableDeliveryListWindow.Show();

            if (consumableDeliveryListWindow.DialogResult == true || false)
            {
                StockInHistorySession.ItemCode = "";
                StockInHistorySession.SpareItemCode = "";
                RefreshGRid();
            }
        }

        private void BtnRemoveAsset_Click(object sender, RoutedEventArgs e)
        {
            if (DXMessageBox.Show("Are you sure you want to delete this record?","InventorySystem",MessageBoxButton.YesNo,MessageBoxImage.Question) ==  MessageBoxResult.Yes)
            {
                var selectedItem = dgAssets.SelectedItem as AssetAndEquipment;
                if (selectedItem != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var delete = context.AssetAndEquipments.FirstOrDefault(br => br.AssetID == selectedItem.AssetID);
                        if (delete != null)
                        {
                            context.AssetAndEquipments.Remove(delete);
                            context.SaveChanges();
                            RefreshGRid();
                        }
                    }
                } 
            }
        }

        private void BtnAddAsset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click_1(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSaveAsset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAssetGenerate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DgAssets_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            try
            {
                var selectedItem = dgAssets.SelectedItem as AssetAndEquipment;
                if (selectedItem != null)
                {
                    txtAssetItemCode.Text = selectedItem.ItemCode;
                    txtAssetItemName.Text = selectedItem.ItemDescription;
                    txtAssetSerialNo.Text = selectedItem.SerialNo;
                    txtAssetRtpNo.Text = selectedItem.RTPNo;
                    txtAssetItemDescription.Text = selectedItem.ItemDescription;
                    cmbAssetUOM.Text = selectedItem.UOM;
                    txtAssetRemaining.Text = selectedItem.Qty_Available.ToString();
                    txtAssetMaintaining.Text = selectedItem.MaintainingQty.ToString();
                }
            }
            catch (Exception)
            {

                //throw;
            }
        }

        private void BtnConsumablePrint_Click(object sender, RoutedEventArgs e)
        {
            dgConsumables.View.ShowPrintPreviewDialog(Application.Current.Windows[0]);
        }

        private void BtnToolsPrint_Click(object sender, RoutedEventArgs e)
        {


            PrintingSystemBase printingSystemBase = new PrintingSystemBase();
            printingSystemBase.PageSettings.Landscape = true;
            
            dgTool.View.ShowPrintPreview(Application.Current.Windows[0]); 
        }

        

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            dgSpareParts.View.ShowPrintPreviewDialog(Application.Current.Windows[0]);
        }

        private void BtnAssetPrint_Click(object sender, RoutedEventArgs e)
        {
            dgAssets.View.ShowPrintPreviewDialog(Application.Current.Windows[0]);
        }

        private void DgAssetTableView_CellValueChanged(object sender, CellValueChangedEventArgs e)
        {
            var selectedItem=  dgAssets.SelectedItem as AssetAndEquipment;
            if (selectedItem != null)
            {
                using (var context = new DatabaseContext())
                {
                    var asset = context.AssetAndEquipments.FirstOrDefault(br => br.AssetID == selectedItem.AssetID);
                    if (asset != null)
                    {
                        asset.RTPNo = selectedItem.RTPNo;
                        asset.LineName = selectedItem.LineName;
                        asset.ItemDescription = selectedItem.ItemDescription;
                        asset.UOM = selectedItem.UOM;
                        asset.SerialNo = selectedItem.SerialNo;
                        asset.AssetNo = selectedItem.AssetNo;
                        asset.MaintainingQty = selectedItem.MaintainingQty;
                        asset.Condition = selectedItem.Condition;
                        asset.UnitCost = selectedItem.UnitCost;
                        asset.TotalCost = selectedItem.UnitCost * selectedItem.Qty_Available;
                        context.SaveChanges();
                        RefreshGRid();

                    }
                }
            }

        }

        private void BtnConsumableAssetTag_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void BtnToolAssetTag_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgTool.SelectedItem as Tool;
            if (selectedItem != null)
            {
                AssetTag tag = new AssetTag(); 
                using (var context = new DatabaseContext())
                {
                    var item = context.Tools.Where(br => br.ItemCode == selectedItem.ItemCode);
                    if (item != null)
                    {
                        tag.DataSource = item.ToList();
                        PrintHelper.ShowPrintPreview(null, tag, "Asset Tag - " + selectedItem.Item);
                    }
                } 
            }
        }

        private void BtnAssetTagAssets_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgAssets.SelectedItem as AssetAndEquipment;
            if (selectedItem != null)
            {
                if (selectedItem.SerialNo == "" || selectedItem.SerialNo == null)
                {
                    DXMessageBox.Show("Item can not be tagged", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }


                AssetTagEquipment tag = new AssetTagEquipment();
                using (var context = new DatabaseContext())
                {
                    var item = context.AssetAndEquipments.Where(br => br.AssetID == selectedItem.AssetID);
                    if (item != null)
                    {
                        tag.DataSource = item.ToList();
                        PrintHelper.ShowPrintPreview(null, tag,"Asset Tag - "+selectedItem.ItemDescription);
                    }
                }

            }
        }
    }
}
