import { Component, OnInit, Inject } from "@angular/core";
import { InquiryGeneralsService } from "services/inquiryGenerals.service";
import { InquiryGeneral } from "entities/inquiry";
import { Profile } from "entities/person";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
import { MatSnackBar } from "@angular/material";
@Component({
    selector: "inquiryGeneralCreate",
    templateUrl: "inquiryGeneralCreate.dialog.html",
    styleUrls: ["inquiryGeneralCreate.dialog.css"],
    providers: [
        InquiryGeneralsService
    ]
})
export class InquiryGeneralCreateDialog {
    withRefresh = false;
    filteredWorkFlows$: Observable<InquiryGeneral[]>;
    private anyLike$ = new Subject<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private inquiryGeneralsService: InquiryGeneralsService,
        private dialogRef: MatDialogRef<InquiryGeneralCreateDialog>,
        @Inject(MAT_DIALOG_DATA) public inquiryGeneral: InquiryGeneral
    ) {

    }

    ngOnInit() {

    }
    search(anyLike: string) {
        this.anyLike$.next(anyLike);
    }

    create() {
        this.inquiryGeneralsService.create(this.inquiryGeneral).subscribe((createdInquiryGeneral: InquiryGeneral) => {
            this.matSnackBar.open(`${createdInquiryGeneral.value} 新增成功`, "萬德佛!ಥ◡ಥ", {
                duration: 5000,
            });
        });
    }
}
