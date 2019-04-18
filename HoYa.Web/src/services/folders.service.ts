import { Folder } from "entities/entity";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
@Injectable()
export class FoldersService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Folders/";
    }

    select(x: any) {
        if (x) return this.httpService.select(this.api +
            ("ByGroup?Id=" + x.groupId).replace("undefine", ""));
        else return this.httpService.select(this.api);
    }

    find(id: string) {
        return this.httpService.select(`${this.api}/${id}`);
    }

    create(folder: Folder) {
        return this.httpService.create(this.api, folder);
    }

    update(folder: Folder) {
        return this.httpService.update(this.api + folder.id, folder);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
