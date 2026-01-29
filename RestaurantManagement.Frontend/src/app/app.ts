import { Component } from '@angular/core';
import { CreateUser } from './create-user/create-user';
import { Login } from './login/login';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [CreateUser,Login],
  templateUrl: './app.html',
})
export class App {}
