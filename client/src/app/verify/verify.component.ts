import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { EmailService } from '../_services/email.service';

@Component({
  selector: 'app-verify',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './verify.component.html',
  styleUrl: './verify.component.css'
})
export class VerifyComponent implements OnInit {
  email: string | null = null;
  private toastr = inject(ToastrService);
  private emailService = inject(EmailService);
  constructor(private route: ActivatedRoute,private router: Router) {}
  verificationCode: String | null = null;
  ngOnInit(): void {
    this.email = history.state.email || null;
  }
  onSubmit(){
    if (!this.email || !this.verificationCode) {
      this.toastr.error("Something is missing.");
      return;
    }
    this.emailService.verifyEmail(this.email, this.verificationCode).subscribe({
      next: () => {
        console.log("success");
        this.toastr.success("Verification Code is correctly verified");
        this.router.navigate(['/']);
      },
      error: (error) => {
        this.toastr.error(error.error);
      }
    })
  }
  onCancel(){
    this.router.navigate(['/']);
  }
  onInputChange(event: any) {
    const input = event.target as HTMLInputElement;
    input.value = input.value.replace(/[^0-9]/g, '');
    // Cập nhật giá trị verificationCode
    this.verificationCode = input.value;
  }

}
