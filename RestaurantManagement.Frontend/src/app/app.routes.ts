import { Routes } from '@angular/router';

export const routes: Routes = [
    { path: "", redirectTo: 'login', pathMatch: 'full' },
    {
        path: "login",
        loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent)
    },
    {
        path: "change-password",
        loadComponent: () => import('./features/auth/change-password/change-password.component').then(m => m.ChangePasswordComponent)
    },
    {
        path: 'waiter',
        loadChildren: () => import('./routes/waiter.routes').then(m => m.WAITER_ROUTES)
    },
    {
        path: 'chef',
        loadChildren: () => import('./routes/chef.routes').then(m => m.CHEF_ROUTES)
    },
    {
        path: 'admin',
        loadChildren: () => import('./routes/admin.routes').then(m => m.ADMIN_ROUTES)
    },
    { path: 'menu/list', loadComponent: () => import('./features/order/add-order/add-order.component').then(m => m.AddOrderComponent) },
    { path: 'review/:billId', loadComponent: () => import('./features/review/add-review/review.component').then(m => m.ReviewComponent) },
    { path: 'table-session/:id', loadComponent: () => import('./features/table/table-session/table-session.component').then(m => m.TableSessionComponent) },
    { path: 'table-session/:id/add-order', loadComponent: () => import('./features/order/add-order/add-order.component').then(m => m.AddOrderComponent) },
    { path: 'billing/customer/:tableId', loadComponent: () => import('./features/billing/customer-billing/customer-billing.component').then(m => m.CustomerBillingComponent) },

    { path: "**", redirectTo: 'login' }
];
