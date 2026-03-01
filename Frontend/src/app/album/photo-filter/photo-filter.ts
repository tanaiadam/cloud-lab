import { Component, ChangeDetectionStrategy, inject } from '@angular/core';
import { NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { PhotoStore } from '../services/photo-store';

@Component({
  selector: 'app-photo-filter',
  templateUrl: './photo-filter.html',
  styleUrl: './photo-filter.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReactiveFormsModule],
})
export class PhotoFilter {
  private readonly fb = inject(NonNullableFormBuilder);
  private readonly photoStore = inject(PhotoStore);

  readonly filterForm = this.fb.group({
    name: [''],
    uploadDateAfter: [''],
    uploadDateBefore: [''],
  });

  applyFilters(): void {
    const values = this.filterForm.getRawValue();
    this.photoStore.applyFilters({
      name: values.name || undefined,
      uploadDateAfter: values.uploadDateAfter || undefined,
      uploadDateBefore: values.uploadDateBefore || undefined,
    });
  }

  onFileSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (input.files?.length) {
      this.photoStore.uploadPhoto(input.files[0]);
      input.value = '';
    }
  }
}
