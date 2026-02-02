import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable({ providedIn: 'root' })
export class TableService {

  private apiUrl = 'https://localhost:7095/api/table';
  private apiUrlForOrder = 'https://localhost:7095/api/orders/table';

  constructor(private http: HttpClient) {}

  getAllTables() {
    return this.http.get<any[]>(this.apiUrl);
  }

  getOrdersByTable(tableId: string) {
    return this.http.get<any[]>(`${this.apiUrlForOrder}/${tableId}`);
  }
}
