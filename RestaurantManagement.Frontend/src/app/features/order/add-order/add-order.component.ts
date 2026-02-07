import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, CurrencyPipe, TitleCasePipe } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../services/orderService/order.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { OrderCreateRequest, OrderMenuItem } from '../../../models/order.model';
import { DialogService } from '../../../services/dialogService/dialog.service';
import { Location } from '@angular/common';


@Component({
  selector: 'app-add-order',
  imports: [CommonModule, MatCardModule, MatButtonModule, MatIconModule, FormsModule, TitleCasePipe, CurrencyPipe, SearchBoxComponent],
  templateUrl: './add-order.component.html',
  styleUrl: './add-order.component.css'
})
export class AddOrderComponent implements OnInit {
  private orderService = inject(OrderService);
  private route = inject(ActivatedRoute);
  private router = inject(Router);
  private dialog = inject(DialogService)
  private location = inject(Location);
  tableId = signal<string>('');
  menuItems = signal<OrderMenuItem[]>([]);
  searchText = signal<string>('');

  filteredItems = computed(() => {
    const text = this.searchText().toLowerCase().trim();
    if (!text) return this.menuItems();
    return this.menuItems().filter(i => i.name.toLowerCase().includes(text));
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.tableId.set(id);
    this.loadMenu();
  }

  loadMenu() {
    this.orderService.getMenuItems().subscribe(data => {
      const mapped = data.map(m => ({
        ...m,
        quantity: 0
      }));
      this.menuItems.set(mapped);
    });
  }

  onSearch(val: string) {
    this.searchText.set(val);
  }

  increase(item: OrderMenuItem) {
    this.menuItems.update(items =>
      items.map(i => i.id === item.id ? { ...i, quantity: i.quantity + 1 } : i)
    );
  }

  decrease(item: OrderMenuItem) {
    if (item.quantity <= 0) return;
    this.menuItems.update(items =>
      items.map(i => i.id === item.id ? { ...i, quantity: i.quantity - 1 } : i)
    );
  }

  placeOrder() {
    const selectedItems = this.menuItems().filter(i => i.quantity > 0);
    if (selectedItems.length === 0) {
      this.dialog.open('Select atleast one item')
        .afterClosed()
        .subscribe(() => {
          return;
        });
      return;
    }

    const payload: OrderCreateRequest = {
      tableId: this.tableId(),
      items: selectedItems.map(i => ({ menuItemId: i.id, quantity: i.quantity }))
    };

    this.orderService.createOrder(payload).subscribe(() => {
      this.dialog.open('Order Placed Successfully')
        .afterClosed()
        .subscribe(() => {
          this.router.navigate(['/table-session', this.tableId()]).then(() => {

          });
        });
    });
  }

  goBack() {
    this.location.back();
  }
}