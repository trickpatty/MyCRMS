using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MyCRMS_API.Models;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace MyCRMS_API.Controllers
{
    [Route("api/[controller]")]
    public class ObservationController : Controller
    {
        // GET: api/observation
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/observation/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/observation
        [HttpPost]
        public void Post([FromBody]Observation value)
        {
        }

        // PUT api/observation/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/observation/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
