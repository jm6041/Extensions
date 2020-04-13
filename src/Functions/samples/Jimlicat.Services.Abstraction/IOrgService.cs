using Jimlicat.Services.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Jimlicat.Functions;

namespace Jimlicat.Services
{
    /// <summary>
    /// Org服务
    /// </summary>
    public interface IOrgService
    {
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="org"></param>
        /// <returns></returns>
        [Function("orgc", "组织机构新增")]
        Task Create(OrgAddDto org);
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [Function("orgg", "组织机构查询")]
        Task<OrgDto[]> Get();
        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        [Function("orgg", "组织机构查询")]
        Task<OrgDto[]> Get2();
    }
}
