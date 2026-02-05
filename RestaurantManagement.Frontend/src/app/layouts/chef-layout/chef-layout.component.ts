import { Component, inject } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { NavbarComponent } from "../../shared/components/navbar/navbar.component";

@Component({
  selector: 'app-chef-layout',
  imports: [CommonModule, RouterModule, MatIconModule, MatMenuModule, NavbarComponent],
  templateUrl: './chef-layout.component.html',
  styleUrl: './chef-layout.component.css'
})
export class ChefLayoutComponent {
  router = inject(Router);

  viewProfile() {
    this.router.navigate(['/waiter/profile']);
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/login']);
  }
}
