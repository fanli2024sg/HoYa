import { InquiryGeneral } from "entities/inquiry";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from "rxjs";
import { Query } from "models/query";
@Injectable()
export class InquiryGeneralsService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/InquiryGenerals";
    }

    find(id: string) {
        return this.httpService.select<InquiryGeneral>(`${this.api}/${id}`);
    }
    update(id: string, inquiryGeneral: InquiryGeneral): Observable<InquiryGeneral> {
        return this.httpService.update(`${this.api}/${id}`, inquiryGeneral);
    }
    create(inquiryGeneral: InquiryGeneral): Observable<InquiryGeneral> {
        return this.httpService.create(`${this.api}`, inquiryGeneral);
    }
    select(params: any, withRefresh: boolean): Observable<Query<InquiryGeneral>> {
        return this.httpService.select<Query<InquiryGeneral>>(`${this.api}`, params, withRefresh);
    }

    filter(params: any, withRefresh: boolean): Observable<Query<InquiryGeneral>> {
        return this.httpService.select<Query<InquiryGeneral>>(`${this.api}`, params, withRefresh);
    }
}
