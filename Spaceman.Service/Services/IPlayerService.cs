using Spaceman.Service.Models;
using System.Threading.Tasks;

namespace Spaceman.Service.Services
{
    public interface IPlayerService
    {
        Player Create(Player player, string password);
        Task<Player> GetByUsername(string username);
        Task<Player> Authenticate(string username, string password);
    }
}