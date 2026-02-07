import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TableCreateRequest, TableResponse } from '../../models/table.model';
import { OrderResponse } from '../../models/order.model';

@Injectable({ providedIn: 'root' })
export class TableService {
  private http = inject(HttpClient);
  private readonly tableUrl = 'https://localhost:7095/api/table';
  private readonly ordersForTable = 'https://localhost:7095/api/orders/table';

  getAllTables(): Observable<TableResponse[]> {
    return this.http.get<TableResponse[]>(this.tableUrl);
  }
  getTableById(id: string) {
    return this.http.get<any>(`${this.tableUrl}/${id}`);
  }
  getOrdersByTable(tableId: string): Observable<OrderResponse[]> {
    return this.http.get<OrderResponse[]>(`${this.ordersForTable}/${tableId}`);
  }

  create(table: TableCreateRequest) {
    return this.http.post(this.tableUrl, table);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.tableUrl}/${id}`);
  }
}