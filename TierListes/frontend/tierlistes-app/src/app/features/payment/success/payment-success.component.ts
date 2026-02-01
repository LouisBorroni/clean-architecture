import { Component, OnInit, OnDestroy, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { PaymentService } from '../../../core/services/payment.service';

@Component({
  selector: 'app-payment-success',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="page-container">
      <header class="header">
        <h1>TierListes</h1>
      </header>
      <main class="main-content">
        <div class="card">
          <div class="icon-wrapper">
            <div class="icon">&#10003;</div>
          </div>
          <h2>Paiement réussi !</h2>
          <p class="message">Merci pour votre achat. Vous pouvez maintenant créer et organiser vos tier lists.</p>
          <div class="countdown-wrapper">
            <div class="countdown-circle">{{ countdown() }}</div>
            <p class="redirect-info">Redirection automatique...</p>
          </div>
          <button (click)="goToDashboard()">Accéder au dashboard</button>
        </div>
      </main>
    </div>
  `,
  styles: [`
    .page-container {
      min-height: 100vh;
      background: #f5f5f5;
    }

    .header {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      padding: 16px 32px;
      box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);

      h1 {
        margin: 0;
        color: white;
        font-size: 24px;
      }
    }

    .main-content {
      display: flex;
      align-items: center;
      justify-content: center;
      padding: 60px 20px;
      min-height: calc(100vh - 72px);
    }

    .card {
      background: white;
      border-radius: 16px;
      padding: 48px;
      text-align: center;
      max-width: 420px;
      width: 100%;
      box-shadow: 0 4px 20px rgba(0, 0, 0, 0.1);
    }

    .icon-wrapper {
      margin-bottom: 24px;
    }

    .icon {
      width: 80px;
      height: 80px;
      border-radius: 50%;
      display: inline-flex;
      align-items: center;
      justify-content: center;
      font-size: 40px;
      background: linear-gradient(135deg, #4ade80 0%, #22c55e 100%);
      color: white;
      box-shadow: 0 4px 12px rgba(74, 222, 128, 0.4);
    }

    h2 {
      color: #1a1a2e;
      margin: 0 0 12px;
      font-size: 28px;
      font-weight: 700;
    }

    .message {
      color: #64748b;
      margin: 0 0 32px;
      line-height: 1.6;
      font-size: 16px;
    }

    .countdown-wrapper {
      display: flex;
      align-items: center;
      justify-content: center;
      gap: 12px;
      margin-bottom: 32px;
      padding: 16px;
      background: #f8fafc;
      border-radius: 12px;
    }

    .countdown-circle {
      width: 40px;
      height: 40px;
      border-radius: 50%;
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
      display: flex;
      align-items: center;
      justify-content: center;
      font-size: 18px;
      font-weight: 700;
    }

    .redirect-info {
      margin: 0;
      font-size: 14px;
      color: #94a3b8;
    }

    button {
      background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
      color: white;
      border: none;
      padding: 14px 32px;
      border-radius: 8px;
      font-size: 16px;
      font-weight: 600;
      cursor: pointer;
      transition: transform 0.2s, box-shadow 0.2s;
      width: 100%;
    }

    button:hover {
      transform: translateY(-2px);
      box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
    }
  `]
})
export class PaymentSuccessComponent implements OnInit, OnDestroy {
  private router = inject(Router);
  private paymentService = inject(PaymentService);
  private intervalId: ReturnType<typeof setInterval> | null = null;

  countdown = signal(5);

  ngOnInit(): void {
    this.paymentService.setHasPaid(true);

    this.intervalId = setInterval(() => {
      const current = this.countdown();
      if (current <= 1) {
        this.goToDashboard();
      } else {
        this.countdown.set(current - 1);
      }
    }, 1000);
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  goToDashboard(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
    this.router.navigate(['/dashboard']);
  }
}
