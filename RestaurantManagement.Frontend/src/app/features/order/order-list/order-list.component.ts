import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { OrderService } from '../../../services/orderService/order.service';
import { forkJoin } from 'rxjs';
import { OrderResponse, OrderUI } from '../../../models/order.model';
import { TableService } from '../../../services/tableService/table.service';

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
  private tableService = inject(TableService);
  orders = signal<OrderUI[]>([]);

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
      preparing: this.service.getOrdersByStatus('Preparing'),
      tables: this.tableService.getAllTables()
    }).subscribe(res => {

      const tableMap = new Map<string, string>();
      res.tables.forEach(t => tableMap.set(t.id, t.tableName));

      const combinedOrders = [...res.pending, ...res.preparing];

      const mapped: OrderUI[] = combinedOrders.map(o => ({
        ...o,
        tableName: tableMap.get(o.tableId) ?? 'Unknown Table'
      }));

      this.orders.set(mapped);
    });
  }


  loadByStatus(status: string) {
    forkJoin({
      orders: this.service.getOrdersByStatus(status),
      tables: this.tableService.getAllTables()
    }).subscribe(res => {

      const tableMap = new Map<string, string>();
      res.tables.forEach(t => tableMap.set(t.id, t.tableName));

      const mapped: OrderUI[] = res.orders.map(o => ({
        ...o,
        tableName: tableMap.get(o.tableId) ?? 'Unknown Table'
      }));

      this.orders.set(mapped);
    });
  }

  viewOrder(id: string) {
    this.router.navigate(['/chef/orders', id]);
  }
}