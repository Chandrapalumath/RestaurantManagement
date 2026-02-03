import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { OrderService } from '../order.service';

@Component({
  selector: 'app-order-list',
  imports: [CommonModule, MatCardModule],
  templateUrl: './order-list.component.html'
})
export class OrderListComponent implements OnInit {

  service = inject(OrderService);
  route = inject(ActivatedRoute);
  router = inject(Router);
  private cdr = inject(ChangeDetectorRef);

  orders: any[] = [];
  title = "All Orders";

  ngOnInit() {
    this.route.queryParamMap.subscribe(params => {
      const status = params.get('status') || 'all';
      console.log("STATUS FILTER:", status);

      if (status === 'all') {
        this.title = "All Orders";
        this.loadAll();
        this.cdr.detectChanges();
      } else {
        this.title = `${status} Orders`;
        this.loadByStatus(status);
        this.cdr.detectChanges();
      }
    });
  }

  loadAll() {
    this.service.getOrdersByStatus('Pending').subscribe(p => {
      this.service.getOrdersByStatus('Preparing').subscribe(pr => {
        this.orders = [...p, ...pr];
        console.log("ALL ORDERS:", this.orders);
        this.cdr.detectChanges();
      });
    });
  }

  loadByStatus(status: string) {
    this.service.getOrdersByStatus(status).subscribe(data => {
      this.orders = data;
      console.log("FILTERED ORDERS:", data);
      this.cdr.detectChanges();
    });
  }

  viewOrder(id: string) {
    console.log("OPEN ORDER:", id);
    this.router.navigate(['/chef/orders', id]);
  }
}
