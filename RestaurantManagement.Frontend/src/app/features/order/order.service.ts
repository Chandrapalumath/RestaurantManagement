import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class OrderService {

  private menuUrl = 'https://localhost:7095/api/menu';
  private orderUrl = 'https://localhost:7095/api/orders';

  constructor(private http: HttpClient) {}

  getMenuItems() {
    return this.http.get<any[]>(this.menuUrl);
  }

  createOrder(orderData: any) {
    return this.http.post(this.orderUrl, orderData);
  }
  getOrdersForWaiter() {
    return this.http.get<any[]>(`${this.orderUrl}/waiter`); 
  }
  getOrdersByStatus(status: string) {
    return this.http.get<any[]>(`${this.orderUrl}/status?status=${status}`);
  }

  updateStatus(id: string, status: string) {
    return this.http.patch(`${this.orderUrl}/${id}`, { status });
  }
  getOrderById(id: string) {
    return this.http.get<any>(`${this.orderUrl}/${id}`);
  }
}
