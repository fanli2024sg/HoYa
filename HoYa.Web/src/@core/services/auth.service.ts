import { Injectable } from "@angular/core";

import { Observable, of } from 'rxjs';
import { map, catchError } from 'rxjs/operators';

import { LoginModel } from "models/login";
import {Headers, Response } from "@angular/http";
import { HttpClient } from "@angular/common/http";

@Injectable()
export class AuthService {
    userName: string;
    userId: string;
    profileValue: string;
    host: string;
    // store the URL so we can redirect after logging in
    redirectUrl: string;
    loginModel: LoginModel;

    headers = new Headers();
    constructor(private http: HttpClient) {
        this.headers.append("Content-Type", "application/x-www-form-urlencoded");
        this.loginModel = new LoginModel();
        //let urlSplit = window.location.href.split("/");
        //this.host = "http://" + urlSplit[0] + "/" + urlSplit[1] + "/";
        //this.host = "http://118.163.183.248/hoya/";
        this.host = "http://localhost:3001/";
        this.userName = "";
        this.userId = "";
        this.profileValue = "";
    }
   
    isLoggedIn(): boolean {
        
        if (localStorage.getItem("user")) return true;
        else {
            if (localStorage.getItem("ad")) return true;
            else return false;
        }
    }

    ad() {
        localStorage.removeItem("ad");
        this.loginModel = new LoginModel();
        return this.http.get(this.host + "api/Settings", { withCredentials: true }).pipe(
            map((response2: Response) => {
                let setting = response2.json();
                localStorage.setItem("ad", setting.ad);
                this.userName = setting.ad;
                this.userId = setting.profile.userId;
                this.profileValue = setting.profile.value;
                
            }));
    }

    login(username, password) {
        localStorage.removeItem("login");
        this.loginModel = new LoginModel();
        this.loginModel.userName = username;
        this.loginModel.password = password;
        console.log("login:" + username + "," + password);
        return this.http.post(this.host + "auth/Login", this.loginModel).pipe(map((x: any) => {
            let user = JSON.stringify(x);
            localStorage.setItem("user", user);
        }));
    }

    logout(): void {
        localStorage.removeItem("user");
        localStorage.removeItem("ad");
        this.userName = "";
        this.userId = "";
        this.profileValue = "";
        console.log(this.profileValue);
        console.log(this.userName);
        console.log(this.userId);
        console.log(this.isLoggedIn());
    }
    
}
