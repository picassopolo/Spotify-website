using System.Threading.Tasks;

namespace Spotify.Services
{
    public interface IspotifyAccountService
    {
        Task<string> GetToken(string clientID, string clientSecret);
    }
}
