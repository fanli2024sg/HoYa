import { Injectable } from "@angular/core";
import { RequestOptions } from "@angular/http";
import { HttpHeaders ,HttpClient } from "@angular/common/http";
import { CurrentUserModel } from "models/login";
import { map, catchError } from 'rxjs/operators';
@Injectable()
export class HttpService {

    public host: string;
    currentUser: CurrentUserModel;
    constructor(private http: HttpClient) {
        //let urlSplit = window.location.href.split("/");      
        //this.host = "http://" + urlSplit[0] + "/" + urlSplit[1] + "/";
        //this.host = "http://118.163.183.248/hoya/";
        this.host = "http://localhost:3001/";
    }
    getLocal(port:number, url: string) {
        url.replace("u", "");
        return this.http.get("http://127.0.0.1:" + port + "/" + url).pipe(map((x: any) => eval("(" + x._body + ")").Certificate));
    }

    get(url: string) {
      url.replace("u", "");
      return  this.http.get(this.host + url, this.jwt()).pipe(map(x => x));
    }

    create(url: string, model: any) {
        return this.http.post(this.host + url, model, this.jwt()).pipe(map(x => x));

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
        return this.http.put(this.host + url, model, this.jwt()).pipe(map(x => x));
    }

    delete(url: string) {
        return this.http.delete(this.host + url, this.jwt()).pipe(map(x => x));
    } 
    private jwt() {
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
            headers.append("Content-Type", "multipart/form-data;boundary=AaB03x");
            headers.append("Accept", "application/json");
            return { headers: headers };
        } else {
            let headers = new HttpHeaders({ "Content-Type": "multipart/form-data;boundary=AaB03x"});
            headers.append("Accept", "application/json");
            return { headers: headers, withCredentials: true };
        }
    }
}
