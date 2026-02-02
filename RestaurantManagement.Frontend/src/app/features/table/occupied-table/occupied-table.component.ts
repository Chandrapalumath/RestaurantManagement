import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TableService } from '../../table/table.service';

@Component({
  selector: 'app-waiter-occupied',
  imports: [CommonModule, MatCardModule],
  template: `
  <h2>My Occupied Tables</h2>

<div class="grid">
  @for (t of tables; track t.id ?? $index) {
    <mat-card>
      <h3>{{ t.tableName }}</h3>
      <p>Size: {{ t.size }}</p>
      <span class="badge bg-warning text-dark">Occupied</span>
    </mat-card>
  } @empty {
    <div class="no-tables">
      <p>No tables are currently occupied.</p>
    </div>
  }
</div>
  `,
  styles: [`.grid{display:grid;grid-template-columns:repeat(auto-fill,minmax(200px,1fr));gap:16px}`]
})
export class WaiterOccupiedTablesComponent implements OnInit {

  tables: any[] = [];
  service = inject(TableService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.service.getAllTables().subscribe(data => {
      this.tables = data.filter(t => t.isOccupied === true);
      this.cdr.detectChanges();
    });
  }
}
