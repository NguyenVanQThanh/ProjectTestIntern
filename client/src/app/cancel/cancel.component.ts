import { CommonModule } from '@angular/common';
import { Component, inject, ViewChild } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { EmailService } from '../_services/email.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-cancel',
  standalone: true,
  imports: [CommonModule,FormsModule],
  templateUrl: './cancel.component.html',
  styleUrl: './cancel.component.css'
})
export class CancelComponent {
  @ViewChild('cancel') cancelForm: NgForm | undefined;
  private router = inject(Router);
  private toastr = inject(ToastrService);
  private emailService = inject(EmailService);
  email=''
  onCancel(){
    this.router.navigate(['/']);
  }
  onSubmit(){
    if  (this.email == ''){
      this.toastr.error("Please enter full two boxes");
    } else {
      this.emailService.sendEmailCancel(this.email).subscribe({
        next: () => {
          console.log("success");
          this.toastr.success("Mail was sent successfully");
          this.router.navigate(['/verify'],{ state: { email: this.email } });
        },
        error: (error) => {
          this.toastr.error(error.error);
        }
      })
    }
  }
}
