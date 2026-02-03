import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { CommonModule } from '@angular/common';
import { MatButtonModule } from '@angular/material/button';

@Component({
  selector: 'app-bill-dialog',
  imports: [CommonModule, MatDialogModule, MatButtonModule],
  templateUrl: './bill-dialog.component.html'
})
export class BillDialogComponent {
  constructor(@Inject(MAT_DIALOG_DATA) public bill: any) {}
}
