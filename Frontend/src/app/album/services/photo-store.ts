import { Injectable, signal, inject } from '@angular/core';
import { Photo } from '../models/photo';
import { PhotoFilters } from '../models/photo-filters';
import { PhotoApi } from './photo-api';

@Injectable({ providedIn: 'root' })
export class PhotoStore {
  private readonly photoApi = inject(PhotoApi);

  private readonly _photos = signal<Photo[]>([]);
  private readonly _filters = signal<PhotoFilters>({});
  private readonly _selectedPhoto = signal<Photo | null>(null);

  readonly photos = this._photos.asReadonly();
  readonly filters = this._filters.asReadonly();
  readonly selectedPhoto = this._selectedPhoto.asReadonly();

  async loadPhotos(): Promise<void> {
    try {
      const photos = await this.photoApi.getPhotos(this._filters());
      this._photos.set(photos);
    } catch (error) {
      console.error('Failed to load photos:', error);
    }
  }

  async applyFilters(newFilters: PhotoFilters): Promise<void> {
    this._filters.update((f) => ({ ...f, ...newFilters }));
    await this.loadPhotos();
  }

  async uploadPhoto(file: File): Promise<void> {
    try {
      const newPhoto = await this.photoApi.uploadPhoto(file);
      this._photos.update((photos) => [...photos, newPhoto]);
    } catch (error) {
      console.error('Failed to upload photo:', error);
    }
  }

  async deletePhoto(id: string): Promise<void> {
    try {
      await this.photoApi.deletePhoto(id);
      this._photos.update((photos) => photos.filter((p) => p.id !== id));

      if (this._selectedPhoto()?.id === id) {
        this._selectedPhoto.set(null);
      }
    } catch (error) {
      console.error('Failed to delete photo:', error);
    }
  }

  async renamePhoto(id: string, newName: string): Promise<void> {
    try {
      const updatedPhoto = await this.photoApi.renamePhoto(id, newName);

      this._photos.update((photos) => photos.map((p) => (p.id === id ? updatedPhoto : p)));

      if (this._selectedPhoto()?.id === id) {
        this._selectedPhoto.set(updatedPhoto);
      }
    } catch (error) {
      console.error('Failed to rename photo:', error);
    }
  }

  selectPhoto(photo: Photo): void {
    this._selectedPhoto.set(photo);
  }
}
