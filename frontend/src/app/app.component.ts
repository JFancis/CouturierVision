import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="min-h-screen bg-gray-100 dark:bg-gray-900">
      <nav class="bg-white dark:bg-gray-800 shadow-sm border-b border-gray-200 dark:border-gray-700">
        <div class="max-w-7xl mx-auto px-4 py-3 flex items-center gap-6">
          <span class="text-purple-700 dark:text-purple-400 font-bold text-lg tracking-tight">✂️ CouturierVision</span>
          <a routerLink="/orders" routerLinkActive="text-blue-600 dark:text-blue-400 font-semibold border-b-2 border-blue-600"
             class="text-sm text-gray-600 dark:text-gray-300 hover:text-blue-600 dark:hover:text-blue-400 pb-1 transition-colors">
            Production
          </a>
          <a routerLink="/clients" routerLinkActive="text-purple-600 dark:text-purple-400 font-semibold border-b-2 border-purple-600"
             class="text-sm text-gray-600 dark:text-gray-300 hover:text-purple-600 dark:hover:text-purple-400 pb-1 transition-colors">
            Clients
          </a>
        </div>
      </nav>
      <main class="max-w-7xl mx-auto px-4 py-6">
        <router-outlet />
      </main>
    </div>
  `,
})
export class AppComponent {}
