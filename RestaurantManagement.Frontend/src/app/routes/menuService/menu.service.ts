import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { MenuItemResponse } from '../../models/billing.model';
import { MenuItemCreateRequest, MenuItemUpdateRequest, UpdateMenuItemRating } from '../../models/menu.model';

@Injectable({ providedIn: 'root' })
export class MenuService {
  private menuUrl = 'https://localhost:7095/api/menu';
  private updateRatingUrl = 'https://localhost:7095/api/menu/rating';

  constructor(private http: HttpClient) { }

  getAll() {
    return this.http.get<MenuItemResponse[]>(this.menuUrl);
  }

  create(item: MenuItemCreateRequest) {
    return this.http.post(this.menuUrl, item);
  }

  update(id: string, item: MenuItemUpdateRequest) {
    return this.http.patch(`${this.menuUrl}/${id}`, item);
  }

  delete(id: string) {
    return this.http.delete(`${this.menuUrl}/${id}`);
  }

  updateRatings(payload: UpdateMenuItemRating[]) {
    return this.http.post(this.updateRatingUrl, payload);
  }
}
