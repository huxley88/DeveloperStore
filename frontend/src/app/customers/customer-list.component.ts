import { Component, OnInit } from '@angular/core';
import { CustomerService, Customer } from './customer.service';
import { ApiResponse } from '../shared/models/api-response';

@Component({
  selector: 'app-customers-list',
  templateUrl: './customer-list.component.html',
})
export class CustomersListComponent implements OnInit {
  customers: Customer[] = [];
  page = 1;
  pageSize = 10;
  total = 0;
  loading = false;
  error: string | null = null;

  form: Partial<Customer> = { name: '', email: '', phone: '' };
  editingId: string | null = null;

  constructor(private service: CustomerService) {}

  ngOnInit(): void {
    this.load();
  }

  load(page: number = 1): void {
    this.loading = true;
    this.error = null;
    this.service.list(page, this.pageSize).subscribe({
      next: (res: ApiResponse<Customer>) => {
        this.customers = res.data || res.data || [];
        this.total = res.total || this.customers.length;
        this.page = page;
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.error = 'Failed to load customers';
        this.loading = false;
      }
    });
  }

  pages(): number[] {
    const count = Math.ceil(this.total / this.pageSize) || 1;
    return Array.from({ length: count }, (_, i) => i + 1);
  }

  save(): void {
    if (!this.form.name) return;

    if (this.editingId) {
      this.service.update(this.editingId, this.form).subscribe({
        next: () => {
          this.load(this.page);
          this.cancel();
        },
        error: (err) => console.error(err)
      });
    } else {
      this.service.create(this.form).subscribe({
        next: () => {
          this.load(1);
          this.resetForm();
        },
        error: (err) => console.error(err)
      });
    }
  }

  edit(c: Customer): void {
    this.form = { ...c };
    this.editingId = String(c.id);
  }

  remove(id?: string): void {
    if (!id) return;
    if (!confirm('Delete this customer?')) return;
    this.service.delete(id).subscribe({
      next: () => this.load(this.page),
      error: (err) => console.error(err)
    });
  }

  resetForm(): void {
    this.form = { name: '', email: '', phone: '' };
  }

  cancel(): void {
    this.editingId = null;
    this.resetForm();
  }
}