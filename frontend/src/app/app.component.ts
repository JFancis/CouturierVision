import { Component } from '@angular/core';
import { RouterOutlet, RouterLink, RouterLinkActive } from '@angular/router';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <div class="min-h-screen bg-gray-50">
      <nav class="bg-purple-700 text-white shadow-lg">
        <div class="max-w-7xl mx-auto px-4 sm:px-6 lg:px-8">
          <div class="flex items-center justify-between h-16">
            <div class="flex items-center gap-2">
              <span class="text-2xl">✂️</span>
              <span class="text-xl font-bold tracking-wide">CouturierVision</span>
            </div>
            <div class="flex gap-6">
              <a routerLink="/orders" routerLinkActive="underline font-semibold"
                 class="hover:text-purple-200 transition-colors">Production</a>
              <a routerLink="/clients" routerLinkActive="underline font-semibold"
                 class="hover:text-purple-200 transition-colors">Clients</a>
            </div>
          </div>
        </div>
      </nav>
      <main class="max-w-7xl mx-auto px-4 py-8">
        <router-outlet />
      </main>
    </div>
  `
})
export class AppComponent {}
