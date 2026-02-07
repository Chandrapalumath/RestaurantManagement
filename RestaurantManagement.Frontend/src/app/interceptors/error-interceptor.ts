import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorToastService } from '../services/errorDialogService/error-toast.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ErrorToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      // Ignore system errors
      if (error.status === 0 || error.status >= 500) {
        console.error('System error:', error);
        return throwError(() => error);
      }

      if ([400, 401, 404, 409].includes(error.status)) {
        const message = error.error?.Message || 'Operation failed';
        toast.showError(message);
      }

      return throwError(() => error);
    })
  );
};
