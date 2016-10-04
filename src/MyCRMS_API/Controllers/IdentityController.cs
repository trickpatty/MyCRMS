using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using IdentityModel.Client;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCRMS_API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            //return new JsonResult(User.Identity.Name);
            return new JsonResult(from c in User.Claims select new { c.Type, c.Value });
        }
    }
}
