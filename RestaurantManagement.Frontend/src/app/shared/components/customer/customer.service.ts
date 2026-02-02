import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class CustomerService {

  private apiUrl = 'https://localhost:7095/api/customers';

  constructor(private http: HttpClient) {}

  searchByMobile(mobile: string) {
    return this.http.get<any[]>(`${this.apiUrl}/mobile/${mobile}`);
  }

  createCustomer(data: any) {
    return this.http.post(this.apiUrl, data);
  }
}
