import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { catchError, of, throwError, Observable } from 'rxjs';
import { CustomerResponse } from '../../../models/review.model';
import { CustomerCreateRequest } from '../../../models/customer.model';

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private http = inject(HttpClient);
  private readonly api = 'https://localhost:7095/api/customers';

  searchByMobile(mobile: string): Observable<CustomerResponse[] | null> {
    return this.http.get<CustomerResponse[]>(`${this.api}/mobile/${mobile}`).pipe(
      catchError(err => {
        if (err.status === 404) return of(null);
        return throwError(() => err);
      })
    );
  }

  createCustomer(payload: CustomerCreateRequest): Observable<any> {
    return this.http.post(this.api, payload, {
      observe: 'response'
    });
  }
}