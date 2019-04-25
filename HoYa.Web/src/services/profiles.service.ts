import { Profile, ProfileGroup } from "entities/person";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from "rxjs";
import { Query } from "models/query";
@Injectable()
export class ProfilesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Profiles";
    }

    get(x: any) {
        return this.httpService.select(this.api);
    }

    find(id: string) {
        return this.httpService.select(`${this.api}/${id}`);
    }

    create(profile: Profile) {
        return this.httpService.create(this.api, profile);
    }

    update(id: string, profile: Profile) {
        return this.httpService.update(`${this.api}/${id}`, profile);
    }


    archiveGroup(id: string, profileGroup: ProfileGroup) {
        console.log(JSON.stringify(profileGroup));
        debugger
        return this.httpService.update(`${this.api}/Groups/${id}`, profileGroup);
    }

    createGroup(profileGroup: ProfileGroup) {
        return this.httpService.create(`${this.api}/Groups`, profileGroup);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }

    filter(params: any, withRefresh: boolean): Observable<Query<Profile>> {
        return this.httpService.select<Query<Profile>>(`${this.api}`, params, withRefresh);
    }

    select(params: any, withRefresh: boolean): Observable<Profile[]> {
        return this.httpService.select<Profile[]>(`${this.api}`, params, withRefresh);
    }

    selectGroups(params: any, withRefresh: boolean): Observable<ProfileGroup[]> {
        return this.httpService.select<ProfileGroup[]>(`${this.api}/Groups`, params, withRefresh);
    }
}
