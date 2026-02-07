import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { OrderService } from '../../../services/orderService/order.service';
import { OrderUI } from '../../../models/order.model';
import { forkJoin } from 'rxjs';
import { TableService } from '../../../services/tableService/table.service';

@Component({
  selector: 'app-waiter-orders',
  imports: [CommonModule, MatCardModule],
  templateUrl: './order-status.component.html',
  styleUrl: './order-status.component.css'
})
export class WaiterOrdersStatusComponent implements OnInit {
  private service = inject(OrderService);
  private tableService = inject(TableService);

  orders = signal<OrderUI[]>([]);

  ngOnInit() {
    forkJoin({
      orders: this.service.getOrdersForWaiter(),
      tables: this.tableService.getAllTables()
    }).subscribe(({ orders, tables }) => {

      const tableMap = new Map<string, string>();

      tables.forEach(t => {
        tableMap.set(t.id, t.tableName);   // ✅ FIX HERE
      });

      const mappedOrders: OrderUI[] = orders.map(order => ({
        ...order,
        tableName: tableMap.get(order.tableId) ?? 'Unknown Table'
      }));

      this.orders.set(mappedOrders);
    });
  }
}