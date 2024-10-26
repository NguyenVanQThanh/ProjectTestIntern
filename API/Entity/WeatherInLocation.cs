using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class WeatherInLocation
    {
        public required string Location { get; set; }
        public required Weather Current { get; set; }
        public required List<Weather> Forecast { get; set; }
    }
}