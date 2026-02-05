import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomerResponse } from '../../models/review.model';

@Injectable({ providedIn: 'root' })
export class AdminCustomerService {

  private api = 'https://localhost:7095/api/customers';

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<CustomerResponse[]>(this.api);
  }
}
