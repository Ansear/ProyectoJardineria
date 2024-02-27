import { Routes } from '@angular/router';
import { SidebarComponent } from './shared/components/sidebar/sidebar.component';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./auth/auth.routes').then(m => m.Auth_Routes)
  },
  {
    path: 'sidebar',
    component: SidebarComponent
  }
];
