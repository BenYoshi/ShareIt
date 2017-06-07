using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ShareIt.Controllers
{
    [Route("[controller]/[action]")]
    public class AboutController : Controller
    {
        public string Phone()
        {
            return "1 234 567 8901";
        }
        
        public string Address()
        {
            return "Israel";
        }
    }
}
