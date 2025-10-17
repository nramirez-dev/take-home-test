export interface Loan {
  id: number;
  amount: number;
  currentBalance: number;
  applicantName: string;
  status: 'active' | 'paid';
}

export interface PaymentDto {
  amount: number;
}
