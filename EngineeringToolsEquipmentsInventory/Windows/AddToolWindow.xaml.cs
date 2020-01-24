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
    /// Interaction logic for AddToolWindow.xaml
    /// </summary>
    public partial class AddToolWindow : Window
    {
        public AddToolWindow()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
                ToolEditSession.toolEditItemCode = "";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (txtItemCode.Text.Trim() == "" || txtItemName.Text.Trim() == ""||txtBrand.Text.Trim() == ""|| txtBrand.Text.Trim() == "")
            {
                MessageBox.Show("Please complete all information","Inventory System",MessageBoxButton.OK,MessageBoxImage.Error);
            }
            else
            {
                if (ToolEditSession.toolEditItemCode == "")
                {
                    using (var context = new DatabaseContext())
                    {
                        Tool tool = new Tool();
                        tool.ItemCode = txtItemCode.Text.Trim();
                        tool.Type = cmbType.Text;
                        tool.Item = txtItemName.Text.Trim();
                        if (txtDescription.Text == "")
                        {
                            tool.ItemDescription = "N/A";
                        }
                        else
                        {
                            tool.ItemDescription = txtDescription.Text;
                        }
                        tool.Brand = txtBrand.Text.Trim();
                        tool.Condition = cmbCondition.Text;
                        tool.Remarks = "N/A";
                        tool.DateDelivered = dtDateDelivered.DateTime;
                        tool.LastUpdate = DateTime.Now;
                        tool.Status = "In-Stock";
                        tool.PECode = txtPECode.Text.Trim();
                        context.Tools.Add(tool);
                        context.SaveChanges();
                        MessageBox.Show("Tool added", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                        ClearForm();
                        DialogResult = true;
                    } 
                }
                else
                {
                    using (var context = new DatabaseContext())
                    {
                        var updateTool = context.Tools.FirstOrDefault(br => br.ItemCode == txtItemCode.Text);
                        if (updateTool != null)
                        {
                            updateTool.Type = cmbType.Text;
                            updateTool.Item = txtItemName.Text.Trim();
                            if (txtDescription.Text == "")
                            {
                                updateTool.ItemDescription = "N/A";
                            }
                            else
                            {
                                updateTool.ItemDescription = txtDescription.Text;
                            }
                            updateTool.Brand = txtBrand.Text.Trim();
                            updateTool.Condition = cmbCondition.Text;
                            updateTool.Remarks = "N/A";
                            updateTool.DateDelivered = dtDateDelivered.DateTime;
                            updateTool.LastUpdate = DateTime.Now; 
                            updateTool.PECode = txtPECode.Text.Trim();
                            context.SaveChanges();
                            MessageBox.Show("Tool Updated", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                            ClearForm();
                            ToolEditSession.toolEditItemCode = "";
                            //DialogResult = true;

                        }
                    }
                }
            }
        }

        private void ClearForm()
        {
            txtItemCode.Text = "";
            txtItemName.Text = "";
            txtBrand.Text = "";
            txtDescription.Text = "";
            cmbCondition.SelectedIndex = 0;
            cmbType.SelectedIndex = 0;
            txtPECode.Text = "";
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
            if (txtItemCode.Text.Trim() == "" || txtItemCode.Text.Trim() == null)
            {
                string ItemCodeTemp = "";
                while (true)
                {
                    ItemCodeTemp = "PE-" + DateTime.Now.ToString("ddMMyy") + RandomString(6);
                    using (var context = new DatabaseContext())
                    {
                        var checking = context.Tools.FirstOrDefault(br => br.ItemCode == ItemCodeTemp);
                        if (checking == null)
                        {
                            break;
                        }
                    }
                }

                txtItemCode.Text = ItemCodeTemp;  
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (ToolEditSession.toolEditItemCode.Trim() != "")
            {
                using (var context = new DatabaseContext())
                {
                    var data = context.Tools.FirstOrDefault(br => br.ItemCode == ToolEditSession.toolEditItemCode);

                    if (data != null)
                    {
                        txtItemCode.Text = data.ItemCode;
                        txtItemName.Text = data.Item;
                        txtBrand.Text = data.Brand;
                        txtDescription.Text = data.ItemDescription;
                        cmbCondition.Text = data.Condition;
                        cmbType.Text = data.Type;
                        txtPECode.Text = data.PECode;
                        dtDateDelivered.DateTime = data.DateDelivered; 
                    }
                }
            }
        }
    }
}
