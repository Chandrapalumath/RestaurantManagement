import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { OrderService } from '../../../services/orderService/order.service';
import { OrderResponse } from '../../../models/order.model';

@Component({
  selector: 'app-order-details',
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, MatSelectModule],
  templateUrl: './order-details.component.html'
})
export class OrderDetailsComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private service = inject(OrderService);

  order = signal<OrderResponse | null>(null);
  selectedStatus = signal<string>('');

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) this.loadOrder(id);
    });
  }

  loadOrder(id: string) {
    this.service.getOrderById(id).subscribe(res => {
      this.order.set(res);
      this.selectedStatus.set(res.status);
    });
  }

  updateStatus() {
    const currentOrder = this.order();
    if (!currentOrder) return;

    this.service.updateStatus(currentOrder.orderId, this.selectedStatus())
      .subscribe(() => {
        alert("Status Updated!");
        this.router.navigate(['/chef/orders']);
      });
  }

  goBack() {
    this.router.navigate(['/chef/orders']);
  }
}