import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { SessionStorage } from '../services/session-storage';

export const tokenInterceptor: HttpInterceptorFn = (req, next) => {
  const token = inject(SessionStorage).getToken();

  if (token) {
    req = req.clone({ setHeaders: { Authorization: `Bearer ${token}` } });
  }

  return next(req);
};
