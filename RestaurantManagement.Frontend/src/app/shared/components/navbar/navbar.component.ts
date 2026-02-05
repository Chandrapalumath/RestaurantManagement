import { Component, inject } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { AuthService } from '../../../services/authService/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-navbar',
  imports: [MatMenuModule, MatButtonModule, MatIconModule],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
})
export class NavbarComponent {
  private authService = inject(AuthService);
  router = inject(Router);
  viewProfile() {
    const role = this.authService.getRole()?.toLowerCase();
    this.router.navigate([`/${role}/profile`]);
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
  changePassword() {
    this.router.navigate(['/change-password']);
  }
}
