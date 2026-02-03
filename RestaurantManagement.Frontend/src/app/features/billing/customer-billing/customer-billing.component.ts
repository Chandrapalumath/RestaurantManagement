import { Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';
import { TableService } from '../../table/table.service';
import { CustomerComponent } from '../../../shared/components/customer/customer.component';
import { BillService } from '../billing.service';
import { BillDialogComponent } from '../../../shared/components/bill-dialog/bill-dialog.component';

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

  tableId!: string;
  selectedCustomerId!: string;
  orderIds: string[] = [];

  ngOnInit() {
    this.tableId = this.route.snapshot.paramMap.get('tableId')!;

    this.tableService.getOrdersByTable(this.tableId).subscribe(data => {
      this.orderIds = data.map((o: any) => o.orderId);
      console.log("ORDER IDS:", this.orderIds);
    });
  }

  onCustomerSelected(id: string) {
    this.selectedCustomerId = id;
  }

  generateBill() {
    const payload = {
      customerId: this.selectedCustomerId,
      ordersId: this.orderIds  
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

        dialogRef.afterClosed().subscribe(() => {
          this.router.navigate(['/waiter/dashboard']);
        });
      });
    });
  }
}
