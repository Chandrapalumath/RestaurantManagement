import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { TableService } from '../../../services/tableService/table.service';
import { SearchBoxComponent } from "../../../shared/components/search/search.component";

@Component({
  selector: 'app-waiter-home',
  imports: [
    CommonModule,
    MatCardModule,
    MatButtonModule,
    RouterModule,
    FormsModule,
    SearchBoxComponent
  ],
  templateUrl: './waiter-dashboard.component.html',
  styleUrl: './waiter-dashboard.component.css'
})
export class WaiterHomeComponent implements OnInit {

  private tableService = inject(TableService);

  tables = signal<any[]>([]);
  searchText = signal('');

  filteredTables = computed(() => {
    const text = this.searchText().trim().toLowerCase();
    const allTables = this.tables();

    return text
      ? allTables.filter(t => t.tableName.toLowerCase().includes(text))
      : allTables;
  });

  ngOnInit() {
    this.loadTables();
  }

  loadTables() {
    this.tableService.getAllTables().subscribe(data => {
      this.tables.set(
        data.map(t => ({
          id: t.id,
          tableName: t.tableName,
          size: t.size,
          isOccupied: t.isOccupied
        }))
      );
    });
  }

  onSearch(val: string) {
    this.searchText.set(val);
  }
}
