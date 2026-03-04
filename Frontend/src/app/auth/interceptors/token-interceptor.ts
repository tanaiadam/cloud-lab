import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from '@angular/router';
import { catchError, throwError } from 'rxjs';
import { SessionStorage } from '../services/session-storage';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const sessionStorage = inject(SessionStorage);
  const router = inject(Router);

  const token = sessionStorage.getToken();

  if (token) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }

  return next(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) {
        sessionStorage.deleteToken();

        router.navigate(['/login']);
      }

      return throwError(() => error);
    }),
  );
};
