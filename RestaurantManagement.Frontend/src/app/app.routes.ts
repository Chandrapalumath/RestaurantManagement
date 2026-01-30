import { Routes } from '@angular/router';
import { LoginComponent } from './pages/auth/login/login.component';
import { ChangePasswordComponent } from './pages/auth/change-password/change-password.component';
import { WaiterHomeComponent } from './pages/waiter/waiter-home/waiter-home.component';
import { TableSessionComponent } from './pages/waiter/table-session/table-session.component';
import { AddOrderComponent } from './pages/waiter/add-order/add-order.component';
import { GenerateBillComponent } from './pages/waiter/generate-bill/generate-bill.component';
import { ChefHomeComponent } from './pages/chef/chef-home/chef-home.component';

export const routes: Routes = [
    {path:"",component:LoginComponent},
    // {path:"about/:name",component:About},
    {path:"change-password",component:ChangePasswordComponent},
    {path:'waiter/dashboard',component:WaiterHomeComponent},
    {path:'table/session',component:TableSessionComponent},
    {path:'menu/list',component:AddOrderComponent},
    {path:'generate-bill',component:GenerateBillComponent},
    {path:'chef/dashboard',component:ChefHomeComponent},
    {path:"**",redirectTo:''}
];
