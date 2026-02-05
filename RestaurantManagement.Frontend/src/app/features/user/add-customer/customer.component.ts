import { Component, EventEmitter, Output, inject, signal } from '@angular/core';
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
  private service = inject(CustomerService);
  @Output() customerSelected = new EventEmitter<string>();

  mobileNumber = signal<string>('');
  customerName = signal<string>('');
  foundCustomer = signal<any>(null);
  searchPerformed = signal<boolean>(false);

  searchCustomer() {
    const mobile = this.mobileNumber();
    if (mobile.length !== 10) {
      alert("Enter valid 10 digit number");
      return;
    }

    this.searchPerformed.set(true);
    this.service.searchByMobile(mobile).subscribe({
      next: (res) => {
        if (!res || res.length === 0) {
          this.foundCustomer.set(null);
          return;
        }
        const customer = res[0];
        this.foundCustomer.set(customer);
        this.customerName.set(customer.name);
        this.customerSelected.emit(customer.id);
      },
      error: () => alert("Server error")
    });
  }

  createCustomer() {
    const name = this.customerName();
    const mobile = this.mobileNumber();

    if (!name) {
      alert("Enter customer name");
      return;
    }

    const payload = { name, mobileNumber: mobile };

    this.service.createCustomer(payload).subscribe({
      next: (res) => {
        const location = res.headers.get('Location');
        const id = location?.split('/').pop();

        this.foundCustomer.set({ id, name, mobileNumber: mobile });
        this.customerSelected.emit(id!);
      },
      error: () => alert("Backend error while creating customer")
    });
  }

  clear() {
    this.searchPerformed.set(false);
    this.foundCustomer.set(null);
    this.customerName.set('');
    this.mobileNumber.set('');
  }
}