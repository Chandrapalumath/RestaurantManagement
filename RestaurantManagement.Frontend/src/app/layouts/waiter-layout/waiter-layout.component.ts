import { Component } from '@angular/core';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-waiter-layout',
  imports: [CommonModule, RouterModule, MatIconModule],
  templateUrl: './waiter-layout.component.html',
  styleUrl: './waiter-layout.component.css',
})
export class WaiterLayoutComponent { }
