import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatDialog } from '@angular/material/dialog';
import { CustomerComponent } from '../../../shared/components/customer/customer.component';
import { BillDialogComponent } from '../../../shared/components/bill-dialog/bill-dialog.component';

@Component({
  selector: 'app-generate-bill',
  imports: [CommonModule, MatCardModule, MatButtonModule, CustomerComponent],
  templateUrl: './generate-bill.component.html'
})
export class GenerateBillComponent {

  tableId = '10'
  customerId: string | null = null;
  private dialog = inject(MatDialog)
  
  orders = [
    { id: 'O1', status: 'Completed' },
    { id: 'O2', status: 'Completed' }
  ];

  onCustomerSelected(id: string) {
    this.customerId = id;
  }

  generateBill() {
    if (!this.customerId) return;

    const dto = {
      customerId: this.customerId,
      ordersId: this.orders.map(o => o.id)
    };

    console.log('BillGenerateRequestDto:', dto);
    // MOCK API RESPONSE
    const apiResponse = {
      billId: 'B101',
      customerId: this.customerId,
      subTotal: 1500,
      discountPercent: 10,
      discountAmount: 150,
      taxPercent: 5,
      taxAmount: 67.5,
      grandTotal: 1417.5,
      isPaymentDone: false,
      generatedAt: new Date()
    };

    console.log('BillResponseDto:', apiResponse);

    this.dialog.open(BillDialogComponent, {
      width: '400px',
      data: apiResponse
    });
  }
}
