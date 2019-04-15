import { Enquiry, EnquiryGeneral } from "entities/enquiry";
import { Injectable } from "@angular/core";
import { HttpService } from "@core/services/http.service";
@Injectable()
export class EnquiriesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Enquiries/";
    }

    get(x: any) {
        return this.httpService.get(this.api + "By?OwnerId=" + (x.ownerId || ""));
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

    create(enquiry: Enquiry) {
        return this.httpService.create(this.api, enquiry);
    }
    updateGeneral(id: string, enquiryGeneral: EnquiryGeneral) {
        return this.httpService.update(this.api + "General/" + id, enquiryGeneral);
    }
    createGeneral(enquiryGeneral: EnquiryGeneral) {
console.log(enquiryGeneral);
debugger
        return this.httpService.create(this.api+"General/", enquiryGeneral);
    }
    getGeneral(x: any) {
        return this.httpService.get(this.api + "General?processId=" + x.processId);
    }
    update(id: string, enquiry: Enquiry) {
        return this.httpService.update(this.api + id, enquiry);
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
