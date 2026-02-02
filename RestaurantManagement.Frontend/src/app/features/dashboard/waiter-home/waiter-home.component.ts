import { ChangeDetectorRef, Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { RouterModule } from '@angular/router';
import { TableService } from '../../table/table.service';
import { TrackByFunction } from '@angular/core';

@Component({
  selector: 'app-waiter-home',
  standalone: true,
  imports: [CommonModule, MatCardModule, MatButtonModule, RouterModule],
  templateUrl: './waiter-home.component.html'
})

export class WaiterHomeComponent implements OnInit {
  tables: any[] = [];
  tableService = inject(TableService);
  private cdr = inject(ChangeDetectorRef);

  ngOnInit() {
    this.loadTables();
  }

  loadTables() {
    this.tableService.getAllTables().subscribe(data => {
      console.log("RAW TABLE DATA:", data);

      this.tables = data.map(t => ({
        id: t.id ?? t.Id,
        tableName: t.tableName ?? t.TableName,
        size: t.size ?? t.Size,
        isOccupied: t.isOccupied ?? t.IsOccupied
      }));

      console.log("MAPPED TABLES:", this.tables);
      this.cdr.detectChanges();
    });
  }

  // get visibleTables() {
  //   return this.tables;
  // }
}
