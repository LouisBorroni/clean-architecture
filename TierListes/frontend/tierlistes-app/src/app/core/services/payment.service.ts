import { Injectable, inject, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, catchError, of } from 'rxjs';

export interface CheckoutResponse {
  checkoutUrl: string;
}

export interface PaymentStatus {
  hasPaid: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class PaymentService {
  private http = inject(HttpClient);
  private apiUrl = 'http://localhost:5000/api/payment';

  private hasPaidSignal = signal<boolean>(false);
  private isLoadingSignal = signal<boolean>(false);
  private isCheckedSignal = signal<boolean>(false);

  hasPaid = computed(() => this.hasPaidSignal());
  isLoading = computed(() => this.isLoadingSignal());
  isChecked = computed(() => this.isCheckedSignal());

  checkout(): void {
    this.isLoadingSignal.set(true);
    this.http.post<CheckoutResponse>(`${this.apiUrl}/checkout`, {}).subscribe({
      next: (response) => {
        window.location.href = response.checkoutUrl;
      },
      error: (error) => {
        console.error('Erreur checkout:', error);
        this.isLoadingSignal.set(false);
      }
    });
  }

  checkPaymentStatus(): Observable<PaymentStatus> {
    return this.http.get<PaymentStatus>(`${this.apiUrl}/status`).pipe(
      tap((status) => {
        this.hasPaidSignal.set(status.hasPaid);
        this.isCheckedSignal.set(true);
      }),
      catchError(() => {
        this.hasPaidSignal.set(false);
        this.isCheckedSignal.set(true);
        return of({ hasPaid: false });
      })
    );
  }

  setHasPaid(value: boolean): void {
    this.hasPaidSignal.set(value);
  }
}
