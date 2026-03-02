import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import { Photo } from '../models/photo';
import { PhotoFilters } from '../models/photo-filters';
import { environment } from '../../../environments/environment';

@Injectable({ providedIn: 'root' })
export class PhotoApi {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = environment.apiUrl;

  getPhotos(filters?: PhotoFilters): Promise<Photo[]> {
    let params = new HttpParams();

    if (filters?.name) params = params.set('name', filters.name);
    if (filters?.uploadDateBefore)
      params = params.set('uploadDateBefore', filters.uploadDateBefore);
    if (filters?.uploadDateAfter) params = params.set('uploadDateAfter', filters.uploadDateAfter);

    return firstValueFrom(this.http.get<Photo[]>(`${this.apiUrl}/photos`, { params }));
  }

  uploadPhoto(file: File): Promise<Photo> {
    const formData = new FormData();
    formData.append('file', file);
    return firstValueFrom(this.http.post<Photo>(`${this.apiUrl}/photos/upload`, formData));
  }

  renamePhoto(id: string, newName: string): Promise<Photo> {
    return firstValueFrom(
      this.http.put<Photo>(`${this.apiUrl}/photos/${id}/name`, { name: newName }),
    );
  }

  deletePhoto(id: string): Promise<void> {
    return firstValueFrom(this.http.delete<void>(`${this.apiUrl}/photos/${id}`));
  }
}
