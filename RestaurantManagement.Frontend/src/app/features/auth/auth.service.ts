import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({ providedIn: 'root' })
export class AuthService {

  private apiUrl = 'https://localhost:7095/api/auth/login';

  constructor(private http: HttpClient, private router: Router) {}

  login(data: any) {
    return this.http.post<any>(this.apiUrl, data);
  }

  saveToken(token: string) {
  localStorage.setItem('token', token);

  const decoded: any = jwtDecode(token);
  console.log("DECODED TOKEN:", decoded);

  const role =
    decoded["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"];

  const email =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"];

  const userId =
    decoded["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"];

  const name = decoded["fullName"];

  localStorage.setItem('role', role);
  localStorage.setItem('email', email);
  localStorage.setItem('userId', userId);
  localStorage.setItem('name', name);
}

  getRole() {
    return localStorage.getItem('role');
  }

  logout() {
    localStorage.clear();
    this.router.navigate(['/']);
  }
}

