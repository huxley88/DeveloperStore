import { Component, OnInit } from '@angular/core';
import { SalesService, Sale } from './sales.service';
import { ApiResponse } from '../shared/models/api-response';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-sales-list',
  templateUrl: './sales-list.component.html'
})
export class SalesListComponent implements OnInit {
  sales: Sale[] = [];
  page = 1;
  pageSize = 10;
  total = 0;
  loading = false;
  error: string | null = null;

  constructor(private salesService: SalesService) { }
  private subscriptions: Subscription = new Subscription();

  ngOnInit(): void {
    this.load(this.page);

    this.subscriptions.add(
      this.salesService.salesUpdated$.subscribe(() => {
        this.load(this.page);
      })
    );
  }

cancelSale(id?: string): void {
  if (!id) return;
  if (!confirm('Cancel this sale?')) return;
  this.salesService.cancel(id).subscribe({
    next: () => this.load(this.page),
    error: (err) => console.error(err)
  });
}

  load(page: number = this.page) {
    this.loading = true;
    this.error = null;
    this.salesService.listSales(page, this.pageSize).subscribe({
      next: (res: ApiResponse<Sale>) => {
        this.page = res.page;
        this.pageSize = res.pageSize;
        this.total = res.total;
        this.sales = res.data || [];
        this.loading = false;
      },
      error: (err) => {
        this.loading = false;
        this.error = 'Failed to load sales';
        console.error(err);
      }
    });
  }

  pages(): number[] {
    const count = Math.ceil(this.total / this.pageSize) || 1;
    return Array.from({ length: count }, (_, i) => i + 1);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
