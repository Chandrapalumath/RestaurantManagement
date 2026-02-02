import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';

import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { AuthService } from '../auth.service';


@Component({
  selector: 'app-login',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './login.component.html'
})

export class LoginComponent {
  private fb = inject(FormBuilder);
  private router = inject(Router);
  private authService = inject(AuthService);

  loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });

  onSubmit() {
    if (this.loginForm.invalid) {
      console.log("FORM INVALID", this.loginForm.value);
      return;
    }

    console.log("FORM DATA:", this.loginForm.value);

    this.authService.login(this.loginForm.value).subscribe({
      next: (res) => {
        console.log("API RESPONSE:", res);
        this.authService.saveToken(res.token);

        const role = this.authService.getRole();
        console.log("DECODED ROLE:", role);

        if (role === 'Waiter') this.router.navigate(['/waiter/dashboard']);
        else if (role === 'Admin') this.router.navigate(['/admin/dashboard']);
        else if (role === 'Chef') this.router.navigate(['/chef/dashboard']);
        else console.error("ROLE NOT MATCHING");
      },
      error: (err) => {
        console.error("API ERROR:", err);
      }
    });
  }
}
