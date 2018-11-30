using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Models
{
    public class SolarSystem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        
    }

    public class SpaceBody
    {
        public Guid Id { get; set; }
        public Guid SolarSystemId { get; set; }
        public Guid BarycenterId { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int OrbitHeight { get; set; }
        public int MaxStableOrbit { get; set; }

    }
}
