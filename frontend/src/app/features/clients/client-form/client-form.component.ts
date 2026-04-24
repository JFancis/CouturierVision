import { Component, EventEmitter, Output, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClientService } from '../../../core/services/client.service';

@Component({
  selector: 'app-client-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
      <h2 class="text-lg font-semibold mb-4">Nouveau client</h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Prénom *</label>
          <input [(ngModel)]="form.firstName" type="text"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
            placeholder="Jean">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Nom *</label>
          <input [(ngModel)]="form.lastName" type="text"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
            placeholder="Dupont">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Email *</label>
          <input [(ngModel)]="form.email" type="email"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
            placeholder="jean@example.com">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Téléphone *</label>
          <input [(ngModel)]="form.phoneNumber" type="tel"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
            placeholder="+33612345678">
        </div>
        <div class="md:col-span-2">
          <label class="block text-sm font-medium text-gray-700 mb-1">Préférences de style</label>
          <input [(ngModel)]="form.stylePreferences" type="text"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
            placeholder="Classique, couleurs neutres...">
        </div>
      </div>
      <div class="flex gap-3 mt-4">
        <button (click)="submit()"
          class="bg-purple-600 text-white px-4 py-2 rounded-lg hover:bg-purple-700 transition-colors text-sm font-medium">
          Enregistrer
        </button>
        <button (click)="cancelled.emit()"
          class="bg-gray-100 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-200 transition-colors text-sm font-medium">
          Annuler
        </button>
      </div>
      @if (errorMsg()) {
        <p class="mt-2 text-sm text-red-600">{{ errorMsg() }}</p>
      }
    </div>
  `
})
export class ClientFormComponent {
  @Output() created = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  private readonly clientService = inject(ClientService);

  protected readonly errorMsg = signal('');

  protected form = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    stylePreferences: '',
  };

  submit() {
    this.errorMsg.set('');
    this.clientService.createClient(this.form).subscribe({
      next: () => {
        this.created.emit();
        this.form = { firstName: '', lastName: '', email: '', phoneNumber: '', stylePreferences: '' };
      },
      error: (err) => this.errorMsg.set(err?.error?.error ?? 'Erreur lors de la création'),
    });
  }
}
