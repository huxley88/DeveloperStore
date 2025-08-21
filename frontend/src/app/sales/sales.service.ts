import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, BehaviorSubject } from 'rxjs';
import { API_URL } from '../shared/env';
import { ApiResponse } from '../shared/models/api-response';

export interface Sale { id: string; saleNumber: string; customerName: string; branchName: string; totalAmount: number; cancelled: boolean; }
export interface Customer { id: string; name: string; }  
export interface Product { id: string; name: string; price?: number; unitPrice?: number; }
export interface SaleItem { productId: string; productName: string; quantity: number; unitPrice: number; }

@Injectable({ providedIn: 'root' })
export class SalesService {
    private readonly baseUrl = `${API_URL}/api/Sales`;
    private readonly baseUrlProduct = `${API_URL}/api/Products`;
    private readonly baseUrlCustomer = `${API_URL}/api/Customers`;

    private salesUpdated = new BehaviorSubject<void>(undefined);
    salesUpdated$ = this.salesUpdated.asObservable();

    constructor(private http: HttpClient) { }

    listCustomers(page = 1, pageSize = 1000): Observable<ApiResponse<Customer>> {
        return this.http.get<ApiResponse<Customer>>(`${this.baseUrlCustomer}?page=${page}&pageSize=${pageSize}`);
    }

    listProducts(page = 1, pageSize = 1000): Observable<ApiResponse<Product>> {
        return this.http.get<ApiResponse<Product>>(`${this.baseUrlProduct}?page=${page}&pageSize=${pageSize}`);
    }

    listSales(page = 1, pageSize = 10): Observable<ApiResponse<Sale>> {
        return this.http.get<ApiResponse<Sale>>(`${this.baseUrl}?page=${page}&pageSize=${pageSize}`);
    }

    createSale(payload: {
        saleNumber: string;
        branchId: string;
        branchName: string;
        customerId: string;
        customerName: string;
        date: string;
        items: { productId: string; productName: string; quantity: number; unitPrice: number; }[];
    }): Observable<any> {
        return this.http.post(`${this.baseUrl}`, payload);
    }

    delete(id: string): Observable<any> {
        return this.http.delete(`${this.baseUrl}/${id}`);
    }

      cancel(id: string): Observable<any> {
  return this.http.post(`${this.baseUrl}/${id}/cancel`, {});
}

    notifySalesUpdated() {
        this.salesUpdated.next();
    }
}