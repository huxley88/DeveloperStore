import { Component, ViewChild } from '@angular/core';
import { SalesListComponent } from './sales/sales-list.component';
import { AuthService } from './auth/auth.service';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css']
})
export class AppComponent {
    constructor(private AuthService: AuthService) { }
    @ViewChild('list') list!: SalesListComponent;

    reload() {
        this.list.ngOnInit();
    }

    isLoggedIn(): boolean {
        return this.AuthService.isAuthenticated();
    }

}
