import { Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BillService } from '../../../services/billingService/billing.service';
import { MenuService } from '../../../services/menuService/menu.service';
import { ReviewService } from '../../../services/reviewService/review.service';

@Component({
  selector: 'app-review',
  imports: [CommonModule, FormsModule],
  templateUrl: './review.component.html'
})
export class ReviewComponent implements OnInit {
  private route = inject(ActivatedRoute);
  private billService = inject(BillService);
  private menuService = inject(MenuService);
  private reviewService = inject(ReviewService);
  private router = inject(Router);

  billId = signal<string>('');
  items = signal<any[]>([]);
  restaurantRating = signal<number>(0);
  comment = signal<string>('');
  customerId = signal<string | undefined>('');

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('billId');
    if (id) {
      this.billId.set(id);
      this.loadBillData(id);
    }
  }

  loadBillData(id: string) {
    this.billService.getItemsByBillId(id).subscribe(res => {
      this.items.set(res.map(i => ({ ...i, rating: 0 })));
      if (res.length > 0) {
        this.customerId.set(res[0].customerId);
      }
    });
  }

  submit() {
    const menuRatings = this.items()
      .filter(i => i.rating > 0)
      .map(i => ({ id: i.id, rating: i.rating }));

    this.menuService.updateRatings(menuRatings).subscribe(() => {
      const restaurantReview = {
        customerId: this.customerId() || '',
        rating: this.restaurantRating(),
        comment: this.comment()
      };

      this.reviewService.createRestaurantReview(restaurantReview).subscribe(() => {
        alert('Review Added Successfully!');
        this.router.navigate(['/waiter/dashboard']);
      });
    });
  }
}