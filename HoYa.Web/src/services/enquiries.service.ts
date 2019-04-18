import { Enquiry, EnquiryGeneral } from "entities/enquiry";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from "rxjs";
import { Query } from "models/query";
@Injectable()
export class EnquiriesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Enquiries";
    }

    select(params: any, withRefresh: boolean): Observable<Enquiry[]> {
        return this.httpService.select<Enquiry[]>(this.api, params, withRefresh);
    }

    findGeneral(id: string) {
        return this.httpService.select<EnquiryGeneral>(`${this.api}/General/${id}`);
    }

    create(enquiry: Enquiry): Observable<Enquiry> {
        return this.httpService.create(this.api, enquiry);
    }
    updateGeneral(id: string, enquiryGeneral: EnquiryGeneral): Observable<EnquiryGeneral> {
        return this.httpService.update(`${this.api}/General/${id}`, enquiryGeneral);
    }
    createGeneral(enquiryGeneral: EnquiryGeneral): Observable<EnquiryGeneral> {
        return this.httpService.create(`${this.api}/General`, enquiryGeneral);
    }
    selectGeneral(params: any, withRefresh: boolean): Observable<Query<EnquiryGeneral>> {
        return this.httpService.select<Query<EnquiryGeneral>>(`${this.api}/General`, params, withRefresh);
    }
    update(id: string, enquiry: Enquiry): Observable<Enquiry> {
        return this.httpService.update(`${this.api}/${id}`, enquiry);
    }

    delete(id: string): Observable<any> {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
