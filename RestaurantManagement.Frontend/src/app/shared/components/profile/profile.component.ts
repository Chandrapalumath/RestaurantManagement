import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, MatCardModule],
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {

  http = inject(HttpClient);
  user = signal<UserResponse | null>(null);

  ngOnInit() {
    const id = localStorage.getItem('userId');
    console.log(id);
    this.http.get<UserResponse>(`https://localhost:7095/api/users/${id}`)
      .subscribe(res => this.user.set(res));
  }
}

interface UserResponse {
  id: string;
  fullName: string;
  mobileNumber: string;
  email: string;
  role: string;
  isActive: boolean;
  aadharNumber: string;
  dateOfBirth: string;
}