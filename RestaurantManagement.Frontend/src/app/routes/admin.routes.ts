import { Routes } from '@angular/router';
import { roleGuard } from '../guards/role-guard';
import { authGuard } from '../guards/auth-guard';

export const ADMIN_ROUTES: Routes = [
    {
        path: '',
        canActivate: [authGuard, roleGuard],
        data: { role: 'Admin' },
        loadComponent: () => import('../layouts/admin-layout/admin-layout.component').then(m => m.AdminLayoutComponent),
        children: [
            { path: 'dashboard', loadComponent: () => import('../features/dashboard/admin-dashboard/admin-dashboard.component').then(m => m.AdminDashboardComponent) },
            { path: 'users', loadComponent: () => import('../features/user/user-management/user-management.component').then(m => m.UserManagementComponent) },
            { path: 'settings', loadComponent: () => import('../features/settings/update-settings/update-settings').then(m => m.UpdateSettingsComponent) },
            { path: 'menu', loadComponent: () => import('../features/menu/menu-management/menu-management.component').then(m => m.MenuManagementComponent) },
            { path: 'tables', loadComponent: () => import('../features/table/table-management/table-management.component').then(m => m.TableManagementComponent) },
            { path: 'customers', loadComponent: () => import('../features/user/view-customer/admin-customer.component').then(m => m.AdminCustomerComponent) },
            { path: 'reviews', loadComponent: () => import('../features/review/view-review/admin-review.component').then(m => m.AdminReviewComponent) },
            { path: 'profile', loadComponent: () => import('../shared/components/profile/profile.component').then(m => m.ProfileComponent) },
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
        ]
    }
];
