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

namespace Spotify.Controllers
{
    public class HomeController_bkp : Controller
    {
        private readonly ILogger<HomeController_bkp> _logger;

        public HomeController_bkp(ILogger<HomeController_bkp> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
             string URL = "https://api.spotify.com/v1/me/top/artists?time_range=long_term&limit=50";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(URL);

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "BQAy_n2XDHgSn3Z0ykpOmpTHtfS8si2mN6h6udyuemiPR6YSTe6ob1Jy69oZIBuEzBD8Jk-G_lkOu500G3UwWsEf3r4YJ0PFdx8qL87NS4QM3jmsH39qWCMn8HFof54EMXaY-9mds2lk7vgVU_1RyY5o7jSx5QE_8dBbBSUb3mw");

            var request = new HttpRequestMessage(HttpMethod.Get, URL);

            var response = client.GetAsync(URL);

            var responseBody = response.Result.Content.ReadAsStringAsync().Result;

            var test = JsonConvert.DeserializeObject<TopTrackRoot>(responseBody);
            
            return View(test);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
