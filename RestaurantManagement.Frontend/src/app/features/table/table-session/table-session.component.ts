import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { TableService } from '../../../services/tableService/table.service';
import { BillService } from '../../../services/billingService/billing.service';

@Component({
  selector: 'app-table-session',
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule],
  templateUrl: './table-session.component.html'
})
export class TableSessionComponent implements OnInit {
  private tableService = inject(TableService);
  private billService = inject(BillService);
  private route = inject(ActivatedRoute);
  private dialog = inject(MatDialog);

  tableId = signal<string>('');
  orders = signal<any[]>([]);
  isBillGenerated = signal<boolean>(false);

  canGenerateBill = computed(() => {
    const currentOrders = this.orders();
    return currentOrders.length > 0 && currentOrders.every(o => o.status === 'Completed');
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.tableId.set(id);
      this.loadOrders();
    }
  }

  loadOrders() {
    this.tableService.getOrdersByTable(this.tableId()).subscribe(data => {
      this.orders.set(data.map(o => ({
        id: o.orderId ?? o.OrderId,
        status: o.status ?? o.Status,
        items: (o.items ?? o.Items ?? []).map((i: any) => ({
          name: i.menuItemName ?? i.MenuItemName,
          quantity: i.quantity ?? i.Quantity
        }))
      })));
    });
  }

  generateBill() {
    this.isBillGenerated.set(true);
  }
}