using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spaceman.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class LocationController : Controller
    {
    }
}
