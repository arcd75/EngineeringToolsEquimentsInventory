namespace EngineeringToolsEquipmentsInventory.Views.InventoryManagement {
    partial class Dashboard1 {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            DevExpress.DashboardCommon.ChartPane chartPane1 = new DevExpress.DashboardCommon.ChartPane();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup1 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup2 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem1 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup3 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem2 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem3 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem4 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutGroup dashboardLayoutGroup4 = new DevExpress.DashboardCommon.DashboardLayoutGroup();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem5 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            DevExpress.DashboardCommon.DashboardLayoutItem dashboardLayoutItem6 = new DevExpress.DashboardCommon.DashboardLayoutItem();
            this.gaugeDashboardItem1 = new DevExpress.DashboardCommon.GaugeDashboardItem();
            this.chartDashboardItem1 = new DevExpress.DashboardCommon.ChartDashboardItem();
            this.cardDashboardItem1 = new DevExpress.DashboardCommon.CardDashboardItem();
            this.gaugeDashboardItem2 = new DevExpress.DashboardCommon.GaugeDashboardItem();
            this.cardDashboardItem2 = new DevExpress.DashboardCommon.CardDashboardItem();
            this.pieDashboardItem1 = new DevExpress.DashboardCommon.PieDashboardItem();
            ((System.ComponentModel.ISupportInitialize)(this.gaugeDashboardItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDashboardItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDashboardItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gaugeDashboardItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDashboardItem2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pieDashboardItem1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // gaugeDashboardItem1
            // 
            this.gaugeDashboardItem1.ComponentName = "gaugeDashboardItem1";
            this.gaugeDashboardItem1.DataItemRepository.Clear();
            this.gaugeDashboardItem1.InteractivityOptions.IgnoreMasterFilters = false;
            this.gaugeDashboardItem1.Name = "Gauges 1";
            this.gaugeDashboardItem1.ShowCaption = true;
            // 
            // chartDashboardItem1
            // 
            this.chartDashboardItem1.AxisX.TitleVisible = false;
            this.chartDashboardItem1.ComponentName = "chartDashboardItem1";
            this.chartDashboardItem1.DataItemRepository.Clear();
            this.chartDashboardItem1.InteractivityOptions.IgnoreMasterFilters = false;
            this.chartDashboardItem1.Name = "Chart 1";
            chartPane1.Name = "Pane 1";
            chartPane1.PrimaryAxisY.AlwaysShowZeroLevel = true;
            chartPane1.PrimaryAxisY.ShowGridLines = true;
            chartPane1.PrimaryAxisY.TitleVisible = true;
            chartPane1.SecondaryAxisY.AlwaysShowZeroLevel = true;
            chartPane1.SecondaryAxisY.ShowGridLines = false;
            chartPane1.SecondaryAxisY.TitleVisible = true;
            this.chartDashboardItem1.Panes.AddRange(new DevExpress.DashboardCommon.ChartPane[] {
            chartPane1});
            this.chartDashboardItem1.ShowCaption = true;
            // 
            // cardDashboardItem1
            // 
            this.cardDashboardItem1.ComponentName = "cardDashboardItem1";
            this.cardDashboardItem1.DataItemRepository.Clear();
            this.cardDashboardItem1.InteractivityOptions.IgnoreMasterFilters = false;
            this.cardDashboardItem1.Name = "Cards 1";
            this.cardDashboardItem1.ShowCaption = true;
            // 
            // gaugeDashboardItem2
            // 
            this.gaugeDashboardItem2.ComponentName = "gaugeDashboardItem2";
            this.gaugeDashboardItem2.DataItemRepository.Clear();
            this.gaugeDashboardItem2.InteractivityOptions.IgnoreMasterFilters = false;
            this.gaugeDashboardItem2.Name = "Gauges 2";
            this.gaugeDashboardItem2.ShowCaption = true;
            // 
            // cardDashboardItem2
            // 
            this.cardDashboardItem2.ComponentName = "cardDashboardItem2";
            this.cardDashboardItem2.DataItemRepository.Clear();
            this.cardDashboardItem2.InteractivityOptions.IgnoreMasterFilters = false;
            this.cardDashboardItem2.Name = "Cards 2";
            this.cardDashboardItem2.ShowCaption = true;
            // 
            // pieDashboardItem1
            // 
            this.pieDashboardItem1.ComponentName = "pieDashboardItem1";
            this.pieDashboardItem1.DataItemRepository.Clear();
            this.pieDashboardItem1.InteractivityOptions.IgnoreMasterFilters = false;
            this.pieDashboardItem1.Name = "Pies 1";
            this.pieDashboardItem1.ShowCaption = true;
            // 
            // Dashboard1
            // 
            this.Items.AddRange(new DevExpress.DashboardCommon.DashboardItem[] {
            this.gaugeDashboardItem1,
            this.chartDashboardItem1,
            this.cardDashboardItem1,
            this.gaugeDashboardItem2,
            this.cardDashboardItem2,
            this.pieDashboardItem1});
            dashboardLayoutItem1.DashboardItem = this.chartDashboardItem1;
            dashboardLayoutItem1.Weight = 25.056433408577877D;
            dashboardLayoutItem2.DashboardItem = this.gaugeDashboardItem2;
            dashboardLayoutItem2.Weight = 50D;
            dashboardLayoutItem3.DashboardItem = this.pieDashboardItem1;
            dashboardLayoutItem3.Weight = 50D;
            dashboardLayoutGroup3.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutItem2,
            dashboardLayoutItem3});
            dashboardLayoutGroup3.DashboardItem = null;
            dashboardLayoutGroup3.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup3.Weight = 24.943566591422123D;
            dashboardLayoutItem4.DashboardItem = this.cardDashboardItem2;
            dashboardLayoutItem4.Weight = 50D;
            dashboardLayoutGroup2.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutItem1,
            dashboardLayoutGroup3,
            dashboardLayoutItem4});
            dashboardLayoutGroup2.DashboardItem = null;
            dashboardLayoutGroup2.Weight = 50D;
            dashboardLayoutItem5.DashboardItem = this.cardDashboardItem1;
            dashboardLayoutItem5.Weight = 50D;
            dashboardLayoutItem6.DashboardItem = this.gaugeDashboardItem1;
            dashboardLayoutItem6.Weight = 50D;
            dashboardLayoutGroup4.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutItem5,
            dashboardLayoutItem6});
            dashboardLayoutGroup4.DashboardItem = null;
            dashboardLayoutGroup4.Weight = 50D;
            dashboardLayoutGroup1.ChildNodes.AddRange(new DevExpress.DashboardCommon.DashboardLayoutNode[] {
            dashboardLayoutGroup2,
            dashboardLayoutGroup4});
            dashboardLayoutGroup1.DashboardItem = null;
            dashboardLayoutGroup1.Orientation = DevExpress.DashboardCommon.DashboardLayoutGroupOrientation.Vertical;
            dashboardLayoutGroup1.Weight = 100D;
            this.LayoutRoot = dashboardLayoutGroup1;
            this.Title.Text = "Dashboard";
            ((System.ComponentModel.ISupportInitialize)(this.gaugeDashboardItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartDashboardItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDashboardItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gaugeDashboardItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cardDashboardItem2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pieDashboardItem1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.DashboardCommon.GaugeDashboardItem gaugeDashboardItem1;
        private DevExpress.DashboardCommon.ChartDashboardItem chartDashboardItem1;
        private DevExpress.DashboardCommon.CardDashboardItem cardDashboardItem1;
        private DevExpress.DashboardCommon.GaugeDashboardItem gaugeDashboardItem2;
        private DevExpress.DashboardCommon.CardDashboardItem cardDashboardItem2;
        private DevExpress.DashboardCommon.PieDashboardItem pieDashboardItem1;
    }
}
