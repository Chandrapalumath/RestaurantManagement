import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { AdminCustomerService } from '../../../services/userService/user.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";

@Component({
  selector: 'app-admin-customer',
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, SearchBoxComponent],
  templateUrl: './admin-customer.component.html'
})
export class AdminCustomerComponent implements OnInit {
  private service = inject(AdminCustomerService);

  customers = signal<any[]>([]);
  searchText = signal<string>('');
  page = signal<number>(1);
  pageSize = signal<number>(5);

  filteredCustomers = computed(() => {
    const text = this.searchText().toLowerCase().trim();
    return text
      ? this.customers().filter(c => c.name.toLowerCase().includes(text))
      : this.customers();
  });

  pagedData = computed(() => {
    const start = (this.page() - 1) * this.pageSize();
    return this.filteredCustomers().slice(start, start + this.pageSize());
  });

  totalPages = computed(() => Math.ceil(this.filteredCustomers().length / this.pageSize()) || 1);

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAll().subscribe(res => this.customers.set(res));
  }

  onSearch(val: string) {
    this.searchText.set(val);
    this.page.set(1);
  }

  next() { if (this.page() < this.totalPages()) this.page.update(p => p + 1); }
  prev() { if (this.page() > 1) this.page.update(p => p - 1); }
}