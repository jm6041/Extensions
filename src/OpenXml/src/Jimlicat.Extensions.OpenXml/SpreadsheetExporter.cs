using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

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
        /// 属性名对应属性字典
        /// </summary>
        private readonly Dictionary<string, PropertyInfo> _propertyDic;
        private readonly ColumnCollection _columnCollection;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="sourceDatas">要导出的数据</param>
        /// <param name="columns">列信息</param>
        public SpreadsheetExporter(IEnumerable<T> sourceDatas, ColumnCollection columns)
        {
            if (sourceDatas == null)
            {
                throw new ArgumentNullException(nameof(sourceDatas));
            }
            _sourceDatas = sourceDatas;
            _columnCollection = columns;
            _propertyDic = GetPropertyDic(typeof(T));
            SheetName = typeof(T).Name;
        }

        /// <summary>
        /// 通过反射获得属性
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static Dictionary<string, PropertyInfo> GetPropertyDic(Type type)
        {
            return type.GetRuntimeProperties().Where(x => x.CanRead).ToDictionary(k => k.Name);
        }

        /// <summary>
        /// Sheet名
        /// </summary>
        public string SheetName { get; set; }

        /// <summary>
        /// 导出
        /// </summary>
        /// <returns></returns>
        public MemoryStream Export()
        {
            MemoryStream ms = new MemoryStream();
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(ms, SpreadsheetDocumentType.Workbook))
            {
                CreateParts(document);
            }
            return ms;
        }


        private void CreateParts(SpreadsheetDocument document)
        {
            WorkbookPart workbookPart1 = document.AddWorkbookPart();
            GenerateWorkbookPartContent(workbookPart1);

            WorkbookStylesPart workbookStylesPart1 = workbookPart1.AddNewPart<WorkbookStylesPart>("rId3");
            GenerateWorkbookStylesPart(workbookStylesPart1);

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

            OpenXmlUnknownElement openXmlUnknownElement2 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<xr:revisionPtr revIDLastSave=\"0\" documentId=\"13_ncr:1_{430224E7-E52E-4CB2-AD4B-D26B961B38B6}\" xr6:coauthVersionLast=\"45\" xr6:coauthVersionMax=\"45\" xr10:uidLastSave=\"{00000000-0000-0000-0000-000000000000}\" xmlns:xr10=\"http://schemas.microsoft.com/office/spreadsheetml/2016/revision10\" xmlns:xr6=\"http://schemas.microsoft.com/office/spreadsheetml/2016/revision6\" xmlns:xr=\"http://schemas.microsoft.com/office/spreadsheetml/2014/revision\" />");

            Sheets sheets = new Sheets();
            Sheet sheet = new Sheet() { Name = SheetName, SheetId = (UInt32Value)1U, Id = "rId1" };

            sheets.Append(sheet);

            WorkbookExtensionList workbookExtensionList = new WorkbookExtensionList();

            WorkbookExtension workbookExtension1 = new WorkbookExtension() { Uri = "{140A7094-0E35-4892-8432-C4D2E57EDEB5}" };
            workbookExtension1.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

            OpenXmlUnknownElement openXmlUnknownElement3 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<x15:workbookPr chartTrackingRefBase=\"1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");

            workbookExtension1.Append(openXmlUnknownElement3);
            workbookExtensionList.Append(workbookExtension1);

            workbook.Append(openXmlUnknownElement2);
            workbook.Append(sheets);
            workbook.Append(workbookExtensionList);

            workbookPart.Workbook = workbook;
        }

        /// <summary>
        /// 样式
        /// </summary>
        /// <returns></returns>
        private void GenerateWorkbookStylesPart(WorkbookStylesPart workbookStylesPart)
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

            StylesheetExtensionList stylesheetExtensionList = new StylesheetExtensionList();

            StylesheetExtension stylesheetExtensionX15 = new StylesheetExtension() { Uri = "{9260A510-F301-46a8-8635-F512D64BE5F5}" };
            stylesheetExtensionX15.AddNamespaceDeclaration("x15", "http://schemas.microsoft.com/office/spreadsheetml/2010/11/main");

            OpenXmlUnknownElement openXmlUnknownElementX15 = OpenXmlUnknownElement.CreateOpenXmlUnknownElement("<x15:timelineStyles defaultTimelineStyle=\"TimeSlicerStyleLight1\" xmlns:x15=\"http://schemas.microsoft.com/office/spreadsheetml/2010/11/main\" />");

            stylesheetExtensionX15.Append(openXmlUnknownElementX15);
            stylesheetExtensionList.Append(stylesheetExtensionX15);

            styleSheet.Append(fonts);
            styleSheet.Append(fills);
            styleSheet.Append(borders);
            styleSheet.Append(cellStyleFormats);
            styleSheet.Append(cellFormats);
            styleSheet.Append(cellStyles);
            styleSheet.Append(stylesheetExtensionList);

            workbookStylesPart.Stylesheet = styleSheet;
        }
        /// <summary>
        /// 构造Cell
        /// </summary>
        /// <param name="value">值</param>
        /// <param name="dataType">类型</param>
        /// <param name="styleIndex">样式编号CellFormats中对应的编号</param>
        /// <returns></returns>
        private static Cell ConstructCell(string value, CellValues dataType, uint styleIndex = 0)
        {
            return new Cell()
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(dataType),
                StyleIndex = styleIndex
            };
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
            if (stringIndexDic.TryGetValue(str, out string indexStr))
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
            if (val == null)
            {
                val = string.Empty;
            }
            CellSetString(cell, val, 1);
            return cell;
        }
        /// <summary>
        /// 构造Cell
        /// </summary>
        /// <param name="val">值</param>
        /// <returns></returns>
        private Cell ConstructCell(object val)
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
            else if (Utils.IsNumericType(val))
            {
                cell.DataType = new EnumValue<CellValues>(CellValues.Number);
                cell.StyleIndex = 3;
                cell.CellValue = new CellValue(val.ToString());
            }
            else if (val is DateTimeOffset vdto)
            {
                var cv = new CellValue(vdto);
                CellSetString(cell, cv.InnerText);
            }
            else if (val is DateTimeOffset?)
            {
                DateTimeOffset? nvdto = (DateTimeOffset?)val;
                var cv = new CellValue(nvdto.Value);
                CellSetString(cell, cv.InnerText);
            }
            else if (val is DateTime vdt)
            {
                var cv = new CellValue(vdt);
                CellSetString(cell, cv.InnerText);
            }
            else if (val is DateTime?)
            {
                DateTime? nvdt = (DateTime?)val;
                var cv = new CellValue(nvdt.Value);
                CellSetString(cell, cv.InnerText);
            }
            else if (val is Guid gv)
            {
                CellSetString(cell, gv.ToString());
            }
            else if (val is Guid?)
            {
                Guid? ngv = (Guid?)val;
                CellSetString(cell, ngv.Value.ToString());
            }
            else
            {
                cell.CellValue = new CellValue(val.ToString());
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
        /// <summary>
        /// 格式化文本
        /// </summary>
        /// <param name="v"></param>
        /// <param name="dt">数据类型</param>
        /// <returns></returns>
        private string FormatValue(object v, Type dt)
        {
            if (dt == typeof(bool))
            {
                bool vv = (bool)v;
                return FormatBool(vv);
            }
            else if (dt == typeof(bool?))
            {
                if (v == null)
                {
                    return string.Empty;
                }
                bool vv = ((bool?)v).Value;
                return FormatBool(vv);
            }
            else if (dt.DeclaringType == typeof(DateTime))
            {
                DateTime vv = (DateTime)v;
                return FormatDateTime(vv);
            }
            else if (dt.DeclaringType == typeof(DateTime?))
            {
                if (v == null)
                {
                    return string.Empty;
                }
                DateTime vv = (DateTime)v;
                return FormatDateTime(vv);
            }
            else
            {
                return v == null ? "" : v.ToString();
            }
        }

        private string FormatBool(bool v)
        {
            string vs;
            if (v)
            {
                vs = "√";
            }
            else
            {
                vs = "×";
            }
            return vs;
        }

        private string FormatDateTime(DateTime v)
        {
            string vs;
            if (v.Hour == 0 && v.Minute == 0 && v.Second == 0)
            {
                vs = v.ToString("yyyy-MM-dd");
            }
            else
            {
                vs = v.ToString("yyyy-MM-dd HH:mm:ss");
            }
            return vs;
        }

        /// <summary>
        /// 表格内容
        /// </summary>
        /// <param name="worksheetPart"></param>
        private void GenerateWorksheetPartContent(WorksheetPart worksheetPart)
        {
            using (OpenXmlWriter writer = OpenXmlWriter.Create(worksheetPart))
            {
                // worksheet 开始
                writer.WriteStartElement(new Worksheet());

                // cols 开始
                writer.WriteStartElement(new Columns());
                int columnCount = _columnCollection.Items.Count;
                // 表头
                Row headRow = new Row() { RowIndex = 1 };
                Cell[] headCells = new Cell[columnCount];
                for (int i = 0; i < columnCount; i++)
                {
                    ColumnItem citem = _columnCollection.Items[i];
                    uint cindex = (uint)i + 1;
                    Column column = new Column() { Min = cindex, Max = cindex, Width = citem.Width, CustomWidth = true };
                    // col
                    writer.WriteElement(column);
                    Cell cell = ConstructHeadCell(citem.Show);
                    cell.CellReference = CellReferenceUtil.GetCellReference(0, (uint)i);
                    headCells[i] = cell;
                }
                headRow.Append(headCells);
                // cols 结束
                writer.WriteEndElement();

                // sheetData 开始
                writer.WriteStartElement(new SheetData());

                // 写入表头行
                writer.WriteElement(headRow);

                // 写入数据
                uint rowIndex = 2;
                foreach (T data in _sourceDatas)
                {
                    Row row = new Row() { RowIndex = rowIndex };
                    // row 开始
                    writer.WriteStartElement(row);
                    for (int k = 0; k < columnCount; k++)
                    {
                        ColumnItem citem = _columnCollection.Items[k];
                        PropertyInfo pi = _propertyDic[citem.PropertyName];
                        object v = pi.GetValue(data, null);
                        Cell cell = ConstructCell(v);
                        cell.CellReference = CellReferenceUtil.GetCellReference(rowIndex - 1, (uint)k);
                        // 写入 c
                        writer.WriteElement(cell);
                    }
                    // row 结束
                    writer.WriteEndElement();
                    rowIndex++;
                }
                // sheetData 结束
                writer.WriteEndElement();
                // worksheet 结束
                writer.WriteEndElement();

                writer.Close();
            }
        }
        /// <summary>
        /// 共享字符串
        /// </summary>
        /// <param name="sharedStringTablePart"></param>
        private void GenerateSharedStringTablePartContent(SharedStringTablePart sharedStringTablePart)
        {
            using (OpenXmlWriter writer = OpenXmlWriter.Create(sharedStringTablePart))
            {
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
}