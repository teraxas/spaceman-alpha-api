using MongoDB.Driver;
using Spaceman.Service.Models;
using Spaceman.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Spaceman.Service.Services
{
    public class LocationService : ILocationService
    {
        private MongoProvider _db;

        public LocationService( MongoProvider db )
        {
            _db = db;
        }

        public Task<SolarSystem> StoreSolarSystem(SolarSystem solarSystem)
        {
            if (solarSystem.Id == Guid.Empty)
            {
                solarSystem.Id = Guid.NewGuid();
            }

            // TODO validate
            return _db.SolarSystemCollection.ReplaceOneAsync(GetIdFilterSolarSystem(solarSystem.Id), solarSystem, _db.UpsertOptions)
                .ContinueWith(r => solarSystem);
        }

        public async Task<SolarSystem> GetSolarSystem(Guid id)
        {
            return (await _db.SolarSystemCollection.FindAsync<SolarSystem>(GetIdFilterSolarSystem(id)))
                .FirstOrDefault();
        }

        public Task<SpaceBody> StoreSpaceBody(SpaceBody spaceBody)
        {
            if (spaceBody.Id == Guid.Empty)
            {
                spaceBody.Id = Guid.NewGuid();
            }

            // TODO validate
            return _db.SpaceBodyCollection.ReplaceOneAsync(GetIdFilterSpaceBody(spaceBody.Id), spaceBody, _db.UpsertOptions)
                .ContinueWith(r => spaceBody);
        }

        public async Task<SpaceBody> GetSpaceBody(Guid id)
        {
            return (await _db.SpaceBodyCollection.FindAsync<SpaceBody>(GetIdFilterSpaceBody(id)))
                .FirstOrDefault();
        }

        public Task<NamedLocation> StoreNamedLocation(NamedLocation spaceBody)
        {
            if (spaceBody.Id == Guid.Empty)
            {
                spaceBody.Id = Guid.NewGuid();
            }

            // TODO validate
            return _db.NamedLocationCollection.ReplaceOneAsync(GetIdNamedLocation(spaceBody.Id), spaceBody, _db.UpsertOptions)
                .ContinueWith(r => spaceBody);
        }

        public async Task<NamedLocation> GetNamedLocation(Guid id)
        {
            return (await _db.SpaceBodyCollection.FindAsync<NamedLocation>(GetIdFilterSpaceBody(id)))
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

        private static FilterDefinition<NamedLocation> GetIdNamedLocation(Guid id)
        {
            return Builders<NamedLocation>.Filter.Eq(p => p.Id, id);
        }
    }
}
