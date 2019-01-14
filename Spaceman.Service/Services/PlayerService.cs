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

        public PlayerService( MongoProvider db )
        {
            _db = db;
        }

        public async Task<Player> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

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

        public async Task<Player> Create(Player player, string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new AppException("Password is required");

            if (await CheckIfUsernameExists(player.Username))
                throw new AppException("Username \"" + player.Username + "\" is already taken");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            player.Id = Guid.NewGuid();
            player.PasswordHash = passwordHash;
            player.PasswordSalt = passwordSalt;

            await _db.PlayerCollection.InsertOneAsync(player);
            return player;
        }

        public Task<Player> Update(Player player)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Id, player.Id);
            return _db.PlayerCollection.ReplaceOneAsync(filter, player)
                .ContinueWith(r => player);
        }

        public Task<bool> CheckIfUsernameExists(string username)
        {
            FilterDefinition<Player> filter = Builders<Player>.Filter.Eq(p => p.Username, username);
            return _db.PlayerCollection.CountDocumentsAsync(filter)
                .ContinueWith(r => r.Result > 0);
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
