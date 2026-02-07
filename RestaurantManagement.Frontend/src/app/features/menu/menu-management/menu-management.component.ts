import { Component, OnInit, inject, signal, computed, ViewChild } from '@angular/core';
import { CommonModule, TitleCasePipe, CurrencyPipe } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MenuService } from '../../../routes/menuService/menu.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { MenuItemResponse } from '../../../models/billing.model';
import { MenuItemCreateRequest } from '../../../models/menu.model';
import { DialogService } from '../../../services/dialogService/dialog.service';
import { FormGroupDirective } from '@angular/forms';

@Component({
  selector: 'app-menu-management',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatButtonModule,
    MatInputModule,
    MatCheckboxModule,
    TitleCasePipe,
    CurrencyPipe,
    SearchBoxComponent
  ],
  templateUrl: './menu-management.component.html',
  styleUrls: ['./menu-management.component.css']
})
export class MenuManagementComponent implements OnInit {

  private service = inject(MenuService);
  private fb = inject(FormBuilder);
  private dialog = inject(DialogService);
  @ViewChild(FormGroupDirective) formDirective!: FormGroupDirective;

  menuItems = signal<MenuItemResponse[]>([]);
  searchText = signal<string>('');
  page = signal<number>(1);
  pageSize = signal<number>(5);
  editId = signal<string | null>(null);

  filteredItems = computed(() => {
    const text = this.searchText().toLowerCase().trim();
    if (!text) return this.menuItems();
    return this.menuItems().filter(i => i.name.toLowerCase().includes(text));
  });

  pagedData = computed(() => {
    const start = (this.page() - 1) * this.pageSize();
    return this.filteredItems().slice(start, start + this.pageSize());
  });

  totalPages = computed(() =>
    Math.ceil(this.filteredItems().length / this.pageSize()) || 1
  );

  form = this.fb.nonNullable.group({
    name: ['', [
      Validators.required,
      Validators.minLength(2),
      Validators.maxLength(50),
      Validators.pattern(/^[A-Za-z0-9 ]+$/)
    ]],

    price: [0, [
      Validators.required,
      Validators.min(1),
      Validators.max(999999),
      Validators.pattern(/^\d+(\.\d{1,2})?$/)
    ]],

    isAvailable: [true]
  });


  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAll().subscribe(res => this.menuItems.set(res));
  }

  onSearch(val: string) {
    this.searchText.set(val);
    this.page.set(1);
  }

  next() {
    if (this.page() < this.totalPages()) this.page.update(p => p + 1);
  }

  prev() {
    if (this.page() > 1) this.page.update(p => p - 1);
  }

  submit() {
    if (this.form.invalid) {
      return;
    }

    const itemData = this.form.value as MenuItemCreateRequest;

    if (this.editId()) {
      this.service.update(this.editId()!, itemData).subscribe(() => {
        this.dialog.open('Item Updated!')
          .afterClosed()
          .subscribe(() => {
            this.reset()
            this.formDirective.resetForm();
          });
      });
    } else {
      this.service.create(itemData).subscribe(() => {
        this.dialog.open('Item Added!')
          .afterClosed()
          .subscribe(() => {
            this.reset()
            this.formDirective.resetForm();
          });
      });
    }
  }

  edit(item: MenuItemResponse) {
    this.editId.set(item.id);
    this.form.patchValue(item);
  }

  delete(id: string) {
    if (!confirm("Delete item?")) return;
    this.service.delete(id).subscribe(() => {
      this.dialog.open('Item Deleted!')
        .afterClosed()
        .subscribe(() => this.load());
    });
  }

  reset() {
    this.editId.set(null);
    this.form.reset({ isAvailable: true, price: 0 });
    this.load();
  }

  allowMenuNameInput(event: KeyboardEvent) {
    if (!/^[a-zA-Z0-9 ]$/.test(event.key) && event.key !== 'Backspace') {
      event.preventDefault();
    }
  }

  allowOnlyPriceInput(event: KeyboardEvent) {
    if (!/[\d.]/.test(event.key) && event.key !== 'Backspace') {
      event.preventDefault();
    }
  }
}
