import { Activity } from "entities/workflow";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
@Injectable()
export class ActivitiesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Activities/";
    }

    get(x) {
        return this.httpService.select(this.api);
    }

    find(id: string) {
        return this.httpService.select(`${this.api}/${id}`);
    }

    create(activity: Activity) {
        return this.httpService.create(this.api, activity);
    }

    update(id: string, activity: Activity) {
        return this.httpService.update(`${this.api}/${id}`, activity);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
