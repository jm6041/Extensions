using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;

namespace Jimlicat.ExcelAddIns.ViewData
{
    public partial class JimlicatRibbon
    {
        private void JimlicatRibbon_Load(object sender, RibbonUIEventArgs e)
        {

        }

        private void btnViewData_Click(object sender, RibbonControlEventArgs e)
        {
            Excel.Sheets ss = Globals.ThisAddIn.Application.ActiveWorkbook?.Worksheets;
            if (ss == null)
            {
                return;
            }
            using (ViewDataForm form = new ViewDataForm(ss))
            {
                form.ShowDialog();
            }
        }
    }
}
