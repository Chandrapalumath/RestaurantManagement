import { Routes } from '@angular/router';
import { roleGuard } from '../guards/role-guard';
import { authGuard } from '../guards/auth-guard';

export const CHEF_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard],
        data: { role: 'Chef' },
        loadComponent: () => import('../layouts/chef-layout/chef-layout.component').then(m => m.ChefLayoutComponent),
        children: [
            { path: 'orders', loadComponent: () => import('../features/order/order-list/order-list.component').then(m => m.OrderListComponent) },
            { path: 'orders/:id', loadComponent: () => import('../features/order/order-details/order-details.component').then(m => m.OrderDetailsComponent) },
            { path: 'profile', loadComponent: () => import('../shared/components/profile/profile.component').then(m => m.ProfileComponent) },
            { path: '', redirectTo: 'orders', pathMatch: 'full' }
        ]
    }
];
