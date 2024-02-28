import { Routes } from '@angular/router';
import { SidebarComponent } from './shared/components/sidebar/sidebar.component';
import { ProfileComponent } from './home/components/profile/profile.component';

export const routes: Routes = [
  {
    path: '',
    loadChildren: () => import('./auth/auth.routes').then(m => m.Auth_Routes)
  },
  {
    path: 'sidebar',
    component: SidebarComponent
  },
  {
    path:"profile",
    component:ProfileComponent
  }
];
