import { Component, OnInit, inject, signal, computed } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { TableService } from '../../../services/tableService/table.service';
import { TableResponse } from '../../../models/table.model';

@Component({
  selector: 'app-waiter-occupied',
  imports: [CommonModule, MatCardModule],
  templateUrl: './occupied-table.component.html',
  styleUrl: './occupied-table.component.css'
})
export class WaiterOccupiedTablesComponent implements OnInit {
  private service = inject(TableService);

  tables = signal<TableResponse[]>([]);
  occupiedTables = computed(() => this.tables().filter(t => t.isOccupied === true));

  ngOnInit() {
    this.service.getAllTables().subscribe(data => {
      this.tables.set(data);
    });
  }
}