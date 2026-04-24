import { Component, EventEmitter, Output, inject, signal, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../core/services/order.service';
import { ClientService } from '../../../core/services/client.service';

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
      <h2 class="text-lg font-semibold mb-4">Nouvelle commande</h2>
      <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
        <div class="md:col-span-2">
          <label class="block text-sm font-medium text-gray-700 mb-1">Client *</label>
          <select [(ngModel)]="form.clientId"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500">
            <option value="">-- Sélectionner un client --</option>
            @for (client of clients(); track client.id) {
              <option [value]="client.id">{{ client.firstName }} {{ client.lastName }}</option>
            }
          </select>
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Prix total (€) *</label>
          <input [(ngModel)]="form.totalPrice" type="number" min="0" step="0.01"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500"
            placeholder="0.00">
        </div>
        <div>
          <label class="block text-sm font-medium text-gray-700 mb-1">Date limite *</label>
          <input [(ngModel)]="form.deadline" type="date"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500">
        </div>
        <div class="md:col-span-2">
          <label class="block text-sm font-medium text-gray-700 mb-1">Mesures (JSON) *</label>
          <textarea [(ngModel)]="form.measurementsJson" rows="3"
            class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 font-mono"
            placeholder='{"chest": 95, "waist": 75, "hips": 100}'></textarea>
        </div>
      </div>
      <div class="flex gap-3 mt-4">
        <button (click)="submit()"
          class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition-colors text-sm font-medium">
          Créer la commande
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
export class OrderFormComponent implements OnInit {
  @Output() created = new EventEmitter<void>();
  @Output() cancelled = new EventEmitter<void>();

  private readonly orderService = inject(OrderService);
  private readonly clientService = inject(ClientService);

  protected readonly clients = this.clientService.clients;
  protected readonly errorMsg = signal('');

  protected form = {
    clientId: '',
    totalPrice: 0,
    measurementsJson: '{}',
    deadline: '',
  };

  ngOnInit(): void {
    if (this.clientService.clients().length === 0) {
      this.clientService.loadClients().subscribe();
    }
  }

  submit() {
    this.errorMsg.set('');
    if (!this.form.clientId) {
      this.errorMsg.set('Veuillez sélectionner un client.');
      return;
    }
    if (!this.form.deadline) {
      this.errorMsg.set('Veuillez renseigner une date limite.');
      return;
    }
    const deadlineUtc = new Date(this.form.deadline).toISOString();
    this.orderService.createOrder({
      clientId: this.form.clientId,
      totalPrice: this.form.totalPrice,
      measurementsJson: this.form.measurementsJson,
      deadline: deadlineUtc,
    }).subscribe({
      next: () => {
        this.created.emit();
        this.form = { clientId: '', totalPrice: 0, measurementsJson: '{}', deadline: '' };
      },
      error: (err) => this.errorMsg.set(err?.error?.error ?? 'Erreur lors de la création'),
    });
  }
}
