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
    /// Interaction logic for AddNewConsumable.xaml
    /// </summary>
    public partial class AddNewConsumable : Window
    {
        public AddNewConsumable()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtItemCode.Text = GenerateID();
            txtMaintainingQty.IsReadOnly = false;
            txtRemainingQty.IsReadOnly = true;
            txtMaintainingQty.Text = "0";
            txtRemainingQty.Text = "0";

        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private static string GenerateID()
        {
            string tempID = "";
            while (true)
            {
                using (var context2 = new DatabaseContext())
                {
                    tempID = "CNSMBL-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                    var checkID = context2.Jigs.FirstOrDefault(br => br.ItemCode == tempID);
                    if (checkID == null)
                    {
                        return tempID;
                    }
                }
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtItemCode.Text == "" || txtDescription.Text == "" || txtMaintainingQty.Text == ""
                || txtMaintainingQty.Text == null || txtItemName.Text == "" || cmbUOM.Text == ""
                || cmbGroup.Text == "" || txtRemainingQty.Text == "" || txtProductCode.Text == "")
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Please complete all information!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var context = new DatabaseContext ())
            {
                var check = context.Consumables.FirstOrDefault(br => br.ProductCode == txtProductCode.Text);
                if (check != null)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("This item is already added into the system\nItem will not be added.", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
            }
            
            Consumable consumable = new Consumable();
            consumable.ItemCode = txtItemCode.Text.Trim();
            consumable.ItemDescription = txtDescription.Text;
            consumable.MaintainingQuantity = Int32.Parse( txtMaintainingQty.Text);
            consumable.ItemName = txtItemName.Text;
            consumable.UOM = cmbUOM.Text;
            consumable.Group = cmbGroup.Text;
            consumable.RemainingQuantity = Int32.Parse(txtRemainingQty.Text);
            consumable.DateAdd = DateTime.Now;
            consumable.ProductCode = txtProductCode.Text;
            using (var context = new DatabaseContext())
            {
                context.Consumables.Add(consumable);
                context.SaveChanges();
                DevExpress.Xpf.Core.DXMessageBox.Show("Consumable Added!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true; 
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void CmbProductLookUp_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            txtProductCode.Text = cmbProductLookUp.Text;
            using (var context = new DatabaseContext())
            {
                var item = context.ItemMsts.FirstOrDefault(br => br.item_cd == cmbProductLookUp.Text);
                if (item != null)
                {
                    txtDescription.Text = item.description;
                    txtItemName.Text = item.description;
                    cmbUOM.Text = item.uom;
                }
            }
            btnDropDown.IsPopupOpen = false;
        }
    }
}
