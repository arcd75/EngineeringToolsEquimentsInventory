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
    /// Interaction logic for ToolLookupWindow.xaml
    /// </summary>
    public partial class ToolLookupWindow : Window
    {
        public ToolLookupWindow()
        {
            InitializeComponent();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (HistorySession.historyItemLookUp == false)
            {
                var selectedItems = dgGrid.SelectedItems.Cast<Tool>().ToList();
                if (selectedItems != null)
                {
                    string duplicateItem = "";
                    foreach (var item in selectedItems)
                    {
                        foreach (var item2 in TransactionSession.barrowTools)
                        {
                            if (item2.ItemCode == item.ItemCode)
                            {
                                duplicateItem = item.Item;
                                goto Skip;
                            }
                        }
                        TransactionSession.barrowTools.Add(item);
                    Skip:
                        if (duplicateItem != "")
                        {
                            MessageBox.Show("Duplicate '" + duplicateItem + "' is found and will not be included", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Warning); 
                        } ;
                        duplicateItem = "";
                    }
                    DialogResult = true;
                } 
            }
            else
            {
                var selectedItems = dgGrid.SelectedItem as Tool; 
                HistorySession.queryTool = selectedItems.ItemCode;
                HistorySession.historyItemLookUp = false;
                DialogResult = true;
            }

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();

            if (HistorySession.historyItemLookUp == true)
            {
                viewGrid.ShowCheckBoxSelectorColumn = false;
                txtBtnContent.Text = "Search ItemCode";
            }
            else
            {
                viewGrid.ShowCheckBoxSelectorColumn = true;

            }
        }

        private void LoadData()
        {
            if (HistorySession.historyItemLookUp == false)
            {
                using (var context = new DatabaseContext())
                {
                    var data = context.Tools.Where(br => br.Status == "In-Stock" && br.Condition == "GOOD");
                    dgGrid.ItemsSource = data.ToList();
                } 
            }
            else
            {
                using (var context = new DatabaseContext())
                {
                    var data = context.Tools;
                    dgGrid.ItemsSource = data.ToList();
                }
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.DialogResult = false;
                HistorySession.historyItemLookUp = false;
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGrid.SelectedItem as Tool;
            if (selectedItem != null)
            {
                ToolEditSession.toolEditItemCode = selectedItem.ItemCode;
                AddToolWindow addToolWindow = new AddToolWindow();
                addToolWindow.ShowDialog(); 
                if (addToolWindow.DialogResult == true)
                {
                    LoadData();
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = dgGrid.SelectedItem as Tool;
            if (MessageBox.Show("Are you sure you want to delete this tool?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
            {
                if (selectedItem != null)
                {
                    using (var context = new DatabaseContext())
                    {
                        var toDelete = context.Tools.FirstOrDefault(br => br.ItemCode == selectedItem.ItemCode);
                        if (toDelete != null)
                        {
                            context.Tools.Remove(toDelete);
                            context.SaveChanges();
                            MessageBox.Show("Tool deleted", "Inventory System", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadData();
                        }
                    }
                } 
            }
        }

        private void BtnAddTool_Click(object sender, RoutedEventArgs e)
        {
            AddToolWindow addToolWindow = new AddToolWindow();
            addToolWindow.ShowDialog(); 
            if (addToolWindow.DialogResult == true)
            {
                HistorySession.historyItemLookUp = false;
                LoadData();
            }
        }

        private void BtnAddDistinct_Click(object sender, RoutedEventArgs e)
        {
            var selectedItems = dgGrid.SelectedItem as Tool;
            HistorySession.queryTool = selectedItems.ItemCode;
            HistorySession.historyItemLookUp = false;
            DialogResult = true;
        }
    }
}
