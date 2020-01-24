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
        
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            using (var context = new DatabaseContext())
            {
                var consumables = context.Consumables;
                if (consumables != null)
                {
                    dgConsumables.ItemsSource = consumables.ToList();
                }
            }


            using (var context = new DatabaseContext())
            {
                var tools = context.Tools;
                if (tools != null)
                {
                    dgTool.ItemsSource = tools.ToList();
                }
            }

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

        private void DgConsumables_SelectedItemChanged(object sender, DevExpress.Xpf.Grid.SelectedItemChangedEventArgs e)
        {
            //var selectedItem = dgConsumables.SelectedItem as Consumable;
            //using (var context = new DatabaseContext())
            //{
            //    var data = context.Consumables.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
            //    if (data != null)
            //    {
            //        txtItemCode.Text = data.ItemCode;
            //        cmbGroup.Text = data.Group;
            //        txtItemName.Text = data.ItemName;
            //        txtItemDescription.Text = data.ItemDescription;
            //        cmbUOM.Text = data.UOM;
            //        txtRemaining.Text = data.RemainingQuantity.ToString();
            //        txtMaintaining.Text = data.MaintainingQuantity.ToString();

            //    }
            //}
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
            else if (editMode == false && addMode == true)
            {
                using (var context = new DatabaseContext())
                {
                    var tool = context.Tools.FirstOrDefault(br => br.ItemCode == txtToolName.Text  && br.PECode == txtToolPECode.Text );
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
                if (MessageBox.Show("Are you sure you want to delete this item?","Inventory System",MessageBoxButton.YesNo,MessageBoxImage.Question) == MessageBoxResult.Yes)
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
    }
}
