import { Component, ChangeDetectionStrategy, inject, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { PhotoStore } from '../services/photo-store';
import { Photo } from '../models/photo';

@Component({
  selector: 'app-photo-list',
  templateUrl: './photo-list.html',
  styleUrl: './photo-list.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [DatePipe],
  host: {
    '(document:click)': 'closeContextMenu()',
  },
})
export class PhotoList {
  readonly photoStore = inject(PhotoStore);
  readonly contextMenu = signal({ visible: false, x: 0, y: 0, photo: null as Photo | null });
  readonly editingId = signal<string | null>(null); // Tracks the photo being renamed

  onLeftClick(photo: Photo): void {
    if (this.editingId() === photo.id) return; // Don't select/close if currently typing
    this.photoStore.selectPhoto(photo);
    this.closeContextMenu();
  }

  onRightClick(event: MouseEvent, photo: Photo): void {
    event.preventDefault();
    this.contextMenu.set({ visible: true, x: event.clientX, y: event.clientY, photo });
  }

  closeContextMenu(): void {
    if (this.contextMenu().visible) {
      this.contextMenu.update((state) => ({ ...state, visible: false }));
    }
  }

  renamePhoto(): void {
    const photo = this.contextMenu().photo;
    if (photo) this.editingId.set(photo.id);
    this.closeContextMenu();
  }

  async saveRename(photo: Photo, event: Event): Promise<void> {
    const input = event.target as HTMLInputElement;
    const newName = input.value.trim();

    if (newName && newName !== photo.name) {
      await this.photoStore.renamePhoto(photo.id, newName);
    }

    this.editingId.set(null);
  }

  cancelRename(): void {
    this.editingId.set(null);
  }

  deletePhoto(): void {
    const photoId = this.contextMenu().photo?.id;
    if (photoId) this.photoStore.deletePhoto(photoId);
    this.closeContextMenu();
  }
}
