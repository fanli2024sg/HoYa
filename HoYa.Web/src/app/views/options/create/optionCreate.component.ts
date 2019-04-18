import { Component } from "@angular/core";
import { Process } from "entities/workflow";
import { Option } from "entities/entity";
import { FormGroup, FormBuilder } from "@angular/forms";
import { Router } from "@angular/router";
import { OptionsService } from "services/options.service";
import { MatSnackBar } from '@angular/material';

@Component({
    selector: "optionCreate",
    templateUrl: "optionCreate.component.html",
    styleUrls: ["optionCreate.component.css"],
    providers: [OptionsService]
})

export class OptionCreateComponent {
    option: Option;
    constructor(
        private matSnackBar: MatSnackBar,
        public optionsService: OptionsService,
        public router: Router) { 
    }

    create() {
        this.optionsService.create(this.option).subscribe((createdOption: Option) => {


            this.matSnackBar.open(`${createdOption.value}新增成功`, "萬德佛!ಥ◡ಥ", {
                duration: 5000,
            });
        }
        );
    }
}
