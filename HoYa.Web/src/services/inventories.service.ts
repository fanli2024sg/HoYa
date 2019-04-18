import { Inventory } from "entities/inventory";
import { Injectable } from "@angular/core";
;
import { HttpService } from "services/http.service";

@Injectable()
export class InventoriesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Inventories/";
    }

    get(x: any) {
        return this.httpService.select(this.api)
    }

    getByProfileId(profileId: number) {
        return this.httpService.select(this.api + "ByProfile?Id=" + profileId)
    }

    getById(id: string) {
        return this.httpService.select(`${this.api}/${id}`)
    }

    create(inventory: Inventory) {
        return this.httpService.create(this.api, inventory)
    }

    update(inventory:Inventory) {
        return this.httpService.update(this.api + inventory.id, inventory)
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
