import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { CommonModule } from '@angular/common';

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

  fb = inject(FormBuilder)
  passwordPattern =
    /^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).*$/;

  changePasswordForm = this.fb.group({
    currentPassword: ['', [
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
      console.log('Change Password Data:', this.changePasswordForm.value);
    }
    // Call API for change password
  }
}
