using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace SpeedTest.Controllers
{
    [Route("[controller]")]
    public class PingController : Controller
    {
        [HttpGet]
        public ContentResult Get()
        {
            return Content("true");
        }
    }
}
