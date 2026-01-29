import { Component } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

/* Angular Material */
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-login',
  // standalone: true,  
  templateUrl: './login.html',
  styleUrls: ['./login.css'],
  imports: [
    CommonModule,          
    ReactiveFormsModule,  
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatCardModule,
    MatIconModule
  ]
})
export class Login {

  hidePassword = true;
  loginForm;
  constructor(private fb: FormBuilder) {
    this.loginForm = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', Validators.required]
  });
  }
  submit() {
    console.log(this.loginForm.value);
  }
}
