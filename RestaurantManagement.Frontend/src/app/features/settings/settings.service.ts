import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class SettingsService {
  private api = 'https://localhost:7095/api/settings';

  constructor(private http: HttpClient) {}

  getSettings() {
    return this.http.get<any>(this.api);
  }

  updateSettings(payload: any) {
    return this.http.patch(this.api, payload);
  }
}
