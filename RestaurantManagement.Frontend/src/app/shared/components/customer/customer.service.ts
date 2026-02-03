import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, of, throwError } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class CustomerService {

  private api = 'https://localhost:7095/api/customers';

  constructor(private http: HttpClient) {}

  searchByMobile(mobile: string) {
    return this.http.get<any[]>(`${this.api}/mobile/${mobile}`).pipe(
      catchError(err => {
        if (err.status === 404) return of(null);
        return throwError(() => err);
      })
    );
  }
  createCustomer(payload: any) {
    return this.http.post(this.api, payload, {
      observe: 'response'
    });
  }
}
