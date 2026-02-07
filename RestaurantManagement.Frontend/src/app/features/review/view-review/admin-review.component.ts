import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { ReviewService } from '../../../services/reviewService/review.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { ReviewResponse } from '../../../models/review.model';
import { debounceTime, distinctUntilChanged, filter, Subject, switchMap } from 'rxjs';

@Component({
  selector: 'app-admin-review',
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, SearchBoxComponent],
  templateUrl: './admin-review.component.html'
})
export class AdminReviewComponent implements OnInit {
  private service = inject(ReviewService);

  reviews = signal<ReviewResponse[]>([]);
  page = signal(1);
  pageSize = signal(5);
  totalCount = signal(0);

  private searchSubject = new Subject<string>();

  ngOnInit() {
    this.load('');

    this.searchSubject.pipe(
      debounceTime(400),
      distinctUntilChanged(),
      filter(text => text.length === 0 || text.length >= 2),
      switchMap(text =>
        this.service.getReviews(this.page(), this.pageSize(), text)
      )
    ).subscribe(res => {
      this.reviews.set(res.items);
      this.totalCount.set(res.totalCount);
    });
  }

  load(search: string) {
    this.service.getReviews(this.page(), this.pageSize(), search)
      .subscribe(res => {
        this.reviews.set(res.items);
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
      this.load('');
    }
  }

  prev() {
    if (this.page() > 1) {
      this.page.update(p => p - 1);
      this.load('');
    }
  }
}
