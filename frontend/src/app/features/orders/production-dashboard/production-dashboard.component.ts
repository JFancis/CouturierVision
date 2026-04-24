import {
  Component,
  OnInit,
  signal,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { OrderDto, OrderService, OrderStatus } from '../../../core/services/order.service';
import { OrderFormComponent } from '../order-form/order-form.component';

interface KanbanColumn {
  status: OrderStatus;
  label: string;
  colorClass: string;
}

@Component({
  selector: 'app-production-dashboard',
  standalone: true,
  imports: [CommonModule, FormsModule, OrderFormComponent],
  template: `
    <div class="min-h-screen bg-gray-100 dark:bg-gray-900 p-4" role="main">
      <div class="flex items-center justify-between mb-6">
        <h1 class="text-2xl font-bold text-gray-800 dark:text-white">
          Tableau de bord de production
        </h1>
        <button
          (click)="showOrderForm.set(!showOrderForm())"
          class="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 transition-colors text-sm font-medium">
          + Nouvelle commande
        </button>
      </div>

      @if (showOrderForm()) {
        <div class="mb-6">
          <app-order-form (created)="onOrderCreated()" (cancelled)="showOrderForm.set(false)" />
        </div>
      }

      @if (loading()) {
        <div class="flex justify-center items-center h-64" role="status" aria-label="Chargement en cours">
          <div class="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
        </div>
      } @else {
        <div class="flex gap-4 overflow-x-auto pb-4" role="list" aria-label="Colonnes de production">
          @for (column of kanbanColumns; track column.status) {
            <div
              class="flex-shrink-0 w-72 bg-white dark:bg-gray-800 rounded-xl shadow-md"
              role="listitem"
              [attr.aria-label]="'Colonne ' + column.label"
            >
              <div class="p-4 border-b dark:border-gray-700">
                <div class="flex items-center justify-between">
                  <h2 class="font-semibold text-gray-700 dark:text-gray-200" [attr.id]="'col-' + column.status">
                    {{ column.label }}
                  </h2>
                  <span
                    class="text-xs font-medium px-2 py-1 rounded-full"
                    [class]="column.colorClass"
                    [attr.aria-label]="ordersForStatus(column.status).length + ' commandes'"
                  >
                    {{ ordersForStatus(column.status).length }}
                  </span>
                </div>
              </div>

              <div
                class="p-3 space-y-3 min-h-32"
                role="list"
                [attr.aria-labelledby]="'col-' + column.status"
              >
                @for (order of ordersForStatus(column.status); track order.id) {
                  <div
                    class="bg-gray-50 dark:bg-gray-700 rounded-lg p-3 focus-visible:ring-2 focus-visible:ring-blue-500"
                    role="listitem"
                    tabindex="0"
                    [attr.aria-label]="'Commande de ' + (order.clientName ?? order.clientId)"
                  >
                    <div class="flex justify-between items-start mb-2">
                      <span class="font-medium text-gray-800 dark:text-white text-sm">
                        {{ order.clientName ?? ('Client #' + order.clientId.substring(0, 8)) }}
                      </span>
                      <span class="text-xs text-gray-500 dark:text-gray-400 font-semibold">
                        {{ order.totalPrice | currency:'EUR':'symbol':'1.0-0' }}
                      </span>
                    </div>

                    <div class="text-xs text-gray-500 dark:text-gray-400 space-y-1">
                      <div class="flex items-center gap-1" [attr.aria-label]="'Échéance: ' + formatDate(order.deadline)">
                        <span>📅</span>
                        <span>{{ formatDate(order.deadline) }}</span>
                      </div>

                      @if (order.artisanName) {
                        <div class="flex items-center gap-1" [attr.aria-label]="'Artisan: ' + order.artisanName">
                          <span>👤</span>
                          <span>{{ order.artisanName }}</span>
                        </div>
                      }
                    </div>

                    <div class="mt-2">
                      <div class="flex justify-between text-xs mb-1">
                        <span class="text-gray-500 dark:text-gray-400">Acompte</span>
                        <span
                          class="font-semibold"
                          [class.text-green-600]="getDepositPercent(order) >= 30"
                          [class.text-red-500]="getDepositPercent(order) < 30"
                          [attr.aria-label]="getDepositPercent(order) + '% d\\'acompte payé'"
                        >
                          {{ getDepositPercent(order) }}%
                        </span>
                      </div>
                      <div
                        class="w-full bg-gray-200 dark:bg-gray-600 rounded-full h-1.5"
                        role="progressbar"
                        [attr.aria-valuenow]="getDepositPercent(order)"
                        aria-valuemin="0"
                        aria-valuemax="100"
                      >
                        <div
                          class="h-1.5 rounded-full transition-all"
                          [class.bg-green-500]="getDepositPercent(order) >= 30"
                          [class.bg-red-400]="getDepositPercent(order) < 30"
                          [style.width]="getDepositPercent(order) + '%'"
                        ></div>
                      </div>
                    </div>

                    @if (depositOrderId() === order.id) {
                      <div class="mt-2 space-y-1">
                        <div class="flex gap-2">
                          <input
                            type="number"
                            min="1"
                            step="0.01"
                            [(ngModel)]="depositAmount"
                            class="w-full border border-gray-300 rounded px-2 py-1 text-xs focus:outline-none focus:ring-1 focus:ring-blue-500"
                            placeholder="Montant (€)"
                            aria-label="Montant de l'acompte"
                          />
                          <button
                            class="text-xs font-medium px-2 py-1 bg-green-600 text-white rounded hover:bg-green-700 focus:outline-none focus:ring-2 focus:ring-green-500"
                            (click)="submitDeposit(order)"
                            aria-label="Confirmer l'acompte"
                          >✓</button>
                          <button
                            class="text-xs font-medium px-2 py-1 bg-gray-300 text-gray-700 rounded hover:bg-gray-400 focus:outline-none focus:ring-2 focus:ring-gray-400"
                            (click)="depositOrderId.set(null)"
                            aria-label="Annuler"
                          >✕</button>
                        </div>
                        @if (depositError()) {
                          <p class="text-xs text-red-600">{{ depositError() }}</p>
                        }
                      </div>
                    }

                    @if (rejectOrderId() === order.id) {
                      <div class="mt-2 space-y-1">
                        <div class="flex gap-2">
                          <input
                            type="text"
                            [(ngModel)]="rejectReason"
                            class="w-full border border-gray-300 rounded px-2 py-1 text-xs focus:outline-none focus:ring-1 focus:ring-red-500"
                            placeholder="Motif du rejet"
                            aria-label="Motif du rejet"
                          />
                          <button
                            class="text-xs font-medium px-2 py-1 bg-red-600 text-white rounded hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500"
                            (click)="submitReject(order)"
                            aria-label="Confirmer le rejet"
                          >✓</button>
                          <button
                            class="text-xs font-medium px-2 py-1 bg-gray-300 text-gray-700 rounded hover:bg-gray-400 focus:outline-none focus:ring-2 focus:ring-gray-400"
                            (click)="rejectOrderId.set(null)"
                            aria-label="Annuler"
                          >✕</button>
                        </div>
                        @if (rejectError()) {
                          <p class="text-xs text-red-600">{{ rejectError() }}</p>
                        }
                      </div>
                    }

                    <div class="mt-3 flex gap-2">
                      @if (column.status !== 'Delivered') {
                        <button
                          class="flex-1 min-h-[36px] text-xs font-medium py-1 px-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 dark:focus:ring-offset-gray-700 transition-colors"
                          (click)="advanceOrder(order)"
                          [attr.aria-label]="'Avancer la commande de ' + (order.clientName ?? order.clientId)"
                        >
                          Avancer →
                        </button>
                        <button
                          class="min-h-[36px] text-xs font-medium py-1 px-2 rounded-lg bg-green-100 text-green-700 hover:bg-green-200 focus:outline-none focus:ring-2 focus:ring-green-500 transition-colors"
                          (click)="toggleDeposit(order)"
                          [attr.aria-label]="'Enregistrer un acompte pour la commande de ' + (order.clientName ?? order.clientId)"
                          title="Enregistrer un acompte"
                        >
                          💰
                        </button>
                        <button
                          class="min-h-[36px] text-xs font-medium py-1 px-2 rounded-lg bg-red-100 text-red-700 hover:bg-red-200 focus:outline-none focus:ring-2 focus:ring-red-500 transition-colors"
                          (click)="toggleReject(order)"
                          [attr.aria-label]="'Rejeter la commande de ' + (order.clientName ?? order.clientId)"
                          title="Rejeter la commande"
                        >
                          ✕
                        </button>
                      }
                    </div>
                  </div>
                }
              </div>
            </div>
          }
        </div>
      }
    </div>
  `,
})
export class ProductionDashboardComponent implements OnInit {
  private orderService = inject(OrderService);

  orders = signal<OrderDto[]>([]);
  loading = signal(true);
  showOrderForm = signal(false);
  depositOrderId = signal<string | null>(null);
  rejectOrderId = signal<string | null>(null);
  depositError = signal('');
  rejectError = signal('');
  depositAmount = 0;
  rejectReason = '';

  readonly kanbanColumns: KanbanColumn[] = [
    { status: 'Draft', label: 'Brouillon', colorClass: 'bg-gray-100 text-gray-700 dark:bg-gray-700 dark:text-gray-200' },
    { status: 'Confirmed', label: 'Confirmé', colorClass: 'bg-blue-100 text-blue-700 dark:bg-blue-900 dark:text-blue-200' },
    { status: 'Cutting', label: 'Coupe', colorClass: 'bg-yellow-100 text-yellow-700 dark:bg-yellow-900 dark:text-yellow-200' },
    { status: 'Assembly', label: 'Montage', colorClass: 'bg-orange-100 text-orange-700 dark:bg-orange-900 dark:text-orange-200' },
    { status: 'Fitting', label: 'Essayage', colorClass: 'bg-purple-100 text-purple-700 dark:bg-purple-900 dark:text-purple-200' },
    { status: 'Finishing', label: 'Finitions', colorClass: 'bg-pink-100 text-pink-700 dark:bg-pink-900 dark:text-pink-200' },
    { status: 'Ready', label: 'Prêt', colorClass: 'bg-green-100 text-green-700 dark:bg-green-900 dark:text-green-200' },
    { status: 'Delivered', label: 'Livré', colorClass: 'bg-emerald-100 text-emerald-700 dark:bg-emerald-900 dark:text-emerald-200' },
  ];

  ordersForStatus = (status: OrderStatus): OrderDto[] =>
    this.orders().filter(o => o.status === status);

  getDepositPercent(order: OrderDto): number {
    return this.orderService.getDepositPercent(order);
  }

  formatDate(dateStr: string): string {
    return new Date(dateStr).toLocaleDateString('fr-FR', {
      day: '2-digit', month: '2-digit', year: 'numeric'
    });
  }

  ngOnInit(): void {
    this.loadOrders();
  }

  private loadOrders(): void {
    this.loading.set(true);
    this.orderService.getOrders().subscribe({
      next: (orders) => {
        this.orders.set(orders);
        this.loading.set(false);
      },
      error: () => {
        this.loading.set(false);
      },
    });
  }

  onOrderCreated(): void {
    this.showOrderForm.set(false);
    this.loadOrders();
  }

  advanceOrder(order: OrderDto): void {
    this.orderService.advanceOrder(order.id).subscribe({
      next: (updated) => {
        this.orders.update(orders =>
          orders.map(o => o.id === updated.id ? updated : o)
        );
      },
    });
  }

  toggleDeposit(order: OrderDto): void {
    this.depositAmount = 0;
    this.depositError.set('');
    this.depositOrderId.set(this.depositOrderId() === order.id ? null : order.id);
    this.rejectOrderId.set(null);
  }

  submitDeposit(order: OrderDto): void {
    if (this.depositAmount <= 0) {
      this.depositError.set('Le montant doit être supérieur à 0.');
      return;
    }
    this.depositError.set('');
    this.orderService.registerDeposit(order.id, this.depositAmount).subscribe({
      next: (updated) => {
        this.orders.update(orders =>
          orders.map(o => o.id === updated.id ? updated : o)
        );
        this.depositOrderId.set(null);
        this.depositAmount = 0;
      },
    });
  }

  toggleReject(order: OrderDto): void {
    this.rejectReason = '';
    this.rejectError.set('');
    this.rejectOrderId.set(this.rejectOrderId() === order.id ? null : order.id);
    this.depositOrderId.set(null);
  }

  submitReject(order: OrderDto): void {
    if (!this.rejectReason.trim()) {
      this.rejectError.set('Le motif du rejet est requis.');
      return;
    }
    this.rejectError.set('');
    this.orderService.rejectOrder(order.id, this.rejectReason).subscribe({
      next: (updated) => {
        this.orders.update(orders =>
          orders.map(o => o.id === updated.id ? updated : o)
        );
        this.rejectOrderId.set(null);
        this.rejectReason = '';
      },
    });
  }
}
