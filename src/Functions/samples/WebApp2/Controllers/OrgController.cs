using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jimlicat.Services;
using Jimlicat.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp2.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrgController : ControllerBase
    {
        private readonly IOrgService _orgService;
        public OrgController(IOrgService orgService)
        {
            _orgService = orgService;
        }

        [HttpPost]
        public async Task Create([FromBody]OrgAddDto org)
        {
            await _orgService.Create(org);
        }

        [HttpGet]
        public async Task<OrgDto[]> Get()
        {
            return await _orgService.Get();
        }

        [HttpGet]
        public async Task<OrgDto[]> Get2()
        {
            return await _orgService.Get2();
        }
    }
}
