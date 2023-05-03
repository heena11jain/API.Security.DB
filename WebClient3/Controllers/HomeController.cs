using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using WebClient3.Models;
using WebClient3.Services;

namespace WebClient3.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        public ITokenService _tokenService;
        public HomeController(ILogger<HomeController> logger, ITokenService tokenService)
        {
            _logger = logger;
            _tokenService = tokenService;

        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Weather()
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var accessToken = await _tokenService.GetToken("");
            //var data = new List<WeatherModel>();
            //var client = new HttpClient();
            //client.SetBearerToken(accessToken.AccessToken);
            //var content = await client.GetAsync("https://localhost:5004/weatherforecast"); //api 3 port

            //if (content.IsSuccessStatusCode)
            //{
            //    var model = await content.Content.ReadAsStringAsync();
            //    data = JsonConvert.DeserializeObject<List<WeatherModel>>(model);
            //    return View(data);
            //}
            //else
            //{
            //    throw new Exception("Failed to get Data from API");
            //}

            var accessToken = await HttpContext.GetTokenAsync("access_token");

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new
                AuthenticationHeaderValue("Bearer", accessToken);
            var content = await client.GetStringAsync("https://localhost:5004/weatherforecast/get"); //api 3 port

            var model = JsonConvert.DeserializeObject<List<WeatherModel>>(content);

            return View(model);

        }

        public async Task Logout()
        {
            await HttpContext.SignOutAsync(
               CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignOutAsync(
                OpenIdConnectDefaults.AuthenticationScheme);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
