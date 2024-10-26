using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Entity;

namespace API.Services
{
    public class WeatherService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public WeatherService(IConfiguration configuration, HttpClient httpClient)
        {
            _configuration = configuration;
            _httpClient = httpClient;
        }
        public async Task<WeatherInLocation?> GetWeatherAsync(string location)
        {
            string apiKey = _configuration["ApiKey"];
            var response = await _httpClient.GetAsync($"https://api.weatherapi.com/v1/forecast.json?key={apiKey}&q={location}&days=5");
            response.EnsureSuccessStatusCode();
            // Assuming the API returns JSON data compatible with your WeatherInLocation class
            var weatherData = await response.Content.ReadFromJsonAsync<WeatherInLocation>();
            if (weatherData != null)
                return weatherData;
            return null;
        }


    }
}