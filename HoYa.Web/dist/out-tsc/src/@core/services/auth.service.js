import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { map } from 'rxjs/operators';
import { LoginModel } from "models/login";
import { Headers } from "@angular/http";
import { HttpClient } from "@angular/common/http";
var AuthService = /** @class */ (function () {
    function AuthService(http) {
        this.http = http;
        this.headers = new Headers();
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
    AuthService.prototype.isLoggedIn = function () {
        if (localStorage.getItem("user"))
            return true;
        else {
            if (localStorage.getItem("ad"))
                return true;
            else
                return false;
        }
    };
    AuthService.prototype.ad = function () {
        var _this = this;
        localStorage.removeItem("ad");
        this.loginModel = new LoginModel();
        return this.http.get(this.host + "api/Settings", { withCredentials: true }).pipe(map(function (response2) {
            var setting = response2.json();
            localStorage.setItem("ad", setting.ad);
            _this.userName = setting.ad;
            _this.userId = setting.profile.userId;
            _this.profileValue = setting.profile.value;
        }));
    };
    AuthService.prototype.login = function (username, password) {
        localStorage.removeItem("login");
        this.loginModel = new LoginModel();
        this.loginModel.userName = username;
        this.loginModel.password = password;
        console.log("login:" + username + "," + password);
        return this.http.post(this.host + "auth/Login", this.loginModel).pipe(map(function (x) {
            var user = JSON.stringify(x);
            localStorage.setItem("user", user);
        }));
    };
    AuthService.prototype.logout = function () {
        localStorage.removeItem("user");
        localStorage.removeItem("ad");
        this.userName = "";
        this.userId = "";
        this.profileValue = "";
        console.log(this.profileValue);
        console.log(this.userName);
        console.log(this.userId);
        console.log(this.isLoggedIn());
    };
    AuthService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpClient])
    ], AuthService);
    return AuthService;
}());
export { AuthService };
//# sourceMappingURL=auth.service.js.map