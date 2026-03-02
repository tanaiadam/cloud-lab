import { Injectable } from '@angular/core';

const TOKEN = 'SESSION_TOKEN';

@Injectable({ providedIn: 'root' })
export class SessionStorage {
  setToken(token: string): void {
    sessionStorage.setItem(TOKEN, token);
  }

  getToken(): string | null {
    return sessionStorage.getItem(TOKEN);
  }

  deleteToken(): void {
    sessionStorage.removeItem(TOKEN);
  }
}
