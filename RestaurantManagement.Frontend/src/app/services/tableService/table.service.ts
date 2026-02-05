import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TableCreateRequest, TableResponse } from '../../models/table.model';

@Injectable({ providedIn: 'root' })
export class TableService {
  private http = inject(HttpClient);
  private readonly tableUrl = 'https://localhost:7095/api/table';
  private readonly ordersForTable = 'https://localhost:7095/api/orders/table';

  getAllTables(): Observable<TableResponse[]> {
    return this.http.get<TableResponse[]>(this.tableUrl);
  }

  getOrdersByTable(tableId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.ordersForTable}/${tableId}`);
  }

  create(table: TableCreateRequest): Observable<any> {
    return this.http.post(this.tableUrl, table);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.tableUrl}/${id}`);
  }
}