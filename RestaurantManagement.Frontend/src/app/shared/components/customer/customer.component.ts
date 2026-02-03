import { Component, EventEmitter, Output, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { CustomerService } from './customer.service';

@Component({
  selector: 'app-customer',
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, MatInputModule],
  templateUrl: './customer.component.html'
})
export class CustomerComponent {

  service = inject(CustomerService);
  @Output() customerSelected = new EventEmitter<string>();

  mobileNumber = '';
  customerName = '';
  foundCustomer: any = null;

  searchPerformed = false;   

  searchCustomer() {

  if (this.mobileNumber.length !== 10) {
    alert("Enter valid 10 digit number");
    return;
  }

  this.searchPerformed = true;

  this.service.searchByMobile(this.mobileNumber).subscribe({
    next: (res) => {

      if (!res || res.length === 0) {
        this.foundCustomer = null;
        return;
      }

      this.foundCustomer = res[0];

      this.customerName = this.foundCustomer.name;
      this.mobileNumber = this.foundCustomer.mobileNumber;

      this.customerSelected.emit(this.foundCustomer.id);
    },
    error: () => alert("Server error")
  });
}


  createCustomer() {

    if (!this.customerName) {
      alert("Enter customer name");
      return;
    }

    const payload = {
      name: this.customerName,
      mobileNumber: this.mobileNumber
    };

    this.service.createCustomer(payload).subscribe({
      next: (res) => {

        const location = res.headers.get('Location');
        const id = location?.split('/').pop();

        this.foundCustomer = {
          id,
          name: this.customerName,
          mobileNumber: this.mobileNumber
        };

        this.customerSelected.emit(id!);
      },
      error: () => alert("Backend error while creating customer")
    });
  }

  clear() {
    this.searchPerformed = false;
    this.foundCustomer = null;
    this.customerName = '';
    this.mobileNumber = '';
  }
}
