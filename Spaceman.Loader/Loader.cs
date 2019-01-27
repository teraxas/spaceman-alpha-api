using Newtonsoft.Json;
using Spaceman.Service.Models;
using Spaceman.Service.Services;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Spaceman.Loader
{
    public class Loader
    {

        private IPlayerService PlayerService { get; set; }
        private ILocationService LocationService { get; set; }

        public Loader(IPlayerService playerService, ILocationService locationService)
        {
            PlayerService = playerService;
            LocationService = locationService;
        }

        public void ImportFile(string type, string path)
        {
            System.Console.WriteLine($"Importing: {type} from {path}");

            if (IsType<Player>(type))
            {
                ReadFile<Player>(path).ToList().ForEach(async (p) => await PlayerService.Update(p));
            }
            else if (IsType<SolarSystem>(type))
            {
                ReadFile<SolarSystem>(path).ToList().ForEach(async (p) => await LocationService.StoreSolarSystem(p));
            }
            else if (IsType<SpaceBody>(type))
            {
                ReadFile<SpaceBody>(path).ToList().ForEach(async (p) => await LocationService.StoreSpaceBody(p));
            }
            else if (IsType<NamedLocation>(type))
            {
                ReadFile<NamedLocation>(path).ToList().ForEach(async (p) => await LocationService.StoreNamedLocation(p));
            }
        }

        private bool IsType<T>(string type)
        {
            return typeof(T).Name.Equals(type);
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

