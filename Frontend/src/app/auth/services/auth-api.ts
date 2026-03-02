import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { environment } from '../../../environments/environment';
import { LoginRequest } from '../models/login-request';
import { RegisterRequest } from '../models/register-request';

@Injectable({ providedIn: 'root' })
export class AuthApi {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  register(data: RegisterRequest) {
    return this.http.post(`${this.apiUrl}/auth/register`, data);
  }

  login(data: LoginRequest) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/auth/login`, data);
  }
}
