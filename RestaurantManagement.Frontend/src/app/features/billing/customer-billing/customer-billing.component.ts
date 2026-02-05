import { Component, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { TableService } from '../../../services/tableService/table.service';
import { CustomerComponent } from '../../user/add-customer/customer.component';
import { BillService } from '../../../services/billingService/billing.service';
import { BillDialogComponent } from '../bill-dialog/bill-dialog.component';
import { BillGenerateRequest } from '../../../models/billing.model';

@Component({
  selector: 'app-customer-billing',
  imports: [CommonModule, MatButtonModule, CustomerComponent],
  templateUrl: './customer-billing.component.html'
})
export class CustomerBillingComponent {

  route = inject(ActivatedRoute);
  router = inject(Router);
  dialog = inject(MatDialog);
  billService = inject(BillService);
  tableService = inject(TableService);

  tableId = signal<string>('');
  selectedCustomerId = signal<string>('');
  orderIds = signal<string[]>([]);

  ngOnInit() {
    this.tableId.set(this.route.snapshot.paramMap.get('tableId')!);

    this.tableService.getOrdersByTable(this.tableId()).subscribe(data => {
      this.orderIds.set(data.map((o: any) => o.orderId));

      console.log("ORDER IDS:", this.orderIds);
    });
  }


  onCustomerSelected(id: string) {
    this.selectedCustomerId.set(id);
  }

  generateBill() {
    const payload: BillGenerateRequest = {
      customerId: this.selectedCustomerId(),
      ordersId: this.orderIds()
    };

    console.log("BILL PAYLOAD:", payload);

    this.billService.generateBill(payload).subscribe(res => {

      const location = res.headers.get('Location');
      const billId = location?.split('/').pop();

      if (!billId) return;

      this.billService.getBillById(billId).subscribe(bill => {

        const dialogRef = this.dialog.open(BillDialogComponent, {
          width: '450px',
          data: bill
        });
      });
    });
  }
}
