using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jimlicat.ExcelAddIns.ViewData
{
    /// <summary>
    /// ViewData视图
    /// </summary>
    internal class ViewDataView : INotifyPropertyChanged
    {
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public ViewDataView()
        {
            dataRowIndex = 2;
            selectedSheet = string.Empty;
            // 默认A-Z,总计26列
            endColumnName = "Z";
        }
        /// <summary>
        /// Sheets
        /// </summary>
        public ObservableCollection<SheetView> Sheets { get; set; } = new ObservableCollection<SheetView>();
        private string selectedSheet;
        /// <summary>
        /// 选择的Sheet
        /// </summary>
        public string SelectedSheet
        {
            get { return selectedSheet; }
            set
            {
                if (selectedSheet != value)
                {
                    selectedSheet = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedSheet)));
                }
            }
        }
        private int dataRowIndex;
        /// <summary>
        /// 数据开始行索引
        /// </summary>
        public int RataRowIndex
        {
            get { return dataRowIndex; }
            set
            {
                if (dataRowIndex != value)
                {
                    dataRowIndex = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RataRowIndex)));
                }
            }
        }
        private string endColumnName;
        /// <summary>
        /// 结束列名
        /// </summary>
        public string EndColumnName
        {
            get { return endColumnName; }
            set
            {
                if (endColumnName != value)
                {
                    endColumnName = value.ToUpper();
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EndColumnName)));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
    /// <summary>
    /// Sheet试图
    /// </summary>
    public class SheetView : INotifyPropertyChanged
    {
        private string name;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public SheetView()
        {
            name = string.Empty;
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="name"></param>
        public SheetView(string name)
        {
            this.name = name;
        }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name != value)
                {
                    name = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Name)));
                }
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
