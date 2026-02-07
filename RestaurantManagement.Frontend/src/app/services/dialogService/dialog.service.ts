import { Injectable, inject } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { AppDialogComponent } from '../../shared/components/app-dialog/app-dialog.component';

@Injectable({ providedIn: 'root' })
export class DialogService {
  private dialog = inject(MatDialog);

  open(message: string) {
    return this.dialog.open(AppDialogComponent, {
      width: '350px',
      data: { message }
    });
  }
}