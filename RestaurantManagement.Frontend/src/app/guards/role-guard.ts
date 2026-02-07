import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/authService/auth.service';

export const roleGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  const expectedRole = route.data?.['role'];
  const userRole = auth.getRole();

  if (!userRole) {
    router.navigate(['/login']);
    return false;
  }

  if (expectedRole && userRole !== expectedRole) {
    router.navigate(['/' + userRole.toLowerCase()]);
    return false;
  }

  return true;
};
