import { Person } from "entities/person";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from "rxjs";
import { Query } from "models/query";

@Injectable()
export class PeopleService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/People/";
    }

    update(id: string, person: Person) {
        return this.httpService.update(`${this.api}/${id}`, person);
    }

    create(person: Person) {
        return this.httpService.create(this.api, person);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }

    select(params: any, withRefresh: boolean): Observable<Person[]> {
        return this.httpService.select<Person[]>(`${this.api}`, params, withRefresh);
    }
}
