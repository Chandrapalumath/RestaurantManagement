import { CommonModule } from '@angular/common';
import { ChangeDetectorRef, Component } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatChipsModule } from '@angular/material/chips';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmDialog } from '../../../shared/components/confirm-dialog/confirm-dialog.component';


@Component({
  selector: 'app-chef-dashboard',
  imports: [CommonModule, MatCardModule, MatButtonModule, MatChipsModule],
  templateUrl: './chef-home.component.html'
})
export class ChefHomeComponent {
  constructor(private dialog: MatDialog, private cd: ChangeDetectorRef) {}
  OrderStatus : 'Pending' | 'In Progress' | 'Completed'='Pending';
  orders = [
    { id: 'O101', table: 'T1', status: 'Pending' },
    { id: 'O102', table: 'T2', status: 'In Progress' },
    { id: 'O103', table: 'T3', status: 'Pending' }
  ];

  get activeOrders() {
    return this.orders.filter(o => o.status !== 'Completed');
  }

  updateStatus(order: any) {

  const nextStatus =
    order.status === 'Pending' ? 'In Progress' :
    order.status === 'In Progress' ? 'Completed' : null;

  if (!nextStatus) return;

  const dialogRef = this.dialog.open(ConfirmDialog, {
    width: '350px',
    data: {
      message: `Change Order ${order.id} from ${order.status} → ${nextStatus}?`
    }
  });

  dialogRef.afterClosed().subscribe(result => {
    if (result) {
      order.status = nextStatus;
      this.cd.detectChanges();
      console.log(`Order ${order.id} updated to ${order.status}`);
    }
  });
}
}

