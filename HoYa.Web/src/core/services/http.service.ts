import { Injectable } from "@angular/core";
import { HttpHeaders, HttpClient, HttpParams } from "@angular/common/http";
import { CurrentUserModel } from "models/login";
import { map } from "rxjs/operators";
import { Observable, of } from 'rxjs';
import { HttpParamsOptions } from '@angular/common/http/src/params';

@Injectable()
export class HttpService {

    host: string;
    currentUser: CurrentUserModel;
    constructor(private http: HttpClient) {
        //let urlSplit = window.location.href.split("/");      
        //this.host = "http://" + urlSplit[0] + "/" + urlSplit[1] + "/";
        //this.host = "http://118.163.183.248/hoya/";
        this.host = "http://localhost:3001/";
    }


    /** GET heroes from the server */
    search<I>(api: string, fromObject: HttpParamsOptions["fromObject"], refresh = false): Observable<I> {
        let params = new HttpParams({ fromObject: fromObject });
        let headerMap = refresh ? { "x-refresh": "true" } : {};
        let headers = new HttpHeaders(headerMap);
        return this.http.get<I>(this.host + api, { headers, params }).pipe(map(x => x));
    }



    get(url: string) {
        url.replace("u", "");
        return this.http.get(this.host + url, this.getHttpOptions()).pipe(map(x => x));
    }

    create(url: string, model: any) {
        return this.http.post(this.host + url, model, this.getHttpOptions()).pipe(map(x => x));

    }


    upload(url: string, files: Array<File>) {
        return new Promise((resolve, reject) => {
            var formData: any = new FormData();
            var xhr = new XMLHttpRequest();
            for (var i = 0; i < files.length; i++) {
                formData.append("uploads[]", files[i], files[i].name);
            }
            xhr.onreadystatechange = function () {
                if (xhr.readyState == 4) {
                    if (xhr.status == 200) {
                        resolve(JSON.parse(xhr.response));
                    } else {
                        reject(xhr.response);
                    }
                }
            }
            xhr.open("POST", this.host + url, true);
            xhr.send(formData);
        });
    }

    update(url: string, model: any) {
        return this.http.put(this.host + url, model, this.getHttpOptions()).pipe(map(x => x));
    }

    delete(url: string) {
        return this.http.delete(this.host + url, this.getHttpOptions()).pipe(map(x => x));
    }
    private getHttpOptions() {
        this.currentUser = JSON.parse(localStorage.getItem("user"));
        if (this.currentUser) {
            return {
                headers: { "Authorization": "Bearer " + this.currentUser.token }
            };
        } else {
            return { withCredentials: true };
        }
    }

    private jwtFile() {
        if (localStorage.getItem("user")) {
            let headers = new HttpHeaders({ "Authorization": "Bearer " + localStorage.getItem("token") });
            headers.append("View-Type", "multipart/form-data;boundary=AaB03x");
            headers.append("Accept", "application/json");
            return { headers: headers };
        } else {
            let headers = new HttpHeaders({ "View-Type": "multipart/form-data;boundary=AaB03x" });
            headers.append("Accept", "application/json");
            return { headers: headers, withCredentials: true };
        }
    }
}
