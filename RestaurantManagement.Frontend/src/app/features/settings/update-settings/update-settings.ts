import { Component, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { SettingsService } from '../../../services/settingsService/settings.service';
import { SettingsResponse, SettingsUpdateRequest } from '../../../models/setting.model';

@Component({
  selector: 'app-update-settings',
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatFormFieldModule
  ],
  templateUrl: './update-settings.html',
  styleUrl: './update-settings.css'
})
export class UpdateSettingsComponent implements OnInit {
  private fb = inject(FormBuilder);
  private service = inject(SettingsService);

  currentSettings = signal<SettingsResponse | null>(null);

  form = this.fb.group({
    taxPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
    discountPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
  });

  ngOnInit() {
    this.loadSettings();
  }

  loadSettings() {
    this.service.getSettings().subscribe(res => {
      this.currentSettings.set(res);
      this.form.patchValue({
        taxPercent: res.taxPercent,
        discountPercent: res.discountPercent
      });
    });
  }

  update() {
    if (this.form.invalid) return;

    const payload = this.form.value as SettingsUpdateRequest;

    this.service.updateSettings(payload).subscribe({
      next: () => {
        alert("Settings updated successfully");
        this.loadSettings();
      },
      error: (err) => console.error("Update failed", err)
    });
  }
}