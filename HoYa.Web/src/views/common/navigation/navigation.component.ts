import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { CurrentUserModel } from "models/login";
import { AuthService } from "@core/services/auth.service";
import { SettingsService } from "services/settings.service";
import { Profile } from "entities/person";
import * as $ from "jquery";

@Component({
    selector: 'navigation',
    templateUrl: 'navigation.template.html',
    providers: [AuthService, SettingsService]
})

export class NavigationComponent implements OnInit {
    currentUser: CurrentUserModel;
    isAdmin: boolean;
    activeLink: number;
    selectedProfile: Profile;
    constructor(
        private router: Router,
        private authService: AuthService,
        private settingsService: SettingsService) {
        this.activeLink = 1;
        this.currentUser = JSON.parse(localStorage.getItem("user"));        
        if (this.currentUser.userId) {
            this.settingsService.find().subscribe((x: any) => {
                this.selectedProfile = x.profile;
            });
        }
    }

    ngOnInit() {
       
    }

    ngAfterViewInit() {
        //$("#side-menu").metisMenu();
    }

    activedRoute(route: string): boolean {
        return this.router.url.indexOf(route) > -1;

    }
    getAuth() {
      
    }
    logout() {
        this.authService.logout();
        localStorage.removeItem("currentUser");
        this.router.navigate(["/login"]);
    }
}
