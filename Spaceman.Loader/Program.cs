
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
            var loadPath = app.Argument("loadPath", "Folder path with JSON files").IsRequired(true);
            
            app.OnExecute(() => ImportFiles(loadPath.Value));
            app.Execute(args);
        }

        private static int ImportFiles(string loadPath)
        {
            var loader = GetLoader();
            //loader.ImportFile("Player", loadPath + "\\Player.json");
            loader.ImportFile("SolarSystem", loadPath + "\\SolarSystem.json");
            loader.ImportFile("NamedLocation", loadPath + "\\NamedLocation.json");
            loader.ImportFile("SpaceBody", loadPath + "\\SpaceBody.json");
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
