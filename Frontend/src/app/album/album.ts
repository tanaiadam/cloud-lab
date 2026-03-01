import { Component, ChangeDetectionStrategy, inject, OnInit } from '@angular/core';
import { PhotoFilter } from './photo-filter/photo-filter';
import { PhotoList } from './photo-list/photo-list';
import { PhotoStore } from './services/photo-store';

@Component({
  selector: 'app-album',
  templateUrl: './album.html',
  styleUrl: './album.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [PhotoFilter, PhotoList],
})
export class Album implements OnInit {
  readonly photoStore = inject(PhotoStore);

  ngOnInit(): void {
    this.photoStore.loadPhotos();
  }
}
