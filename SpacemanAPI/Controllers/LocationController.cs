using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spaceman.Service.Models;
using Spaceman.Service.Services;
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
        private readonly ILocationService _locationService;

        public LocationController(ILocationService locationService)
        {
            _locationService = locationService;
        }

        [HttpPost("solarSystem")]
        public Task<SolarSystem> StoreSolarSystem([FromBody] SolarSystem solarSystem) {
            return _locationService.StoreSolarSystem(solarSystem);
        }

        [HttpPost("spaceBody")]
        public Task<SpaceBody> StoreSpaceBody([FromBody] SpaceBody spaceBody)
        {
            return _locationService.StoreSpaceBody(spaceBody);
        }

        [HttpPost("namedLocation")]
        public Task<NamedLocation> StoreNamedLocation([FromBody] NamedLocation namedLocation)
        {
            return _locationService.StoreNamedLocation(namedLocation);
        }

        [HttpGet("solarSystem")]
        public Task<SolarSystem> GetSolarSystem(Guid id)
        {
            return _locationService.GetSolarSystem(id);
        }

        [HttpGet("spaceBody")]
        public Task<SpaceBody> GetSpaceBody(Guid id)
        {
            return _locationService.GetSpaceBody(id);
        }

        [HttpGet("namedLocation")]
        public Task<NamedLocation> GetNamedLocation(Guid id)
        {
            return _locationService.GetNamedLocation(id);
        }
    }
}
