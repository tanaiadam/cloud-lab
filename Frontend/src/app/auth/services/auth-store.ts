import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { tap } from 'rxjs';
import { LoginRequest } from '../models/login-request';
import { RegisterRequest } from '../models/register-request';
import { User } from '../models/user';
import { AuthApi } from './auth-api';
import { SessionStorage } from './session-storage';

@Injectable({ providedIn: 'root' })
export class AuthStore {
  private readonly session = inject(SessionStorage);
  private readonly authApi = inject(AuthApi);
  private readonly router = inject(Router);

  private readonly _user = signal<User | null>(null);

  readonly user = this._user.asReadonly();
  readonly isLoggedIn = computed(() => this._user() !== null);

  constructor() {
    const token = this.session.getToken();
    if (token) this.setUserFromToken(token);
  }

  login(credentials: LoginRequest) {
    return this.authApi.login(credentials).pipe(
      tap((res) => {
        this.session.setToken(res.token);
        this.setUserFromToken(res.token);
        this.router.navigate(['/']);
      }),
    );
  }

  register(data: RegisterRequest) {
    return this.authApi.register(data).pipe(tap(() => this.router.navigate(['/login'])));
  }

  logout(): void {
    this.session.deleteToken();
    this._user.set(null);
    this.router.navigate(['/login']);
  }

  private setUserFromToken(token: string): void {
    try {
      const payload = JSON.parse(atob(token.split('.')[1]));
      this._user.set({
        id: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
        name: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'],
        email: payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
      });
    } catch {
      this.logout();
    }
  }
}
