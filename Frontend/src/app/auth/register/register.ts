import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AuthStore } from '../services/auth-store';

@Component({
  selector: 'app-register',
  imports: [ReactiveFormsModule, RouterLink],
  templateUrl: './register.html',
  styleUrl: './register.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class Register {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly authStore = inject(AuthStore);

  readonly form = this.fb.group({
    name: ['', [Validators.required]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(6)]],
  });

  readonly error = signal<string | null>(null);
  readonly isSubmitted = signal(false);

  onSubmit(): void {
    this.isSubmitted.set(true);
    if (this.form.invalid) return;

    this.error.set(null);

    this.authStore.register(this.form.getRawValue()).subscribe({
      error: (err: any) => this.error.set(err.error?.message || 'Registration failed.'),
    });
  }
}
