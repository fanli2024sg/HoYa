import { Component } from "@angular/core";
import { AspNetUser } from "entities/identity";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { AuthService } from "services/auth.service";
import { Observable } from 'rxjs';

@Component({
    selector: "login",
    templateUrl: "login.component.html",
    styleUrls: ['login.component.css']
})

export class LoginComponent {

    aspNetUser: FormGroup;
    constructor(
        public authService: AuthService,
        public router: Router,
        public formBuilder: FormBuilder) {
        this.aspNetUser = formBuilder.group({
            // 定義表格的預設值
            "value": [null, Validators.compose([Validators.required, Validators.minLength(6)])],
            "password": [null, Validators.compose([Validators.required, Validators.minLength(6)])]
        });
    }

    login(aspNetUser: AspNetUser) {
        this.authService.login(aspNetUser.value, aspNetUser.password).subscribe(() => {
            if (this.authService.isLoggedIn()) {
                let redirect = this.authService.redirectUrl ? this.authService.redirectUrl : "/views";
                this.router.navigate([redirect]);
            }
        });
    }

    adlogin() {
        this.authService.ad().subscribe(() => {
            if (this.authService.isLoggedIn()) {
                let redirect = this.authService.redirectUrl ? this.authService.redirectUrl : "/views";
                this.router.navigate([redirect]);
            }
        });
    }

    logout() {
        this.authService.logout();
    }
}
