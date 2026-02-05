import { Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef, MatDialogContent } from '@angular/material/dialog';
import { Router } from '@angular/router';
import { BillResponse } from '../../../models/billing.model';

@Component({
  selector: 'app-bill-dialog',
  templateUrl: './bill-dialog.component.html',
  styleUrl: './bill-dialog.component.css',
  imports: [MatDialogContent]
})
export class BillDialogComponent {

  constructor(
    private dialogRef: MatDialogRef<BillDialogComponent>,
    private router: Router,
    @Inject(MAT_DIALOG_DATA) public bill: BillResponse
  ) { }

  close() {
    this.dialogRef.close();
    this.router.navigate(['/review', this.bill.billId]);
  }
}