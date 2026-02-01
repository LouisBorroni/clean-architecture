import { Component, inject, signal, computed, effect, PLATFORM_ID } from '@angular/core';
import { Router, RouterOutlet, NavigationEnd } from '@angular/router';
import { CommonModule, isPlatformBrowser } from '@angular/common';
import { CookieConsentComponent } from './shared/components/cookie-consent/cookie-consent.component';
import { AuthService } from './core/services/auth.service';
import { filter, take } from 'rxjs';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, CommonModule, CookieConsentComponent],
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  authService = inject(AuthService);
  private router = inject(Router);
  private platformId = inject(PLATFORM_ID);

  private initialNavigationComplete = signal(false);

  isAppReady = computed(() => !this.authService.isLoading() && this.initialNavigationComplete());

  constructor() {
    effect(() => {
      if (this.isAppReady() && isPlatformBrowser(this.platformId)) {
        const loader = document.getElementById('initial-loader');
        if (loader) {
          loader.classList.add('hidden');
          setTimeout(() => loader.remove(), 200);
        }
      }
    });

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      take(1)
    ).subscribe(() => {
      this.initialNavigationComplete.set(true);
    });
  }
}