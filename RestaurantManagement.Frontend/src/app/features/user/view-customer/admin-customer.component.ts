import { Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { AdminCustomerService } from '../../../services/userService/user.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { CustomerResponse } from '../../../models/review.model';
import { Subject, debounceTime, distinctUntilChanged, filter, switchMap } from 'rxjs';

@Component({
  selector: 'app-admin-customer',
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, SearchBoxComponent],
  templateUrl: './admin-customer.component.html',
  styleUrl: './admin-customer.component.css'
})
export class AdminCustomerComponent implements OnInit {

  private service = inject(AdminCustomerService);

  customers = signal<CustomerResponse[]>([]);
  page = signal<number>(1);
  pageSize = signal<number>(5);
  totalCount = signal<number>(0);

  private searchSubject = new Subject<string>();

  ngOnInit() {
    this.load();

    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      filter(text => text.length === 0 || text.length >= 2),
      switchMap(text =>
        this.service.getCustomers(this.page(), this.pageSize(), text)
      )
    ).subscribe(res => {
      this.customers.set(res.items);
      this.totalCount.set(res.totalCount);
    });
  }

  load() {
    this.service.getCustomers(this.page(), this.pageSize(), '')
      .subscribe(res => {
        this.customers.set(res.items);
        this.totalCount.set(res.totalCount);
      });
  }

  onSearch(val: string) {
    this.page.set(1);
    this.searchSubject.next(val);
  }

  next() {
    if (this.page() * this.pageSize() < this.totalCount()) {
      this.page.update(p => p + 1);
      this.load();
    }
  }

  prev() {
    if (this.page() > 1) {
      this.page.update(p => p - 1);
      this.load();
    }
  }
}
