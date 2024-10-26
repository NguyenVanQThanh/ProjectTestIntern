import { CommonModule } from '@angular/common';

import { Component, inject, OnInit, ViewChild } from '@angular/core';
import { SearchService } from '../_services/search.service';
import { FormsModule, NgForm } from '@angular/forms';
import { DataWeather } from '../_models/weather';
import { RouterModule } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule,RouterModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  @ViewChild('searchForm') searchForm: NgForm | undefined;
  private searchService = inject(SearchService);
  private toastr = inject(ToastrService);
  location='';
  weatherLocation? : DataWeather;
  ngOnInit(): void {
    this.loadWeatherData();
  }
  onSearch(){
    this.searchService.getWeatherData(this.location).subscribe({
      next: data => {
        this.weatherLocation = data;
        this.saveWeatherData(data);
      },
      error: error => this.toastr.error("No weather data")
    });
    this.searchForm?.resetForm();
  }
  saveWeatherData(data: DataWeather) {
    localStorage.setItem('weatherData', JSON.stringify(data));
  }
  loadWeatherData() {
    const storedData = localStorage.getItem('weatherData');
    if (storedData) {
      this.weatherLocation = JSON.parse(storedData);
    }
  }
}
