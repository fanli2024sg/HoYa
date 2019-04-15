import { Component } from "@angular/core";
import { smoothlyMenu } from "@core/helpers/app.helpers";
import { Router } from "@angular/router";
import { AuthService } from "@core/services/auth.service";
declare var $:any;

@Component({
    selector: 'topnavbar',
    templateUrl: 'topnavbar.template.html'
})
export class TopnavbarComponent {
    constructor(
        private router: Router,
         private  authService: AuthService) {
    }

    toggleNavigation(): void {
        $("body").toggleClass("mini-navbar");
        smoothlyMenu();
    }

    logout() {
        this.authService.logout();
        this.router.navigate(['/login']);
    }
}