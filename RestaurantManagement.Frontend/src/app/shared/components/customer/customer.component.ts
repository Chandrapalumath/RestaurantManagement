import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CustomerService } from './customer.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule
  ],
  templateUrl: './customer.component.html'
})

export class CustomerComponent {

  @Output() customerSelected = new EventEmitter<string>();

  mobileNumber = '';
  customerName = '';
  foundCustomer: any = null;

  service = inject(CustomerService);

  searchCustomer() {
    this.service.searchByMobile(this.mobileNumber).subscribe({
      next: (res: any[]) => {

        if (!res || res.length === 0) {
          this.foundCustomer = null;
          alert("Customer not found");
          return;
        }

        this.foundCustomer = res[0];

        console.log("FOUND CUSTOMER:", this.foundCustomer);

        this.customerSelected.emit(this.foundCustomer.id);
      },
      error: err => {
        console.log(err);
        alert("Customer not found");
      }
    });
  }

  createCustomer() {
    const payload = { name: this.customerName, mobile: this.mobileNumber };

    this.service.createCustomer(payload).subscribe((res: any) => {
      this.foundCustomer = res;
      this.customerSelected.emit(res.id);
    });
  }
}
