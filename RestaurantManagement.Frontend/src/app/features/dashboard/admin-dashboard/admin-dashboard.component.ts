import { ChangeDetectorRef, Component, inject, OnInit } from '@angular/core';
import { AdminService } from './admin.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-dashboard',
  imports: [CommonModule],
  templateUrl: './admin-dashboard.component.html',
  styleUrls: ['./admin-dashboard.component.css']
})
export class AdminDashboardComponent implements OnInit {

  dashboard: any;
  cdr = inject(ChangeDetectorRef);
  constructor(private adminService: AdminService) { }

  ngOnInit() {
    this.adminService.getDashboardData().subscribe(res => {
      this.dashboard = res;
      this.cdr.detectChanges();
      console.log(res);
    });
  }
}
