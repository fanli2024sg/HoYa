import { Inquiry, InquiryGeneral } from "entities/inquiry";
import { Injectable } from "@angular/core";
import { HttpService } from "services/http.service";
import { Observable } from "rxjs";
import { Query } from "models/query";
@Injectable()
export class InquiriesService {
    private api: string;
    constructor(private httpService: HttpService) {
        this.api = "api/Inquiries";
    }

    select(params: any, withRefresh: boolean): Observable<Inquiry[]> {
        return this.httpService.select<Inquiry[]>(this.api, params, withRefresh);
    }

    create(inquiry: Inquiry): Observable<Inquiry> {
        return this.httpService.create(this.api, inquiry);
    }
  
    update(id: string, inquiry: Inquiry): Observable<Inquiry> {
        return this.httpService.update(`${this.api}/${id}`, inquiry);
    }

    delete(id: string): Observable<any> {
        return this.httpService.delete(`${this.api}/${id}`);
    }
}
