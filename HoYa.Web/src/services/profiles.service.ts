import { Profile } from "entities/person";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
import { Observable } from "rxjs";
@Injectable()
export class ProfilesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Profiles/";
    }

    get(x: any) {
        return this.httpService.get(this.api);
    }

    find(id: string) {
        return this.httpService.get(this.api + id);
    }

    create(profile: Profile) {
        return this.httpService.create(this.api, profile);
    }

    update(id: string, profile: Profile) {
        return this.httpService.update(this.api + id, profile);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
