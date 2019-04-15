import * as tslib_1 from "tslib";
import { Injectable } from "@angular/core";
import { HttpHeaders, HttpClient } from "@angular/common/http";
import { map } from 'rxjs/operators';
var HttpService = /** @class */ (function () {
    function HttpService(http) {
        this.http = http;
        //let urlSplit = window.location.href.split("/");      
        //this.host = "http://" + urlSplit[0] + "/" + urlSplit[1] + "/";
        //this.host = "http://118.163.183.248/hoya/";
        this.host = "http://localhost:3001/";
    }
    HttpService.prototype.getLocal = function (port, url) {
        url.replace("u", "");
        return this.http.get("http://127.0.0.1:" + port + "/" + url).pipe(map(function (x) { return eval("(" + x._body + ")").Certificate; }));
    };
    HttpService.prototype.get = function (url) {
        url.replace("u", "");
        return this.http.get(this.host + url, this.jwt()).pipe(map(function (x) { return x; }));
    };
    HttpService.prototype.create = function (url, model) {
        return this.http.post(this.host + url, model, this.jwt()).pipe(map(function (x) { return x; }));
    };
    HttpService.prototype.upload = function (url, files) {
        var _this = this;
        return new Promise(function (resolve, reject) {
            var formData = new FormData();
            var xhr = new XMLHttpRequest();
            for (var i = 0; i < files.length; i++) {
                formData.append("uploads[]", files[i], files[i].name);
            }
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4) {
                    if (xhr.status == 200) {
                        resolve(JSON.parse(xhr.response));
                    }
                    else {
                        reject(xhr.response);
                    }
                }
            };
            xhr.open("POST", _this.host + url, true);
            xhr.send(formData);
        });
    };
    HttpService.prototype.update = function (url, model) {
        return this.http.put(this.host + url, model, this.jwt()).pipe(map(function (x) { return x; }));
    };
    HttpService.prototype.delete = function (url) {
        return this.http.delete(this.host + url, this.jwt()).pipe(map(function (x) { return x; }));
    };
    HttpService.prototype.jwt = function () {
        this.currentUser = JSON.parse(localStorage.getItem("user"));
        if (this.currentUser) {
            return {
                headers: { "Authorization": "Bearer " + this.currentUser.token }
            };
        }
        else {
            return { withCredentials: true };
        }
    };
    HttpService.prototype.jwtFile = function () {
        if (localStorage.getItem("user")) {
            var headers = new HttpHeaders({ "Authorization": "Bearer " + localStorage.getItem("token") });
            headers.append("Content-Type", "multipart/form-data;boundary=AaB03x");
            headers.append("Accept", "application/json");
            return { headers: headers };
        }
        else {
            var headers = new HttpHeaders({ "Content-Type": "multipart/form-data;boundary=AaB03x" });
            headers.append("Accept", "application/json");
            return { headers: headers, withCredentials: true };
        }
    };
    HttpService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [HttpClient])
    ], HttpService);
    return HttpService;
}());
export { HttpService };
//# sourceMappingURL=http.service.js.map