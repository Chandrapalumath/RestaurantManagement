import { Component, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-user-management',
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatInputModule, MatButtonModule, MatSelectModule],
  templateUrl: './user-management.component.html',
  styleUrl: './user-management.component.css'
})
export class UserManagementComponent {
  private fb = inject(FormBuilder);
  private http = inject(HttpClient);

  roles = ['Admin', 'Waiter', 'Chef'];

  form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    fullName: ['', [Validators.required, Validators.minLength(3)]],
    mobileNumber: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
    password: ['', [
      Validators.required,
      Validators.minLength(12),
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).*$/)
    ]],
    role: [null as number | null, Validators.required]
  });

  submit() {

    this.http.post('https://localhost:7095/api/users', this.form.value)
      .subscribe({
        next: () => {
          alert("User Created Successfully");
          this.form.reset();
        },
        error: err => {
          console.error(err);
          alert(err.error?.Message || "Error creating user");
        }
      });
  }
}