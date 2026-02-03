import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class MenuService {
  private api = 'https://localhost:7095/api/menu';

  constructor(private http: HttpClient) {}

  getAll() {
    return this.http.get<any[]>(this.api);
  }

  create(item: any) {
    return this.http.post(this.api, item);
  }

  update(id: string, item: any) {
    return this.http.patch(`${this.api}/${id}`, item);
  }

  delete(id: string) {
    return this.http.delete(`${this.api}/${id}`);
  }
}
