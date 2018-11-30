using System.Threading.Tasks;
using Spaceman.Service.Models;

namespace Spaceman.Service.Services
{
    public interface IPlayerService
    {
        Task<Player> Authenticate(string username, string password);
        Task<bool> CheckIfUsernameExists(string username);
        Task<Player> Create(Player player, string password);
        Task<Player> GetByUsername(string username);
        Task<Player> Update(Player player);
    }
}