import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_URL } from '../shared/env';
import { ApiResponse } from '../shared/models/api-response';

export interface User {
  id: string;
  email: string;
}

@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly baseUrl = `${API_URL}/api/Users`;

  constructor(private http: HttpClient) {}

  list(page = 1, pageSize = 10): Observable<ApiResponse<User>> {
    return this.http.get<ApiResponse<User>>(`${this.baseUrl}?page=${page}&pageSize=${pageSize}`);
  }
}
