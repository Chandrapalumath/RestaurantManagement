import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CustomerResponse } from '../../models/review.model';

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
}

@Injectable({ providedIn: 'root' })
export class AdminCustomerService {

  private api = 'https://localhost:7095/api/customers';

  constructor(private http: HttpClient) { }
  getCustomers(page: number, pageSize: number, search: string) {
    return this.http.get<PagedResult<CustomerResponse>>(
      `${this.api}?page=${page}&pageSize=${pageSize}&search=${search}`
    );
  }
}
