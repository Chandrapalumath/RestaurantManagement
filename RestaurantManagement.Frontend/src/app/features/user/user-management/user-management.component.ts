import { Component, inject, ViewChild } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule, AbstractControl, ValidatorFn, ValidationErrors } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { HttpClient } from '@angular/common/http';
import { DialogService } from '../../../services/dialogService/dialog.service';
import { FormGroupDirective } from '@angular/forms';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatNativeDateModule } from '@angular/material/core';

@Component({
  selector: 'app-user-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatDatepickerModule,
    MatNativeDateModule
  ],
  templateUrl: './user-management.component.html',
  styleUrls: ['./user-management.component.css']
})
export class UserManagementComponent {

  private fb = inject(FormBuilder);
  private http = inject(HttpClient);
  private dialog = inject(DialogService);
  @ViewChild(FormGroupDirective) formDirective!: FormGroupDirective;

  roles = ['Admin', 'Waiter', 'Chef'];
  maxDate = new Date();
  form = this.fb.group({
    email: ['', [
      Validators.required,
      Validators.pattern(/^[^\s@]+@[^\s@]+\.[^\s@]{2,}$/)
    ]],

    fullName: ['', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(50),
      this.fullNameValidator
    ]],

    mobileNumber: ['', [
      Validators.required,
      Validators.pattern(/^\d{10}$/)
    ]],

    password: ['', [
      Validators.required,
      Validators.minLength(12),
      Validators.maxLength(20),
      Validators.pattern(/^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).*$/)
    ]],
    confirmPassword: ['', Validators.required],
    dateOfBirth: ['', [
      Validators.required,
      this.ageValidator
    ]],

    aadharNumber: ['', [
      Validators.required,
      Validators.pattern(/^\d{12}$/)
    ]],
    role: [null, Validators.required]
  }, { validators: this.passwordMatchValidator('password', 'confirmPassword') });

  passwordMatchValidator(controlName: string, matchingControlName: string): ValidatorFn {
    return (abstractControl: AbstractControl): ValidationErrors | null => {
      const control = abstractControl.get(controlName);
      const matchingControl = abstractControl.get(matchingControlName);

      if (!control || !matchingControl || matchingControl.errors && !matchingControl.errors['mismatch']) {
        return null;
      }

      if (control.value !== matchingControl.value) {
        matchingControl.setErrors({ mismatch: true });
        return { mismatch: true };
      } else {
        matchingControl.setErrors(null);
        return null;
      }
    };
  }
  fullNameValidator(control: AbstractControl) {
    const value: string = control.value || '';

    if (value.length < 3) return { minlength: true };

    const firstThree = value.substring(0, 3);
    if (!/^[A-Za-z]{3}$/.test(firstThree)) {
      return { firstThreeLetters: true };
    }

    if (!/^[A-Za-z0-9 ]+$/.test(value)) {
      return { invalidChars: true };
    }
    return null;
  }
  allowOnlyNumbers(event: KeyboardEvent) {
    if (!/^\d$/.test(event.key) && event.key !== 'Backspace' && event.key !== 'Tab') {
      event.preventDefault();
    }
  }
  ageValidator(control: AbstractControl) {
    if (!control.value) return null;

    const dob = new Date(control.value);
    const today = new Date();

    // Invalid Date check (jaise 2/30/2002)
    if (isNaN(dob.getTime())) {
      return { invalidDate: true };
    }

    let age = today.getFullYear() - dob.getFullYear();
    const m = today.getMonth() - dob.getMonth();
    if (m < 0 || (m === 0 && today.getDate() < dob.getDate())) {
      age--;
    }

    if (age < 18) return { underAge: true };
    if (age > 150) return { tooOld: true };

    return null;
  }
  submit() {
    if (this.form.invalid) {
      return;
    }
    if (this.form.value.password != this.form.value.confirmPassword) return;
    const { confirmPassword, ...payload } = this.form.value;
    this.http.post('https://localhost:7095/api/users', payload)
      .subscribe({
        next: () => {
          this.dialog.open('User Created Successfully');
          this.form.reset();
          this.formDirective.resetForm();
        },
        error: err => alert(err.error?.Message || "Error creating user")
      });
  }
}
