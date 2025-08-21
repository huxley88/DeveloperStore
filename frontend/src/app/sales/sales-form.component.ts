import { Component, OnInit } from '@angular/core';
import { SalesService, Customer, Product, SaleItem } from './sales.service';
import { ApiResponse } from '../shared/models/api-response';

@Component({
  selector: 'app-sales-form',
  templateUrl: './sales-form.component.html'
})
export class SalesFormComponent implements OnInit {

  customerQuery = '';
  productQuery = '';
  customerResults: Customer[] = [];
  productResults: Product[] = [];
  selectedCustomer: Customer | null = null;
  items: SaleItem[] = [];
  discountPercent = 0;

  mainBranches = [
    { id: 1, name: 'Main Branch - São Paulo' },
    { id: 2, name: 'Branch - Rio de Janeiro' },
    { id: 3, name: 'Branch - Belo Horizonte' }
  ];
  selectedBranchId: number | null = null;

  private allCustomers: Customer[] = [];
  private allProducts: Product[] = [];

  constructor(private service: SalesService) { }

  ngOnInit() {
    this.service.listCustomers().subscribe({
      next: (res: ApiResponse<Customer>) => {
        this.allCustomers = res.data || [];
      },
      error: (err: any) => {
        console.error(err);
        this.allCustomers = [];
      }
    });

    this.service.listProducts().subscribe({
      next: (res: ApiResponse<Product>) => {
        this.allProducts = res.data || [];
      },
      error: (err: any) => {
        console.error(err);
        this.allProducts = [];
      }
    });
  }

  get subtotal(): number {
    return this.items.reduce((s, i) => s + i.unitPrice * i.quantity, 0);
  }

  get total(): number {
    const p = Number(this.discountPercent) || 0;
    const d = this.subtotal * (p / 100);
    return Math.max(0, Math.round((this.subtotal - d) * 100) / 100);
  }

  searchCustomers() {
    const q = this.customerQuery.toLowerCase().trim();
    if (!q) {
      this.customerResults = [];
      return;
    }
    this.customerResults = this.allCustomers.filter(c => c.name.toLowerCase().includes(q));
  }

  chooseCustomer(c: Customer) {
    this.selectedCustomer = c;
    this.customerQuery = c.name;
    this.customerResults = [];
  }

  searchProducts() {
    const q = this.productQuery.toLowerCase().trim();
    if (!q) {
      this.productResults = [];
      return;
    }
    this.productResults = this.allProducts.filter(p => p.name.toLowerCase().includes(q));
  }

  addProduct(p: Product) {
    const existing = this.items.find(i => i.productId === p.id);
    const price = p.unitPrice || p.price || 0;
    if (existing) existing.quantity += 1;
    else this.items.push({ productId: p.id, productName: p.name, quantity: 1, unitPrice: price });
    this.productQuery = '';
    this.productResults = [];
    this.updateDiscount(); 
  }

  inc(i: SaleItem) { i.quantity += 1; this.updateDiscount(); }
  dec(i: SaleItem) { if (i.quantity > 1) i.quantity -= 1; this.updateDiscount(); }
  remove(i: SaleItem) { this.items = this.items.filter(x => x !== i); this.updateDiscount(); }

  updateDiscount() {
  let maxDiscount = 0;

  for (const item of this.items) {
    if (item.quantity >= 10 && item.quantity <= 20) {
      maxDiscount = Math.max(maxDiscount, 20);
    } else if (item.quantity >= 4) {
      maxDiscount = Math.max(maxDiscount, 10);
    }
  }

  this.discountPercent = maxDiscount;
}

  saveSale() {
    if (!this.selectedCustomer || !this.items.length || !this.selectedBranchId) return;

    const branch = this.mainBranches.find(b => b.id.toString() === this.selectedBranchId?.toString()); if (!branch) return;

    const payload = {
      saleNumber: `SALE-${new Date().getFullYear()}-${Math.floor(Math.random() * 10000).toString().padStart(4, '0')}`,
      branchId: branch.id.toString(),
      branchName: branch.name,
      customerId: this.selectedCustomer.id,
      customerName: this.selectedCustomer.name,
      date: new Date().toISOString(),
      items: this.items.map(i => ({
        productId: i.productId,
        productName: i.productName,
        quantity: i.quantity,
        unitPrice: i.unitPrice
      }))
    };

    this.service.createSale(payload).subscribe({
      next: _ => {
        this.selectedCustomer = null;
        this.customerQuery = '';
        this.items = [];
        this.discountPercent = 0;
        this.selectedBranchId = null;
        alert('Sale saved successfully.');

        this.service.notifySalesUpdated();
      },
      error: err => {
        console.error(err);
        const msg = err.error?.message || 'Failed to save sale';
        alert(msg);
      }
    });
  }
}