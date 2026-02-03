import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ChangePasswordComponent } from './features/auth/change-password/change-password.component';
import { WaiterHomeComponent } from './features/dashboard/waiter-home/waiter-home.component';
import { AddOrderComponent } from './features/order/add-order/add-order.component';
import { TableSessionComponent } from './features/table/table-session/table-session.component';
import { GenerateBillComponent } from './features/billing/generate-bill/generate-bill/generate-bill.component';
import { WaiterLayoutComponent } from './layouts/waiter-layout/waiter-layout.component';
import { WaiterOccupiedTablesComponent } from './features/table/occupied-table/occupied-table.component';
import { WaiterOrdersStatusComponent } from './features/order/order-status/order-status.component';
import { CustomerBillingComponent } from './features/billing/customer-billing/customer-billing.component';
import { ReviewComponent } from './features/review/review/review.component';
import { ChefLayoutComponent } from './layouts/chef-layout/chef-layout.component';
import { OrderDetailsComponent } from './features/order/order-details/order-details.component';
import { OrderListComponent } from './features/order/order-list/order-list.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { UserManagementComponent } from './features/user/user-management/user-management.component';
import { UpdateSettingsComponent } from './features/settings/update-settings/update-settings';
import { MenuManagementComponent } from './features/menu/menu-management/menu-management.component';
import { TableManagementComponent } from './features/table/table-management/table-management.component';

export const routes: Routes = [
    {path:"",component:LoginComponent},
    {path:"change-password",component:ChangePasswordComponent},
    {path:'menu/list',component:AddOrderComponent},
    { path: 'table-session/:id/add-order', component: AddOrderComponent },
    { path: 'table-session/:id/generate-bill', component: GenerateBillComponent },
    {path:'generate-bill',component:GenerateBillComponent},
    { path: 'billing/customer/:tableId', component: CustomerBillingComponent },
    {path:'table-session/:id',component:TableSessionComponent},
    { path: 'review/:tableId', component: ReviewComponent },
    { path: 'table-session/:id/add-order', component: AddOrderComponent },
    {
        path: 'waiter',
        component: WaiterLayoutComponent,
        children: [
            { path: 'dashboard', component: WaiterHomeComponent }, 
            { path: 'occupied', component: WaiterOccupiedTablesComponent },
            { path: 'orders', component: WaiterOrdersStatusComponent },
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' }
        ]
    },
    {
        path: 'chef',
        component: ChefLayoutComponent,
        children: [
            { path: 'orders', component: OrderListComponent },
            { path: 'orders/:id', component: OrderDetailsComponent },
            { path: '', redirectTo: 'orders', pathMatch: 'full' }
        ]
    },
    {
  path: 'admin',
  component: AdminLayoutComponent,
  children: [
    { path: 'users', component: UserManagementComponent },
    { path: 'settings', component: UpdateSettingsComponent },
    { path: 'menu', component: MenuManagementComponent },
    { path: 'tables', component: TableManagementComponent },
    { path: '', redirectTo: 'users', pathMatch: 'full' }
  ]
},
    {path:"**",redirectTo:''}
];
