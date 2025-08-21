import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../shared/env';

export interface Product {
  id: string;
  name: string;
  price: number;
  stock?: number;
  unitPrice?: number;
}

@Injectable({ providedIn: 'root' })
export class ProductService {
  private readonly baseUrl = `${API_URL}/api/Products`;

  constructor(private http: HttpClient) {}

  list(page = 1, pageSize = 10): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}?page=${page}&pageSize=${pageSize}`);
  }

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}`);
  }

  search(q: string): Observable<Product[]> {
    return this.http.get<Product[]>(`${this.baseUrl}?q=${encodeURIComponent(q)}`);
  }

  create(payload: Partial<Product>): Observable<any> {
    return this.http.post(this.baseUrl, payload);
  }

  update(id: string, payload: Partial<Product>): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, payload);
  }

  delete(id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`);
  }
}