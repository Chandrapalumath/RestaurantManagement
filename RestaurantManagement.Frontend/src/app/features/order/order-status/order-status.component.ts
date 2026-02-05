import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { OrderService } from '../../../services/orderService/order.service';
import { OrderResponse } from '../../../models/order.model';

@Component({
  selector: 'app-waiter-orders',
  imports: [CommonModule, MatCardModule],
  templateUrl: './order-status.component.html',
  styleUrl: './order-status.component.css'
})
export class WaiterOrdersStatusComponent implements OnInit {
  private service = inject(OrderService);

  orders = signal<OrderResponse[]>([]);

  ngOnInit() {
    this.service.getOrdersForWaiter().subscribe(data => {
      this.orders.set(data);
    });
  }
}