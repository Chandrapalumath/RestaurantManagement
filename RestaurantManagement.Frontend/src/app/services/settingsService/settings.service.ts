import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { SettingsResponse, SettingsUpdateRequest } from '../../models/setting.model';

@Injectable({ providedIn: 'root' })
export class SettingsService {
  private http = inject(HttpClient);
  private readonly api = 'https://localhost:7095/api/settings';

  getSettings() {
    return this.http.get<SettingsResponse>(this.api);
  }

  updateSettings(payload: SettingsUpdateRequest) {
    return this.http.patch(this.api, payload);
  }
}