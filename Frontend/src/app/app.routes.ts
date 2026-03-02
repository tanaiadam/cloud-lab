import { Routes } from '@angular/router';
import { Album } from './album/album';
import { authorizedGuard } from './auth/guards/authorized-guard';
import { notAuthorizedGuard } from './auth/guards/not-authorized-guard';
import { Login } from './auth/login/login';
import { Register } from './auth/register/register';

export const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    component: Album,
    canActivate: [authorizedGuard],
  },
  {
    path: 'login',
    component: Login,
    canActivate: [notAuthorizedGuard],
  },
  {
    path: 'register',
    component: Register,
    canActivate: [notAuthorizedGuard],
  },
  {
    path: '**',
    redirectTo: '/',
  },
];
