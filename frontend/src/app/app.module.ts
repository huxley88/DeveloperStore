import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { LoginComponent } from './auth/login/login.component';
import { UsersListComponent } from './users/users-list.component';

import { SalesComponent } from './sales/sales.component';
import { SalesFormComponent } from './sales/sales-form.component';
import { SalesListComponent } from './sales/sales-list.component';

import { CustomersListComponent } from './customers/customer-list.component';
import { ProductListComponent } from './products/product-list.component';

import { NavbarComponent } from './shared/navbar.component';
import { AuthInterceptor } from './auth/auth.interceptor';

@NgModule({
  declarations: [
    AppComponent,

    LoginComponent,
    UsersListComponent,

    SalesComponent,
    SalesFormComponent,
    SalesListComponent,

    CustomersListComponent,
    ProductListComponent,

    NavbarComponent
  ],
  imports: [BrowserModule, FormsModule, ReactiveFormsModule, HttpClientModule, AppRoutingModule],
  providers: [{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }],
  bootstrap: [AppComponent]
})
export class AppModule { }
