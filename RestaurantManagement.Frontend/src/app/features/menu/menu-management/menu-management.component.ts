import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MenuService } from '../menu.service';
import { MatCheckboxModule } from '@angular/material/checkbox';


@Component({
  selector: 'app-menu-management',
  imports: [CommonModule, ReactiveFormsModule, MatCardModule, MatButtonModule, MatInputModule, MatCheckboxModule ],
  templateUrl: './menu-management.component.html'
})

export class MenuManagementComponent implements OnInit {
  service = inject(MenuService);
  fb = inject(FormBuilder);

  menuItems: any[] = [];
  editId: string | null = null;

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
      console.log("MENU ITEMS:", res);
      this.menuItems = res;
    });
  }

  submit() {
    if (this.form.invalid) return;

    if (this.editId) {
      this.service.update(this.editId, this.form.value).subscribe(() => {
        alert("Item updated");
        this.reset();
      });
    } else {
      this.service.create(this.form.value).subscribe(() => {
        alert("Item added");
        this.reset();
      });
    }
  }

  edit(item: any) {
    this.editId = item.id;
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
    this.editId = null;
    this.form.reset({ isAvailable: true, price: 0 });
    this.load();
  }
}
