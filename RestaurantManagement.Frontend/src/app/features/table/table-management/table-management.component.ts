import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, TitleCasePipe } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { TableService } from '../../../services/tableService/table.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { TableCreateRequest, TableResponse } from '../../../models/table.model';

@Component({
  selector: 'app-table-management',
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    TitleCasePipe,
    SearchBoxComponent
  ],
  templateUrl: './table-management.component.html'
})
export class TableManagementComponent implements OnInit {
  private service = inject(TableService);
  private fb = inject(FormBuilder);

  tables = signal<TableResponse[]>([]);
  searchText = signal<string>('');
  page = signal<number>(1);
  pageSize = signal<number>(5);

  form = this.fb.group({
    tableName: ['', Validators.required],
    size: [1, [Validators.required, Validators.min(1)]]
  });

  filteredTables = computed(() => {
    const text = this.searchText().toLowerCase().trim();
    if (!text) return this.tables();
    return this.tables().filter(t => t.tableName.toLowerCase().includes(text));
  });

  pagedData = computed(() => {
    const start = (this.page() - 1) * this.pageSize();
    return this.filteredTables().slice(start, start + this.pageSize());
  });

  totalPages = computed(() => Math.ceil(this.filteredTables().length / this.pageSize()) || 1);

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAllTables().subscribe(res => {
      this.tables.set(res);
    });
  }

  onSearch(val: string) {
    this.searchText.set(val);
    this.page.set(1);
  }

  add() {
    if (this.form.invalid) return;
    const requestData = this.form.value as TableCreateRequest;
    this.service.create(requestData).subscribe(() => {
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

  next() {
    if (this.page() < this.totalPages()) this.page.update(p => p + 1);
  }

  prev() {
    if (this.page() > 1) this.page.update(p => p - 1);
  }
}