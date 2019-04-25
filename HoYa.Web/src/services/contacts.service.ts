import { Contact } from "entities/person";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from "rxjs";
import { Query } from "models/query";

@Injectable()
export class ContactsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Contacts/";
    }

    update(id: string, contact: Contact) {
        return this.httpService.update(`${this.api}/${id}`, contact);
    }

    create(contact: Contact) {
        return this.httpService.create(this.api, contact);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }

    select(params: any, withRefresh: boolean): Observable<Contact[]> {
        return this.httpService.select<Contact[]>(`${this.api}`, params, withRefresh);
    }
}
