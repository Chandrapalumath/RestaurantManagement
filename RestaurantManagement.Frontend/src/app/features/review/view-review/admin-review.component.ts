import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { ReviewService } from '../../../services/reviewService/review.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { forkJoin } from 'rxjs';
import { ReviewResponse } from '../../../models/review.model';

@Component({
  selector: 'app-admin-review',
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, SearchBoxComponent],
  templateUrl: './admin-review.component.html'
})
export class AdminReviewComponent implements OnInit {
  private service = inject(ReviewService);

  reviews = signal<ReviewResponse[]>([]);
  searchText = signal<string>('');
  page = signal<number>(1);
  pageSize = signal<number>(5);

  filteredReviews = computed(() => {
    const text = this.searchText().toLowerCase().trim();
    if (!text) return this.reviews();
    return this.reviews().filter(r =>
      r.customerName?.toLowerCase().includes(text) ||
      r.comment?.toLowerCase().includes(text)
    );
  });

  pagedData = computed(() => {
    const start = (this.page() - 1) * this.pageSize();
    return this.filteredReviews().slice(start, start + this.pageSize());
  });

  totalPages = computed(() => Math.ceil(this.filteredReviews().length / this.pageSize()) || 1);

  ngOnInit() {
    this.loadData();
  }

  loadData() {
    forkJoin({
      customers: this.service.getCustomers(),
      reviews: this.service.getReviews()
    }).subscribe(({ customers, reviews }) => {
      const customerMap = new Map(customers.map(c => [c.id, c.name]));

      const mappedReviews = reviews.map(r => ({
        ...r,
        customerName: customerMap.get(r.customerId) || 'Unknown'
      }));

      this.reviews.set(mappedReviews);
    });
  }

  onSearch(val: string) {
    this.searchText.set(val);
    this.page.set(1);
  }

  next() { if (this.page() < this.totalPages()) this.page.update(p => p + 1); }
  prev() { if (this.page() > 1) this.page.update(p => p - 1); }
}