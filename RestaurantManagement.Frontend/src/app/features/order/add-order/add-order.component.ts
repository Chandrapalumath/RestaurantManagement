import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router } from '@angular/router';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-add-order',
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './add-order.component.html'
})

export class AddOrderComponent implements OnInit {
  tableId!: string;
  menuItems: any[] = [];

  orderService = inject(OrderService);
  route = inject(ActivatedRoute);
  router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.tableId = this.route.snapshot.paramMap.get('id')!;
    this.loadMenu();
    this.cdr.detectChanges();
  }

  loadMenu() {
    this.orderService.getMenuItems().subscribe(data => {
      console.log("MENU API:", data);
      this.menuItems = data.map(m => ({
        id: m.id ?? m.Id,
        name: m.name ?? m.Name,
        price: m.price ?? m.Price,
        quantity: 0
      }));
      this.cdr.detectChanges();
    });
    
  }

  increase(item: any) { item.quantity++; this.cdr.detectChanges();}
  decrease(item: any) { if (item.quantity > 0) item.quantity--; this.cdr.detectChanges();}

  placeOrder() {
    const items = this.menuItems
      .filter(i => i.quantity > 0)
      .map(i => ({
        menuItemId: i.id,
        quantity: i.quantity
      }));

    if (items.length === 0) {
      alert("Select at least one item");
      return;
    }

    const orderPayload = {
      tableId: this.tableId,
      items: items
    };

    console.log("ORDER PAYLOAD:", orderPayload);

    this.orderService.createOrder(orderPayload).subscribe({
      next: () => {
        alert("Order placed successfully! ");
        this.router.navigate(['/table-session', this.tableId]).then(() => {
          
        });
      },
      error: err => console.log("ORDER ERROR:", err)
    });
  }
}
