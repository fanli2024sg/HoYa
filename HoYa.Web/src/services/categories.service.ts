import { Category } from "entities/entity";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { Query } from "models/query";
export const searchUrl = 'https://npmsearch.com/query';





@Injectable()
export class CategoriesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Categories";
    }

    //select(x: any): Observable<Category[]> {
        //return this.httpService.get(`${this.api}?anyLike=${x.anyLike}`);
    //}

    search(anyLike: string, withRefresh: boolean): Observable<Category[]> {
        // TODO: Add error handling
        return this.httpService.select<Category[]>(this.api, { anyLike: anyLike }, withRefresh);
    }

    create(category: Category) {
        return this.httpService.create(this.api, category);
    }

    update(id: string, category: Category) {
        return this.httpService.update(`${this.api}/${id}`, category);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }

    
}
