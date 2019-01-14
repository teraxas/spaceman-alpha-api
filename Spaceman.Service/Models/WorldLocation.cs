using System;

namespace Spaceman.Service.Models
{
    public class WorldLocation
    {
        public Guid SpaceBodyId { get; set; }
        public int? OrbitHeight { get; set; }

        public WorldLocation()
        {
            SpaceBodyId = Guid.Empty;
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