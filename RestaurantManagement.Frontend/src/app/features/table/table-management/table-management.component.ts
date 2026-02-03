import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { TableService } from '../table.service';
import { MatIcon } from "@angular/material/icon";

@Component({
  selector: 'app-table-management',
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatButtonModule, MatInputModule, MatIcon],
  templateUrl: './table-management.component.html'
})
export class TableManagementComponent implements OnInit {

  service = inject(TableService);
  fb = inject(FormBuilder);

  tables: any[] = [];

  form = this.fb.group({
    tableName: ['', Validators.required],
    size: [1, [Validators.required, Validators.min(1)]]
  });

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAll().subscribe(res => {
      console.log("TABLES:", res);
      this.tables = res;
    });
  }

  add() {
    if (this.form.invalid) return;

    this.service.create(this.form.value).subscribe(() => {
      alert("Table Added");
      this.form.reset({ size: 1 });
      this.load();
    });
  }

  delete(id: string) {
    if (!confirm("Delete table?")) return;

    this.service.delete(id).subscribe({
      next: () => {
        alert("Deleted");
        this.load();
      },
      error: err => alert(err.error?.Message || "Cannot delete occupied table")
    });
  }
}
