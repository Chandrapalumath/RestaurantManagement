import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { TableService } from '../../table/table.service';

@Component({
  selector: 'app-review',
  imports: [CommonModule, MatCardModule, MatButtonModule],
  templateUrl: './review.component.html'
})
export class ReviewComponent implements OnInit {

  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private http = inject(HttpClient);
  private tableService = inject(TableService);

  tableId!: string;
  items: any[] = [];
  restaurantRating = 0;

  ngOnInit() {
    this.tableId = this.route.snapshot.params['tableId'];
    this.loadItems();
  }

  loadItems() {
    this.tableService.getOrdersByTable(this.tableId).subscribe(res => {

      const allItems = res.flatMap((o: any) => o.items);

      const unique = new Map();

      allItems.forEach((i: any) => {
        if (!unique.has(i.menuItemId)) {
          unique.set(i.menuItemId, {
            id: i.menuItemId,
            name: i.menuItemName,
            rating: 0
          });
        }
      });

      this.items = Array.from(unique.values());
    });
  }

  submitRating() {

    const payload = this.items.map(i => ({
      id: i.id,
      rating: i.rating
    }));

    this.http.post('https://localhost:7095/api/menu/rating', payload)
      .subscribe(() => {
        alert("Thank you for the feedback!");
        this.router.navigate(['/waiter/dashboard']);
      });
  }
}
