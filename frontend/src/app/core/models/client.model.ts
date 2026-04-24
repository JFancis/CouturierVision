export interface Client {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  stylePreferences: string;
  measurementsJson?: string;
  createdAt: string;
}

export interface CreateClientRequest {
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber: string;
  stylePreferences?: string;
}
