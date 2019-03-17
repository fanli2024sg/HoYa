import { Quotation ,QuotationGeneral } from "entities/quotation";
import { Injectable } from "@angular/core";
;
import { HttpService } from "@core/services/http.service";
@Injectable()
export class QuotationsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Quotations/";
    }

    get(x: any) {
        return this.httpService.get(this.api + "By?ownerId=" + x.ownerId + "&enquiryId=" + x.enquiryId)
    }

    getByTypeId(typeId: number) {
        return this.httpService.get(this.api + "ByType?Id=" + typeId)
    }

    getByCodeOrValue(typeId: string, codeOrValue: string) {
       return this.httpService.get(this.api + "ByCodeOrValue?typeId=" + typeId + "&&like=" + codeOrValue)
    }

    getById(id: string) {
        return this.httpService.get(this.api + id)
    }

    create(quotation: Quotation) {
        return this.httpService.create(this.api, quotation)
    }

    createGeneral(quotationGeneral: QuotationGeneral) {
        return this.httpService.create(this.api, quotationGeneral)
    }

    updateGeneral(quotationGeneral: QuotationGeneral) {
        return this.httpService.update(this.api + quotationGeneral.id, quotationGeneral)
    }

    update(quotation: Quotation) {
        return this.httpService.update(this.api + quotation.id, quotation)
    }

    delete(id: string) {
        return this.httpService.delete(this.api + id);
    }
}
