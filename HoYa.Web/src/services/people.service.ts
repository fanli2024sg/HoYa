import { PersonOption, PersonModel } from "models/person";
import { Injectable } from "@angular/core";
import { Response } from "@angular/http";
import { HttpService } from "@core/services/http.service";
@Injectable()
export class PeopleService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/People/";
    }

    get() {
        return this.httpService.get(this.api);
    }

    getBy(x: any) {
        return this.httpService.get(this.api + "By?ParentId=" + x.parentId);
    }

    findByDocumentNo(documentNo: string) {
        return this.httpService.get(this.api + "By?documentNo=" + documentNo);
    }

    findByEmployeeNo(employeeNo: string) {
        return this.httpService.get(this.api + "By?employeeNo=" + employeeNo);
    }

    getOptions(x: any) {
        return this.httpService.get(this.api +
            "Option?typeId=e979aefd-385c-4445-9844-6e151ee141a1" +
            "&anyLike=" + (x.anyLike ? x.anyLike : "") +
            "&pageSize=200"
        );
    }

    getById(id: string) {
        return this.httpService.get(this.api + id);
    }

    create(option: PersonModel) {
        return this.httpService.create(this.api, option);
    }

    update(id: string, option: PersonModel) {
        return this.httpService.update(this.api + id, option);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
