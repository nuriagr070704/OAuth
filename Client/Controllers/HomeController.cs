using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using API;

namespace Client.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public IActionResult Privacy() => View();

        [Authorize]
        [HttpGet("/call-api")]
        public async Task<IActionResult> CallApi()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");
            if (accessToken == null) throw new InvalidOperationException("Could not find access token");
            
            var client = new HttpClient(); // you shouldn't do this. Instead use IHttpClientFactory
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync("https://localhost:5001/weatherforecast");

            if (!response.IsSuccessStatusCode)
                return StatusCode((int)response.StatusCode, "API access failed");

            var content = await response.Content.ReadAsStringAsync();

            // Deserializa el JSON a una lista de objetos WeatherForecast
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var forecasts = JsonSerializer.Deserialize<List<WeatherForecast>>(content, options);

            return Ok(forecasts);

            //return Ok(response.IsSuccessStatusCode 
            //    ? "API access authorized!" : $"API access failed. Status code: {response.StatusCode}");
        }
    }
}
