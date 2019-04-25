import { Option } from "entities/entity";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from 'rxjs';
import { Query } from "models/query";
@Injectable()
export class OptionsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Options";
    }
    get(x: any) {
        return this.httpService.select(this.api + "By?parentId=" + x.parentId +
            "&anyLike="+x.anyLike);
    }
    getByGroupValue(groupValue: string) {
        return this.httpService.select(this.api + "ByGroupValue?Value=" + groupValue);
    }

    find(id: string): Observable<Option> {
        return this.httpService.select(`${this.api}/${id}`);
    }

    select(params: any, withRefresh: boolean): Observable<Option[]> {
        return this.httpService.select<Option[]>(`${this.api}`, params, withRefresh);
    }

    create(option: Option) {
        return this.httpService.create(this.api, option);
    }

    update(id: string, option: Option) {
        return this.httpService.update(`${this.api}/${id}`, option);
    } 

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
