import { Component, OnInit } from "@angular/core";
import { MatSidenav, MatDrawerToggleResult } from "@angular/material";
import { Router } from "@angular/router";
import { AuthService } from "services/auth.service";
@Component({
    selector: "views",
    templateUrl: "views.component.html",
    styleUrls: ["views.component.css"],
    providers: []
})
export class ViewsComponent implements OnInit {
    title: string;
    constructor(
        public router: Router,
        public authService: AuthService
    ) {
        this.title = "";
    }

    ngOnInit() { }

    toggleSideNav(sideNav: MatSidenav) {
        sideNav.toggle().then((result: any) => {
            console.log(result);
            console.log(`¿ï³æª¬ºA¡G${result.type}`);
        });
    }

    opened() {
        console.log("open");
    }

    closed() {
        console.log("close");
    }

    goto(title: string) {
        this.title = title;
    }

    back() {
        this.title = "";
    }
    logout() {
        this.authService.logout();
        this.router.navigate(["./login"]);
    }
}
