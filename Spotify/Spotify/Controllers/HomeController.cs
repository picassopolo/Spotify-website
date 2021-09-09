using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Spotify.Models;
using Spotify.Models.TopTracks;
using Spotify.Services;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

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
            string URL = "https://api.spotify.com/v1/me/top/artists?time_range=long_term&limit=50";

            string token = (string)TempData["token"];

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var request = new HttpRequestMessage(HttpMethod.Get, URL);

            var response = await client.GetAsync(URL);

            var responseBody = response.Content.ReadAsStringAsync().Result;

            var test = JsonConvert.DeserializeObject<TopTrackRoot>(responseBody);

            return View(test);
        }

        [HttpGet]
        public RedirectResult Login()
        {
            string url = "https://accounts.spotify.com/authorize" +
                "?client_id=f0aa1b6e083f43bd99735c1645e80dca" +
                "&response_type=code" +
                //"&redirect_uri=https://localhost:44324/Home/Callback/" +
                "&redirect_uri=https://spotifyapisimon.azurewebsites.net/Home/Callback/" +
            "&scope=user-top-read";

            return Redirect(url);
        }

        public async Task<IActionResult> Callback()
        {
            string code = HttpContext.Request.QueryString.ToString();

            string[] codes = code.Split('=');



            var encodedData = Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(_configuration["Spotify:ClientId"] + ":" + _configuration["Spotify:ClientSecret"]));

            string URL = "https://accounts.spotify.com/api/token" +
                "?grant_type=authorization_code" +
                "&code=" + codes[1] +
                //"&redirect_uri=https://localhost:44324/Home/Callback/";
                "&redirect_uri=https://spotifyapisimon.azurewebsites.net/Home/Callback/";


            var httpClient = new HttpClient();

            var request = new HttpRequestMessage(new HttpMethod("POST"), URL);

            request.Headers.TryAddWithoutValidation("Authorization", "Basic " + encodedData);

            request.Content = new StringContent("");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            var response = await httpClient.SendAsync(request);

            var responseBody = response.Content.ReadAsStringAsync().Result;

            var test = JsonConvert.DeserializeObject<AccessToken>(responseBody);


            string token = test.access_token;

            TempData["token"] = token;

            return RedirectToAction("index");



        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
