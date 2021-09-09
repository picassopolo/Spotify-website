using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Spotify.Models;
using System.Text.Json;

namespace Spotify.Services
{
    public class SpotifyAccountService : IspotifyAccountService
    {

        private readonly HttpClient _httpClient;

        public SpotifyAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetToken(string clientID, string clientSecret)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, "token");

            request.Headers.Authorization = new AuthenticationHeaderValue(
                "basic", Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientID}:{clientSecret}")));

            request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                {"grant_type", "client_credentials" }
            });

            var response = await _httpClient.SendAsync(request);

            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            //var authResult = await JsonSerializer.DeserializeAsync<AuthResult>(responseStream);

            return null;

        }


    }
}
