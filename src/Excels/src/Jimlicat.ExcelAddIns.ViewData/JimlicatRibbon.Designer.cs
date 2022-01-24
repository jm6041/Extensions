namespace Jimlicat.ExcelAddIns.ViewData
{
    partial class JimlicatRibbon : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public JimlicatRibbon()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.tabJimlicat = this.Factory.CreateRibbonTab();
            this.groupTools = this.Factory.CreateRibbonGroup();
            this.btnViewData = this.Factory.CreateRibbonButton();
            this.tabJimlicat.SuspendLayout();
            this.groupTools.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabJimlicat
            // 
            this.tabJimlicat.Groups.Add(this.groupTools);
            this.tabJimlicat.Label = "Jim开发";
            this.tabJimlicat.Name = "tabJimlicat";
            // 
            // groupTools
            // 
            this.groupTools.Items.Add(this.btnViewData);
            this.groupTools.Label = "数据查看";
            this.groupTools.Name = "groupTools";
            // 
            // btnViewData
            // 
            this.btnViewData.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnViewData.Description = "查看数据";
            this.btnViewData.Label = "查看数据";
            this.btnViewData.Name = "btnViewData";
            this.btnViewData.OfficeImageId = "ViewNormalViewExcel";
            this.btnViewData.ShowImage = true;
            this.btnViewData.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.btnViewData_Click);
            // 
            // JimlicatRibbon
            // 
            this.Name = "JimlicatRibbon";
            this.RibbonType = "Microsoft.Excel.Workbook";
            this.Tabs.Add(this.tabJimlicat);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.JimlicatRibbon_Load);
            this.tabJimlicat.ResumeLayout(false);
            this.tabJimlicat.PerformLayout();
            this.groupTools.ResumeLayout(false);
            this.groupTools.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tabJimlicat;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupTools;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnViewData;
    }

    partial class ThisRibbonCollection
    {
        internal JimlicatRibbon JimlicatRibbon
        {
            get { return this.GetRibbon<JimlicatRibbon>(); }
        }
    }
}
