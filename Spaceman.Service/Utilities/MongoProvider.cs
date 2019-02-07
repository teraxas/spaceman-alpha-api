using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Spaceman.Service.Models;

namespace Spaceman.Service.Utilities
{
    public class MongoProvider
    {
        private const string DbCollectionNamePlayer = "Player";
        private const string DbCollectionNameSolarSystem = "SolarSystem";
        private const string DbCollectionNameSpaceBody = "SpaceBody";
        private const string DbCollectionNameNamedLocation = "NamedLocation";
        private const string DbCollectionNameWorldObject = "WorldObject";

        internal Options Options { get; }
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public MongoProvider(IOptions<Options> options)
        {
            Options = options.Value;
            Client = new MongoClient(Options.ConnectionString);
            Database = Client.GetDatabase(Options.DBName);
        }

        public MongoProvider(Options options)
        {
            Options = options;
            Client = new MongoClient(Options.ConnectionString);
            Database = Client.GetDatabase(Options.DBName);
        }

        public IMongoCollection<Player> PlayerCollection
        {
            get
            {
                return Database.GetCollection<Player>(DbCollectionNamePlayer);
            }
        }

        public IMongoCollection<SolarSystem> SolarSystemCollection
        {
            get
            {
                return Database.GetCollection<SolarSystem>(DbCollectionNameSolarSystem);
            }
        }

        public IMongoCollection<SpaceBody> SpaceBodyCollection
        {
            get
            {
                return Database.GetCollection<SpaceBody>(DbCollectionNameSpaceBody);
            }
        }

        public IMongoCollection<WorldObject> WorldObjectCollection
        {
            get
            {
                return Database.GetCollection<WorldObject>(DbCollectionNameWorldObject);
            }
        }

        public IMongoCollection<NamedLocation> NamedLocationCollection
        {
            get
            {
                return Database.GetCollection<NamedLocation>(DbCollectionNameNamedLocation);
            }
        }

        public UpdateOptions UpsertOptions
        {
            get
            {
                return new UpdateOptions
                {
                    IsUpsert = true
                };
            }
        }

    }
}
