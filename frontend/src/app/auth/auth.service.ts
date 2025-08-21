import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap, BehaviorSubject } from 'rxjs';
import { API_URL } from '../shared/env';

@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly baseUrl = `${API_URL}/api/Auth`;
  private readonly tokenKey = 'jwt';
  private loggedIn = new BehaviorSubject<boolean>(this.hasToken());
  isLoggedIn$ = this.loggedIn.asObservable();

  constructor(private http: HttpClient) {}

  private hasToken(): boolean {
    return !!localStorage.getItem(this.tokenKey);
  }

  updateLoginStatus() {
    this.loggedIn.next(this.hasToken());
  }

  login(email: string, password: string): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}`, { email, password }).pipe(
      tap(res => {
        if (res?.data?.token) {
          localStorage.setItem(this.tokenKey, res.data.token);
          
          localStorage.setItem('currentUser', JSON.stringify({
          email: res.data.email,
          name: res.data.name,
          role: res.data.role
        }));

          this.loggedIn.next(true);
        }
      })
    );
  }

  logout(): void {
    localStorage.removeItem(this.tokenKey);
    this.loggedIn.next(false);
  }

  getToken(): string | null {
    return localStorage.getItem(this.tokenKey);
  }

  isAuthenticated(): boolean {
    return !!this.getToken();
  }
}
