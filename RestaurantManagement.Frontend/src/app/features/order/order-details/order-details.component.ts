import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { OrderService } from '../order.service';
import { MatOption, MatFormField, MatLabel } from "@angular/material/select";

@Component({
  selector: 'app-order-details',
  imports: [CommonModule, FormsModule, MatCardModule, MatButtonModule, MatOption, MatFormField, MatLabel],
  templateUrl: './order-details.component.html'
})
export class OrderDetailsComponent implements OnInit {

  route = inject(ActivatedRoute);
  router = inject(Router);
  service = inject(OrderService);
  private cdr = inject(ChangeDetectorRef);

  order: any;
  selectedStatus = '';

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) this.loadOrder(id);
      this.cdr.detectChanges();
    });
  }

  loadOrder(id: string) {
    this.order = null;
    this.service.getOrderById(id).subscribe(res => {
      console.log("ORDER DETAILS:", res);
      this.order = res;
      this.selectedStatus = res.status;
      this.cdr.detectChanges();
    });
  }

  updateStatus() {
    this.service.updateStatus(this.order.orderId, this.selectedStatus)
      .subscribe(() => {
        alert("Updated!");
        this.router.navigate(['/chef/orders']);
      });
  }

  goBack() {
    this.router.navigate(['/chef/orders']);
  }
}
