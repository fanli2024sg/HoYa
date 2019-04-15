import { Mission } from "entities/process";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
@Injectable()
export class MissionsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Missions/";
    }

    get(x) {
        return this.httpService.get(this.api);
    }

    find(id: string) {
        return this.httpService.get(this.api + id);
    }

    create(mission: Mission) {
        return this.httpService.create(this.api, mission);
    }

    update(id: string, mission: Mission) {
        return this.httpService.update(this.api + id, mission);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
