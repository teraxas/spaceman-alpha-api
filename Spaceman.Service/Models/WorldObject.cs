using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Models
{
    public class WorldObject
    {
        public Guid Id { get; set; }
        public WorldObjectType Type { get; set; }
        public string Name { get; set; }
        public Dictionary<WorldObjectProperty, int> Properties { get; set; }

        public WorldObject()
        {
            Id = Guid.Empty;
        }

    }

    public enum WorldObjectType
    {
        Ship,
        Station,
        Debris,
    }

    public enum WorldObjectProperty
    {
        Speed,
        WarpPower,
        ShieldHP,
        HullHP,
        Sectors,
    }

}
