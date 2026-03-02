import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthStore } from '../services/auth-store';

@Component({
  selector: 'app-login',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './login.html',
  styleUrl: './login.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Login {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly authStore = inject(AuthStore);

  readonly form = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required]],
  });

  readonly error = signal<string | null>(null);
  readonly isSubmitted = signal(false);

  onSubmit(): void {
    this.isSubmitted.set(true);
    if (this.form.invalid) return;

    this.error.set(null);

    this.authStore.login(this.form.getRawValue()).subscribe({
      error: () => this.error.set('Invalid credentials.'),
    });
  }
}
