import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../shared/env';

export interface Customer {
  id: string;
  name: string;
  email?: string;
  phone?: string;
}

@Injectable({ providedIn: 'root' })
export class CustomerService {
  private readonly baseUrl = `${API_URL}/api/Customers`;

  constructor(private http: HttpClient) {}

  list(page = 1, pageSize = 10): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}?page=${page}&pageSize=${pageSize}`);
  }

  getAll(): Observable<Customer[]> {
    return this.http.get<Customer[]>(`${this.baseUrl}`);
  }

  search(q: string): Observable<Customer[]> {
    return this.http.get<Customer[]>(`${this.baseUrl}?q=${encodeURIComponent(q)}`);
  }

  create(payload: Partial<Customer>): Observable<any> {
    return this.http.post(this.baseUrl, payload);
  }

  update(id: string, payload: Partial<Customer>): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }

}