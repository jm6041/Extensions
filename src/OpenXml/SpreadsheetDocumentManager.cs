using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OpenXml
{
    /// <summary>
    /// 
    /// </summary>
    public class CellItem
    {
        /// <summary>
        /// 
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Text;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    public class RowRecord
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="cellItems"></param>
        public RowRecord(uint rowIndex, SortedDictionary<string, CellItem> cellItems)
        {
            RowIndex = rowIndex;
            CellItems = cellItems;
        }

        /// <summary>
        /// 
        /// </summary>
        public uint RowIndex { get; }
        /// <summary>
        /// 
        /// </summary>
        public SortedDictionary<string, CellItem> CellItems { get; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SheetRecord
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public SheetRecord(string name)
        {
            Name = name;
            Rows = new List<RowRecord>();
        }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// 
        /// </summary>
        public List<RowRecord> Rows { get; }
    }

    /// <summary>
    /// 电子表格
    /// </summary>
    public class SpreadsheetDocumentManager : IDisposable
    {
        private readonly SpreadsheetDocument _doc;
        // Sheet Id，Name对应关系
        private readonly SortedDictionary<string, string> _sheetIdNameDic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="doc"></param>
        public SpreadsheetDocumentManager(SpreadsheetDocument doc)
        {
            _doc = doc;
            _sheetIdNameDic = GetSheetIdNameDic();
        }

        private string[] GetSharedStrings()
        {
            SharedStringTablePart sstpart = _doc.WorkbookPart.GetPartsOfType<SharedStringTablePart>().FirstOrDefault();
            string[] sstArr = null;
            if (sstpart != null)
            {
                sstArr = sstpart.SharedStringTable.Elements<SharedStringItem>().Select(x => x.InnerText).ToArray();
            }
            return sstArr;
        }

        private SortedDictionary<string, string> GetSheetIdNameDic()
        {
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            foreach (Sheet sheet in _doc.WorkbookPart.Workbook.Sheets)
            {
                dic.Add(sheet.Id, sheet.Name);
            }
            return dic;
        }

        private static uint GetRowIndex(string cellReference)
        {
            char[] cs = cellReference.ToArray();
            int ni = Array.FindIndex(cs, x => x >= '0' && x <= '9');
            int count = cs.Length - ni;
            char[] ncs = new char[count];
            Array.Copy(cs, ni, ncs, 0, count);
            string nstr = new string(ncs.ToArray());
            return uint.Parse(nstr);
        }

        /// <summary>
        /// 获得合并cell对应值的cell
        /// </summary>
        /// <param name="sheet"></param>
        /// <returns></returns>
        private static (List<string> ValueCells, SortedDictionary<string, string> MergeCellsRefDic) GetMergeCellDic(Worksheet sheet)
        {
            List<string> vcs = new List<string>();
            SortedDictionary<string, string> dic = new SortedDictionary<string, string>();
            MergeCells mergeCells = sheet.Elements<MergeCells>().FirstOrDefault();
            if (mergeCells != null && mergeCells.Any())
            {
                foreach (MergeCell mergeCell in mergeCells)
                {
                    string cr = mergeCell.Reference;
                    char[] sep = { ':' };
                    string[] crs = cr.Split(sep, StringSplitOptions.RemoveEmptyEntries);
                    string sref = crs[0];

                    // 开始cell作为有值的cell
                    vcs.Add(sref);

                    string eref = crs[1];
                    // 开始行号
                    uint sri = GetRowIndex(sref);
                    // 开始行标记
                    string srr = Regex.Replace(sref, @"[\d]", string.Empty);

                    // 结束行号
                    uint eri = GetRowIndex(eref);
                    string err = Regex.Replace(eref, @"[\d]", string.Empty);

                    // 同列
                    if (srr == err)
                    {
                        for (uint ri = sri; ri <= eri; ri++)
                        {
                            string cref = srr + ri.ToString();
                            dic.Add(cref, sref);
                        }
                    }
                    else
                    {
                        // 开始列索引 0 开始
                        uint sci = (uint)Utils.CellReferenceUtil.GetColumnIndex(sref);
                        // 结束列索引 0 开始
                        uint eci = (uint)Utils.CellReferenceUtil.GetColumnIndex(eref);
                        // 同行
                        if (sri == eri)
                        {
                            for (uint ci = sci; ci <= eci; ci++)
                            {
                                string cref = Utils.CellReferenceUtil.GetColumnReference(ci) + sri.ToString();
                                dic.Add(cref, sref);
                            }
                        }
                        else
                        {
                            for (uint ci = sci; ci <= eci; ci++)
                            {
                                for (uint ri = sri; ri <= eri; ri++)
                                {
                                    string cref = Utils.CellReferenceUtil.GetColumnReference(ci) + ri.ToString();
                                    dic.Add(cref, sref);
                                }
                            }
                        }
                    }
                }
            }
            return (vcs, dic);
        }

        /// <summary>
        /// 生成默认值
        /// </summary>
        /// <param name="rowTag">行号标签</param>
        /// <param name="sref">开始列索引</param>
        /// <param name="eref">结束列索引</param>
        /// <returns></returns>
        private SortedDictionary<string, CellItem> GenDefaultValues(uint rowTag, string sref, string eref)
        {
            SortedDictionary<string, CellItem> dic = new SortedDictionary<string, CellItem>();
            // 开始列索引 0 开始
            uint sci = (uint)Utils.CellReferenceUtil.GetColumnIndex(sref);
            // 结束列索引 0 开始
            uint eci = (uint)Utils.CellReferenceUtil.GetColumnIndex(eref);
            for (uint ci = sci; ci <= eci; ci++)
            {
                string cref = Utils.CellReferenceUtil.GetColumnReference(ci) + rowTag.ToString();
                dic.Add(cref, null);
            }
            return dic;
        }

        private int GetStartRowIndex(int[] rowStartIndexs, int i)
        {
            if (i < 0)
            {
                return 2;
            }
            if (rowStartIndexs == null || !rowStartIndexs.Any())
            {
                return 2;
            }

            // 索引超出
            if (i > rowStartIndexs.Length - 1)
            {
                return 2;
            }
            return rowStartIndexs[i];
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="rowStartIndex">每个Sheet开始读取数据行号，1：表示第一行，默认从第二行读取</param>
        /// <returns></returns>
        public Dictionary<string, SheetRecord> Parse(int rowStartIndex = 2)
        {
            int length = _sheetIdNameDic.Count;
            int[] rowStartIndexs = new int[length];
            for (int i = 0; i < length; i++)
            {
                rowStartIndexs[i] = rowStartIndex;
            }
            return Parse(rowStartIndexs);
        }

        /// <summary>
        /// 解析数据
        /// </summary>
        /// <param name="rowStartIndexs">每个Sheet开始读取数据行号，1：表示第一行，默认从第二行读取</param>
        /// <returns></returns>
        public Dictionary<string, SheetRecord> Parse(int[] rowStartIndexs)
        {
            Dictionary<string, SheetRecord> dic = new Dictionary<string, SheetRecord>();
            // 共享字符串
            string[] sharedStrings = GetSharedStrings();
            WorkbookPart workbookPart = _doc.WorkbookPart;
            int si = 0;   // 记录表格循环次数
            foreach (var sheetIdName in _sheetIdNameDic)
            {
                int rowStartIndex = GetStartRowIndex(rowStartIndexs, si);
                si++;

                WorksheetPart worksheetPart = (WorksheetPart)workbookPart.GetPartById(sheetIdName.Key);
                Worksheet workSheet = worksheetPart.Worksheet;
                // 保存表格数据
                SheetRecord sheetRecord = new SheetRecord(sheetIdName.Value);

                var mc = GetMergeCellDic(workSheet);
                SortedDictionary<string, string> mcRefDic = mc.MergeCellsRefDic;
                SortedDictionary<string, CellItem> mcValuesDic = new SortedDictionary<string, CellItem>();
                foreach (var vcRef in mc.ValueCells.Distinct())
                {
                    mcValuesDic.Add(vcRef, null);
                }

                // 是否有合并数据
                bool hasMerge = mcRefDic.Any();
                foreach (Row row in workSheet.Descendants<Row>())
                {
                    uint rowIndex = row.RowIndex;
                    if (rowIndex < rowStartIndex)
                    {
                        continue;
                    }
                    
                    Cell lcel = row.Elements<Cell>().Last();
                    
                    SortedDictionary<string, CellItem> cdvDic = GenDefaultValues(rowIndex, "A", lcel.CellReference);
                    foreach (Cell cell in row.Elements<Cell>())
                    {
                        string keyRef = cell.CellReference.Value;
                        CellItem val = new CellItem();
                        if (cell.CellValue != null)
                        {
                            val.Text = cell.CellValue.Text;
                            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                            {
                                int ssid = int.Parse(val.Text);
                                val.Text = sharedStrings[ssid];
                            }
                            val.Text = val.Text?.Trim();
                            if (mcValuesDic.ContainsKey(keyRef))
                            {
                                mcValuesDic[keyRef] = val;
                            }
                        }
                        else if (hasMerge && mcRefDic.ContainsKey(keyRef))
                        {
                            string vc = mcRefDic[keyRef];
                            if (mcValuesDic.ContainsKey(vc))
                            {
                                val = mcValuesDic[vc];
                            }
                        }
                        cdvDic[keyRef] = val;
                    }
                    RowRecord rowRecord = new RowRecord(rowIndex, cdvDic);
                    sheetRecord.Rows.Add(rowRecord);
                }
                dic.Add(sheetRecord.Name, sheetRecord);
            }
            return dic;
        }
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            _doc.Dispose();
        }
    }
}
