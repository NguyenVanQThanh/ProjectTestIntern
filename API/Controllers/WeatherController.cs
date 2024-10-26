using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class WeatherController(HttpClient httpClient, IConfiguration configuration) : BaseApiController(httpClient, configuration)
    {
        [HttpGet]
        public async Task<IActionResult> GetWeather(string q){
            string apiKey = _configuration["ApiKey"];
            if (apiKey is null){
                return BadRequest("Not Access");
            }
            HttpResponseMessage response = await _httpClient.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={q}&days=5");
            if (!response.IsSuccessStatusCode){
                return StatusCode((int)response.StatusCode, "Error occurred while fetching weather data");
            }
            var result = await response.Content.ReadAsStringAsync();
            var location = MapperWeather.MapToWeatherInLocation(result);
            return Ok(location);
        }
    }
}