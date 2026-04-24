import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface OrderDto {
  id: string;
  clientId: string;
  clientName?: string;
  status: OrderStatus;
  totalPrice: number;
  depositPaid: number;
  measurementsJson: string;
  deadline: string;
  assignedArtisanId?: string;
  artisanName?: string;
}

export type OrderStatus =
  | 'Draft'
  | 'Confirmed'
  | 'Cutting'
  | 'Assembly'
  | 'Fitting'
  | 'Finishing'
  | 'Ready'
  | 'Delivered';

@Injectable({ providedIn: 'root' })
export class OrderService {
  private readonly apiUrl = '/api/orders';

  constructor(private http: HttpClient) {}

  getOrders(): Observable<OrderDto[]> {
    return this.http.get<OrderDto[]>(this.apiUrl);
  }

  advanceOrder(id: string): Observable<OrderDto> {
    return this.http.put<OrderDto>(`${this.apiUrl}/${id}/advance`, {});
  }

  registerDeposit(id: string, amount: number): Observable<OrderDto> {
    return this.http.post<OrderDto>(`${this.apiUrl}/${id}/deposit`, { amount });
  }

  getDepositPercent(order: OrderDto): number {
    if (order.totalPrice === 0) return 0;
    return Math.round((order.depositPaid / order.totalPrice) * 100);
  }
}
