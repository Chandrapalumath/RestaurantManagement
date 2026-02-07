import { Component, EventEmitter, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { CustomerService } from './customer.service';
import { CustomerResponse } from '../../../models/review.model';
import { DialogService } from '../../../services/dialogService/dialog.service';

@Component({
  selector: 'app-customer',
  standalone: true,
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, MatInputModule],
  templateUrl: './customer.component.html',
  styleUrls: ['./customer.component.css']
})
export class CustomerComponent {

  private service = inject(CustomerService);
  private dialog = inject(DialogService);

  @Output() customerSelected = new EventEmitter<string>();

  mobileNumber = signal<string>('');
  customerName = signal<string>('');
  foundCustomer = signal<CustomerResponse | null>(null);
  searchPerformed = signal<boolean>(false);

  allowOnlyNumbers(event: KeyboardEvent) {
    if (!/^\d$/.test(event.key) && event.key !== 'Backspace') {
      event.preventDefault();
    }
  }

  searchCustomer() {
    const mobile = this.mobileNumber().trim();

    if (!/^\d{10}$/.test(mobile)) {
      this.dialog.open('Mobile number must be exactly 10 digits');
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
      error: () => this.dialog.open('Server error')
    });
  }

  createCustomer() {
    const name = this.customerName().trim();
    const mobile = this.mobileNumber().trim();

    if (!/^\d{10}$/.test(mobile)) {
      this.dialog.open('Mobile number must be 10 digits');
      return;
    }

    if (name.length < 3) {
      this.dialog.open('Customer name must be at least 3 characters');
      return;
    }

    const payload = { name, mobileNumber: mobile };

    this.service.createCustomer(payload).subscribe({
      next: (res) => {
        const location = res.headers.get('Location');
        const id = location?.split('/').pop()!;
        this.foundCustomer.set({ id, name, mobileNumber: mobile });
        this.customerSelected.emit(id);
      },
      error: () => this.dialog.open('Backend error while creating customer')
    });
  }

  clear() {
    this.searchPerformed.set(false);
    this.foundCustomer.set(null);
    this.customerName.set('');
    this.mobileNumber.set('');
  }
}
