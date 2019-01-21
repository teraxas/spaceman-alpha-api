using Newtonsoft.Json;
using Spaceman.Service.Models;
using Spaceman.Service.Services;
using System;
using System.Collections.Generic;
using System.IO;

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
            if (IsType<Player>(type))
            {
                ReadFile<Player>(path).ForEach((p) => PlayerService.Update(p));
            }
            else if (IsType<SolarSystem>(type))
            {
                ReadFile<SolarSystem>(path).ForEach((p) => LocationService.StoreSolarSystem(p));
            }
            else if (IsType<SpaceBody>(type))
            {
                ReadFile<SpaceBody>(path).ForEach((p) => LocationService.StoreSpaceBody(p));
            }
            else if (IsType<NamedLocation>(type))
            {
                ReadFile<NamedLocation>(path).ForEach((p) => LocationService.StoreNamedLocation(p));
            }
        }

        private bool IsType<T>(string type)
        {
            throw new NotImplementedException();
        }
        private List<T> ReadFile<T>(string path)
        {
            using (StreamReader r = new StreamReader(path))
            {
                string json = r.ReadToEnd();
                List<T> items = JsonConvert.DeserializeObject<List<T>>(json);
                return items;
            }
        }
    }
}

