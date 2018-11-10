using AutoMapper;
using MongoDB.Driver;
using Spaceman.Service.Models;
using Spaceman.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Spaceman.Service.Services
{
    public class PlayerService : IPlayerService
    {
        private readonly MongoProvider _db;

        public PlayerService(
            MongoProvider db
            )
        {
            _db = db;
        }

        public async Task<Player> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;
            Player player = await GetByUsername(username);

            // check if username exists
            if (player == null)
            {
                return null;
            }

            // check if password is correct
            if (!VerifyPasswordHash(password, player.PasswordHash, player.PasswordSalt))
            {
                return null;
            }

            // authentication successful
            return player;
        }

        public Player Create(Player player)
        {
            player.Id = Guid.NewGuid();
            _db.PlayerCollection.InsertOne(player);
            return player;
        }

        public async Task<Player> GetByUsername(string username)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Username, username);
            return (await _db.PlayerCollection.FindAsync(filter)).FirstOrDefault();
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }
    }
}
