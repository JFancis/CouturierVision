import {
  Component,
  OnInit,
  signal,
  inject,
} from '@angular/core';
import { CommonModule } from '@angular/common';
import { OrderDto, OrderService, OrderStatus } from '../../../core/services/order.service';

interface KanbanColumn {
  status: OrderStatus;
  label: string;
  colorClass: string;
}

@Component({
  selector: 'app-production-dashboard',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="min-h-screen bg-gray-100 dark:bg-gray-900 p-4" role="main">
      <h1 class="text-2xl font-bold text-gray-800 dark:text-white mb-6">
        Tableau de bord de production
      </h1>

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
                    class="bg-gray-50 dark:bg-gray-700 rounded-lg p-3 cursor-pointer hover:shadow-md transition-shadow focus-visible:ring-2 focus-visible:ring-blue-500"
                    role="listitem"
                    tabindex="0"
                    [attr.aria-label]="'Commande de ' + (order.clientName ?? order.clientId)"
                  >
                    <div class="flex justify-between items-start mb-2">
                      <span class="font-medium text-gray-800 dark:text-white text-sm">
                        {{ order.clientName ?? ('Client #' + order.clientId.substring(0, 8)) }}
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

                    <button
                      class="mt-3 w-full min-h-[44px] text-xs font-medium py-2 px-3 rounded-lg bg-blue-600 text-white hover:bg-blue-700 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:ring-offset-2 dark:focus:ring-offset-gray-700 transition-colors"
                      (click)="advanceOrder(order)"
                      [attr.aria-label]="'Avancer la commande de ' + (order.clientName ?? order.clientId)"
                    >
                      Avancer →
                    </button>
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

  readonly kanbanColumns: KanbanColumn[] = [
    { status: 'Cutting', label: 'Coupe', colorClass: 'bg-yellow-100 text-yellow-700 dark:bg-yellow-900 dark:text-yellow-200' },
    { status: 'Assembly', label: 'Montage', colorClass: 'bg-blue-100 text-blue-700 dark:bg-blue-900 dark:text-blue-200' },
    { status: 'Fitting', label: 'Essayage', colorClass: 'bg-purple-100 text-purple-700 dark:bg-purple-900 dark:text-purple-200' },
    { status: 'Finishing', label: 'Finitions', colorClass: 'bg-orange-100 text-orange-700 dark:bg-orange-900 dark:text-orange-200' },
    { status: 'Ready', label: 'Prêt', colorClass: 'bg-green-100 text-green-700 dark:bg-green-900 dark:text-green-200' },
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

  advanceOrder(order: OrderDto): void {
    this.orderService.advanceOrder(order.id).subscribe({
      next: (updated) => {
        this.orders.update(orders =>
          orders.map(o => o.id === updated.id ? updated : o)
        );
      },
    });
  }
}
