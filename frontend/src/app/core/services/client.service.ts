import { Injectable, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs/operators';
import { Client, CreateClientRequest } from '../models/client.model';

@Injectable({ providedIn: 'root' })
export class ClientService {
  private readonly apiUrl = '/api/clients';

  private _clients = signal<Client[]>([]);
  readonly clients = this._clients.asReadonly();

  constructor(private http: HttpClient) {}

  loadClients() {
    return this.http.get<Client[]>(this.apiUrl).pipe(
      tap(clients => this._clients.set(clients))
    );
  }

  getById(id: string) {
    return this.http.get<Client>(`${this.apiUrl}/${id}`);
  }

  createClient(request: CreateClientRequest) {
    return this.http.post<Client>(this.apiUrl, request).pipe(
      tap(client => this._clients.update(clients => [...clients, client]))
    );
  }
}
