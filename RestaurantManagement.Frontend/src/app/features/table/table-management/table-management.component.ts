import { Component, OnInit, inject, signal, computed, ViewChild } from '@angular/core';
import { CommonModule, TitleCasePipe } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { TableService } from '../../../services/tableService/table.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { TableCreateRequest, TableResponse } from '../../../models/table.model';
import { DialogService } from '../../../services/dialogService/dialog.service';
import { FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-table-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatIconModule,
    TitleCasePipe,
    SearchBoxComponent
  ],
  templateUrl: './table-management.component.html',
  styleUrls: ['./table-management.component.css']
})
export class TableManagementComponent implements OnInit {

  private service = inject(TableService);
  private fb = inject(FormBuilder);
  private dialog = inject(DialogService);
  @ViewChild(FormGroupDirective) formDirective!: FormGroupDirective;

  tables = signal<TableResponse[]>([]);
  searchText = signal<string>('');
  page = signal<number>(1);
  pageSize = signal<number>(5);

  form = this.fb.nonNullable.group({
    tableName: ['', [
      Validators.required,
      Validators.maxLength(50),
      Validators.pattern(/^[A-Za-z0-9 ]+$/)
    ]],
    size: [1, [
      Validators.required,
      Validators.min(1),
      Validators.max(50)
    ]]
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

  totalPages = computed(() =>
    Math.ceil(this.filteredTables().length / this.pageSize()) || 1
  );

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAllTables().subscribe(res => this.tables.set(res));
  }

  onSearch(val: string) {
    this.searchText.set(val);
    this.page.set(1);
  }

  add() {
    if (this.form.invalid) {
      return;
    }

    const requestData: TableCreateRequest = this.form.getRawValue();

    this.service.create(requestData).subscribe(() => {
      this.dialog.open('Table Added !');
      this.form.reset({ tableName: '', size: 1 });
      this.formDirective.resetForm();
      this.load();
    });
  }

  delete(id: string) {
    if (!confirm("Delete table?")) return;
    this.service.delete(id).subscribe({
      next: () => {
        this.dialog.open('Table Deleted !');
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

  allowTableNameInput(event: KeyboardEvent) {
    if (!/^[a-zA-Z0-9 ]$/.test(event.key) && event.key !== 'Backspace') {
      event.preventDefault();
    }
  }

  allowOnlyNumbers(event: KeyboardEvent) {
    if (!/^\d$/.test(event.key) && event.key !== 'Backspace') {
      event.preventDefault();
    }
  }
}
