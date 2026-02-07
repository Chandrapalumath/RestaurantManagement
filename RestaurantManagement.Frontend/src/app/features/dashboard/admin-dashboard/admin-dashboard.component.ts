import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminService, AdminDashboardDto } from './admin.service';

@Component({
  selector: 'app-admin-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {

  private adminService = inject(AdminService);

  dashboard = signal<AdminDashboardDto | null>(null);

  ngOnInit() {
    this.adminService.getDashboardData().subscribe(res => {
      this.dashboard.set(res);
      console.log(res);
    });
  }
}
