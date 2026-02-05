import { Routes } from '@angular/router';
import { ChefLayoutComponent } from '../../layouts/chef-layout/chef-layout.component';
import { OrderListComponent } from '../../features/order/order-list/order-list.component';
import { OrderDetailsComponent } from '../../features/order/order-details/order-details.component';

export const CHEF_ROUTES: Routes = [
    {
        path: '',
        component: ChefLayoutComponent,
        children: [
            { path: 'orders', component: OrderListComponent },
            { path: 'orders/:id', component: OrderDetailsComponent }
        ]
    }
];
