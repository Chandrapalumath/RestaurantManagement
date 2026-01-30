import { Component, EventEmitter, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, MatInputModule],
  templateUrl: './customer.component.html'
})
export class CustomerComponent {

  @Output() customerSelected = new EventEmitter<string>();

  mobileNumber = '';
  customerName = '';
  foundCustomer: any = null;

  customers = [
    { id: 'C1', name: 'Rahul', mobile: '9999999999' }
  ];

  searchCustomer() {
    this.foundCustomer = this.customers.find(c => c.mobile === this.mobileNumber);

    if (this.foundCustomer!=null) {
      console.log('Customer Found:', this.foundCustomer);
      this.customerSelected.emit(this.foundCustomer.id);
    } else {
      console.log('Customer Not Found');
    }
  }

  createCustomer() {
    const newCustomer = {
      id: 'C' + (this.customers.length + 1),
      name: this.customerName,
      mobile: this.mobileNumber
    };

    this.customers.push(newCustomer);
    this.foundCustomer = newCustomer;

    console.log('Customer Created:', newCustomer);
    this.customerSelected.emit(newCustomer.id);
  }
}
