import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface TopWaiterDto {
  waiterId: string;
  waiterName: string;
  ordersServed: number;
}

export interface AdminDashboardDto {
  totalTables: number;
  occupiedTables: number;
  totalSales: number;
  taxPercent: number;
  discountPercent: number;
  averageRating: number;
  topWaiters: TopWaiterDto[];
}

@Injectable({ providedIn: 'root' })
export class AdminService {

  private baseUrl = 'https://localhost:7095/api';

  constructor(private http: HttpClient) { }

  getDashboardData(): Observable<AdminDashboardDto> {
    return this.http.get<AdminDashboardDto>(`${this.baseUrl}/dashboard`);
  }
}
