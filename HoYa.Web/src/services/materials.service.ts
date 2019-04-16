import { Material } from "entities/material";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';

export const searchUrl = 'https://npmsearch.com/query';





@Injectable()
export class MaterialsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Materials";
    }

    //select(x: any): Observable<Material[]> {
        //return this.httpService.get(`${this.api}?anyLike=${x.anyLike}`);
    //}

    search(anyLike: string, withRefresh: boolean): Observable<Material[]> {
        // TODO: Add error handling
        return this.httpService.search<Material[]>(this.api, { anyLike: anyLike }, withRefresh);
    }

    create(material: Material) {
        return this.httpService.create(this.api, material);
    }

    update(id: string, material: Material) {
        return this.httpService.update(this.api + id, material);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }

    
}
