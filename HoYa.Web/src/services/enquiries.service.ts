import { Enquiry, EnquiryGeneral } from "entities/enquiry";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from 'rxjs';
@Injectable()
export class EnquiriesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Enquiries";
    }

    select(x: any, withRefresh: boolean): Observable<Enquiry[]> {
        // TODO: Add error handling
        return this.httpService.search<Enquiry[]>(this.api, x, withRefresh);
    }

    findGeneral(id: string) {
        return this.httpService.get(`${this.api}/General/${id}`);
    }

    create(enquiry: Enquiry): Observable<any> {
        return this.httpService.create(this.api, enquiry);
    }
    updateGeneral(id: string, enquiryGeneral: EnquiryGeneral) {
        return this.httpService.update(`${this.api}/General/${id}`, enquiryGeneral);
    }
    createGeneral(enquiryGeneral: EnquiryGeneral) {
        return this.httpService.create(`${this.api}/General`, enquiryGeneral);
    }
    selectGeneral(x: any) {
        return this.httpService.search(`${this.api}/General`, x, false);
    }
    update(id: string, enquiry: Enquiry) {
        return this.httpService.update(this.api + id, enquiry);
    }

    delete(id: string) {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
