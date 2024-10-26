using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entity
{
    public class Weather
    {
        public DateTime Date { get; set; }
        public double Temperature { get; set; }
        public double Wind { get; set; }
        public int Humidity { get; set; }
        public string? Icon { get; set; }
        public string? TextCondition { get; set; }
    }
}