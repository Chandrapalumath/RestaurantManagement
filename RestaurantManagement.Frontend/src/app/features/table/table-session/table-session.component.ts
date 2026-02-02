import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule, ActivatedRoute } from '@angular/router';
import { TableService } from '../table.service';
import { BillService } from '../../billing/billing.service';
import { MatDialog } from '@angular/material/dialog';
import { BillDialogComponent } from '../../../shared/components/bill-dialog/bill-dialog.component';

@Component({
  selector: 'app-table-session',
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule],
  templateUrl: './table-session.component.html'
})

export class TableSessionComponent implements OnInit {

  tableId!: string;
  orders: any[] = [];
  selectedCustomerId!: string;

  billService = inject(BillService);
  dialog = inject(MatDialog);

  onCustomerSelected(id: string) {
    this.selectedCustomerId = id;
  }
  tableService = inject(TableService);
  route = inject(ActivatedRoute);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.tableId = this.route.snapshot.paramMap.get('id')!;
    this.loadOrders();
  }
  loadOrders() {
    this.tableService.getOrdersByTable(this.tableId).subscribe(data => {
      console.log("RAW ORDER DATA:", data);

      this.orders = data.map(o => ({
        id: o.orderId ?? o.OrderId,
        status: o.status ?? o.Status,

        items: (o.items ?? o.Items ?? []).map((i: any) => ({
          name: i.menuItemName ?? i.MenuItemName,
          quantity: i.quantity ?? i.Quantity
        }))
      }));
      this.cdr.detectChanges();
      console.log("MAPPED ORDERS:", this.orders);
    });
  }

  get canGenerateBill(): boolean {
    return this.orders.length > 0 &&
      this.orders.every(o => o.status === 'Completed');
  }

  generateBill() {

    if (!this.selectedCustomerId) {
      return;
    }

    const orderIds = this.orders.map(o => o.id);

    const payload = {
      customerId: this.selectedCustomerId,
      ordersId: orderIds
    };

    this.billService.generateBill(payload).subscribe({
      next: (res: any) => {

        const location = res.headers.get('Location');
        const billId = location?.split('/').pop();

        console.log("BILL ID:", billId);

        if (!billId) {
          alert("Bill created but ID not found");
          return;
        }

        this.billService.getBillById(billId).subscribe(bill => {

          console.log("FULL BILL:", bill);

          this.dialog.open(BillDialogComponent, {
            width: '450px',
            data: bill
          });

        });

      },
      error: err => console.log("BILL ERROR:", err)
    });
  }
}
