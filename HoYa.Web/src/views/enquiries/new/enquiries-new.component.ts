import { Component } from "@angular/core";
import { Process } from "entities/process";
import { EnquiryGeneral } from "entities/enquiry";
import { FormGroup, FormBuilder, Validators } from "@angular/forms";
import { Router } from "@angular/router";
import { EnquiriesService } from "services/enquiries.service";

@Component({
    selector: "enquiries-new",
    templateUrl: "enquiries-new.component.html",
    styleUrls: ['enquiries-new.component.css'],
    providers: [EnquiriesService]
})

export class EnquiriesNewComponent {
    enquiryGeneral: FormGroup;
    constructor(
        public enquiriesService: EnquiriesService,
        public router: Router,
        public formBuilder: FormBuilder) {
        this.enquiryGeneral = formBuilder.group({ 
            "customerName": "",
            "contactValue": "",
            "contactPerson": "",
            "content": ""
        });
    }

    createGeneral(enquiryGeneral: EnquiryGeneral) {
        enquiryGeneral.process = new Process();
        enquiryGeneral.process.typeId = "e0691faf-7c45-4400-abb1-48b9be24bc34";

        this.enquiriesService.createGeneral(enquiryGeneral).subscribe((createdEnquiryGeneral: EnquiryGeneral) => {
            console.log(createdEnquiryGeneral);
        });
    }
}
