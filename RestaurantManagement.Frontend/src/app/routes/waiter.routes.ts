import { Routes } from '@angular/router';
import { roleGuard } from '../guards/role-guard';
import { authGuard } from '../guards/auth-guard';

export const WAITER_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard],
        data: { role: 'Waiter' },
        loadComponent: () => import('../layouts/waiter-layout/waiter-layout.component').then(m => m.WaiterLayoutComponent),
        children: [
            { path: 'dashboard', loadComponent: () => import('../features/dashboard/waiter-dashboard/waiter-dashboard.component').then(m => m.WaiterHomeComponent) },
            { path: 'occupied', loadComponent: () => import('../features/table/occupied-table/occupied-table.component').then(m => m.WaiterOccupiedTablesComponent) },
            { path: 'orders', loadComponent: () => import('../features/order/order-status/order-status.component').then(m => m.WaiterOrdersStatusComponent) },
            { path: 'profile', loadComponent: () => import('../shared/components/profile/profile.component').then(m => m.ProfileComponent) },
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
        ]
    }
];
