
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Spaceman.Service;
using Spaceman.Service.Services;
using Spaceman.Service.Utilities;

namespace Spaceman.Loader
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "spaceman-loader";
            app.HelpOption("-?|-h|--help");
            var loadType = app.Argument("loadType", "Entity type").IsRequired(true);
            var loadPath = app.Argument("loadPath", "Path to JSON file to load").IsRequired(true);
            
            app.OnExecute(() => ImportFile(loadType.Value, loadPath.Value));
            app.Execute();
        }

        private static int ImportFile(string loadType, string loadPath)
        {
            var loader = GetLoader();
            loader.ImportFile(loadType, loadPath);
            return 0;
        }

        private static Loader GetLoader()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("LoadConfig.json")
                .AddUserSecrets<Program>()
                .Build();
            var serviceConfig = new Options();
            config.Bind("SpacemanService", serviceConfig);

            var db = new MongoProvider(serviceConfig);
            var playerService = new PlayerService(db);
            var locationService = new LocationService(db);

            return new Loader(playerService, locationService);
        }

        
    }
}
