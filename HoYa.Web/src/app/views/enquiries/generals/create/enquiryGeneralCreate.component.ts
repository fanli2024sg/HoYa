import { Component } from "@angular/core";
import { Process } from "entities/process";
import { EnquiryGeneral } from "entities/enquiry";
import { FormGroup, FormBuilder } from "@angular/forms";
import { Router } from "@angular/router";
import { EnquiriesService } from "services/enquiries.service";
import { MatSnackBar } from '@angular/material';

@Component({
    selector: "enquiryGeneralCreate",
    templateUrl: "enquiryGeneralCreate.component.html",
    styleUrls: ["enquiryGeneralCreate.component.css"],
    providers: [EnquiriesService]
})

export class EnquiryGeneralCreateComponent {
    enquiryGeneral: FormGroup;
    constructor(
        private snackBar: MatSnackBar,
        public enquiriesService: EnquiriesService,
        public router: Router,
        public formBuilder: FormBuilder) {
        this.enquiryGeneral = formBuilder.group({
            "customerName": "",
            "contactValue": "",
            "contactPerson": "",
            "view": ""
        });
    }

    createGeneral(enquiryGeneral: EnquiryGeneral) {
        enquiryGeneral.process = new Process("952a14ad-d72d-41e8-a863-13398f29fe08", "28ae3c46-8156-4963-bbf3-4aca6317e103");
        this.enquiriesService.createGeneral(enquiryGeneral).subscribe((createdEnquiryGeneral: EnquiryGeneral) => {
            this.openSnackBar("儲存成功", "萬德佛!ಥ◡ಥ");
            this.router.navigate(["./views/enquiries/list"]);
        });
    }
     
    openSnackBar(message: string, action: string) {
        this.snackBar.open(message, action, {
            duration: 5000,
        });
    }
}
