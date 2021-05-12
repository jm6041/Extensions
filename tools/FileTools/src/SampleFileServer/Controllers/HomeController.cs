using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SampleFileServer.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SampleFileServer.Controllers
{
    /// <summary>
    /// 主界面
    /// </summary>
    public class HomeController : Controller
    {
        private readonly FileDirsInfo _fileDirsInfo;
        private readonly ILogger<HomeController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fileDirsInfo"></param>
        /// <param name="logger"></param>
        public HomeController(FileDirsInfo fileDirsInfo, ILogger<HomeController> logger)
        {
            _fileDirsInfo = fileDirsInfo;
            _logger = logger;
        }
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View(_fileDirsInfo.Data);
        }
        /// <summary>
        /// 个人信息
        /// </summary>
        /// <returns></returns>
        public IActionResult Privacy()
        {
            return View();
        }
        /// <summary>
        /// 异常
        /// </summary>
        /// <returns></returns>
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
