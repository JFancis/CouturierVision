import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Order, OrderStatus } from '../models/order.model';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly apiUrl = '/api/orders';

  private _orders = signal<Order[]>([]);

  readonly orders = this._orders.asReadonly();

  readonly ordersByStatus = computed(() => {
    const map = new Map<OrderStatus, Order[]>();
    for (const order of this._orders()) {
      const list = map.get(order.status) ?? [];
      list.push(order);
      map.set(order.status, list);
    }
    return map;
  });

  constructor(private http: HttpClient) {}

  createOrder(command: {
    clientId: string;
    totalPrice: number;
    measurementsJson: string;
    deadline: string;
    assignedArtisanId?: string;
  }) {
    return this.http.post<Order>(this.apiUrl, command).pipe(
      tap(order => this._orders.update(orders => [...orders, order]))
    );
  }

  advanceOrder(id: string) {
    return this.http.put<void>(`${this.apiUrl}/${id}/advance`, {}).pipe(
      tap(() => {
        this._orders.update(orders =>
          orders.map(o => {
            if (o.id !== id) return o;
            const statusFlow: OrderStatus[] = [
              'Draft', 'Confirmed', 'Cutting', 'Assembly',
              'Fitting', 'Finishing', 'Ready', 'Delivered'
            ];
            const idx = statusFlow.indexOf(o.status);
            const nextStatus = idx < statusFlow.length - 1 ? statusFlow[idx + 1] : o.status;
            return { ...o, status: nextStatus };
          })
        );
      })
    );
  }

  registerDeposit(id: string, amount: number) {
    return this.http.post<void>(`${this.apiUrl}/${id}/deposit`, { amount }).pipe(
      tap(() => {
        this._orders.update(orders =>
          orders.map(o => {
            if (o.id !== id) return o;
            const newDeposit = o.depositPaid + amount;
            return {
              ...o,
              depositPaid: newDeposit,
              depositPercentage: o.totalPrice > 0
                ? Math.round((newDeposit / o.totalPrice) * 100 * 100) / 100
                : 0
            };
          })
        );
      })
    );
  }
}
