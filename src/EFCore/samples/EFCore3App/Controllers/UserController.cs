using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreData;
using EFCoreEntities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore3App.Controllers
{
    /// <summary>
    /// User控制器
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="context"></param>
        public UserController(MyDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获得分页数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DataResult<User>> GetPagedData([FromBody]QueryDto query)
        {
            var source = _context.Users.Where(x => 1 == 1);
            if (query.Min != null)
            {
                source = source.Where(x => x.DouV >= query.Min.Value);
            }
            if (query.Max != null)
            {
                source = source.Where(x => x.DouV <= query.Max.Value);
            }
            if (query.OrderingsIsNullOrEmpty())
            {
                source = source.OrderBy(x => x.Id);
            }
            return await source.ToPagedResultAsync(query);
        }

        /// <summary>
        /// 获得OData数据
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<DataResult<User>> GetODataData([FromBody]QueryDto2 query)
        {
            var source = _context.Users.Where(x => 1 == 1);
            if (query.Min != null)
            {
                source = source.Where(x => x.DouV >= query.Min.Value);
            }
            if (query.Max != null)
            {
                source = source.Where(x => x.DouV <= query.Max.Value);
            }
            if (query.OrderingsIsNullOrEmpty())
            {
                source = source.OrderBy(x => x.Id);
            }
            return await source.ToODataResultAsync(query);
        }
        /// <summary>
        /// 获得所有数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<DataResult<User>> GetAllData()
        {
            QueryDto2 query = new QueryDto2()
            {
                Skip = 0,
                Top = int.MaxValue,
                Count = false,
            };            
            return await GetODataData(query);
        }
        /// <summary>
        /// 获得跳过后获得的数据
        /// </summary>
        /// <param name="st"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<User>> GetData(SkipTakeDto st)
        {
            return await _context.Users.Skip(st.Skip).Take(st.Take).ToListAsync();
        }
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    public class QueryDto : PageParameter
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public double? Min { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public double? Max { get; set; }
    }

    /// <summary>
    /// 分页查询
    /// </summary>
    public class QueryDto2 : ODataParameter
    {
        /// <summary>
        /// 最小值
        /// </summary>
        public double? Min { get; set; }
        /// <summary>
        /// 最大值
        /// </summary>
        public double? Max { get; set; }
    }

    /// <summary>
    /// 跳过，获得查询
    /// </summary>
    public class SkipTakeDto
    {
        /// <summary>
        /// 跳过数据
        /// </summary>
        public int Skip { get; set; }
        /// <summary>
        /// 获得数据
        /// </summary>
        public int Take { get; set; }
    }
}