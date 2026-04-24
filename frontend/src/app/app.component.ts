import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { ProductionDashboardComponent } from './features/orders/production-dashboard/production-dashboard.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, ProductionDashboardComponent],
  template: `
    <div class="dark">
      <app-production-dashboard></app-production-dashboard>
    </div>
  `,
})
export class AppComponent {}
