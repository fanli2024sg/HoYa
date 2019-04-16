import { FolderFile } from "entities/entity";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
@Injectable()
export class FolderFilesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/FolderFiles/";
    }

    get() {
        return this.httpService.get(this.api);
    }

    getByGroupId(groupId: number) {
        return this.httpService.get(this.api + "ByGroup?Id=" + groupId);
    }

    getById(id: number) {
        return this.httpService.get(this.api + id);
    }

    create(folderFile: FolderFile) {
        return this.httpService.create(this.api, folderFile);
    }

    update(folderFile: FolderFile) {
        return this.httpService.update(this.api + folderFile.id, folderFile);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
