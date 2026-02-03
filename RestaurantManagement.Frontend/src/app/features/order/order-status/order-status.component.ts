import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { OrderService } from '../../order/order.service';

@Component({
  selector: 'app-waiter-orders',
  imports: [CommonModule, MatCardModule],
  template: `
  <h2>My Orders Status</h2>

  @for (o of orders; track o.orderId ?? $index) {
  <mat-card class="order-card mb-3">
    <p><strong>Table ID:</strong> {{ o.tableId }}</p>
    <p><strong>Order ID:</strong> {{ o.orderId }}</p>

    <p><strong>Status:</strong>
      <span class="badge"
        [ngClass]="{
          'bg-warning text-dark': o.status === 'Pending',
          'bg-info text-white': o.status === 'Preparing'
        }">
        {{ o.status }}
      </span>
    </p>

    <div class="items-section mt-2">
      <strong>Items:</strong>
      @for (item of o.items; track $index) {
        <div>
          • {{ item.menuItemName }} × {{ item.quantity }}
        </div>
      } @empty {
        <p class="text-muted small">No items in this order.</p>
      }
    </div>
  </mat-card>
} @empty {
  <div class="alert alert-info">
    No active orders found for this table.
  </div>
}
  `,
  styles: [`.order-card{margin-bottom:15px}`]
})

export class WaiterOrdersStatusComponent implements OnInit {

  orders: any[] = [];
  service = inject(OrderService);
  private cdr = inject(ChangeDetectorRef);
  
  ngOnInit() {
    this.service.getOrdersForWaiter().subscribe(data => {
      console.log("WAITER ORDERS:", data);
      this.orders = data;
      this.cdr.detectChanges();
    });
  }
}