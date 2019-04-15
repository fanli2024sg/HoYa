import { Profile } from "entities/person";
import { Injectable } from "@angular/core";
import { HttpService } from "core/services/http.service";
import { Observable } from "rxjs";
@Injectable()
export class SettingsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Settings/";
    }

    find() {
        return this.httpService.get(this.api + "Settings");
    }
}
