using System;
using System.Collections.Generic;
using System.Text;

namespace Spaceman.Service.Models
{
    public class Player : PlayerDTO
    {
        public string Username { get; internal set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
    }

    public class PlayerDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
