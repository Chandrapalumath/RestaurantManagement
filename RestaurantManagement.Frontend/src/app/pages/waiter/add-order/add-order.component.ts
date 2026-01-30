import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-add-order',
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './add-order.component.html'
})
export class AddOrderComponent {

  tableId='10'

  menuItems = [
    { id: 'M1', name: 'Pizza', price: 250, quantity: 0 },
    { id: 'M2', name: 'Burger', price: 180, quantity: 0 },
    { id: 'M3', name: 'Pasta', price: 220, quantity: 0 },
    { id: 'M4', name: 'Cold Drink', price: 60, quantity: 0 }
  ];

  increase(item: any) {
    item.quantity++;
  }

  decrease(item: any) {
    if (item.quantity > 0) item.quantity--;
  }

  placeOrder() {
    const selectedItems = this.menuItems
      .filter(item => item.quantity > 0)
      .map(item => ({
        menuItemId: item.id,
        name: item.name,
        quantity: item.quantity,
        unitPrice: item.price
      }));

    console.log('Table ID:', this.tableId);
    console.log('Selected Items:', selectedItems);
  }
}
