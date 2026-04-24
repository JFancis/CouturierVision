import { Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderService } from '../../../core/services/order.service';
import {
  ORDER_STATUS_COLUMNS,
  OrderStatus,
  STATUS_COLORS,
  STATUS_LABELS,
} from '../../../core/models/order.model';

@Component({
  selector: 'app-production-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <div class="space-y-6">
      <div class="flex items-center justify-between">
        <h1 class="text-2xl font-bold text-gray-800">Tableau de Production</h1>
        <button
          (click)="showNewOrderForm.set(!showNewOrderForm())"
          class="bg-purple-600 text-white px-4 py-2 rounded-lg hover:bg-purple-700 transition-colors text-sm font-medium">
          + Nouvelle commande
        </button>
      </div>

      @if (showNewOrderForm()) {
        <div class="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
          <h2 class="text-lg font-semibold mb-4">Créer une commande</h2>
          <div class="grid grid-cols-1 md:grid-cols-2 gap-4">
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">ID Client</label>
              <input [(ngModel)]="newOrder.clientId" type="text"
                class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                placeholder="UUID du client">
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Prix total (€)</label>
              <input [(ngModel)]="newOrder.totalPrice" type="number"
                class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                placeholder="0.00">
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Date limite</label>
              <input [(ngModel)]="newOrder.deadline" type="date"
                class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500">
            </div>
            <div>
              <label class="block text-sm font-medium text-gray-700 mb-1">Mesures (JSON)</label>
              <input [(ngModel)]="newOrder.measurementsJson" type="text"
                class="w-full border border-gray-300 rounded-lg px-3 py-2 text-sm focus:outline-none focus:ring-2 focus:ring-purple-500"
                placeholder='{"chest":95,"waist":80}'>
            </div>
          </div>
          <div class="flex gap-3 mt-4">
            <button (click)="createOrder()"
              class="bg-purple-600 text-white px-4 py-2 rounded-lg hover:bg-purple-700 transition-colors text-sm font-medium">
              Créer
            </button>
            <button (click)="showNewOrderForm.set(false)"
              class="bg-gray-100 text-gray-700 px-4 py-2 rounded-lg hover:bg-gray-200 transition-colors text-sm font-medium">
              Annuler
            </button>
          </div>
          @if (errorMsg()) {
            <p class="mt-2 text-sm text-red-600">{{ errorMsg() }}</p>
          }
        </div>
      }

      <!-- Kanban Board -->
      <div class="overflow-x-auto pb-4">
        <div class="flex gap-4 min-w-max">
          @for (status of statusColumns; track status) {
            <div class="w-56 flex-shrink-0">
              <div class="flex items-center justify-between mb-2 px-1">
                <h3 class="text-sm font-semibold text-gray-600 uppercase tracking-wide">
                  {{ statusLabels[status] }}
                </h3>
                <span class="bg-gray-200 text-gray-600 text-xs font-medium px-2 py-0.5 rounded-full">
                  {{ (orderService.ordersByStatus().get(status) ?? []).length }}
                </span>
              </div>
              <div class="space-y-2 min-h-16">
                @for (order of (orderService.ordersByStatus().get(status) ?? []); track order.id) {
                  <div [class]="'rounded-lg border-2 p-3 shadow-sm cursor-default ' + statusColors[status]">
                    <div class="text-xs text-gray-500 mb-1 truncate">{{ order.id.substring(0, 8) }}…</div>
                    <div class="font-semibold text-sm text-gray-800">{{ order.totalPrice | currency:'EUR' }}</div>
                    <div class="text-xs text-gray-500 mt-1">
                      Acompte: {{ order.depositPercentage }}%
                    </div>
                    <div class="text-xs text-gray-400">
                      Échéance: {{ order.deadline | date:'dd/MM/yy' }}
                    </div>
                    @if (order.status !== 'Delivered') {
                      <button
                        (click)="advance(order.id)"
                        class="mt-2 w-full text-xs bg-white border border-gray-300 rounded px-2 py-1 hover:bg-gray-50 transition-colors font-medium text-gray-700">
                        Avancer →
                      </button>
                    }
                  </div>
                }
              </div>
            </div>
          }
        </div>
      </div>
    </div>
  `
})
export class ProductionDashboardComponent {
  protected readonly orderService = inject(OrderService);

  protected readonly statusColumns = ORDER_STATUS_COLUMNS;
  protected readonly statusLabels = STATUS_LABELS;
  protected readonly statusColors = STATUS_COLORS;

  protected readonly showNewOrderForm = signal(false);
  protected readonly errorMsg = signal('');

  protected newOrder = {
    clientId: '',
    totalPrice: 0,
    measurementsJson: '{}',
    deadline: '',
  };

  createOrder() {
    this.errorMsg.set('');
    const deadline = new Date(this.newOrder.deadline).toISOString();
    this.orderService.createOrder({ ...this.newOrder, deadline }).subscribe({
      next: () => {
        this.showNewOrderForm.set(false);
        this.newOrder = { clientId: '', totalPrice: 0, measurementsJson: '{}', deadline: '' };
      },
      error: (err) => this.errorMsg.set(err?.error?.error ?? 'Erreur lors de la création'),
    });
  }

  advance(orderId: string) {
    this.orderService.advanceOrder(orderId).subscribe({
      error: (err) => alert(err?.error?.error ?? 'Erreur lors de l\'avancement'),
    });
  }
}
