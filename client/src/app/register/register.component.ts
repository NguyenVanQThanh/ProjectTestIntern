import { CommonModule } from '@angular/common';
import { Component, inject, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { provideToastr, ToastrModule, ToastrService } from 'ngx-toastr';
import { EmailService } from '../_services/email.service';
import { bootstrapApplication } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  @ViewChild('registerForm') registerForm: NgForm | undefined;
  private router = inject(Router);
  // private toastr = inject(ToastrService);
  private emailService = inject(EmailService);
  email=''
  location=''
  constructor(private toastr: ToastrService){}
  onCancel(){
    this.router.navigate(['/']);
  }
  onSubmit(){
    if (this.location == '' || this.email == ''){
      this.toastr.error("Please enter full two boxes");
    } else {
      this.emailService.sendEmailRegister(this.email,this.location).subscribe({
        next: () => {
          console.log("success");
          this.toastr.success("Mail was sent successfully");
          this.router.navigate(['/verify'],{ state: { email: this.email } });
        },
        error: (error) => {
          console.log("failed");
          this.toastr.error(error.error);
        }
      })
    }
  }
}
// bootstrapApplication(RegisterComponent, {
//   providers: [
//     provideHttpClient(),
//     provideAnimations(), // required animations providers
//     provideToastr({
//       timeOut: 10000,
//       positionClass: 'toast-bottom-right',
//       preventDuplicates: true,
//     }), // Toastr providers
//   ]
// });
