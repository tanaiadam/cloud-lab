import { Routes } from '@angular/router';
import { Album } from './album/album';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: Album,
  },
  {
    path: '**',
    redirectTo: '/',
  },
];
