using Spaceman.Service.Models;
using Spaceman.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Services
{
    public class PlayerService : IPlayerService
    {

        public PlayerService(MongoProvider db)
        {
            
        }

        public void Create(Player player)
        {
            player.Id = Guid.NewGuid();

        }

    }
}
