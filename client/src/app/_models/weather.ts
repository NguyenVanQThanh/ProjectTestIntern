export interface DataWeather {
  location: string;
  current: Weather;
  forecast: Weather[];
}

export interface Weather {
  date: string;
  temperature: number;
  wind: number;
  humidity: number;
  icon: string;
  textCondition: string;
}
