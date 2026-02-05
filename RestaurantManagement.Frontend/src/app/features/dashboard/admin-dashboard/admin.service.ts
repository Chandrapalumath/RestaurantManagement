import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { forkJoin, map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AdminService {

  private baseUrl = 'https://localhost:7095/api';

  constructor(private http: HttpClient) {}

  getDashboardData() {
  return forkJoin({
    reviews: this.http.get<any[]>(`${this.baseUrl}/reviews`),
    tables: this.http.get<any[]>(`${this.baseUrl}/table`),
    bills: this.http.get<any[]>(`${this.baseUrl}/billing`),
    settings: this.http.get<any>(`${this.baseUrl}/settings`)
  }).pipe(
    map(data => {

      const totalTables = data.tables.length;
      const occupiedTables = data.tables.filter(t => t.isOccupied).length;

      const totalSales = data.bills
        .filter(b => b.isPaymentDone)
        .reduce((sum, bill) => sum + bill.grandTotal, 0);

      const averageRating =
        data.reviews.length === 0 ? 0 :
        (data.reviews.reduce((sum, r) => sum + r.rating, 0) / data.reviews.length).toFixed(1);

      const topWaiters = this.getTopWaiters(data.bills);

      return {
        totalTables,
        occupiedTables,
        totalSales,
        tax: data.settings.taxPercent,
        discount: data.settings.discountPercent,
        averageRating,
        topWaiters
      };
    })
  );
}


  private getTopWaiters(bills: any[]) {
    const mapWaiters: any = {};

    bills
      .filter(b => b.isPaymentDone)  
      .forEach(b => {
        if (!b.WaiterId) return;    
        mapWaiters[b.WaiterId] = (mapWaiters[b.WaiterId] || 0) + 1;
      });

    return Object.entries(mapWaiters)
      .map(([WaiterId, count]) => ({ WaiterId, count }))
      .sort((a: any, b: any) => b.count - a.count)
      .slice(0, 5);
  }
}
