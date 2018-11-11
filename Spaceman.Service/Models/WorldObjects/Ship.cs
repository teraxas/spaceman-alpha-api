using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Models.WorldObjects
{
    public class Ship : WorldObject
    {
        public Ship() : base()
        {
            Type = WorldObjectType.Ship;
        }

    }
}
