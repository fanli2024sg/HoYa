import { Process } from "entities/workflow";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
@Injectable()
export class ProcessesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Processes/";
    }

    get(x) {
        return this.httpService.select(this.api);
    }

    getNo(x) {
        return this.httpService.select(this.api + "No?typeId=" + x.typeId);
    }

    find(id: string) {
        return this.httpService.select(`${this.api}/${id}`);
    }

    create(process: Process) {
        return this.httpService.create(this.api, process);
    }

    update(id: string, process: Process) {
        return this.httpService.update(`${this.api}/${id}`, process);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
