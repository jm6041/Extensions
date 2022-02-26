namespace Wsfg.Controls
{
    partial class ViewDataForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxRange = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanelDataRange = new System.Windows.Forms.TableLayoutPanel();
            this.comboBoxSheets = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxDataRowIndex = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxColEndName = new System.Windows.Forms.TextBox();
            this.btnView = new System.Windows.Forms.Button();
            this.tableLayoutPanelMain = new System.Windows.Forms.TableLayoutPanel();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.flowLayoutPanelStates = new System.Windows.Forms.FlowLayoutPanel();
            this.labelDataReadState = new System.Windows.Forms.Label();
            this.groupBoxRange.SuspendLayout();
            this.tableLayoutPanelDataRange.SuspendLayout();
            this.tableLayoutPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.flowLayoutPanelStates.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxRange
            // 
            this.groupBoxRange.Controls.Add(this.tableLayoutPanelDataRange);
            this.groupBoxRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxRange.Location = new System.Drawing.Point(3, 3);
            this.groupBoxRange.Name = "groupBoxRange";
            this.groupBoxRange.Size = new System.Drawing.Size(778, 50);
            this.groupBoxRange.TabIndex = 1;
            this.groupBoxRange.TabStop = false;
            this.groupBoxRange.Text = "数据范围";
            // 
            // tableLayoutPanelDataRange
            // 
            this.tableLayoutPanelDataRange.ColumnCount = 7;
            this.tableLayoutPanelDataRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelDataRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanelDataRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanelDataRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanelDataRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanelDataRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanelDataRange.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanelDataRange.Controls.Add(this.comboBoxSheets, 1, 0);
            this.tableLayoutPanelDataRange.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanelDataRange.Controls.Add(this.label2, 2, 0);
            this.tableLayoutPanelDataRange.Controls.Add(this.textBoxDataRowIndex, 3, 0);
            this.tableLayoutPanelDataRange.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanelDataRange.Controls.Add(this.textBoxColEndName, 5, 0);
            this.tableLayoutPanelDataRange.Controls.Add(this.btnView, 6, 0);
            this.tableLayoutPanelDataRange.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelDataRange.Location = new System.Drawing.Point(3, 17);
            this.tableLayoutPanelDataRange.Name = "tableLayoutPanelDataRange";
            this.tableLayoutPanelDataRange.RowCount = 1;
            this.tableLayoutPanelDataRange.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelDataRange.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelDataRange.Size = new System.Drawing.Size(772, 30);
            this.tableLayoutPanelDataRange.TabIndex = 0;
            // 
            // comboBoxSheets
            // 
            this.comboBoxSheets.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBoxSheets.FormattingEnabled = true;
            this.comboBoxSheets.Location = new System.Drawing.Point(80, 5);
            this.comboBoxSheets.Name = "comboBoxSheets";
            this.comboBoxSheets.Size = new System.Drawing.Size(80, 20);
            this.comboBoxSheets.TabIndex = 1;
            this.comboBoxSheets.SelectedIndexChanged += new System.EventHandler(this.comboBoxSheets_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "选择表格：";
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(212, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "数据开始行：";
            // 
            // textBoxDataRowIndex
            // 
            this.textBoxDataRowIndex.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxDataRowIndex.Location = new System.Drawing.Point(295, 4);
            this.textBoxDataRowIndex.Name = "textBoxDataRowIndex";
            this.textBoxDataRowIndex.Size = new System.Drawing.Size(80, 21);
            this.textBoxDataRowIndex.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(436, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "结束列：";
            // 
            // textBoxColEndName
            // 
            this.textBoxColEndName.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.textBoxColEndName.Location = new System.Drawing.Point(495, 4);
            this.textBoxColEndName.Name = "textBoxColEndName";
            this.textBoxColEndName.Size = new System.Drawing.Size(80, 21);
            this.textBoxColEndName.TabIndex = 7;
            // 
            // btnView
            // 
            this.btnView.Location = new System.Drawing.Point(618, 3);
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(75, 23);
            this.btnView.TabIndex = 8;
            this.btnView.Text = "查看";
            this.btnView.UseVisualStyleBackColor = true;
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // tableLayoutPanelMain
            // 
            this.tableLayoutPanelMain.ColumnCount = 1;
            this.tableLayoutPanelMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.Controls.Add(this.groupBoxRange, 0, 0);
            this.tableLayoutPanelMain.Controls.Add(this.dataGridView, 0, 1);
            this.tableLayoutPanelMain.Controls.Add(this.flowLayoutPanelStates, 0, 2);
            this.tableLayoutPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelMain.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanelMain.Name = "tableLayoutPanelMain";
            this.tableLayoutPanelMain.RowCount = 3;
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 56F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanelMain.Size = new System.Drawing.Size(784, 411);
            this.tableLayoutPanelMain.TabIndex = 2;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(3, 59);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ReadOnly = true;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.Size = new System.Drawing.Size(778, 319);
            this.dataGridView.TabIndex = 2;
            this.dataGridView.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            // 
            // flowLayoutPanelStates
            // 
            this.flowLayoutPanelStates.Controls.Add(this.labelDataReadState);
            this.flowLayoutPanelStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelStates.Location = new System.Drawing.Point(3, 386);
            this.flowLayoutPanelStates.Margin = new System.Windows.Forms.Padding(3, 5, 3, 3);
            this.flowLayoutPanelStates.Name = "flowLayoutPanelStates";
            this.flowLayoutPanelStates.Size = new System.Drawing.Size(778, 22);
            this.flowLayoutPanelStates.TabIndex = 3;
            // 
            // labelDataReadState
            // 
            this.labelDataReadState.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.labelDataReadState.AutoSize = true;
            this.labelDataReadState.Location = new System.Drawing.Point(3, 4);
            this.labelDataReadState.Margin = new System.Windows.Forms.Padding(3, 4, 3, 0);
            this.labelDataReadState.Name = "labelDataReadState";
            this.labelDataReadState.Size = new System.Drawing.Size(0, 12);
            this.labelDataReadState.TabIndex = 0;
            // 
            // ViewDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 411);
            this.Controls.Add(this.tableLayoutPanelMain);
            this.Name = "ViewDataForm";
            this.Text = "股票数据";
            this.groupBoxRange.ResumeLayout(false);
            this.tableLayoutPanelDataRange.ResumeLayout(false);
            this.tableLayoutPanelDataRange.PerformLayout();
            this.tableLayoutPanelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.flowLayoutPanelStates.ResumeLayout(false);
            this.flowLayoutPanelStates.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxRange;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanelDataRange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxSheets;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxDataRowIndex;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxColEndName;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelStates;
        private System.Windows.Forms.Label labelDataReadState;
        private System.Windows.Forms.Button btnView;
    }
}
