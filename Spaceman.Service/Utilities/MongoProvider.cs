using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Utilities
{
    public class MongoProvider
    {
        internal Options Options { get; }
        public MongoClient Client { get; }

        public MongoProvider(IOptions<Options> options)
        {
            Options = options.Value;
            Client = new MongoClient($"mongodb://{Options.Username}:{Options.Password}@ds143893.mlab.com:43893/spaceman-tst");
        }

    }
}
