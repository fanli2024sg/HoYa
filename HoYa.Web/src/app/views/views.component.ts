import { Component, OnInit } from "@angular/core";
import { MatSidenav, MatDrawerToggleResult } from "@angular/material";
import { Router } from "@angular/router";
import { AuthService } from "services/auth.service";
import { AppInterface } from "interfaces/app.interface";
import { AppService } from "services/app.service";
import { Observable } from 'rxjs';
import { Location } from '@angular/common';
@Component({
    selector: "views",
    templateUrl: "views.component.html",
    styleUrls: ["views.component.css"],
    providers: [
        AppService
    ]
})
export class ViewsComponent implements OnInit {
    leftIcon$: Observable<string> = this.appService.leftIcon$;
    title$: Observable<string> = this.appService.title$;
    rightIcons$: Observable<string[]> = this.appService.rightIcons$;
    profileMenus$: Observable<string[]> = this.appService.profileMenus$;
    searchBar: boolean;
    title: string;
    sidenavOpen: boolean;
    constructor(
        public router: Router,
        public authService: AuthService,
        public appService: AppInterface,
        private location: Location
    ) {
        this.title = "";

    }

    ngOnInit() {
        this.appService.leftIcon$.next("menu");
        this.appService.title$.next("HoYa");
        this.appService.rightIcons$.next(["search", "more_vert"]);
        this.appService.profileMenus$.next([
            { icon: "account_circle", menu: "我的檔案" },
            { icon: "mail", menu: "待辦" },
            { icon: "exit_to_app", menu: "登出" }
        ]);
    }

    toggleSideNav(sideNav: MatSidenav) {
        sideNav.toggle().then((result: any) => {
            console.log(result);
            console.log(`選單狀態：${result.type}`);
        });
    }

    iconClick(icon: string) {
        if (icon === "leftIcon$") icon = this.appService.leftIcon$.getValue();

        switch (icon) {
            case "search":
                this.searchBar = true;
                break;
            case "menu":
                this.sidenavOpen = true;
                break;
            case "arrow_back":
                if (this.sidenavOpen) this.sidenavOpen = false;
                else this.location.back();
                break;
            default:
                break;
        }
    }

    search(anyLike: string) {
        this.appService.anyLike$.next(anyLike);
    }

    opened() {
    }

    closed() {
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
