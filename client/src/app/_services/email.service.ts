import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class EmailService {
  baseUrl = environment.apiUrl;
  constructor(private http: HttpClient) {

  }
  sendEmailRegister(email: string, location: string){
    return this.http.post(`${this.baseUrl}email/send-verification`,{email, location},{ responseType: 'text' });
  }
  sendEmailCancel(email:string){
    return this.http.post(`${this.baseUrl}email/send-disabled-verification`,{email},{ responseType: 'text' });
  }
  verifyEmail(email: String, verificationCode: String){
    return this.http.post(`${this.baseUrl}email/verify`,{email, verificationCode},{ responseType: 'text' });
  }
}
