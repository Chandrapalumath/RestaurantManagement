import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-profile',
  imports: [CommonModule, MatCardModule],
  templateUrl: './profile.component.html'
})
export class ProfileComponent implements OnInit {

  http = inject(HttpClient);
  user: any;

  ngOnInit() {
    const id = localStorage.getItem('userId'); 
    console.log(id);
    this.http.get(`https://localhost:7095/api/users/${id}`)
      .subscribe(res => this.user = res);
  }
}
