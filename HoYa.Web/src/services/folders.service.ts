import { Folder } from "entities/entity";
import { Injectable } from "@angular/core";
import { Response } from "@angular/http";
import { HttpService } from "@core/services/http.service";
@Injectable()
export class FoldersService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Folders/";
    }

    get() {
        return this.httpService.get(this.api);
    }

    getByGroupId(groupId: string) {
        return this.httpService.get(this.api + "ByGroup?Id=" + groupId);
    }

    getById(id: string) {
        return this.httpService.get(this.api + id);
    }

    create(folder: Folder) {
        return this.httpService.create(this.api, folder);
    }

    update(folder: Folder) {
        return this.httpService.update(this.api + folder.id, folder);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
