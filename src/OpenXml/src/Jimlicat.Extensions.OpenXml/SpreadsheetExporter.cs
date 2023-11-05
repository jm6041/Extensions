using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Jimlicat.OpenXml
{
    /// <summary>
    /// 电子表格导出
    /// </summary>
    public class SpreadsheetExporter<T> : ISpreadsheetExporter where T : class
    {
        /// <summary>
        /// 导出数据集合
        /// </summary>
        private readonly IEnumerable<T> _sourceDatas;
        /// <summary>
        /// Sheet名
        /// </summary>
        private readonly string sheetName;
        /// <summary>
        /// <see cref="ExpandoObject"/> 数据
        /// </summary>
        private readonly ICollection<ExpandoObject>? _expandoObjectData;
        /// <summary>
        /// 是否 <see cref="ExpandoObject"/> 数据
        /// </summary>
        private readonly bool isExpandoObjectData = false;
        /// <summary>
        /// <see cref="SpreadsheetInfo"/>
        /// </summary>
        private readonly SpreadsheetInfo _info;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceDatas">要导出的数据</param>
        /// <param name="info"><see cref="SpreadsheetInfo"/></param>
        public SpreadsheetExporter(IEnumerable<T> sourceDatas, SpreadsheetInfo info)
        {
            _sourceDatas = sourceDatas ?? throw new ArgumentNullException(nameof(sourceDatas));
            _info = info;
            sheetName = _info.SheetName.Trim();
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = typeof(T).Name;
            }
            SetPropertyInfo(typeof(T), _info.Columns);
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceDatas">要导出的数据</param>
        /// <param name="info"><see cref="SpreadsheetInfo"/></param>
        public SpreadsheetExporter(ICollection<ExpandoObject> sourceDatas, SpreadsheetInfo info)
        {
            _expandoObjectData = sourceDatas ?? throw new ArgumentNullException(nameof(sourceDatas));
            _sourceDatas = Enumerable.Empty<T>();
            _info = info;
            isExpandoObjectData = true;
            foreach (var c in _info.Columns)
            {
                c.Width ??= 18;
            }
            sheetName = _info.SheetName.Trim();
            if (string.IsNullOrEmpty(sheetName))
            {
                sheetName = "Data";
            }
        }
        private static readonly Dictionary<Type, double> TypeWidthDic = new Dictionary<Type, double>()
        {
            { typeof(string), 18 },
            { typeof(DateTime), 18 },
            { typeof(DateTime?), 18 },
            { typeof(DateTimeOffset), 18 },
            { typeof(DateTimeOffset?), 18 },
            { typeof(Guid), 18 },
            { typeof(Guid?), 18 },
        };
        /// <summary>
        /// 通过反射设置属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="columnInfos"></param>
        private static void SetPropertyInfo(Type type, ICollection<ColumnInfo> columnInfos)
        {
            var dict = type.GetRuntimeProperties().Where(x => x.CanRead).ToDictionary(k => k.Name);
            foreach (var columnInfo in columnInfos)
            {
                if (dict.ContainsKey(columnInfo.PropertyName))
                {
                    var pinfo = dict[columnInfo.PropertyName];
                    columnInfo.PropertyInfo = pinfo;
                    if (columnInfo.Width == null && TypeWidthDic.TryGetValue(pinfo.PropertyType, out var w))
                    {
                        columnInfo.Width = w;
                    }
                }
            }
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public void Export(string path, SpreadsheetDocumentType type = SpreadsheetDocumentType.Workbook)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Create(path, SpreadsheetDocumentType.Workbook);
            CreateParts(document);
        }
        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public void Export(Stream stream, SpreadsheetDocumentType type = SpreadsheetDocumentType.Workbook)
        {
            using SpreadsheetDocument document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
            CreateParts(document);
        }

        private void CreateParts(SpreadsheetDocument document)
        {
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            SpreadsheetExporter<T>.GenerateWorkbookStylesPart(workbookStylesPart1);

            WorksheetPart worksheetPart1 = workbookPart1.AddNewPart<WorksheetPart>("rId1");
            GenerateWorksheetPartContent(worksheetPart1);

            SharedStringTablePart sharedStringTablePart1 = workbookPart1.AddNewPart<SharedStringTablePart>("rId4");
            GenerateSharedStringTablePartContent(sharedStringTablePart1);
        }

        private void GenerateWorkbookPartContent(WorkbookPart workbookPart)
        {
            Workbook workbook = new Workbook() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x15 xr xr6 xr10 xr2" } };
            workbook.AddNamespaceDeclaration("r", "http://schemas.openxmlformats.org/officeDocument/2006/relationships");
            workbook.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            workbook.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");
            workbook.AddNamespaceDeclaration("xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision");
            workbook.AddNamespaceDeclaration("xr6", "http://schemas.microsoft.com/office/spreadsheetml/2016/revision6");
            workbook.AddNamespaceDeclaration("xr10", "http://schemas.microsoft.com/office/spreadsheetml/2016/revision10");
            workbook.AddNamespaceDeclaration("xr2", "http://schemas.microsoft.com/office/spreadsheetml/2015/revision2");

            Sheets sheets = new Sheets();
            Sheet sheet = new Sheet() { Name = sheetName, SheetId = (UInt32Value)1U, Id = "rId1" };

            sheets.Append(sheet);
            workbook.Append(sheets);

            workbookPart.Workbook = workbook;
        }

        /// <summary>
        /// 样式
        /// </summary>
        /// <returns></returns>
        private static void GenerateWorkbookStylesPart(WorkbookStylesPart workbookStylesPart)
        {
            Stylesheet styleSheet = new Stylesheet() { MCAttributes = new MarkupCompatibilityAttributes() { Ignorable = "x14ac x16r2 xr" } };
            styleSheet.AddNamespaceDeclaration("mc", "http://schemas.openxmlformats.org/markup-compatibility/2006");
            styleSheet.AddNamespaceDeclaration("x14ac", "http://schemas.microsoft.com/office/spreadsheetml/2009/9/ac");
            styleSheet.AddNamespaceDeclaration("x16r2", "http://schemas.microsoft.com/office/spreadsheetml/2015/02/main");
            styleSheet.AddNamespaceDeclaration("xr", "http://schemas.microsoft.com/office/spreadsheetml/2014/revision");

            // 字体
            Fonts fonts = new Fonts(
                // 0 默认
                new Font(),
                // 1 表头
                new Font(
                    new Bold()
                ));

            // 填充
            Fills fills = new Fills(
                    new Fill(new PatternFill() { PatternType = PatternValues.None }), // 0 默认
                    new Fill(new PatternFill() { PatternType = PatternValues.Gray125 }) // 0 默认
                );

            // 边框
            Borders borders = new Borders(
                    // 0 默认，无边框
                    new Border(
                        new LeftBorder(),
                        new RightBorder(),
                        new TopBorder(),
                        new BottomBorder(),
                        new DiagonalBorder()),
                    new Border(   // 1 四周边框
                        new LeftBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new RightBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new TopBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new BottomBorder(new Color() { Auto = true }) { Style = BorderStyleValues.Thin },
                        new DiagonalBorder())
                    );

            // 表格总体样式格式
            CellStyleFormats cellStyleFormats = new CellStyleFormats(
                new CellFormat() { NumberFormatId = 0, FontId = 0, FillId = 0, BorderId = 0 }
                );

            // 表格格式
            CellFormats cellFormats = new CellFormats(
                    new CellFormat() { NumberFormatId = 0, FontId = 0, FillId = 0, BorderId = 0, FormatId = 0 }, // 默认
                    new CellFormat() { NumberFormatId = 0, FontId = 1, FillId = 0, BorderId = 1, FormatId = 0, ApplyFont = true, ApplyBorder = true }, // 表头
                    new CellFormat() { NumberFormatId = 0, FontId = 0, FillId = 0, BorderId = 1, FormatId = 0, ApplyBorder = true },  // 文本内容
                    new CellFormat() { NumberFormatId = 0, FontId = 0, FillId = 0, BorderId = 1, FormatId = 0, ApplyNumberFormat = true, ApplyBorder = true }  // 数字内容
                );

            // 表格样式
            CellStyles cellStyles = new CellStyles(
                new CellStyle() { FormatId = 0, BuiltinId = 0 }
                );

            styleSheet.Append(fonts);
            styleSheet.Append(fills);
            styleSheet.Append(borders);
            styleSheet.Append(cellStyleFormats);
            styleSheet.Append(cellFormats);
            styleSheet.Append(cellStyles);

            workbookStylesPart.Stylesheet = styleSheet;
        }
        // 共享字符串与索引对应关系
        private readonly Dictionary<string, string> stringIndexDic = new Dictionary<string, string>();
        private uint curIndex = 0;
        private uint totalCount = 0;
        /// <summary>
        /// Put 共享字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns>字符串索引</returns>
        private string PutSharedString(string str)
        {
            totalCount++;
            if (stringIndexDic.TryGetValue(str, out var indexStr))
            {
                return indexStr;
            }
            else
            {
                indexStr = curIndex.ToString();
                stringIndexDic.Add(str, indexStr);
                curIndex++;
                return indexStr;
            }
        }
        /// <summary>
        /// 构造Cell
        /// </summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        private Cell ConstructHeadCell(string val)
        {
            Cell cell = new Cell();
            CellSetString(cell, val, 1);
            return cell;
        }
        /// <summary>
        /// 构造Cell
        /// </summary>
        /// <param name="val">值</param>
        /// <param name="columnInfo"></param>
        /// <returns></returns>
        private Cell ConstructCell(object? val, ColumnInfo columnInfo)
        {
            Cell cell = new Cell();
            if (val is null)
            {
                CellSetString(cell, string.Empty);
            }
            else if (val is string vstr)
            {
                CellSetString(cell, vstr);
            }
            else if (val is double vd && double.IsNaN(vd))
            {
                CellSetString(cell, double.NaN.ToString());
            }
            else if (Utils.IsNumericType(val))
            {
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                cell.StyleIndex = 3;
                cell.CellValue = new CellValue(val.ToString()!);
            }
            else if (val is DateTimeOffset vdto)
            {
                string fv = FormatDateTimeOffset(vdto, columnInfo.FormatString);
                CellSetString(cell, fv);
            }
            else if (val is DateTimeOffset?)
            {
                DateTimeOffset? nvdto = (DateTimeOffset?)val;
                string fv = FormatDateTimeOffset(nvdto.Value, columnInfo.FormatString);
                CellSetString(cell, fv);
            }
            else if (val is DateTime vdt)
            {
                string fv = SpreadsheetExporter<T>.FormatDateTime(vdt, columnInfo.FormatString);
                CellSetString(cell, fv);
            }
            else if (val is DateTime?)
            {
                DateTime? nvdt = (DateTime?)val;
                string fv = SpreadsheetExporter<T>.FormatDateTime(nvdt.Value, columnInfo.FormatString);
                CellSetString(cell, fv);
            }
            else if (val is Guid gv)
            {
                string fv = FormatGuid(gv, columnInfo.FormatString);
                CellSetString(cell, fv);
            }
            else if (val is Guid?)
            {
                Guid? ngv = (Guid?)val;
                string fv = FormatGuid(ngv.Value, columnInfo.FormatString);
                CellSetString(cell, fv);
            }
            else if (val is bool vb)
            {
                string fv = FormatBool(vb, columnInfo);
                CellSetString(cell, fv);
            }
            else if (val is bool?)
            {
                bool? nvb = (bool?)val;
                string fv = FormatBool(nvb.Value, columnInfo);
                CellSetString(cell, fv);
            }
            else if (val.GetType().IsEnum)
            {
                var valType = val.GetType();
                var name = Enum.GetName(valType, val);
                var text = string.Empty;
                if (name != null)
                {
                    FieldInfo? field = valType.GetField(name);
                    if (field != null)
                    {
                        var attribute = field.GetCustomAttribute<DescriptionAttribute>();
                        if (attribute != null && !string.IsNullOrEmpty(attribute.Description))
                        {
                            text = attribute.Description;
                        }
                        else
                        {
                            text = val.ToString();
                        }
                    }
                    else
                    {
                        text = val.ToString();
                    }
                }
                text ??= string.Empty;
                CellSetString(cell, text);
            }
            else
            {
                cell.CellValue = new CellValue(val.ToString() ?? string.Empty);
                cell.DataType = new EnumValue<CellValues>(CellValues.String);
                cell.StyleIndex = 2;
            }
            return cell;
        }
        /// <summary>
        /// Cell 设置字符串
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="str"></param>
        /// <param name="styleIndex">样式索引</param>
        /// <returns></returns>
        private Cell CellSetString(Cell cell, string str, uint styleIndex = 2)
        {
            cell.DataType = new EnumValue<CellValues>(CellValues.SharedString);
            cell.StyleIndex = styleIndex;
            string indexStr = PutSharedString(str);
            cell.CellValue = new CellValue(indexStr);
            return cell;
        }

        private string FormatBool(bool v, ColumnInfo columnInfo)
        {
            if (v)
            {
                if (!string.IsNullOrEmpty(columnInfo.BoolTrueText))
                {
                    return columnInfo.BoolTrueText;
                }
                else
                {
                    if (_info.BoolValueTranslate && !string.IsNullOrEmpty(_info.BoolTrueText))
                    {
                        return _info.BoolTrueText;
                    }
                    else
                    {
                        return v.ToString();
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(columnInfo.BoolFalseText))
                {
                    return columnInfo.BoolFalseText;
                }
                else
                {
                    if (_info.BoolValueTranslate && !string.IsNullOrEmpty(_info.BoolFalseText))
                    {
                        return _info.BoolFalseText;
                    }
                    else
                    {
                        return v.ToString();
                    }
                }
            }
        }

        private static string FormatDateTime(DateTime v, string format)
        {
            string fv;
            if (string.IsNullOrEmpty(format))
            {
                fv = v.ToString(format);
            }
            else
            {
                fv = v.ToString();
            }
            return fv;
        }

        private static string FormatDateTimeOffset(DateTimeOffset v, string format)
        {
            string fv;
            if (string.IsNullOrEmpty(format))
            {
                fv = v.ToString(format);
            }
            else
            {
                fv = v.ToString();
            }
            return fv;
        }

        private static string FormatGuid(Guid v, string format)
        {
            string fv;
            if (string.IsNullOrEmpty(format))
            {
                fv = v.ToString(format);
            }
            else
            {
                fv = v.ToString();
            }
            return fv;
        }

        /// <summary>
        /// 表格内容
        /// </summary>
        /// <param name="worksheetPart"></param>
        private void GenerateWorksheetPartContent(WorksheetPart worksheetPart)
        {
            using OpenXmlWriter writer = OpenXmlWriter.Create(worksheetPart);
            var columnInfos = _info.Columns;
            // worksheet 开始
            writer.WriteStartElement(new Worksheet());

            // 是否有自定义列
            bool hasCustomColume = columnInfos.Any(x => x.Width != null);
            if (hasCustomColume)
            {
                // cols 开始
                writer.WriteStartElement(new Columns());
            }
            int columnCount = columnInfos.Count;
            // 表头
            Row headRow = new Row() { RowIndex = 1, Spans = new ListValue<StringValue>() { InnerText = $"1:{columnCount}" } };
            Cell[] headCells = new Cell[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                ColumnInfo citem = columnInfos[i];
                uint cindex = (uint)i + 1;
                if (citem.Width != null)
                {
                    var column = new Column() { Min = cindex, Max = cindex, Width = citem.Width.Value, BestFit = true, CustomWidth = true };
                    // col
                    writer.WriteElement(column);
                }

                Cell cell = ConstructHeadCell(citem.Show);
                cell.CellReference = CellReferenceUtil.GetCellReference(0, (uint)i);
                headCells[i] = cell;
            }
            headRow.Append(headCells);
            if (hasCustomColume)
            {
                // cols 结束
                writer.WriteEndElement();
            }

            // sheetData 开始
            writer.WriteStartElement(new SheetData());

            // 写入表头行
            writer.WriteElement(headRow);

            // 写入数据
            uint rowIndex = 2;
            if (isExpandoObjectData && _expandoObjectData != null)
            {
                foreach (IDictionary<string, object?> dataDic in _expandoObjectData)
                {
                    Row row = new Row() { RowIndex = rowIndex, Spans = new ListValue<StringValue>() { InnerText = $"1:{columnCount}" } };
                    // row 开始
                    writer.WriteStartElement(row);
                    for (int k = 0; k < columnCount; k++)
                    {
                        ColumnInfo citem = columnInfos[k];
                        dataDic.TryGetValue(citem.PropertyName, out object? v);
                        Cell cell = ConstructCell(v, citem);
                        cell.CellReference = CellReferenceUtil.GetCellReference(rowIndex - 1, (uint)k);
                        // 写入 c
                        writer.WriteElement(cell);
                    }
                    // row 结束
                    writer.WriteEndElement();
                    rowIndex++;
                }
            }
            else
            {
                foreach (T data in _sourceDatas)
                {
                    Row row = new Row() { RowIndex = rowIndex, Spans = new ListValue<StringValue>() { InnerText = $"1:{columnCount}" } };
                    // row 开始
                    writer.WriteStartElement(row);
                    for (int k = 0; k < columnCount; k++)
                    {
                        ColumnInfo citem = columnInfos[k];
                        if (citem.PropertyInfo != null)
                        {
                            object? v = citem.PropertyInfo.GetValue(data, null);
                            Cell cell = ConstructCell(v, citem);
                            cell.CellReference = CellReferenceUtil.GetCellReference(rowIndex - 1, (uint)k);
                            // 写入 c
                            writer.WriteElement(cell);
                        }
                    }
                    // row 结束
                    writer.WriteEndElement();
                    rowIndex++;
                }
            }
            // sheetData 结束
            writer.WriteEndElement();
            // worksheet 结束
            writer.WriteEndElement();

            writer.Close();
        }
        /// <summary>
        /// 共享字符串
        /// </summary>
        /// <param name="sharedStringTablePart"></param>
        private void GenerateSharedStringTablePartContent(SharedStringTablePart sharedStringTablePart)
        {
            using OpenXmlWriter writer = OpenXmlWriter.Create(sharedStringTablePart);
            // SharedStringTable 开始
            writer.WriteStartElement(new SharedStringTable() { Count = totalCount, UniqueCount = (uint)stringIndexDic.Keys.Count });

            foreach (var str in stringIndexDic.Keys)
            {
                // SharedStringItem 开始
                writer.WriteStartElement(new SharedStringItem());

                // 写入 Text
                writer.WriteElement(new Text(str));

                // SharedStringItem 结束
                writer.WriteEndElement();
            }

            // SharedStringTable 结束
            writer.WriteEndElement();

            writer.Close();
        }

    }
}
