import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';
import { ChangePasswordCredentials } from '../../../models/auth.model';
import { AuthService } from '../../../services/authService/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './change-password.component.html'
})
export class ChangePasswordComponent {
  router = inject(Router);
  fb = inject(FormBuilder)
  authService = inject(AuthService)
  passwordPattern =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).*$/;

  changePasswordForm = this.fb.group({
    currentPassword: ['', [
      Validators.required,
      Validators.minLength(12),
      Validators.maxLength(20),
      Validators.pattern(this.passwordPattern)
    ]],
    confirmPassword: ['', [
      Validators.required,
      Validators.minLength(12),
      Validators.maxLength(20),
      Validators.pattern(this.passwordPattern)
    ]],
    newPassword: ['', [
      Validators.required,
      Validators.minLength(12),
      Validators.maxLength(20),
      Validators.pattern(this.passwordPattern)
    ]]
  });

  onSubmit() {
    if (this.changePasswordForm.valid) {
      if (this.changePasswordForm.value.confirmPassword === this.changePasswordForm.value.newPassword) {
        const credentials: ChangePasswordCredentials = {
          currentPassword: this.changePasswordForm.value.currentPassword!,
          newPassword: this.changePasswordForm.value.newPassword!
        };

        console.log("FORM DATA:", credentials);

        this.authService.changePassword(credentials).subscribe({
          next: (res) => {
            console.log("API RESPONSE:", res);
            this.authService.saveToken(res.token);

            const role = this.authService.getRole();
            console.log("DECODED ROLE:", role);

            console.log('Change Password Data:', this.changePasswordForm.value);
            this.router.navigate(['/login']);
          },
          error: (err) => {
            console.error("API ERROR:", err);
          }
        });
      }
    }
  }
}
