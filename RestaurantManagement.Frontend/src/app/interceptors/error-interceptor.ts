import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { ErrorToastService } from '../services/error-toast.service';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ErrorToastService);

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {

      let message = 'Something went wrong';

      if (error.error?.Message) {
        message = error.error.Message;
      } else if (error.status === 0) {
        message = 'Cannot connect to server';
      } else {
        message = `Error ${error.status}: ${error.statusText}`;
      }

      toast.showError(message);

      return throwError(() => error);
    })
  );
};
