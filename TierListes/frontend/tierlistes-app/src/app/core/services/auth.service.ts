import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, tap, catchError, throwError, of } from 'rxjs';
import { User, LoginRequest, RegisterRequest } from '../models/auth.models';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private readonly API_URL = 'http://localhost:5000/api/auth';

  private currentUserSignal = signal<User | null>(null);
  private isLoadingSignal = signal<boolean>(true);

  readonly currentUser = this.currentUserSignal.asReadonly();
  readonly isAuthenticated = computed(() => !!this.currentUserSignal());
  readonly isLoading = this.isLoadingSignal.asReadonly();

  constructor(
    private http: HttpClient,
    private router: Router
  ) {
    this.checkAuthStatus();
  }

  register(request: RegisterRequest): Observable<User> {
    return this.http.post<User>(`${this.API_URL}/register`, request, { withCredentials: true }).pipe(
      tap(user => this.currentUserSignal.set(user)),
      catchError(error => this.handleAuthError(error))
    );
  }

  login(request: LoginRequest): Observable<User> {
    return this.http.post<User>(`${this.API_URL}/login`, request, { withCredentials: true }).pipe(
      tap(user => this.currentUserSignal.set(user)),
      catchError(error => this.handleAuthError(error))
    );
  }

  logout(): void {
    this.http.post(`${this.API_URL}/logout`, {}, { withCredentials: true }).subscribe({
      complete: () => {
        this.currentUserSignal.set(null);
        this.router.navigate(['/']);
      }
    });
  }

  checkAuthStatus(): void {
    this.http.get<User>(`${this.API_URL}/me`, { withCredentials: true }).pipe(
      catchError(() => of(null))
    ).subscribe({
      next: (user) => {
        this.currentUserSignal.set(user);
        this.isLoadingSignal.set(false);
      },
      error: () => {
        this.currentUserSignal.set(null);
        this.isLoadingSignal.set(false);
      }
    });
  }

  private handleAuthError(error: any): Observable<never> {
    const message = error.error?.message || 'Une erreur est survenue';
    return throwError(() => new Error(message));
  }
}
