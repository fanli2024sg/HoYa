import { File } from "entities/entity";
import { Injectable } from "@angular/core";
import { Response } from "@angular/http";
import { HttpService } from "@core/services/http.service";
@Injectable()
export class FilesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Files/";
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

    create(files: any) {
        return this.httpService.upload(this.api, files)
    }

    update(file: File) {
        return this.httpService.update(this.api + file.id, file);
    }

    delete(id: number) {
        return this.httpService.delete(this.api + id);
    }
}
