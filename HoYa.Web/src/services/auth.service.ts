import { Injectable } from "@angular/core";
import { map } from "rxjs/operators";
import { LoginModel, CurrentUserModel } from "models/login";
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from 'rxjs';
import { AppInterface } from "interfaces/app.interface";
@Injectable()
export class AuthService {
    isLogin$ = new BehaviorSubject<boolean>(this.hasToken());
    token$ = new BehaviorSubject<string>(localStorage.getItem("token"));
    userName: string;
    userId: string;
    profileValue: string;
    host: string; 
    redirectUrl: string;
    loginModel: LoginModel;

    headers = new Headers();
    constructor(private http: HttpClient,
        private appInterface: AppInterface
    ) {
    }
    private hasToken(): boolean {
        return !!localStorage.getItem("token");
    }
    isLoggedIn(): boolean {
        this.token$.next(localStorage.getItem("token"));
        return this.isLogin$.value;/*
        if (localStorage.getItem("token")) {
            this.token = localStorage.getItem("token");
            return true;
        }            
        else {
            if (localStorage.getItem("ad")) return true;
            else return false;
        }*/
    }

    ad() {
        localStorage.removeItem("ad");
        this.loginModel = new LoginModel();
        return this.http.get(`${this.appInterface.host$.getValue()}/api/Settings`, { withCredentials: true }).pipe(
            map((response2: Response) => {
                /*   let setting = response2.json();
                   localStorage.setItem("ad", setting.ad);
                   this.userName = setting.ad;
                   this.userId = setting.profile.userId;
                   this.profileValue = setting.profile.value;
                 */
                this.isLogin$.next(true);
            }));
    }

    login(username, password) {
        localStorage.removeItem("login");
        this.loginModel = new LoginModel();
        this.loginModel.userName = username;
        this.loginModel.password = password;
        console.log("login:" + username + "," + password);
        return this.http.post(`${this.appInterface.host$.getValue()}/auth/Login`, this.loginModel).pipe(map((x: any) => {
            let user = x as CurrentUserModel;
            this.token$.next(user.token);
            localStorage.setItem("token", user.token);
            this.isLogin$.next(true);
        }));
    }

    logout(): void {
        localStorage.removeItem("user");
        localStorage.removeItem("ad");
        this.userName = "";
        this.userId = "";
        this.profileValue = "";
        this.isLogin$.next(false);
    }

}
