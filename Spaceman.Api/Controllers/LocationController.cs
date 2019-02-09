using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Spaceman.Service.Models;
using Spaceman.Service.Services;
using System;
using System.Collections.Generic;
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

        [HttpGet("solarSystem/{id}")]
        public Task<SolarSystem> GetSolarSystem(Guid id)
        {
            return _locationService.GetSolarSystem(id);
        }

        [HttpGet("solarSystem/{id}/SpaceBodies")]
        public Task<IEnumerable<SpaceBody>> GetSolarSystemSpaceBodies(Guid id)
        {
            return _locationService.GetSpaceBodiesBySolarSystem(id);
        }

        [HttpGet("solarSystem/all")]
        public Task<IEnumerable<SolarSystem>> GetAllSolarSystems()
        {
            return _locationService.GetAllSolarSystems();
        }

        [HttpGet("spaceBody/{id}")]
        public Task<SpaceBody> GetSpaceBody(Guid id)
        {
            return _locationService.GetSpaceBody(id);
        }

        [HttpGet("spaceBody/{id}/worldObjects")]
        public Task<IEnumerable<WorldObject>> GetSpaceBodyWorldObjects(Guid id)
        {
            return _locationService.GetWorldObjectsBySpacebody(id);
        }

        [HttpGet("worldObject/{id}")]
        public Task<WorldObject> GetWorldObject(Guid id)
        {
            return _locationService.GetWorldObject(id);
        }
    }
}
