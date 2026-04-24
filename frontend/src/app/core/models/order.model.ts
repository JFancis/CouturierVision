export type OrderStatus =
  | 'Draft'
  | 'Confirmed'
  | 'Cutting'
  | 'Assembly'
  | 'Fitting'
  | 'Finishing'
  | 'Ready'
  | 'Delivered';

export interface Order {
  id: string;
  clientId: string;
  status: OrderStatus;
  totalPrice: number;
  depositPaid: number;
  depositPercentage: number;
  measurementsJson: string;
  deadline: string;
  assignedArtisanId?: string;
}

export const ORDER_STATUS_COLUMNS: OrderStatus[] = [
  'Draft',
  'Confirmed',
  'Cutting',
  'Assembly',
  'Fitting',
  'Finishing',
  'Ready',
  'Delivered',
];

export const STATUS_LABELS: Record<OrderStatus, string> = {
  Draft: 'Brouillon',
  Confirmed: 'Confirmé',
  Cutting: 'Coupe',
  Assembly: 'Assemblage',
  Fitting: 'Essayage',
  Finishing: 'Finitions',
  Ready: 'Prêt',
  Delivered: 'Livré',
};

export const STATUS_COLORS: Record<OrderStatus, string> = {
  Draft: 'bg-gray-100 border-gray-300',
  Confirmed: 'bg-blue-50 border-blue-300',
  Cutting: 'bg-yellow-50 border-yellow-300',
  Assembly: 'bg-orange-50 border-orange-300',
  Fitting: 'bg-pink-50 border-pink-300',
  Finishing: 'bg-purple-50 border-purple-300',
  Ready: 'bg-green-50 border-green-300',
  Delivered: 'bg-emerald-50 border-emerald-300',
};
