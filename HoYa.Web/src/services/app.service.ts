import { Injectable } from "@angular/core"; 
import { HttpClient } from "@angular/common/http";
import { BehaviorSubject, Observable } from 'rxjs';
import { AppInterface } from 'interfaces/app.interface';
import { PageEvent, Sort } from '@angular/material';
@Injectable()
export class AppService implements AppInterface {
    page$: BehaviorSubject<PageEvent> = new BehaviorSubject<PageEvent>({
        pageIndex: 0,
        pageSize: 10,
        length: null
    });
    sort$: BehaviorSubject<Sort> = new BehaviorSubject<Sort>({
        active: "createdDate",
        direction: "asc"
    });
    leftIcon$: BehaviorSubject<string> = new BehaviorSubject<string>("menu");
    anyLike$: BehaviorSubject<string> = new BehaviorSubject<string>("");
    title$: BehaviorSubject<string> = new BehaviorSubject<string>("­º­¶");
    rightIcons$: BehaviorSubject<Array<string>> = new BehaviorSubject<Array<string>>([]); 
    profileMenus$: BehaviorSubject<Array<any>> = new BehaviorSubject<Array<any>>([]); 



    headers = new Headers();
    constructor(private http: HttpClient) {
       
    }
   
   
    
}
