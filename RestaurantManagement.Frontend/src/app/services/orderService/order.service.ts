import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { MenuItemResponse } from '../../models/billing.model';
import { OrderCreateRequest, OrderResponse, OrderUpdateRequest } from '../../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {

  private http = inject(HttpClient);
  private readonly menuUrl = 'https://localhost:7095/api/menu';
  private readonly orderUrl = 'https://localhost:7095/api/orders';

  getMenuItems(): Observable<MenuItemResponse[]> {
    return this.http.get<MenuItemResponse[]>(this.menuUrl);
  }

  createOrder(orderData: OrderCreateRequest) {
    return this.http.post(this.orderUrl, orderData);
  }

  getOrdersForWaiter(): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.orderUrl}/waiter`);
  }

  getOrdersByStatus(status: string): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.orderUrl}/status?status=${status}`);
  }

  updateStatus(id: string, status: string) {
    const payload: OrderUpdateRequest = { status };
    return this.http.patch(`${this.orderUrl}/${id}`, payload);
  }

  getOrderById(id: string): Observable<OrderResponse> {
    return this.http.get<OrderResponse>(`${this.orderUrl}/${id}`);
  }
}