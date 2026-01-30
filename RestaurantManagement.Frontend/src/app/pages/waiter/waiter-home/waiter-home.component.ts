import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-waiter-home',
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule],
  templateUrl: './waiter-home.component.html'
})
export class WaiterHomeComponent {

  currentWaiterId = 'W1';

  tables = [
    { id: 'T1', name: 'Table 1', capacity: 4, isOccupied: false, waiterId: null },
    { id: 'T2', name: 'Table 2', capacity: 6, isOccupied: true, waiterId: 'W1' }, 
    { id: 'T3', name: 'Table 3', capacity: 2, isOccupied: true, waiterId: 'W2' }, 
    { id: 'T4', name: 'Table 4', capacity: 8, isOccupied: false, waiterId: null }
  ];

  get visibleTables() {
    return this.tables.filter(t =>
      !t.isOccupied || t.waiterId === this.currentWaiterId
    );
  }
}
