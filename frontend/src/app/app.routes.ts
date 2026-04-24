import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'orders', pathMatch: 'full' },
  {
    path: 'orders',
    loadComponent: () =>
      import('./features/orders/production-dashboard/production-dashboard.component').then(
        m => m.ProductionDashboardComponent
      ),
  },
  {
    path: 'clients',
    loadComponent: () =>
      import('./features/clients/client-list/client-list.component').then(
        m => m.ClientListComponent
      ),
  },
];
