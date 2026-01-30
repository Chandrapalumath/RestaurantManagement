import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-table-session',
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule],
  templateUrl: './table-session.component.html'
})
export class TableSessionComponent {

  tableId = '10';

  // constructor(private route: ActivatedRoute) {
  //   this.tableId = this.route.snapshot.paramMap.get('id')!;
  // }

  orders: any[] | null = [
    { id: 'O1', itemCount: 3, status: 'Pending' },
    { id: 'O2', itemCount: 2, status: 'Completed' }
  ];

  get canGenerateBill(): boolean {
    if (!this.orders || this.orders.length === 0) return false;
    return this.orders.every(o => o.status === 'Completed');
  }

}
