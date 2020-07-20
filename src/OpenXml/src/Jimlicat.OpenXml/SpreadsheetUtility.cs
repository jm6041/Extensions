using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using OpenXml.Attributes;

namespace OpenXml
{
    /// <summary>
    /// 电子表格
    /// </summary>
    /// <typeparam name="T">数据源类型</typeparam>
    public class SpreadsheetUtility<T> : IOpenXmlObject<T> where T : class
    {
        /// <summary>
        /// 是否生成表头
        /// </summary>
        private bool? _generateTableHead = null;

        /// <summary>
        /// 导出数据集合
        /// </summary>
        private ICollection<T> _sourceDatas;
        /// <summary>
        /// 导出数据项对应的属性信息
        /// </summary>
        private PropertyInfo[] propertyInfoes;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceDatas">要导出的数据</param>
        public SpreadsheetUtility(IEnumerable<T> sourceDatas)
        {
            if (sourceDatas == null)
            {
                throw new ArgumentNullException(nameof(sourceDatas));
            }
            this._sourceDatas = sourceDatas as ICollection<T>;
            if (this._sourceDatas == null)
            {
                this._sourceDatas = sourceDatas.ToList();
            }
            this.propertyInfoes = GetTypeProperties(_sourceDatas);
        }

        /// <summary>
        /// 通过反射获得属性
        /// </summary>
        /// <param name="datas"></param>
        /// <returns></returns>
        protected static PropertyInfo[] GetTypeProperties(IEnumerable<T> datas)
        {
            Type t = datas.GetType().GetGenericArguments()[0];
            IEnumerable<PropertyInfo> ps = t.GetProperties().Where(x => x.CanRead == true).ToList();
            if (ps == null || ps.Count() == 0)
            {
                T d = datas.FirstOrDefault();
                if (d != default(T))
                {
                    ps = d.GetType().GetProperties().Where(x => x.CanRead == true).ToList();
                }
            }

            List<PropertyInfo> r = new List<PropertyInfo>();
            foreach (PropertyInfo p in ps)
            {
                int count = p.GetCustomAttributes(typeof(NotOpenXmlAttribute), false).Count();
                if (count == 0)
                {
                    r.Add(p);
                }
            }
            return r.ToArray();
        }

        /// <summary>
        /// 导出类型的属性名
        /// </summary>
        /// <returns></returns>
        protected string[] GetHeadNames()
        {
            if (TableHeads != null && TableHeads.Count() > 0)
            {
                return TableHeads.ToArray();
            }

            int pcount = propertyInfoes.Length;

            string[] names = new string[pcount];
            int i = 0;
            for (; i < pcount; i++)
            {
                PropertyInfo p = propertyInfoes[i];
                DisplayOpenXmlAttribute da = p.GetCustomAttributes(typeof(DisplayOpenXmlAttribute), false).FirstOrDefault() as DisplayOpenXmlAttribute;
                if (da != null)
                {
                    names[i] = da.Name;
                }
                else
                {
                    names[i] = p.Name;
                }
            }
            return names;
        }

        private Column CreateColumnFromAttribute(ColumnOpenXmlAttribute ca)
        {
            if (ca == null)
            {
                return null;
            }

            Column c = new Column();
            if (true == ca.Hidden)
            {
                c.Hidden = ca.Hidden;
            }
            if (ca.CustomWidth != 0)
            {
                c.Width = ca.CustomWidth;
                c.CustomWidth = true;
            }
            return c;
        }

        /// <summary>
        /// 获得列设置
        /// </summary>
        /// <returns></returns>
        protected Columns GetColumnSetting()
        {
            int pcount = propertyInfoes.Length;
            if (pcount == 0)
            {
                return null;
            }
            var cas = (from pi in propertyInfoes
                       let ca = pi.GetCustomAttributes(typeof(ColumnOpenXmlAttribute), false).FirstOrDefault() as ColumnOpenXmlAttribute
                       select ca).ToArray();
            var casNotNull = cas.Where(x => x != null).ToArray();
            if (casNotNull == null || casNotNull.Count() < 1)
            {
                return null;
            }
            List<Column> columnList = new List<Column>();
            var casGrop = from ca in casNotNull
                          group ca by new { ca.CustomWidth, ca.Hidden };
            foreach (var cg in casGrop)
            {
                int cgCount = cg.Count();
                if (cgCount > 0)
                {
                    ColumnOpenXmlAttribute caf = cg.FirstOrDefault();
                    int index = Array.IndexOf(cas, caf);
                    int min = index + 1;
                    int max = index + cgCount;
                    Column c = CreateColumnFromAttribute(caf);
                    c.Min = (uint)min;
                    c.Max = (uint)max;
                    columnList.Add(c);
                }
            }
            Columns columns = new Columns(columnList);
            return columns;
        }

        /// <summary>
        /// 表头
        /// </summary>
        public ICollection<String> TableHeads { get; set; }

        /// <summary>
        /// 导出的数据
        /// </summary>
        public ICollection<T> SourceDatas
        {
            get
            {
                return this._sourceDatas;
            }
        }

        /// <summary>
        /// 导出的模板文件
        /// </summary>
        public string TemplateFile { get; set; }
        /// <summary>
        /// 内容开始行索引（0：第一行），默认1
        /// </summary>
        public uint TemplateRowIndexContent { get; set; } = 1;
        /// <summary>
        /// 是否使用模板最后一行的样式
        /// </summary>
        public bool TemplateUseLastRowStyle { get; set; }

        /// <summary>
        /// 是否使用模板
        /// </summary>
        protected bool UseTemplate
        {
            get
            {
                if (string.IsNullOrWhiteSpace(TemplateFile) || !File.Exists(TemplateFile))
                {
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 是否生成表头,如果设置了值，使用设置值，如果没有设置，当使用模板默认为false，否则为true
        /// </summary>
        public bool GenerateTableHead
        {
            get
            {
                if (_generateTableHead != null)
                {
                    return _generateTableHead.Value;
                }
                if (UseTemplate)
                {
                    return false;
                }
                return true;
            }
            set
            {
                _generateTableHead = value;
            }
        }

        /// <summary>
        /// 构造Cell
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="dataType">类型</param>
        /// <param name="styleIndex">样式编号CellFormats中对应的编号</param>
        /// <returns></returns>
        private Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
        }

        /// <summary>
        /// 属性信息
        /// </summary>
        class PropertyDto
        {
            private Type _type;
            private TypeCode _typeCodeValue;
            private CellValues _cellType;
            private DisplayOpenXmlAttribute _displayOpenXmlAttr;

            /// <summary>
            /// 构造函数
            /// </summary>
            /// <param name="type">属性类型</param>
            /// <param name="displayOpenXmlAttr"><see cref="DisplayOpenXmlAttribute"/></param>
            public PropertyDto(Type type, DisplayOpenXmlAttribute displayOpenXmlAttr)
            {
                _type = type;
                _typeCodeValue = TypeUtil.GetTypeCode(_type);
                _cellType = GetCellValues(_typeCodeValue);
                _displayOpenXmlAttr = displayOpenXmlAttr;
            }

            /// <summary>
            /// 根据<see cref="TypeCode"/>获得<see cref="CellValues"/>
            /// </summary>
            /// <param name="tc"><see cref="TypeCode"/></param>
            /// <returns><see cref="CellValues"/></returns>
            private static CellValues GetCellValues(TypeCode tc)
            {
                CellValues cv = CellValues.String;
                if (TypeUtil.IsNumericTypeCode(tc))
                {
                    cv = CellValues.Number;
                }
                return cv;
            }

            /// <summary>
            /// 属性类型
            /// </summary>
            public Type PropertyType
            {
                get
                {
                    return _type;
                }
            }

            /// <summary>
            /// <see cref="TypeCode"/>
            /// </summary>
            public TypeCode TypeCodeValue
            {
                get
                {
                    return _typeCodeValue;
                }
            }

            /// <summary>
            /// <see cref="CellValues"/>
            /// </summary>
            public CellValues CellType
            {
                get
                {
                    return _cellType;
                }
            }
            
            /// <summary>
            /// <see cref="DisplayOpenXmlAttribute"/>
            /// </summary>
            public DisplayOpenXmlAttribute DisplayOpenXmlAttr
            {
                get
                {
                    return _displayOpenXmlAttr;
                }
            }
            /// <summary>
            /// 样式索引，-1表示无样式索引
            /// </summary>
            public int StyleIndex { get; set; } = -1;
            /// <summary>
            /// 列引用标记
            /// </summary>
            public string ColumnReference { get; set; }
        }

        class CellDto
        {
            private uint _styleIndex;
            private string _cellReference;
            public CellDto(string cellReference, uint styleIndex)
            {
                _styleIndex = styleIndex;
                _cellReference = cellReference;
            }

            public int ColumnIndex
            {
                get
                {
                    return CellReferenceUtil.GetColumnIndex(_cellReference);
                }
            }
            public UInt32 StyleIndex
            {
                get
                {
                    return _styleIndex;
                }
            }
        }

        /// <summary>
        /// 添加表头
        /// </summary>
        /// <param name="sheetData"></param>
        private void AddHead(SheetData sheetData)
        {
            // 是否新增文档
            bool isNew = !this.UseTemplate;
            // 表头
            if (GenerateTableHead)
            {
                Row headRow = new Row();
                string[] heads = this.GetHeadNames();
                IEnumerable<Cell> headCells = null;
                if (isNew)
                {
                    headCells = heads.Select(x => ConstructCell(x, CellValues.String, 1));
                }
                else
                {
                    headCells = heads.Select(x => ConstructCell(x, CellValues.String));
                }

                headRow.Append(headCells);
                sheetData.AppendChild(headRow);
            }
        }

        /// <summary>
        /// 确定属性信息
        /// </summary>
        /// <param name="sheetData"><see cref="SheetData"/>对象</param>
        /// <returns><see cref="PropertyDto"/>对象数组</returns>
        private PropertyDto[] GetPropertyDtos(SheetData sheetData)
        {
            int pcount = propertyInfoes.Length;
            uint i = 0;
            PropertyDto[] pds = new PropertyDto[pcount];
            for (; i < pcount; i++)
            {
                PropertyInfo pi = propertyInfoes[i];
                DisplayOpenXmlAttribute displayAtt = pi.GetCustomAttributes(typeof(DisplayOpenXmlAttribute), false).FirstOrDefault() as DisplayOpenXmlAttribute;
                PropertyDto pd = new PropertyDto(pi.PropertyType, displayAtt);
                pd.ColumnReference = CellReferenceUtil.GetColumnReference(i);
                pds[i] = pd;
            }

            // 使用模板并且使用最后一行的样式
            if (UseTemplate && TemplateUseLastRowStyle)
            {
                // 最后一行
                Row rl = sheetData.LastChild as Row;
                if (rl != null)
                {
                    IEnumerable<Cell> cls = rl.Elements<Cell>().Where(x => false == string.IsNullOrEmpty(x.CellReference));
                    Dictionary<int, CellDto> dic = cls.Select(x => new CellDto(x.CellReference, x.StyleIndex)).ToDictionary(x => x.ColumnIndex);
                    if (dic.Count() > 0)
                    {
                        int j = 0;
                        for (; j < pcount; j++)
                        {
                            PropertyDto pd = pds[j];
                            CellDto cd = null;
                            bool s = dic.TryGetValue(j, out cd);
                            if (true == s && cd != null)
                            {
                                pd.StyleIndex = (int)cd.StyleIndex;
                            }
                        }
                    }
                }
            }
            return pds;
        }

        /// <summary>
        /// 移除冗余的行
        /// </summary>
        /// <param name="sheetData"><see cref="SheetData"/>对象</param>
        private void RemoveRedundancyRows(SheetData sheetData)
        {
            uint ri = TemplateRowIndexContent;
            if (UseTemplate && ri > 0)
            {
                Row[] rows = sheetData.Elements<Row>().ToArray();
                int count = rows.Length;
                int i = (int)ri;
                for (; i < count; i++)
                {
                    rows[i].Remove();
                }
            }
        }

        /// <summary>
        /// 添加内容部分
        /// </summary>
        /// <param name="sheetData"><see cref="SheetData"/>对象</param>
        /// <param name="pds"><see cref="PropertyDto"/>对象数组</param>
        private void AddBody(SheetData sheetData, PropertyDto[] pds)
        {
            // 是否新增文档
            bool isNew = !this.UseTemplate;
            // 有多少属性，一行就会有多少格
            int pcount = propertyInfoes.Length;
            // 行索引
            uint rowIndex = TemplateRowIndexContent;
            foreach (T data in _sourceDatas)
            {
                Row row = new Row();
                Cell[] cells = new Cell[pcount];
                uint k = 0;
                for (; k < pcount; k++)
                {
                    PropertyInfo pi = propertyInfoes[k];
                    PropertyDto pd = pds[k];
                    object v = pi.GetValue(data, null);
                    string f = "{0}";
                    if (pd.DisplayOpenXmlAttr != null && !string.IsNullOrEmpty(pd.DisplayOpenXmlAttr.Formate))
                    {
                        f = "{0:" + pd.DisplayOpenXmlAttr.Formate + "}";
                    }
                    string value = string.Format(f, v);
                    if (pd.TypeCodeValue == TypeCode.Boolean && v != null && pd.DisplayOpenXmlAttr != null &&
                        (!string.IsNullOrEmpty(pd.DisplayOpenXmlAttr.BoolTrueText) || !string.IsNullOrEmpty(pd.DisplayOpenXmlAttr.BoolFalseText)))
                    {
                        bool bv = (bool)v;
                        if (true == bv && !string.IsNullOrEmpty(pd.DisplayOpenXmlAttr.BoolTrueText))
                        {
                            value = pd.DisplayOpenXmlAttr.BoolTrueText;
                        }
                        if (false == bv && !string.IsNullOrEmpty(pd.DisplayOpenXmlAttr.BoolFalseText))
                        {
                            value = pd.DisplayOpenXmlAttr.BoolFalseText;
                        }
                    }

                    Cell cell = ConstructCell(value, pd.CellType);
                    if (isNew)
                    {
                        cell.StyleIndex = 2;
                    }
                    else
                    {
                        cell.CellReference = string.Format("{0}{1}", pd.ColumnReference, rowIndex + 1);
                        if (pd.StyleIndex >= 0)
                        {
                            cell.StyleIndex = (uint)pd.StyleIndex;
                        }
                    }
                    cells[k] = cell;
                }
                if (!isNew)
                {
                    rowIndex++;
                }
                row.Append(cells);
                sheetData.AppendChild(row);
            }
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sheetData"><see cref="SheetData"/>对象</param>
        /// <param name="worksheetPart"><see cref="WorksheetPart"/>对象</param>
        protected void AddData(SheetData sheetData,WorksheetPart worksheetPart)
        {
            AddHead(sheetData);
            worksheetPart.Worksheet.Save();
            PropertyDto[] pds = GetPropertyDtos(sheetData);
            RemoveRedundancyRows(sheetData);
            worksheetPart.Worksheet.Save();
            AddBody(sheetData, pds);
            worksheetPart.Worksheet.Save();
        }

        /// <summary>
        /// 确定<see cref="MemoryStream"/>不为空
        /// </summary>
        /// <param name="memoryStream"><see cref="MemoryStream"/>对象</param>
        private void MemoryStreamNotNull(MemoryStream memoryStream)
        {
            if (memoryStream == null)
            {
                memoryStream = new MemoryStream();
            }
        }

        private Stylesheet GenerateStylesheet()
        {
            Stylesheet styleSheet = null;

            // 字体
            Fonts fonts = new Fonts(
                new Font( // 0 默认
                ),
                new Font( // 1 表头
                    new Bold()
                ));

            // 填充
            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }) // 0 默认
                );

            // 边框
            Borders borders = new Borders(
                    new Border(), // 0 默认
                    new Border(   // 1 四周边框
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                );

            CellFormats cellFormats = new CellFormats(
                    new CellFormat(), // 默认
                    new CellFormat { FontId = 1, FillId = 0, BorderId = 1, ApplyBorder = true }, // 表头
                    new CellFormat { FontId = 0, FillId = 0, BorderId = 1, ApplyBorder = true }  // 内容
                );

            styleSheet = new Stylesheet(fonts, fills, borders, cellFormats);

            return styleSheet;
        }

        /// <summary>
        /// 使用模板导出
        /// </summary>
        private void ExportUseTemplate(MemoryStream memoryStream)
        {
            MemoryStreamNotNull(memoryStream);
            using (FileStream fs = new FileStream(TemplateFile, FileMode.Open, FileAccess.Read))
            {
                fs.Position = 0;
                fs.CopyTo(memoryStream);
            }
            using (SpreadsheetDocument document = SpreadsheetDocument.Open(memoryStream, true))
            {
                WorksheetPart worksheetPart = document.WorkbookPart.WorksheetParts.FirstOrDefault();
                SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().FirstOrDefault();
                AddData(sheetData, worksheetPart);
                worksheetPart.Worksheet.Save();
            }
        }

        /// <summary>
        /// Sheet名
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 获得Sheet名
        /// </summary>
        /// <returns></returns>
        protected string GetSheetName()
        {
            if (string.IsNullOrWhiteSpace(SheetName))
            {
                return typeof(T).Name;
            }
            return SheetName;
        }

        /// <summary>
        /// 新生成文档时，是否生成并使用默认样式，默认为true（整个表格添加边框，表头字体加粗）
        /// </summary>
        public bool UseDefalutStyle { get; set; } = true;

        /// <summary>
        /// 不使用模板导出
        /// </summary>
        private void ExportNoTemplate(MemoryStream memoryStream)
        {
            MemoryStreamNotNull(memoryStream);
            string sn = GetSheetName();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(memoryStream, SpreadsheetDocumentType.Workbook))
            {
                // WorkbookPart
                WorkbookPart workbookPart = document.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                if (UseDefalutStyle)
                {
                    // 样式
                    WorkbookStylesPart stylePart = workbookPart.AddNewPart<WorkbookStylesPart>();
                    stylePart.Stylesheet = GenerateStylesheet();
                    stylePart.Stylesheet.Save();
                }

                // WorksheetPart
                WorksheetPart worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();

                // 列设置
                Columns columns = this.GetColumnSetting();
                if (columns != null && columns.Count() > 0)
                {
                    worksheetPart.Worksheet.AppendChild(columns);
                }

                // Sheets
                Sheets sheets = workbookPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = workbookPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = sn };
                sheets.Append(sheet);
                // 保存
                workbookPart.Workbook.Save();

                // SheetData
                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                // 添加数据
                AddData(sheetData, worksheetPart);
                // 保存
                workbookPart.Workbook.Save();
            }
        }

        /// <summary>
        /// 导出为Stream
        /// </summary>
        /// <returns></returns>
        public MemoryStream Export()
        {
            MemoryStream ms = new MemoryStream();
            if (UseTemplate)
            {
                ExportUseTemplate(ms);
            }
            else
            {
                ExportNoTemplate(ms);
            }
            return ms;
        }

        /// <summary>
        /// 导出到文件
        /// </summary>
        /// <param name="exportFile">导出文件</param>
        public void Export(string exportFile)
        {
            FileStream fs = new FileStream(exportFile, FileMode.Create);
            Stream s = null;
            try
            {
                s = this.Export();
                s.Position = 0;
                s.CopyTo(fs);
                s.Flush();
                fs.Flush();
            }
            finally
            {
                if (s != null)
                {
                    s.Close();
                    s.Dispose();
                }
                fs.Close();
                fs.Dispose();
            }
        }
    }
}