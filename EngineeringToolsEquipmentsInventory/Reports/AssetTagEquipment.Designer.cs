namespace EngineeringToolsEquipmentsInventory.Reports
{
    partial class AssetTagEquipment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator1 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            DevExpress.XtraPrinting.BarCode.Code128Generator code128Generator2 = new DevExpress.XtraPrinting.BarCode.Code128Generator();
            this.Detail = new DevExpress.XtraReports.UI.DetailBand();
            this.TopMargin = new DevExpress.XtraReports.UI.TopMarginBand();
            this.BottomMargin = new DevExpress.XtraReports.UI.BottomMarginBand();
            this.panel1 = new DevExpress.XtraReports.UI.XRPanel();
            this.xrLabel1 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrLabel2 = new DevExpress.XtraReports.UI.XRLabel();
            this.xrItemCode = new DevExpress.XtraReports.UI.XRBarCode();
            this.xrPeCode = new DevExpress.XtraReports.UI.XRBarCode();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // Detail
            // 
            this.Detail.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.panel1});
            this.Detail.HeightF = 185.0394F;
            this.Detail.MultiColumn.ColumnWidth = 354.3307F;
            this.Detail.MultiColumn.Layout = DevExpress.XtraPrinting.ColumnLayout.AcrossThenDown;
            this.Detail.MultiColumn.Mode = DevExpress.XtraReports.UI.MultiColumnMode.UseColumnWidth;
            this.Detail.Name = "Detail";
            // 
            // TopMargin
            // 
            this.TopMargin.HeightF = 98F;
            this.TopMargin.Name = "TopMargin";
            // 
            // BottomMargin
            // 
            this.BottomMargin.HeightF = 145F;
            this.BottomMargin.Name = "BottomMargin";
            // 
            // panel1
            // 
            this.panel1.Borders = ((DevExpress.XtraPrinting.BorderSide)((((DevExpress.XtraPrinting.BorderSide.Left | DevExpress.XtraPrinting.BorderSide.Top) 
            | DevExpress.XtraPrinting.BorderSide.Right) 
            | DevExpress.XtraPrinting.BorderSide.Bottom)));
            this.panel1.CanGrow = false;
            this.panel1.Controls.AddRange(new DevExpress.XtraReports.UI.XRControl[] {
            this.xrLabel1,
            this.xrLabel2,
            this.xrItemCode,
            this.xrPeCode});
            this.panel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.panel1.Name = "panel1";
            this.panel1.SizeF = new System.Drawing.SizeF(354.3307F, 185.0394F);
            // 
            // xrLabel1
            // 
            this.xrLabel1.BackColor = System.Drawing.Color.Green;
            this.xrLabel1.BorderColor = System.Drawing.Color.Bisque;
            this.xrLabel1.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel1.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel1.ForeColor = System.Drawing.Color.White;
            this.xrLabel1.LocationFloat = new DevExpress.Utils.PointFloat(0F, 0F);
            this.xrLabel1.Multiline = true;
            this.xrLabel1.Name = "xrLabel1";
            this.xrLabel1.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel1.SizeF = new System.Drawing.SizeF(354.3307F, 36.25733F);
            this.xrLabel1.StylePriority.UseBackColor = false;
            this.xrLabel1.StylePriority.UseBorderColor = false;
            this.xrLabel1.StylePriority.UseBorders = false;
            this.xrLabel1.StylePriority.UseFont = false;
            this.xrLabel1.StylePriority.UseForeColor = false;
            this.xrLabel1.StylePriority.UseTextAlignment = false;
            this.xrLabel1.Text = "ASSET/EQUIPMENT - ENGINEERING WAREHOUSE PROPERTY";
            this.xrLabel1.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrLabel2
            // 
            this.xrLabel2.BackColor = System.Drawing.Color.Green;
            this.xrLabel2.BorderColor = System.Drawing.Color.Bisque;
            this.xrLabel2.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrLabel2.Font = new System.Drawing.Font("Segoe UI Black", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.xrLabel2.ForeColor = System.Drawing.Color.White;
            this.xrLabel2.LocationFloat = new DevExpress.Utils.PointFloat(0F, 36.25733F);
            this.xrLabel2.Multiline = true;
            this.xrLabel2.Name = "xrLabel2";
            this.xrLabel2.Padding = new DevExpress.XtraPrinting.PaddingInfo(2, 2, 0, 0, 100F);
            this.xrLabel2.SizeF = new System.Drawing.SizeF(354.3307F, 23.85355F);
            this.xrLabel2.StylePriority.UseBackColor = false;
            this.xrLabel2.StylePriority.UseBorderColor = false;
            this.xrLabel2.StylePriority.UseBorders = false;
            this.xrLabel2.StylePriority.UseFont = false;
            this.xrLabel2.StylePriority.UseForeColor = false;
            this.xrLabel2.StylePriority.UseTextAlignment = false;
            this.xrLabel2.Text = "DO NOT REMOVE";
            this.xrLabel2.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrItemCode
            // 
            this.xrItemCode.AutoModule = true;
            this.xrItemCode.BackColor = System.Drawing.Color.White;
            this.xrItemCode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrItemCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[AssetID]")});
            this.xrItemCode.LocationFloat = new DevExpress.Utils.PointFloat(10.00003F, 73.1163F);
            this.xrItemCode.Name = "xrItemCode";
            this.xrItemCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrItemCode.SizeF = new System.Drawing.SizeF(334.3307F, 43.75F);
            this.xrItemCode.StylePriority.UseBackColor = false;
            this.xrItemCode.StylePriority.UseBorders = false;
            this.xrItemCode.StylePriority.UseTextAlignment = false;
            this.xrItemCode.Symbology = code128Generator1;
            this.xrItemCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // xrPeCode
            // 
            this.xrPeCode.AutoModule = true;
            this.xrPeCode.BackColor = System.Drawing.Color.White;
            this.xrPeCode.Borders = DevExpress.XtraPrinting.BorderSide.None;
            this.xrPeCode.ExpressionBindings.AddRange(new DevExpress.XtraReports.UI.ExpressionBinding[] {
            new DevExpress.XtraReports.UI.ExpressionBinding("BeforePrint", "Text", "[SerialNo]")});
            this.xrPeCode.LocationFloat = new DevExpress.Utils.PointFloat(10.00003F, 131.2894F);
            this.xrPeCode.Name = "xrPeCode";
            this.xrPeCode.Padding = new DevExpress.XtraPrinting.PaddingInfo(10, 10, 0, 0, 100F);
            this.xrPeCode.SizeF = new System.Drawing.SizeF(334.3307F, 43.75F);
            this.xrPeCode.StylePriority.UseBackColor = false;
            this.xrPeCode.StylePriority.UseBorders = false;
            this.xrPeCode.StylePriority.UseTextAlignment = false;
            this.xrPeCode.Symbology = code128Generator2;
            this.xrPeCode.TextAlignment = DevExpress.XtraPrinting.TextAlignment.MiddleCenter;
            // 
            // AssetTagEquipment
            // 
            this.Bands.AddRange(new DevExpress.XtraReports.UI.Band[] {
            this.Detail,
            this.TopMargin,
            this.BottomMargin});
            this.Font = new System.Drawing.Font("Arial", 9.75F);
            this.Margins = new System.Drawing.Printing.Margins(39, 78, 98, 145);
            this.PageHeight = 1169;
            this.PageWidth = 827;
            this.PaperKind = System.Drawing.Printing.PaperKind.A4;
            this.ReportPrintOptions.DetailCountOnEmptyDataSource = 10;
            this.Version = "19.1";
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.XtraReports.UI.DetailBand Detail;
        private DevExpress.XtraReports.UI.XRPanel panel1;
        private DevExpress.XtraReports.UI.TopMarginBand TopMargin;
        private DevExpress.XtraReports.UI.BottomMarginBand BottomMargin;
        private DevExpress.XtraReports.UI.XRLabel xrLabel1;
        private DevExpress.XtraReports.UI.XRLabel xrLabel2;
        private DevExpress.XtraReports.UI.XRBarCode xrItemCode;
        private DevExpress.XtraReports.UI.XRBarCode xrPeCode;
    }
}
