import { Routes } from '@angular/router';
import { WaiterLayoutComponent } from '../../layouts/waiter-layout/waiter-layout.component';
import { WaiterOccupiedTablesComponent } from '../../features/table/occupied-table/occupied-table.component';
import { WaiterOrdersStatusComponent } from '../../features/order/order-status/order-status.component';

export const WAITER_ROUTES: Routes = [
    {
        path: '',
        component: WaiterLayoutComponent,
        children: [
            { path: 'occupied', component: WaiterOccupiedTablesComponent },
            { path: 'orders', component: WaiterOrdersStatusComponent },
        ]
    }
];
