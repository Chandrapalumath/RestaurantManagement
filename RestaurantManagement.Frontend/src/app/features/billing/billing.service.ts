import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class BillService {

  private apiUrl = 'https://localhost:7095/api/billing';

  constructor(private http: HttpClient) {}

  generateBill(payload: any) {
    return this.http.post(this.apiUrl, payload, {
      observe: 'response'
    });
  }

  getBillById(id: string) {
    return this.http.get<any>(`${this.apiUrl}/${id}`);
  }
}
