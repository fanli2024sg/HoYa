import { Component, OnInit } from "@angular/core";
import { Sort, PageEvent } from "@angular/material";
import { Router, ActivatedRoute } from "@angular/router";
import { OptionsService } from "services/options.service";
import { MaterialsService } from "services/materials.service";
import { ProfilesService } from "services/profiles.service";
import { Option } from "entities/entity";
import { MatDialog, MatDialogRef } from "@angular/material";
import { MatSnackBar } from "@angular/material";
@Component({
    selector: "optionUpdate",
    templateUrl: "optionUpdate.component.html",
    styleUrls: ["optionUpdate.component.css"],
    providers: [
        OptionsService
    ]
})
export class OptionUpdateComponent implements OnInit {
    option: Option;
    currentPage: PageEvent;
    currentSort: Sort;
    constructor(
        private matSnackBar: MatSnackBar,
        private route: ActivatedRoute,
        public router: Router,
        private optionsService: OptionsService,
        public dialog: MatDialog
    ) {
        this.currentPage = {
            pageIndex: 0,
            pageSize: 10,
            length: null
        };
        this.currentSort = {
            active: "",
            direction: ""
        };
    }
    changeSort(sortInfo: Sort) {
        if (sortInfo.active === "materialValue") {
            sortInfo.active = "materialValue";
        }
        this.currentSort = sortInfo;
    }

    ngOnInit() {
        this.dialog.afterAllClosed.subscribe(() => {
            console.log("no dialog");
        });

        this.dialog.afterOpen.subscribe((dialogRef: MatDialogRef<any>) => {
            console.log(`new dialog:${dialogRef.id}`);
            console.log(`has ${this.dialog.openDialogs.length} dialogs`);
        });

        this.optionsService.find(this.route.snapshot.paramMap.get("id")).subscribe((option: Option) => {
            this.option = option;
        });
    }

    createOption(option: Option) {
        this.optionsService.create(option).subscribe((createdOption: Option) => {
            this.matSnackBar.open(`${createdOption.value}新增成功!`, "萬德佛!ಥ◡ಥ", {
                duration: 5000,
            });
        });
    }
}
