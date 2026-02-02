import { Routes } from '@angular/router';
import { LoginComponent } from './features/auth/login/login.component';
import { ChangePasswordComponent } from './features/auth/change-password/change-password.component';
import { WaiterHomeComponent } from './features/dashboard/waiter-home/waiter-home.component';
// import { TableSessionComponent } from './features/table/table-session/table-session.component';
import { AddOrderComponent } from './features/order/add-order/add-order.component';
import { ChefHomeComponent } from './features/dashboard/chef-home/chef-home.component';
import { TableSessionComponent } from './features/table/table-session/table-session.component';
import { GenerateBillComponent } from './features/billing/generate-bill/generate-bill/generate-bill.component';
import { WaiterLayoutComponent } from './layouts/waiter-layout/waiter-layout.component';
import { WaiterOccupiedTablesComponent } from './features/table/occupied-table/occupied-table.component';
import { WaiterOrdersStatusComponent } from './features/order/order-status/order-status.component';
import { CustomerBillingComponent } from './features/billing/customer-billing/customer-billing.component';

export const routes: Routes = [
    {path:"",component:LoginComponent},
    // {path:"about/:name",component:About},
    {path:"change-password",component:ChangePasswordComponent},
    // {path:'waiter/dashboard',component:WaiterHomeComponent},
    // {path:'table/session',component:TableSessionComponent},
    {path:'menu/list',component:AddOrderComponent},
    { path: 'table-session/:id/add-order', component: AddOrderComponent },
    { path: 'table-session/:id/generate-bill', component: GenerateBillComponent },
    {path:'generate-bill',component:GenerateBillComponent},
    { path: 'billing/customer/:tableId', component: CustomerBillingComponent },
    {path:'chef/dashboard',component:ChefHomeComponent},
    {path:'table-session/:id',component:TableSessionComponent},
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
    {path:"**",redirectTo:''}
];
