import { Routes } from '@angular/router';
import { AdminDashboardComponent } from '../../features/dashboard/admin-dashboard/admin-dashboard.component';
import { UserManagementComponent } from '../../features/user/user-management/user-management.component';
import { UpdateSettingsComponent } from '../../features/settings/update-settings/update-settings';
import { MenuManagementComponent } from '../../features/menu/menu-management/menu-management.component';
import { TableManagementComponent } from '../../features/table/table-management/table-management.component';
import { AdminCustomerComponent } from '../../features/user/view-customer/admin-customer.component';
import { AdminReviewComponent } from '../../features/review/view-review/admin-review.component';
import { ProfileComponent } from '../../shared/components/profile/profile.component';

export const ADMIN_ROUTES: Routes = [
    { path: 'dashboard', component: AdminDashboardComponent },
    { path: 'users', component: UserManagementComponent },
    { path: 'settings', component: UpdateSettingsComponent },
    { path: 'menu', component: MenuManagementComponent },
    { path: 'tables', component: TableManagementComponent },
    { path: 'customers', component: AdminCustomerComponent },
    { path: 'reviews', component: AdminReviewComponent },
    { path: 'profile', component: ProfileComponent },
    { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
];