import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Loan } from './loan.model';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    FormsModule,
  ],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent implements OnInit {
  displayedColumns: string[] = [
    'applicantName',
    'amount',
    'currentBalance',
    'status',
    'actions',
  ];

  loans: Loan[] = [];
  newLoan = { applicantName: '', amount: 0 };
  apiUrl = 'http://localhost:8080/loan';
  message: string | null = null;

  constructor(private http: HttpClient) {}

  ngOnInit(): void {
    this.loadLoans();
  }

  loadLoans(): void {
    this.http
      .get<{ success: boolean; message: string | null; data: Loan[] }>(
        this.apiUrl
      )
      .subscribe({
        next: (res) => {
          this.message =
            res.message ||
            (res.success
              ? 'Loans retrieved successfully.'
              : 'An error occurred.');
          if (res.success && Array.isArray(res.data)) {
            this.loans = res.data;
          } else {
            this.loans = [];
          }
        },
        error: (err) => {
          console.error('Error loading loans:', err);
          this.message =
            err.error?.message ||
            err.error?.Message ||
            'Error loading loans from the server.';
        },
      });
  }

  createLoan(): void {
    if (!this.newLoan.applicantName || !this.newLoan.amount) return;

    this.http
      .post<{ success: boolean; message: string | null }>(
        this.apiUrl,
        this.newLoan
      )
      .subscribe({
        next: (res) => {
          this.message =
            res.message ||
            (res.success
              ? 'Loan created successfully.'
              : 'Failed to create loan.');
          if (res.success) {
            this.newLoan = { applicantName: '', amount: 0 };
            this.loadLoans();
          }
        },
        error: (err) => {
          console.error('Error creating loan:', err);
          this.message =
            err.error?.message || err.error?.Message || 'Error creating loan.';
        },
      });
  }

  applyPayment(id: string): void {
    const paymentAmount = Number(prompt('Enter payment amount:'));

    if (isNaN(paymentAmount) || paymentAmount <= 0) {
      alert('Please enter a valid payment amount greater than 0.');
      return;
    }

    this.http
      .post<{ success: boolean; message: string | null }>(
        `${this.apiUrl}/${id}/payment`,
        { paymentAmount }
      )
      .subscribe({
        next: (res) => {
          this.message =
            res.message ||
            (res.success
              ? 'Payment applied successfully.'
              : 'Payment could not be processed.');
          if (res.success) {
            this.loadLoans();
          }
        },
        error: (err) => {
          console.error('Error applying payment:', err);
          this.message =
            err.error?.message ||
            err.error?.Message ||
            'Error applying payment.';
        },
      });
  }
}
