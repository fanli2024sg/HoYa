import { AfterViewChecked, Component, OnInit, ViewChild } from "@angular/core";
import { MatIconRegistry, MatRipple, Sort, PageEvent, MatTableDataSource } from "@angular/material";
import { DomSanitizer } from "@angular/platform-browser";
import { AfterViewInit } from "@angular/core/src/metadata/lifecycle_hooks";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";
import { EnquiriesService } from "services/enquiries.service";
import { MaterialsService } from "services/materials.service";
import { ProfilesService } from "services/profiles.service";
import { Enquiry, EnquiryGeneral } from "entities/enquiry";
import { Material } from "entities/material";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { EnquiryAppendComponent } from "../../append/enquiryAppend.component";
import { ProfileCreateComponent } from "../../../profiles/create/profileCreate.component";
import { MatDialog, MatDialogRef } from "@angular/material";
import { MatSnackBar } from "@angular/material";
import { Profile } from "entities/person";
@Component({
    selector: "enquiryGeneralUpdate",
    templateUrl: "enquiryGeneralUpdate.component.html",
    providers: [
        EnquiriesService,
        MaterialsService,
        ProfilesService
    ],
    styleUrls: ["enquiryGeneralUpdate.component.css"]
})
export class EnquiryGeneralUpdateComponent implements OnInit {
    @ViewChild(MatRipple) ripple: MatRipple;
    enquiry: Enquiry;

    enquiriesDataSource = new MatTableDataSource<Enquiry>();
    enquiryGeneral: EnquiryGeneral;
    currentPage: PageEvent;
    currentSort: Sort;
    materials: Material[];
    withRefresh = false;
    filteredMaterials$: Observable<Material[]>;
    materialFormGroup: FormGroup;
    private anyLike$ = new Subject<string>();
    deleteEnquiryIds = new Array<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private route: ActivatedRoute,
        public router: Router,
        public formBuilder: FormBuilder,
        private enquiriesService: EnquiriesService,
        private profilesService: ProfilesService,
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

    profileCreateDialog() {
        let profile = new Profile();
        
        profile.definition.givenName = this.enquiryGeneral.customerName;

        this.dialog.open(ProfileCreateComponent, {
            width: "250px",
            data: this.enquiryGeneral.profile
        }).afterClosed().subscribe((profile: Profile) => {
            this.enquiryGeneral.customerName = profile.definition.value;
            this.enquiryGeneral.profile = profile;
            this.enquiryGeneral.profileId = profile.id;
        });
    }

    enquiryAppendDialog() {
        this.dialog.open(EnquiryAppendComponent, {
            width: "250px",
            data: new Enquiry(this.enquiryGeneral.id)
        }).afterClosed().subscribe((enquiry: Enquiry) => {
            if (enquiry.materialId) {
                let enquiries = this.enquiriesDataSource.data;
                enquiries.push(enquiry);
                this.enquiriesDataSource.data = enquiries;
            }
        });
    }
    ngOnInit() {
        this.dialog.afterAllClosed.subscribe(() => {
            console.log("no dialog");
        });

        this.dialog.afterOpen.subscribe((dialogRef: MatDialogRef<any>) => {
            console.log(`new dialog:${dialogRef.id}`);
            console.log(`has ${this.dialog.openDialogs.length} dialogs`);
        });

        this.enquiriesService.findGeneral(this.route.snapshot.paramMap.get("id")).subscribe((enquiryGeneral: EnquiryGeneral) => {
            this.enquiryGeneral = enquiryGeneral;
            if (!this.enquiryGeneral.profileId) {
                this.enquiryGeneral.profile = new Profile();
                this.enquiryGeneral.profile.definition.value = enquiryGeneral.customerName;
            }
            this.enquiriesService.select({ ownerId: this.enquiryGeneral.id }, false).subscribe((enquiries: Enquiry[]) => {

                this.enquiriesDataSource.data = enquiries;
            });


        });
    }

    removeEnquiry(id: string) {
        this.deleteEnquiryIds.push(id);
        let enquiries = this.enquiriesDataSource.data;
        enquiries = enquiries.filter(x => x.id !== id);
        this.enquiriesDataSource.data = enquiries;
    }

    deleteEnquiry(id: string): Promise<any> {
        return new Promise((resolve) => {
            this.enquiriesService.delete(id).subscribe(() => {
                resolve();

            });


        });
    }

    createEnquiry(enquiry: Enquiry): Promise<Enquiry> {


        return new Promise((resolve) => {
            this.enquiriesService.create(enquiry).subscribe((createdEnquiry: Enquiry) => {
                resolve(createdEnquiry);

            });


        });
    }

    updateEnquiryGeneral(enquiryGeneral: EnquiryGeneral): Promise<EnquiryGeneral> {


        return new Promise((resolve) => {
            debugger
            if (this.enquiryGeneral.profileId) {
                this.profilesService.create(this.enquiryGeneral.profile).subscribe((createdProfile: Profile) => {
                    this.enquiriesService.updateGeneral(this.enquiryGeneral.id, this.enquiryGeneral).subscribe((updatedEnquiryGeneral: EnquiryGeneral) => {
                        resolve(updatedEnquiryGeneral);
                    });
                });
            } else {
                this.enquiriesService.updateGeneral(this.enquiryGeneral.id, this.enquiryGeneral).subscribe((updatedEnquiryGeneral: EnquiryGeneral) => {
                    resolve(updatedEnquiryGeneral);
                });
            }
        });
    }

    save() {
        console.log(this.enquiryGeneral.profile.definition);
    
       


        let ps: Promise<Enquiry>[] = new Array<Promise<Enquiry>>();
        ps.push();
        this.enquiriesDataSource.data.filter(x => x.createdDate === null).forEach((enquiry: Enquiry) => {
            console.log(enquiry.createdDate);
            ps.push(this.createEnquiry(enquiry));
        });
        this.deleteEnquiryIds.forEach((enquiryId: string) => {
            ps.push(this.deleteEnquiry(enquiryId));
        });

        Promise.all(ps).then(() => {
            this.updateEnquiryGeneral(this.enquiryGeneral).then(() => {
                this.matSnackBar.open("儲存成功", "萬德佛!ಥ◡ಥ", {
                    duration: 5000,
                });
                this.router.navigate(["./views/enquiries/generals"]);
            });
        });
    }
}
