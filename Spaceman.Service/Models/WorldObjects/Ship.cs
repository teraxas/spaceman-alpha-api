using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Models.WorldObjects
{
    public class Ship : WorldObject
    {
        public ShipType ShipType { get; set; }

        public Ship() : base()
        {
            Type = WorldObjectType.Ship;
        }

    }

    public enum ShipType
    {
        Freighter,
        Fighter,
        BattleCruiser,
        Explorer,
        Shuttle,
        Bomber,
    }
}
