import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';
import { Injectable, inject, signal } from '@angular/core';
import { ChangePasswordCredentials, LoginResponse, UserCredentials } from '../../models/auth.model';

export interface AuthUser {
  userId: string;
  email: string;
  role: string;
  name: string;
}

@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private apiUrl = 'https://localhost:7095/api/auth/login';
  private changePasswordUrl = 'https://localhost:7095/api/auth/change-password';

  currentUser = signal<AuthUser | null>(this.getUserFromStorage());

  login(data: UserCredentials) {
    return this.http.post<LoginResponse>(this.apiUrl, data);
  }

  changePassword(data: ChangePasswordCredentials) {
    alert('Password Change Triggered')
    return this.http.patch<LoginResponse>(this.changePasswordUrl, data);
  }

  saveToken(token: string) {
    localStorage.setItem('token', token);

    const decoded: any = jwtDecode(token);

    const user: AuthUser = {
      role: decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
      email: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
      userId: decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
      name: decoded["fullName"]
    };

    localStorage.setItem('role', user.role);
    localStorage.setItem('email', user.email);
    localStorage.setItem('userId', user.userId);
    localStorage.setItem('name', user.name);

    this.currentUser.set(user);
  }

  getRole() {
    return this.currentUser()?.role || localStorage.getItem('role');
  }

  logout() {
    localStorage.clear();
    this.currentUser.set(null);
    this.router.navigate(['/']);
  }

  private getUserFromStorage(): AuthUser | null {
    const role = localStorage.getItem('role');
    if (!role) return null;

    return {
      role: role,
      email: localStorage.getItem('email') || '',
      userId: localStorage.getItem('userId') || '',
      name: localStorage.getItem('name') || ''
    };
  }
}