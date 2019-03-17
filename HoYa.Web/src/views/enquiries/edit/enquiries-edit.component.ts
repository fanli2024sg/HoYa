import { AfterViewChecked, Component, OnInit, ViewChild } from "@angular/core";
import { MatIconRegistry, MatRipple } from "@angular/material";
import { DomSanitizer } from "@angular/platform-browser";
import { AfterViewInit } from "@angular/core/src/metadata/lifecycle_hooks";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";
import { switchMap } from "rxjs/operators";
import { EnquiriesService } from "services/enquiries.service";
import { Enquiry } from "entities/enquiry";
@Component({
    selector: "enquiries-edit",
    templateUrl: "./enquiries-edit.component.html",
    providers: [EnquiriesService],
    styleUrls: ["./enquiries-edit.component.css"]
})
export class EnquiriesEditComponent implements OnInit {
    @ViewChild(MatRipple) ripple: MatRipple;
    enquiry: Enquiry;
    hero$: any;
    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private matIconRegistry: MatIconRegistry,
        private domSanitizer: DomSanitizer,
        private enquiriesService: EnquiriesService
    ) {
        debugger
        console.log("id:");
        console.log(this.route.snapshot.paramMap.get("id"));
    }

    ngOnInit() {

        console.log(this.route.snapshot.paramMap.get("id"));

        this.matIconRegistry.addSvgIconInNamespace(
            "custom-svg",
            "angular",
            this.domSanitizer.bypassSecurityTrustResourceUrl("assets/images/angular_solidBlack.svg")
        );

        this.matIconRegistry.registerFontClassAlias("fontawesome", "fa");
    }

    triggerRipple() {
        const point1 = this.ripple.launch(0, 0, { color: "pink", centered: true, persistent: true, radius: 50 });
        const point2 = this.ripple.launch(0, 0, { color: "yellow", centered: true, persistent: true, radius: 20 });

        setTimeout(() => {
            point1.fadeOut();
        }, 500);
    }

    clearRipple() {
        this.ripple.fadeOutAll();
    }
}
