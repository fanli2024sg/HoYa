import { Recipe } from "entities/item";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

import { Query } from "models/query";
export const searchUrl = 'https://npmsearch.com/query';





@Injectable()
export class RecipesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Recipes";
    }

    //select(x: any): Observable<Recipe[]> {
        //return this.httpService.get(`${this.api}?anyLike=${x.anyLike}`);
    //}

    search(anyLike: string, withRefresh: boolean): Observable<Recipe[]> {
        // TODO: Add error handling
        return this.httpService.select<Recipe[]>(this.api, { anyLike: anyLike }, withRefresh);
    }

    create(recipe: Recipe) {
        return this.httpService.create(this.api, recipe);
    }

    update(id: string, recipe: Recipe) {
        return this.httpService.update(`${this.api}/${id}`, recipe);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }

    
}
