using Newtonsoft.Json;
using Spaceman.Service.Models;
using Spaceman.Service.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Spaceman.Loader
{
    public class SpacemanLoader
    {

        private IPlayerService PlayerService { get; set; }
        private ILocationService LocationService { get; set; }

        public SpacemanLoader(IPlayerService playerService, ILocationService locationService)
        {
            PlayerService = playerService;
            LocationService = locationService;
        }

        public async Task ImportFile(string type, string path)
        {
            System.Console.WriteLine($"Importing: {type} from {path}");
            Task task;
            if (IsType<Player>(type))
            {
                task = Task.WhenAll(ReadFile<Player>(path).ToList()
                    .Select((p) => PlayerService.Update(p)));
            }
            else if (IsType<SolarSystem>(type))
            {
                task = Task.WhenAll(ReadFile<SolarSystem>(path).ToList()
                    .Select((p) => LocationService.StoreSolarSystem(p)));
            }
            else if (IsType<SpaceBody>(type))
            {
                task = Task.WhenAll(ReadFile<SpaceBody>(path).ToList()
                    .Select((p) => LocationService.StoreSpaceBody(p)));
            }
            else if (IsType<NamedLocation>(type))
            {
                task = Task.WhenAll(ReadFile<NamedLocation>(path).ToList()
                    .Select((p) => LocationService.StoreNamedLocation(p)));
            }
            else if (IsType<WorldObject>(type))
            {
                task = Task.WhenAll(ReadFile<WorldObject>(path).ToList()
                    .Select((p) => LocationService.StoreWorldObject(p)));
            }
            else
            {
                throw new Exception("Illegal import type exception");
            }

            await task;
        }

        private bool IsType<T>(string type)
        {
            return string.Equals(typeof(T).Name, type, comparisonType: StringComparison.InvariantCulture);
        }

        private IEnumerable<T> ReadFile<T>(string path)
        {
            try
            {
                using (StreamReader r = new StreamReader(path))
                {
                    string json = r.ReadToEnd();
                    IEnumerable<T> items = JsonConvert.DeserializeObject<List<T>>(json);
                    System.Console.WriteLine($"Loaded {path} ; Count: {items.Count()}");
                    return items;
                }
            }
            catch (FileNotFoundException e)
            {
                System.Console.WriteLine(e.Message);
                return new List<T>();
            }
        }
    }
}

