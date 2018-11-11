using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Models
{
    /// <summary>
    /// Player model for usage inside the services.
    /// </summary>
    public class Player : PlayerDTO
    {
        public Player() : base()
        {
            Id = Guid.Empty;
        }

        public Guid Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }

    /// <summary>
    /// DTO for public Player data transfer
    /// </summary>
    public class PlayerDTO
    {
        public string Username { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// DTO for Player creation.
    /// Adds Password property to PlayerDTO
    /// </summary>
    public class PlayerCreateDTO : PlayerDTO
    {
        public PlayerCreateDTO() : base() { }
        public string Password { get; set; }
    }
}
