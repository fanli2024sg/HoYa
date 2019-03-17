import { Option } from "entities/entity";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
@Injectable()
export class OptionsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Options/";
    }
    get(x: any) {
        return this.httpService.get(this.api + "By?parentId=" + x.parentId +
            "&anyLike="+x.anyLike);
    }
    getByGroupValue(groupValue: string) {
        return this.httpService.get(this.api + "ByGroupValue?Value=" + groupValue);
    }

    find(id: string) {
        return this.httpService.get(this.api + id);
    }

    create(option: Option) {
        return this.httpService.create(this.api, option);
    }

    update(option: Option) {
        return this.httpService.update(this.api + option.id, option);
    } 

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
