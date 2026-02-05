import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TableService } from '../../../services/tableService/table.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";

@Component({
  selector: 'app-waiter-home',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    RouterModule,
    FormsModule,
    SearchBoxComponent
  ],
  templateUrl: './waiter-home.component.html'
})
export class WaiterHomeComponent implements OnInit {

  tableService = inject(TableService);
  private cdr = inject(ChangeDetectorRef);

  tables: any[] = [];
  filteredTables: any[] = [];

  private _searchText = '';
  onSearch(val: string) {
    this._searchText = val;
    this.applyFilter();
  }

  ngOnInit() {
    this.loadTables();
  }

  loadTables() {
    this.tableService.getAllTables().subscribe(data => {

      this.tables = data.map(t => ({
        id: t.id ?? t.id,
        tableName: t.tableName ?? t.tableName,
        size: t.size ?? t.size,
        isOccupied: t.isOccupied ?? t.isOccupied
      }));

      this.filteredTables = this.tables;
      this.cdr.detectChanges();
    });
  }

  applyFilter() {
    const text = this._searchText.trim().toLowerCase();

    this.filteredTables = text
      ? this.tables.filter(t =>
        t.tableName.toLowerCase().includes(text)
      )
      : this.tables;
  }
}
