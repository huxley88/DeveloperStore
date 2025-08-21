import { Component, OnInit } from '@angular/core';
import { UserService, User } from './users.service';
import { ApiResponse } from '../shared/models/api-response';

@Component({
  selector: 'app-users-list',
  templateUrl: './users-list.component.html',
})
export class UsersListComponent implements OnInit {
  users: User[] = [];
  page = 1;
  pageSize = 10;
  total = 0;
  loading = false;
  error: string | null = null;

  currentUserRole: string | null = null;

  constructor(private userService: UserService) {}

  ngOnInit(): void {
    const currentUser = JSON.parse(localStorage.getItem('currentUser') || '{}');
    this.currentUserRole = currentUser?.role || null;

    if (this.isAdminUser()) {
      this.load();
    }
  }

  load(page: number = 1): void {
    this.loading = true;
    this.error = null;

    this.userService.list(page, this.pageSize).subscribe({
      next: (res: ApiResponse<User>) => {
        this.users = res.data || [];
        this.page = res.page || page;
        this.pageSize = res.pageSize || this.pageSize;
        this.total = res.total || this.users.length;
        this.loading = false;
      },
      error: (err) => {
        this.error = 'Failed to load users';
        this.loading = false;
        console.error(err);
      }
    });
  }

  pages(): number[] {
    const count = Math.ceil(this.total / this.pageSize) || 1;
    return Array.from({ length: count }, (_, i) => i + 1);
  }

  public isAdminUser(): boolean {
    return this.currentUserRole?.toLowerCase() === 'admin';
  }
}
