import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule, TitleCasePipe, CurrencyPipe } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MenuService } from '../../../services/menuService/menu.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";
import { MenuItemResponse } from '../../../models/billing.model';
import { MenuItemCreateRequest } from '../../../models/menu.model';

@Component({
  selector: 'app-menu-management',
  imports: [
    CommonModule, FormsModule, ReactiveFormsModule, MatCardModule,
    MatButtonModule, MatInputModule, MatCheckboxModule,
    TitleCasePipe, CurrencyPipe, SearchBoxComponent
  ],
  templateUrl: './menu-management.component.html'
})
export class MenuManagementComponent implements OnInit {
  private service = inject(MenuService);
  private fb = inject(FormBuilder);

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

  totalPages = computed(() => Math.ceil(this.filteredItems().length / this.pageSize()) || 1);

  form = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(2)]],
    price: [0, [Validators.required, Validators.min(1)]],
    isAvailable: [true]
  });

  ngOnInit() {
    this.load();
  }

  load() {
    this.service.getAll().subscribe(res => {
      this.menuItems.set(res);
    });
  }

  onSearch(val: string) {
    this.searchText.set(val);
    this.page.set(1);
  }

  next() {
    if (this.page() < this.totalPages()) {
      this.page.update(p => p + 1);
    }
  }

  prev() {
    if (this.page() > 1) {
      this.page.update(p => p - 1);
    }
  }

  submit() {
    if (this.form.invalid) return;
    const itemData = this.form.value as MenuItemCreateRequest;

    if (this.editId()) {
      this.service.update(this.editId()!, itemData).subscribe(() => {
        alert("Item updated");
        this.reset();
      });
    } else {
      this.service.create(itemData).subscribe(() => {
        alert("Item added");
        this.reset();
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
      alert("Item deleted");
      this.load();
    });
  }

  reset() {
    this.editId.set(null);
    this.form.reset({ isAvailable: true, price: 0 });
    this.load();
  }
}