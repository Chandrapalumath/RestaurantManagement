import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { OrderService } from '../../../services/orderService/order.service';
import { forkJoin } from 'rxjs';
import { OrderResponse } from '../../../models/order.model';

@Component({
  selector: 'app-order-list',
  imports: [CommonModule, MatCardModule],
  templateUrl: './order-list.component.html',
  styleUrl: './order-list.component.css'
})
export class OrderListComponent implements OnInit {
  private service = inject(OrderService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);

  orders = signal<OrderResponse[]>([]);
  title = signal<string>("All Orders");

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      const status = params.get('status') || 'all';
      if (status === 'all') {
        this.title.set("All Active Orders");
        this.loadActiveOrders();
      } else {
        this.title.set(`${status} Orders`);
        this.loadByStatus(status);
      }
    });
  }

  loadActiveOrders() {
    forkJoin({
      pending: this.service.getOrdersByStatus('Pending'),
      preparing: this.service.getOrdersByStatus('Preparing')
    }).subscribe(res => {
      this.orders.set([...res.pending, ...res.preparing]);
    });
  }

  loadByStatus(status: string) {
    this.service.getOrdersByStatus(status).subscribe(data => this.orders.set(data));
  }

  viewOrder(id: string) {
    this.router.navigate(['/chef/orders', id]);
  }
}