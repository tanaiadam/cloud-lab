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

  onLeftClick(photo: Photo): void {
    this.photoStore.selectPhoto(photo);
    this.closeContextMenu();
  }

  onRightClick(event: MouseEvent, photo: Photo): void {
    event.preventDefault();
    this.contextMenu.set({
      visible: true,
      x: event.clientX,
      y: event.clientY,
      photo,
    });
  }

  closeContextMenu(): void {
    if (this.contextMenu().visible) {
      this.contextMenu.update((state) => ({ ...state, visible: false }));
    }
  }

  renamePhoto(): void {
    console.log('Rename:', this.contextMenu().photo?.name);
    this.closeContextMenu();
  }

  deletePhoto(): void {
    const photoId = this.contextMenu().photo?.id;
    if (photoId) {
      this.photoStore.deletePhoto(photoId);
    }
    this.closeContextMenu();
  }

  openPhoto(): void {
    const photo = this.contextMenu().photo;
    if (photo) this.photoStore.selectPhoto(photo);
    this.closeContextMenu();
  }
}
