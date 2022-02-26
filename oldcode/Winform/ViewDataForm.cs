using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Wsfg.OfficeEx;
using Excel = Microsoft.Office.Interop.Excel;

namespace Wsfg.Controls
{
    public partial class ViewDataForm : Form
    {
        private readonly Excel.Sheets _sheets;
        private readonly ViewDataVm view;
        private readonly string TextBox_Text = nameof(TextBox.Text);
        private readonly Stopwatch stopwatch;
        public ViewDataForm(Excel.Sheets sheets)
        {
            InitializeComponent();
            _sheets = sheets;
            view = new ViewDataVm();
            Init();
            stopwatch = new Stopwatch();
        }

        private void Init()
        {
            foreach (Excel.Worksheet s in _sheets)
            {
                var n = s.Name;
                var desc = n;
                if (s.Visible != Excel.XlSheetVisibility.xlSheetVisible)
                {
                    desc += $"_({RC.Hidden})";
                }
                view.Sheets.Add(new SheetVm(n, desc));
            }
            comboBoxSheets.DataSource = view.Sheets;
            comboBoxSheets.ValueMember = nameof(SheetVm.Name);
            comboBoxSheets.DisplayMember = nameof(SheetVm.Description);
            var fs = view.Sheets.FirstOrDefault();
            if (fs != null)
            {
                comboBoxSheets.SelectedValue = fs.Name;
                view.SelectedSheet = fs.Name;
            }
            textBoxDataRowIndex.DataBindings.Add(TextBox_Text, view, nameof(view.DataRowIndex), false, DataSourceUpdateMode.OnPropertyChanged);
            textBoxColEndName.DataBindings.Add(TextBox_Text, view, nameof(view.EndColumnName), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private async void btnView_Click(object sender, EventArgs e)
        {
            Excel.Worksheet sheet = (Excel.Worksheet)_sheets[view.SelectedSheet];
            if (sheet == null)
            {
                return;
            }
            view.EndColumnName = ExcelLimits.GetEndColumnName(view.EndColumnName);
            var endColName = view.EndColumnName;
            stopwatch.Restart();
            SetDataReadState(string.Empty);
            var data = await Task.Factory.StartNew(() => WorksheetHelper.GetRowContent(sheet, view.DataRowIndex, endColName));
            stopwatch.Stop();
            var dataElapsed = stopwatch.Elapsed;
            stopwatch.Start();
            ShowData(data, dataGridView);
            stopwatch.Stop();
            var desc = $"总计行数：{data.Count}，数据解析耗时：{dataElapsed}，总计耗时：{stopwatch.Elapsed}";
            SetDataReadState(desc);
        }
        // 设置读取状态
        private void SetDataReadState(string desc)
        {
            labelDataReadState.Text = desc;
        }
        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="dataGridView"></param>
        private static void ShowData(SortedDictionary<int, RowContent> data, DataGridView dataGridView)
        {
            dataGridView.Columns.Clear();
            dataGridView.Rows.Clear();
            if (data.Count == 0)
            {
                return;
            }
            // 添加第一列，用于显示原始数据所在excel中的列
            dataGridView.Columns.Add(new DataGridViewColumn()
            {
                SortMode = DataGridViewColumnSortMode.NotSortable,
                HeaderText = "Excel",
                Width = 40,
            });
            // 开始列索引
            var startIndex = data.Max(x => x.Value.MinColoumnIndex);
            // 结束列索引
            var endIndex = data.Max(x => x.Value.MaxColoumnIndex);
            for (int i = startIndex; i <= endIndex; i++)
            {
                DataGridViewColumn dc = new DataGridViewColumn()
                {
                    SortMode = DataGridViewColumnSortMode.NotSortable,
                    HeaderText = CellReferenceHelper.GetColumnReference(i - 1),
                    CellTemplate = new DataGridViewTextBoxCell(),
                };
                dataGridView.Columns.Add(dc);
            }
            foreach (var item in data)
            {
                DataGridViewRow dr = new DataGridViewRow();
                dr.Cells.Add(new DataGridViewTextBoxCell()
                {
                    Value = item.Key,
                    Style = new DataGridViewCellStyle() { Alignment = DataGridViewContentAlignment.MiddleCenter },
                });
                for (int i = startIndex; i <= endIndex; i++)
                {
                    var dc = new DataGridViewTextBoxCell();
                    bool s = item.Value.TryGetCellValue(i, out var val);
                    if (s)
                    {
                        dc.Value = val.Value;
                        dc.ToolTipText = $"{val.Value} {val.Value?.GetType().Name}";
                    }
                    dr.Cells.Add(dc);
                }
                dataGridView.Rows.Add(dr);
            }
        }

        private void comboBoxSheets_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxSheets.SelectedValue is string sn)
            {
                view.SelectedSheet = sn;
            }
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X, e.RowBounds.Location.Y, dataGridView.RowHeadersWidth - 4, e.RowBounds.Height);
            TextRenderer.DrawText(e.Graphics,
                (e.RowIndex + 1).ToString(),
                dataGridView.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
