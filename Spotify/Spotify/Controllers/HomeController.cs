using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Spotify.Models.TopTracks;
using Spotify.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using Spotify.Services;
using Microsoft.Extensions.Configuration;

namespace Spotify.Controllers
{
    public class HomeController : Controller
    {
        private readonly IspotifyAccountService _spotifyAccountService;
        private readonly IConfiguration _configuration;

        public HomeController(IspotifyAccountService spotifyAccountService, IConfiguration configuration)
        {
            this._spotifyAccountService = spotifyAccountService;
            this._configuration = configuration;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var token = await _spotifyAccountService.GetToken(_configuration["Spotify:ClientId"], _configuration["Spotify:ClientSecret"]);
            }
            catch ( Exception ex)
            {

                Debug.Write(ex);
            }
            
            return View();
        }

        [HttpGet]
        public ContentResult Privacy()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("https://accounts.spotify.com/authorize?client_id=f0aa1b6e083f43bd99735c1645e80dca&response_type=code&redirect_uri=https://spotifyapisimon.azurewebsites.net/home/");
            var request = new HttpRequestMessage(HttpMethod.Get, "https://accounts.spotify.com/authorize?client_id=f0aa1b6e083f43bd99735c1645e80dca&response_type=code&redirect_uri=https://spotifyapisimon.azurewebsites.net/home/");

            var response = client.GetAsync("https://accounts.spotify.com/authorize?client_id=f0aa1b6e083f43bd99735c1645e80dca&response_type=code&redirect_uri=https://spotifyapisimon.azurewebsites.net/home/");

            var responseBody = response.Result.Content.ReadAsStringAsync().Result;



            return base.Content(responseBody, "text/html");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
