using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spaceman.Service.Models;

namespace Spaceman.Service.Services
{
    public interface ILocationService
    {
        Task<NamedLocation> GetNamedLocation(Guid id);
        Task<SolarSystem> GetSolarSystem(Guid id);
        Task<IEnumerable<SolarSystem>> GetAllSolarSystems();
        Task<SpaceBody> GetSpaceBody(Guid id);
        Task<NamedLocation> StoreNamedLocation(NamedLocation spaceBody);
        Task<SolarSystem> StoreSolarSystem(SolarSystem solarSystem);
        Task<SpaceBody> StoreSpaceBody(SpaceBody spaceBody);
    }
}