using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jimlicat.Services;
using Jimlicat.Services.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
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
        public void Add([FromBody]OrgAddDto org)
        {
            _orgService.Add(org);
        }

        [HttpGet]
        public OrgDto[] Get()
        {
            return _orgService.Get();
        }
    }
}
