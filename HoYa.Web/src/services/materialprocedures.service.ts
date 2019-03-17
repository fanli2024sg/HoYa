import { MaterialProcedure } from "entities/material";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
@Injectable()
export class MaterialProceduresService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/MaterialProcedures/";
    }

    get(x: any) {
        return this.httpService.get(this.api + "By?anyLike=" + x.anyLike);
    }

    create(materialProcedure: MaterialProcedure) {
        return this.httpService.create(this.api, materialProcedure);
    }

    update(id: string, materialProcedure: MaterialProcedure) {
        return this.httpService.update(this.api + id, materialProcedure);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
