import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { SettingsService } from '../settings.service';

@Component({
  selector: 'app-update-settings',
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatButtonModule, MatInputModule],
  templateUrl: './update-settings.html'
})
export class UpdateSettingsComponent implements OnInit {

  fb = inject(FormBuilder);
  service = inject(SettingsService);

  currentSettings: any;

  form = this.fb.group({
    taxPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]],
    discountPercent: [0, [Validators.required, Validators.min(0), Validators.max(100)]]
  });

  ngOnInit() {
    this.loadSettings();
  }

  loadSettings() {
    this.service.getSettings().subscribe(res => {
      console.log("CURRENT SETTINGS:", res);

      this.currentSettings = res;

      this.form.patchValue({
        taxPercent: res.taxPercent,
        discountPercent: res.discountPercent
      });
    });
  }

  update() {
    if (this.form.invalid) return;

    this.service.updateSettings(this.form.value).subscribe(() => {
      alert("Settings updated successfully");
      this.loadSettings();
    });
  }
}
