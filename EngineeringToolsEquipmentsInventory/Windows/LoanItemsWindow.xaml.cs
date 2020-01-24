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
    /// Interaction logic for LoanItemsWindow.xaml
    /// </summary>
    public partial class LoanItemsWindow : Window
    {
        public LoanItemsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadItems();
        }

        private void LoadItems()
        {
            if (ReturningSession.LoanItemLoanID != null)
            {
                using (var context = new DatabaseContext())
                {
                    var item = context.LoanedTools.Where(br => br.LoanID == ReturningSession.LoanItemLoanID && br.Status == "Active");
                    dgGridLoans.ItemsSource = item.ToList();
                }
            }
            else
            {
                DialogResult = false;
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                DialogResult = false;
            }
        }

        private void BtnReturn_Click(object sender, RoutedEventArgs e)
        {
            
            var selectedItems = dgGridLoans.SelectedItem as LoanedTool;
            
            if (selectedItems != null)
            {
                using (var context = new DatabaseContext())
                {
                    var item = context.LoanedTools.FirstOrDefault(br => br.LoanID == selectedItems.LoanID && br.LoanToolID == selectedItems.LoanToolID);
                    if (item != null)
                    {
                        item.Status = "Returned";
                        item.ReturnLoginName = UserSession.UserName;
                        item.Condition = selectedItems.Condition;
                        item.DateReturned = DateTime.Now;
                        context.SaveChanges();

                        var updateTool = context.Tools.FirstOrDefault(br => br.ItemCode == selectedItems.ItemCode);
                        updateTool.Status = "In-Stock";
                        updateTool.LastUpdate = DateTime.Now;
                        updateTool.Condition = selectedItems.Condition;
                        context.SaveChanges();

                        


                        var LoanedItemCount = context.LoanedTools.Where(br => br.LoanID == ReturningSession.LoanItemLoanID && br.Status == "Active");
                        if (LoanedItemCount.Count() <= 0)
                        {
                            MessageBox.Show("All items returned","Inventory System",MessageBoxButton.OK,MessageBoxImage.Information);
                            var UpdateLoan = context.Loans.FirstOrDefault(br=> br.LoanID == ReturningSession.LoanItemLoanID);
                            UpdateLoan.Status = "Returned";
                            UpdateLoan.ReturnDate = DateTime.Now;
                            context.SaveChanges();
                            DialogResult = true;
                        }
                        else
                        {
                            LoadItems();
                        }
                    }
                } 
            }
        }
    }
}
