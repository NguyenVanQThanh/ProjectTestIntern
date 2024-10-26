using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using API.Entity;

namespace API.Helper
{
    public class MapperWeather
    {
        public static WeatherInLocation? MapToWeatherInLocation(string jsonResponse)
        {
            var json = JsonObject.Parse(jsonResponse);
            if (json != null)
            {
                var currentWeather = new Weather
                {
                    Date = DateTime.Parse((string)json["current"]["last_updated"]),
                    Temperature = (double)json["current"]["temp_c"],
                    Wind = (double)json["current"]["wind_kph"],
                    Humidity = (int)json["current"]["humidity"],
                    Icon = "https:" + (string)json["current"]["condition"]["icon"],
                    TextCondition = (string)json["current"]["condition"]["text"]
                };
                var forecastWeathers = new List<Weather>();
                var forecastArray = (JsonArray)json["forecast"]["forecastday"];
                foreach (var day in forecastArray)
                {
                    
                    var forecastWeather = new Weather
                    {
                        Date = DateTime.Parse((string)day["date"]),
                        Temperature = (double)day["day"]["avgtemp_c"],
                        Wind = (double)day["day"]["maxwind_kph"],
                        Humidity = (int)day["day"]["avghumidity"],
                        Icon = "https:" + (string)day["day"]["condition"]["icon"],
                        TextCondition = (string)day["day"]["condition"]["text"]
                    };
                    if (forecastWeather.Date.Date.Equals(currentWeather.Date.Date)) continue;
                    forecastWeathers.Add(forecastWeather);
                }
                var location = new WeatherInLocation{
                    Location = (string)json["location"]["name"],
                    Current = currentWeather,
                    Forecast = forecastWeathers
                };
                return location;
            }
            else return null;
        }
    }
}