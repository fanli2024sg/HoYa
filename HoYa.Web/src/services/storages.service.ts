import { Storage } from "entities/inventory";
import { Injectable } from "@angular/core";
;
import { HttpService } from "@core/services/http.service";
@Injectable()
export class StoragesService {
   
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Storages/";
    }

    get(x) {
        return this.httpService.get(this.api + "By?anyLike=" + (x.anyLike || ""));
    }

    getById(id: string) {
        return this.httpService.get(this.api + id)
    }

    create(storage: Storage) {
        return this.httpService.create(this.api, storage)
    }

    update(storage: Storage) {
        return this.httpService.update(this.api + storage.id, storage)
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
