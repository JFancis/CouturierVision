import { Component, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ClientService } from '../../../core/services/client.service';
import { ClientFormComponent } from '../client-form/client-form.component';

@Component({
  selector: 'app-client-list',
  standalone: true,
  imports: [CommonModule, ClientFormComponent],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h1 class="text-2xl font-bold text-gray-800">Clients</h1>
        <button
          (click)="showForm.set(!showForm())"
          class="bg-purple-600 text-white px-4 py-2 rounded-lg hover:bg-purple-700 transition-colors text-sm font-medium">
          + Nouveau client
        </button>
      </div>

      @if (showForm()) {
        <app-client-form (created)="onClientCreated()" (cancelled)="showForm.set(false)" />
      }

      <div class="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
        @if (clientService.clients().length === 0) {
          <div class="p-8 text-center text-gray-500">
            <p class="text-lg">Aucun client enregistré</p>
            <p class="text-sm mt-1">Commencez par ajouter un client.</p>
          </div>
        } @else {
          <table class="w-full text-sm">
            <thead class="bg-gray-50 border-b border-gray-200">
              <tr>
                <th class="text-left px-4 py-3 font-semibold text-gray-600">Nom</th>
                <th class="text-left px-4 py-3 font-semibold text-gray-600">Email</th>
                <th class="text-left px-4 py-3 font-semibold text-gray-600">Téléphone</th>
                <th class="text-left px-4 py-3 font-semibold text-gray-600">Préférences</th>
                <th class="text-left px-4 py-3 font-semibold text-gray-600">Inscrit le</th>
              </tr>
            </thead>
            <tbody class="divide-y divide-gray-100">
              @for (client of clientService.clients(); track client.id) {
                <tr class="hover:bg-gray-50 transition-colors">
                  <td class="px-4 py-3 font-medium text-gray-800">
                    {{ client.firstName }} {{ client.lastName }}
                  </td>
                  <td class="px-4 py-3 text-gray-600">{{ client.email }}</td>
                  <td class="px-4 py-3 text-gray-600">{{ client.phoneNumber }}</td>
                  <td class="px-4 py-3 text-gray-500 truncate max-w-xs">{{ client.stylePreferences || '—' }}</td>
                  <td class="px-4 py-3 text-gray-500">{{ client.createdAt | date:'dd/MM/yyyy' }}</td>
                </tr>
              }
            </tbody>
          </table>
        }
      </div>
    </div>
  `
})
export class ClientListComponent implements OnInit {
  protected readonly clientService = inject(ClientService);
  protected readonly showForm = signal(false);

  ngOnInit(): void {
    this.clientService.loadClients().subscribe();
  }

  onClientCreated() {
    this.showForm.set(false);
  }
}
