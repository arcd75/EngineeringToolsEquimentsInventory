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
    /// Interaction logic for JigManualAdd.xaml
    /// </summary>
    public partial class JigManualAdd : Window
    {
        public JigManualAdd()
        {
            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            //if (DevExpress.Xpf.Core.DXMessageBox.Show("Are you sure you want to close this window?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            //{
            //    this.Close();
            //}
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            if (int.Parse(txtQuantity.Text) <= 0)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Balance can't be 0!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cmbLocation.Text == "" || cmbPIC.Text == "" || dtDateReceived.SelectedDate.ToString() == "" || dtDateReceived.SelectedDate.ToString() == null)
            {
                DevExpress.Xpf.Core.DXMessageBox.Show("Please complete all information", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Jig newJig = new Jig();
            newJig.ItemCode = txtItemCode.Text;
            newJig.JigCode = cmbJigCode.Text;
            newJig.JigName = txtJigName.Text;
            newJig.Location = cmbLocation.Text;
            newJig.Type = txtType.Text;
            newJig.Quantity = txtQuantity.Text;
            newJig.Balance = txtBalance.Text;
            newJig.UnitCost = txtUnitCost.Text;
            newJig.TotalCost = txtTotalCost.Text;
            newJig.Specification = cmbSpecs.Text;
            newJig.RefNo = txtRefNo.Text;
            newJig.PIC = cmbOrderedBy.Text;
            newJig.PONo = txtPONo.Text;
            newJig.DateDelivered = DateTime.Parse(dtDateReceived.SelectedDate.ToString());
            newJig.WarehousePIC = cmbPIC.Text;
            using (var context = new DatabaseContext())
            {
                try
                {
                    context.Jigs.Add(newJig);
                    context.SaveChanges();
                    DevExpress.Xpf.Core.DXMessageBox.Show("Jig Added!", "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Information);
                    NewJig();
                    this.Close();
                }
                catch (Exception ex)
                {
                    DevExpress.Xpf.Core.DXMessageBox.Show("" + ex.ToString(), "Inventory Sytem", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            if (DevExpress.Xpf.Core.DXMessageBox.Show("Are you sure you want to close this window?", "Inventory System", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                this.Close();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            txtItemCode.Text = GenerateID();
            cmbPIC.Text = UserSession.UserName;
            LoadLookUpData();
            dtDateReceived.SelectedDate = DateTime.Now;
        }

        private void LoadLookUpData()
        {
            bsyPanel.Visibility = Visibility.Visible;
            var task = Task.Run(() =>
            {
                using (var context = new iJosDatabaseContext())
                {
                    var jigCode = context.JigPriceLists;
                    Dispatcher.Invoke(() =>
                    {
                        cmbJigCode.ItemsSource = jigCode.ToList();
                    });
                }
                using (var context = new DatabaseContext())
                {
                    var classification = context.Classifications;
                    if (classification.Count() > 0)
                    {
                        foreach (var item in classification)
                        {
                            Dispatcher.Invoke(() =>
                            {
                                cmbSpecs.Items.Add(item.ClassName);
                            });
                        }
                    }
                }







            });
            task.ContinueWith((t) =>
            {
                Dispatcher.Invoke(() =>
                {
                    bsyPanel.Visibility = Visibility.Collapsed;
                    task.Dispose();
                });

            });
        }

        private static string GenerateID()
        {
            string tempID = "";
            while (true)
            {
                using (var context2 = new DatabaseContext())
                {
                    tempID = "JIG-" + DateTime.Now.ToString("yyyyMMdd") + RandomString(4);
                    var checkID = context2.Jigs.FirstOrDefault(br => br.ItemCode == tempID);
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

        private void CmbJigCode_PopupClosed(object sender, DevExpress.Xpf.Editors.ClosePopupEventArgs e)
        {
            using (var context = new iJosDatabaseContext())
            {
                string code = cmbJigCode.Text.ToString();
                var Jig = context.JigPriceLists.FirstOrDefault(br => br.Jig_Code == code);
                if (Jig != null)
                {
                    txtJigName.Text = Jig.Jig_Name;
                    txtUnitCost.Text = Jig.Unit_Price.ToString();
                    txtType.Text = Jig.Type;
                    cmbSpecs.Text = Jig.Color;
                    txtQuantity.Text = "0";
                    txtQuantity.Text = "1";
                }
            }
        }

        private void TxtQuantity_KeyDown(object sender, KeyEventArgs e)
        {

        }
        private bool AreAllValidNumericChars(string str)
        {
            foreach (char c in str)
            {
                if (c != '.')
                {
                    if (!Char.IsNumber(c))
                        return false;
                }

            }

            return true;
        }

        private void TxtQuantity_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (txtQuantity.Text != "")
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(txtQuantity.Text, "[^0-9]"))
                    {

                        txtQuantity.Text = txtQuantity.Text.Remove(txtQuantity.Text.Length - 1);
                        txtBalance.Text = txtQuantity.Text;
                        return;
                    }
                    else
                    {
                        txtBalance.Text = txtQuantity.Text;
                        float res = float.Parse(txtBalance.Text) * float.Parse(txtUnitCost.Text);
                        txtTotalCost.Text = res.ToString();
                    }
                }
                else
                {
                    txtQuantity.Text = "1";
                }
            }
            catch (Exception)
            {

            }

        }

        private void CmbJigCode_SelectedIndexChanged(object sender, RoutedEventArgs e)
        {
            using (var context = new iJosDatabaseContext())
            {
                string code = cmbJigCode.Text.ToString();
                var Jig = context.JigPriceLists.FirstOrDefault(br => br.Jig_Code == code);
                if (Jig != null)
                {
                    txtJigName.Text = Jig.Jig_Name;
                    txtUnitCost.Text = Jig.Unit_Price.ToString();
                    txtType.Text = Jig.Type;
                    cmbSpecs.Text = Jig.Color;
                    txtQuantity.Text = "0";
                    txtQuantity.Text = "1";
                }
            }
        }

        private void CmbJigCode_EditValueChanged(object sender, DevExpress.Xpf.Editors.EditValueChangedEventArgs e)
        {
            using (var context = new iJosDatabaseContext())
            {
                string code = cmbJigCode.Text.ToString();
                var Jig = context.JigPriceLists.FirstOrDefault(br => br.Jig_Code == code);
                if (Jig != null)
                {
                    txtJigName.Text = Jig.Jig_Name;
                    txtUnitCost.Text = Jig.Unit_Price.ToString();
                    txtType.Text = Jig.Type;
                    cmbSpecs.Text = Jig.Color;
                    txtQuantity.Text = "0";
                    txtQuantity.Text = "1";
                }
            }
        }

        private void CmbJigCode_Spin(object sender, DevExpress.Xpf.Editors.SpinEventArgs e)
        {
            using (var context = new iJosDatabaseContext())
            {
                string code = cmbJigCode.Text.ToString();
                var Jig = context.JigPriceLists.FirstOrDefault(br => br.Jig_Code == code);
                if (Jig != null)
                {
                    txtJigName.Text = Jig.Jig_Name;
                    txtUnitCost.Text = Jig.Unit_Price.ToString();
                    txtType.Text = Jig.Type;
                    cmbSpecs.Text = Jig.Color;
                    txtQuantity.Text = "0";
                    txtQuantity.Text = "1";
                }
            }
        }

        private void NewJig()
        {
            cmbJigCode.Text = "";
            txtType.Text = "";
            txtUnitCost.Text = "";
            dtDateReceived.SelectedDate = null;
            txtJigName.Text = "";
            txtQuantity.Text = "0";
            txtTotalCost.Text = "0";
            cmbLocation.Text = "";
            txtBalance.Text = "0";
            cmbSpecs.Text = "";
            cmbPIC.Text = "";
            txtItemCode.Text = GenerateID();
        }
    }
}
