import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse } from '@angular/common/http';
import { BillGenerateRequest, BillResponse, MenuItemResponse } from '../../models/billing.model';
import { Observable } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class BillService {

  private apiUrl = 'https://localhost:7095/api/billing';

  constructor(private http: HttpClient) { }

  generateBill(payload: BillGenerateRequest): Observable<HttpResponse<any>> {
    return this.http.post<any>(this.apiUrl, payload, { observe: 'response' });
  }

  getBillById(id: string): Observable<BillResponse> {
    return this.http.get<BillResponse>(`${this.apiUrl}/${id}`);
  }

  getItemsByBillId(billId: string): Observable<MenuItemResponse[]> {
    return this.http.get<MenuItemResponse[]>(`https://localhost:7095/api/menu/bill/${billId}`);
  }
}