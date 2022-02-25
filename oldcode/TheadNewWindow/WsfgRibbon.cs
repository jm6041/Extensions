using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Tools.Ribbon;
using Excel = Microsoft.Office.Interop.Excel;
using Office = Microsoft.Office.Core;
using Microsoft.Office.Tools.Excel;
using Wsfg.Controls;
using System.Threading;
using System.Windows.Threading;
using System.Windows.Interop;
using System.Windows.Forms;

namespace Wsfg.ExcelAddInReadData
{
    public partial class WsfgRibbon
    {
        private void WsfgRibbon_Load(object sender, RibbonUIEventArgs e)
        {
        }
        private readonly SemaphoreSlim semaphoreSlim0 = new SemaphoreSlim(1, 1);
        private void BtnViewData_Click(object sender, RibbonControlEventArgs e)
        {
            semaphoreSlim0.Wait();
            var book = Globals.ThisAddIn.Application.ActiveWorkbook;
            Excel.Sheets ss = Globals.ThisAddIn.Application.ActiveWorkbook?.Worksheets;
            if (ss == null)
            {
                return;
            }
            int count = AppWindows.GetWindowCount<ViewDataWindow>();
            if (count >= 1)
            {
                return;
            }
            var thread = new Thread((obj) =>
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                var vv = new ViewDataWindow(ss);
                AppWindows.AddWindow(vv);
                var ownerWindowHandle = (IntPtr)Globals.ThisAddIn.Application.Hwnd;
                var helper = new WindowInteropHelper(vv)
                {
                    Owner = ownerWindowHandle // COMMENT THAT AND IT WORKS PROPERLY
                };
                vv.Show();
                vv.Closed += (sender2, e2) =>
                {
                    AppWindows.RemoveWindow(vv);
                    vv.Dispatcher.InvokeShutdown();
                    Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);
                    ((SemaphoreSlim)obj).Release();
                };
                Dispatcher.Run();
            })
            {
                IsBackground = true
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(semaphoreSlim0);
        }
        private readonly SemaphoreSlim semaphoreSlim1 = new SemaphoreSlim(1, 1);
        private void BtnStocksView_Click(object sender, RibbonControlEventArgs e)
        {
            semaphoreSlim1.Wait();
            Excel.Sheets ss = Globals.ThisAddIn.Application.ActiveWorkbook?.Worksheets;
            if (ss == null)
            {
                return;
            }
            int count = AppWindows.GetWindowCount<StocksWindow>();
            if (count >= 1)
            {
                return;
            }
            var thread = new Thread((obj) =>
            {
                var sw = new StocksWindow(ss);
                AppWindows.AddWindow(sw);
                var ownerWindowHandle = (IntPtr)Globals.ThisAddIn.Application.Hwnd;
                var helper = new WindowInteropHelper(sw)
                {
                    Owner = ownerWindowHandle // COMMENT THAT AND IT WORKS PROPERLY
                    };
                sw.Show();
                sw.Closed += (sender2, e2) =>
                {
                    AppWindows.RemoveWindow(sw);
                    sw.Dispatcher.InvokeShutdown();
                    ((SemaphoreSlim)obj).Release();
                };
                Dispatcher.Run();
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(semaphoreSlim1);
        }

        private void BtnStocksView_Click2(object sender, RibbonControlEventArgs e)
        {
            semaphoreSlim1.Wait();
            var dd = Dispatcher.CurrentDispatcher;
            Excel.Sheets ss = Globals.ThisAddIn.Application.ActiveWorkbook?.Worksheets;
            if (ss == null)
            {
                return;
            }
            int count = AppWindows.GetWindowCount<StocksWindow>();
            if (count >= 1)
            {
                return;
            }
            var thread = new Thread((obj) =>
            {
                SynchronizationContext.SetSynchronizationContext(new DispatcherSynchronizationContext(Dispatcher.CurrentDispatcher));
                var vv = new ViewDataWindow(ss);
                AppWindows.AddWindow(vv);
                var ownerWindowHandle = (IntPtr)Globals.ThisAddIn.Application.Hwnd;
                var helper = new WindowInteropHelper(vv)
                {
                    Owner = ownerWindowHandle // COMMENT THAT AND IT WORKS PROPERLY
                };
                vv.Show();
                vv.Closed += (sender2, e2) =>
                {
                    AppWindows.RemoveWindow(vv);
                    vv.Dispatcher.InvokeShutdown();
                    ((SemaphoreSlim)obj).Release();
                };
                Dispatcher.Run();
            })
            {
                IsBackground = true
            };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(semaphoreSlim0);
            //var thread = new Thread(() =>
            //{
            //    ViewDataWindow tempWindow = new ViewDataWindow(ss);
            //    // When the window closes, shut down the dispatcher
            //    tempWindow.Closed += (s, e2) =>
            //       Dispatcher.CurrentDispatcher.BeginInvokeShutdown(DispatcherPriority.Background);

            //    tempWindow.Show();
            //    // Start the Dispatcher Processing
            //    Dispatcher.Run();
            //});
            //thread.IsBackground = true;
            //thread.SetApartmentState(ApartmentState.STA);
            //thread.Start(semaphoreSlim0);
        }
    }
}
