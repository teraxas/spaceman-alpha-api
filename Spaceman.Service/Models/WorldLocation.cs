using System;

namespace Spaceman.Service.Models
{
    public class WorldLocation
    {
        public Guid SpaceBody { get; set; }
        public int? OrbitHeight { get; set; }

        public WorldLocation()
        {
            SpaceBody = Guid.Empty;
        }
    }

    public class NamedLocation : WorldLocation
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public NamedLocation()
        {
            Id = Guid.Empty;
        }
    }
}