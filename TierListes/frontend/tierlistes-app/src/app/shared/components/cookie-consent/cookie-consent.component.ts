import { Component, signal, PLATFORM_ID, inject, OnInit } from '@angular/core';
import { CommonModule, isPlatformBrowser } from '@angular/common';

@Component({
  selector: 'app-cookie-consent',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './cookie-consent.component.html',
  styleUrls: ['./cookie-consent.component.scss']
})
export class CookieConsentComponent implements OnInit {
  private readonly COOKIE_KEY = 'cookie_consent';
  private platformId = inject(PLATFORM_ID);
  private isBrowser = isPlatformBrowser(this.platformId);
  isVisible = signal(false);

  ngOnInit(): void {
    if (this.isBrowser && !localStorage.getItem(this.COOKIE_KEY)) {
      this.isVisible.set(true);
    }
  }

  accept(): void {
    if (this.isBrowser) {
      localStorage.setItem(this.COOKIE_KEY, 'accepted');
    }
    this.isVisible.set(false);
  }

  refuse(): void {
    if (this.isBrowser) {
      localStorage.setItem(this.COOKIE_KEY, 'refused');
    }
    this.isVisible.set(false);
  }
}
