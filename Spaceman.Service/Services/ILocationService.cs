using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spaceman.Service.Models;

namespace Spaceman.Service.Services
{
    public interface ILocationService
    {
        Task<NamedLocation> StoreNamedLocation(NamedLocation spaceBody);
        Task<SolarSystem> StoreSolarSystem(SolarSystem solarSystem);
        Task<SpaceBody> StoreSpaceBody(SpaceBody spaceBody);
        Task<WorldObject> StoreWorldObject(WorldObject worldObject);

        Task<NamedLocation> GetNamedLocation(Guid id);
        Task<SolarSystem> GetSolarSystem(Guid id);
        Task<IEnumerable<SolarSystem>> GetAllSolarSystems();
        Task<SpaceBody> GetSpaceBody(Guid id);
        Task<IEnumerable<SpaceBody>> GetSpaceBodiesBySolarSystem(Guid solarSystemId);
        Task<WorldObject> GetWorldObject(Guid id);
        Task<IEnumerable<WorldObject>> GetWorldObjectsBySpacebody(Guid spaceBodyId);

    }
}