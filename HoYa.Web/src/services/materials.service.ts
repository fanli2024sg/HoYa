import { Material } from "entities/material";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
@Injectable()
export class MaterialsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Materials/";
    }

    get(x: any) {
        return this.httpService.get(this.api + "By?anyLike=" + x.anyLike);
    }

    create(material: Material) {
        return this.httpService.create(this.api, material);
    }

    update(id: string, material: Material) {
        return this.httpService.update(this.api + id, material);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
