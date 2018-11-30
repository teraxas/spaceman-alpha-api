using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Models
{
    public class WorldObject
    {
        public Guid Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public WorldLocation Location { get; set; }
        public Dictionary<WorldObjectProperty, int> Properties { get; set; }

        public WorldObject()
        {
            Id = Guid.Empty;
        }

    }

    public enum WorldObjectProperty
    {
        Speed,
        WarpPower,
        ShieldHP,
        HullHP,
        Sectors,
        ShipSlots,
    }

}
