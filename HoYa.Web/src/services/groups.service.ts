import { Group } from "entities/group";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
@Injectable()
export class GroupsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Groups/";
    }

    get(x: any) {
        return this.httpService.get(this.api);
    }

    find(id: string) {
        return this.httpService.get(this.api + id);
    }

    create(group: Group) {
        return this.httpService.create(this.api, group);
    }

    update(id: string, group: Group) {
        return this.httpService.update(this.api + id, group);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
