import { Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { DataWeather } from '../_models/weather';

@Injectable({
  providedIn: 'root'
})
export class SearchService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) { }
  getWeatherData(location: string){
    return this.http.get<DataWeather>(`${this.baseUrl}weather?q=${location}`);
  }

}
