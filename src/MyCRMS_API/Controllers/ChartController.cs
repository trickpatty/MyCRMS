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
    public class ChartController : Controller
    {
        // GET: api/values
        [HttpGet]
        public IActionResult Get()
        {
            Chart carisChart = new Chart();
            carisChart.Id = 1;
            carisChart.OwnerName = "cari@kearney.tk";

            Observation obs1 = new Observation();
            obs1.Date = DateTime.Now.AddDays(-3);
            obs1.Id = 1;
            obs1.Interc = false;
            obs1.Multiplier = 4;
            obs1.ObservedLetter = Letter.NA;
            obs1.ObservedNumber = 4;
            obs1.ObservedRed = "";
            obs1.UserId = 1;

            Observation obs2 = new Observation();
            obs2.Date = DateTime.Now.AddDays(-2);
            obs2.Id = 2;
            obs2.Interc = true;
            obs2.Multiplier = 1;
            obs2.ObservedLetter = Letter.L;
            obs2.ObservedNumber = 10;
            obs2.ObservedRed = "";
            obs2.UserId = 1;

            Observation obs3 = new Observation();
            obs3.Date = DateTime.Now.AddDays(-1);
            obs3.Id = 3;
            obs3.Interc = true;
            obs3.Multiplier = 4;
            obs3.ObservedLetter = Letter.NA;
            obs3.ObservedNumber = 4;
            obs3.ObservedRed = "";
            obs3.UserId = 1;

            Observation obs4 = new Observation();
            obs4.Date = DateTime.Now;
            obs4.Id = 4;
            obs4.Interc = false;
            obs4.Multiplier = 4;
            obs4.ObservedLetter = Letter.NA;
            obs4.ObservedNumber = 4;
            obs4.ObservedRed = "";
            obs4.UserId = 1;

            List<Observation> observations = new List<Observation>();
            observations.Add(obs1);
            observations.Add(obs2);
            observations.Add(obs3);
            observations.Add(obs4);
            carisChart.Observations = observations;

            return new JsonResult(carisChart.Observations(obs => carisChart.CalculateStamp(obs.Date));
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
