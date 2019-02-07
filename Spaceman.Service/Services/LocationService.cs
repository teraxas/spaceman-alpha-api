using MongoDB.Driver;
using Spaceman.Service.Models;
using Spaceman.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Spaceman.Service.Services
{
    public class LocationService : ILocationService
    {
        private MongoProvider _db;

        public LocationService(MongoProvider db)
        {
            _db = db;
        }

        public async Task<SolarSystem> StoreSolarSystem(SolarSystem solarSystem)
        {
            if (solarSystem.Id == Guid.Empty)
            {
                solarSystem.Id = Guid.NewGuid();
            }

            // TODO validate
            var result = await _db.SolarSystemCollection.ReplaceOneAsync(GetIdFilterSolarSystem(solarSystem.Id), solarSystem, _db.UpsertOptions);
            return solarSystem;
        }

        public async Task<SolarSystem> GetSolarSystem(Guid id)
        {
            return (await _db.SolarSystemCollection.FindAsync<SolarSystem>(GetIdFilterSolarSystem(id)))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<SolarSystem>> GetAllSolarSystems()
        {
            return await _db.SolarSystemCollection.Find(Builders<SolarSystem>.Filter.Empty).ToListAsync();
        }

        public async Task<SpaceBody> StoreSpaceBody(SpaceBody spaceBody)
        {
            if (spaceBody.Id == Guid.Empty)
            {
                spaceBody.Id = Guid.NewGuid();
            }

            // TODO validate
            var result = await _db.SpaceBodyCollection.ReplaceOneAsync(GetIdFilterSpaceBody(spaceBody.Id), spaceBody, _db.UpsertOptions);
            return spaceBody;
        }

        public async Task<SpaceBody> GetSpaceBody(Guid id)
        {
            return (await _db.SpaceBodyCollection.FindAsync<SpaceBody>(GetIdFilterSpaceBody(id)))
                .FirstOrDefault();
        }

        public async Task<IEnumerable<SpaceBody>> GetSpaceBodiesBySolarSystem(Guid solarSystemId)
        {
            return (await _db.SpaceBodyCollection.FindAsync<SpaceBody>(
                Builders<SpaceBody>.Filter.Eq(p => p.SolarSystemId, solarSystemId)
                )).ToEnumerable();
        }

        public async Task<IEnumerable<WorldObject>> GetWorldObjectsBySpacebody(Guid spaceBodyId)
        {
            return (await _db.WorldObjectCollection.FindAsync<WorldObject>(
                Builders<WorldObject>.Filter.Eq(p => p.Location.SpaceBodyId, spaceBodyId)
                )).ToEnumerable();
        }

        public async Task<NamedLocation> StoreNamedLocation(NamedLocation namedLocation)
        {
            if (namedLocation.Id == Guid.Empty)
            {
                namedLocation.Id = Guid.NewGuid();
            }

            // TODO validate
            var result = await _db.NamedLocationCollection.ReplaceOneAsync(GetIdFilterNamedLocation(namedLocation.Id), namedLocation, _db.UpsertOptions);
            return namedLocation;
        }

        public async Task<NamedLocation> GetNamedLocation(Guid id)
        {
            return (await _db.NamedLocationCollection.FindAsync<NamedLocation>(GetIdFilterNamedLocation(id)))
                .FirstOrDefault();
        }

        public async Task<WorldObject> StoreWorldObject(WorldObject worldObject)
        {
            if (worldObject.Id == Guid.Empty)
            {
                worldObject.Id = Guid.NewGuid();
            }

            // TODO validate
            var result = await _db.WorldObjectCollection.ReplaceOneAsync(GetIdFilterWorldObject(worldObject.Id), worldObject, _db.UpsertOptions);
            return worldObject;
        }

        public async Task<WorldObject> GetWorldObject(Guid id)
        {
            return (await _db.WorldObjectCollection.FindAsync<WorldObject>(GetIdFilterWorldObject(id)))
                .FirstOrDefault();
        }

        private static FilterDefinition<SolarSystem> GetIdFilterSolarSystem(Guid id)
        {
            return Builders<SolarSystem>.Filter.Eq(p => p.Id, id);
        }

        private static FilterDefinition<SpaceBody> GetIdFilterSpaceBody(Guid id)
        {
            return Builders<SpaceBody>.Filter.Eq(p => p.Id, id);
        }

        private static FilterDefinition<NamedLocation> GetIdFilterNamedLocation(Guid id)
        {
            return Builders<NamedLocation>.Filter.Eq(p => p.Id, id);
        }

        private static FilterDefinition<WorldObject> GetIdFilterWorldObject(Guid id)
        {
            return Builders<WorldObject>.Filter.Eq(p => p.Id, id);
        }
    }
}
