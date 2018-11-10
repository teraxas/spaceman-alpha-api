using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Spaceman.Service.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Utilities
{
    public class MongoProvider
    {
        private const string DbCollectionNamePlayer = "Player";

        internal Options Options { get; }
        public IMongoClient Client { get; }
        public IMongoDatabase Database { get; }

        public IMongoCollection<Player> PlayerCollection { 
            get {
                return Database.GetCollection<Player>(DbCollectionNamePlayer);
            }
        }

        public MongoProvider(IOptions<Options> options)
        {
            Options = options.Value;
            Client = new MongoClient(Options.ConnectionString);
            Database = Client.GetDatabase(Options.DBName);
        }

    }
}
