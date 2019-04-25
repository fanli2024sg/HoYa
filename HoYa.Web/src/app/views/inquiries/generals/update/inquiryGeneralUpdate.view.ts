import { AfterViewChecked, Component, OnInit, ViewChild } from "@angular/core";
import { MatIconRegistry, MatRipple, Sort, PageEvent, MatTableDataSource } from "@angular/material";
import { DomSanitizer } from "@angular/platform-browser";
import { AfterViewInit } from "@angular/core/src/metadata/lifecycle_hooks";
import { Router, ActivatedRoute, ParamMap } from "@angular/router";

import { FormGroup, FormBuilder, Validators, FormControl } from "@angular/forms";
import { InquiriesService } from "services/inquiries.service";
import { ContactsService } from "services/contacts.service";
import { PeopleService } from "services/people.service";
import { InquiryGeneralsService } from "services/inquiryGenerals.service";
import { RecipesService } from "services/recipes.service";
import { ProfilesService } from "services/profiles.service";
import { Inquiry, InquiryGeneral } from "entities/inquiry";
import { Recipe } from "entities/item";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { InquiryAppendComponent } from "../../append/inquiryAppend.component";
import { ProfileAppendDialog } from "../../../profiles/append/profileAppend.dialog";
import { MatDialog, MatDialogRef } from "@angular/material";
import { MatSnackBar } from "@angular/material";
import { Contact, Person } from "entities/person";
import { AppInterface } from "interfaces/app.interface";
@Component({
    selector: "inquiryGeneralUpdate",
    templateUrl: "inquiryGeneralUpdate.view.html",
    styleUrls: ["inquiryGeneralUpdate.view.css"],
    providers: [
        InquiryGeneralsService,
        InquiriesService,
        RecipesService,
        ProfilesService,
        PeopleService,
        ContactsService
    ]
})
export class InquiryGeneralUpdateView implements OnInit {
    @ViewChild(MatRipple) ripple: MatRipple;
    inquiry: Inquiry;

    inquiriesDataSource = new MatTableDataSource<Inquiry>();
    inquiryGeneral: InquiryGeneral;
    currentPage: PageEvent;
    currentSort: Sort;
    recipes: Recipe[];
    withRefresh = false;
    filteredRecipes$: Observable<Recipe[]>;
    recipeFormGroup: FormGroup;
    private anyLike$ = new Subject<string>();
    deleteInquiryIds = new Array<string>();
    constructor(
        private matSnackBar: MatSnackBar,
        private route: ActivatedRoute,
        public router: Router,
        public formBuilder: FormBuilder,
        private inquiriesService: InquiriesService,
        private inquiryGeneralsService: InquiryGeneralsService,
        public profilesService: ProfilesService,
        public peopleService: PeopleService,
        public contactsService: ContactsService,
        public dialog: MatDialog,
        public appService: AppInterface
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
        if (sortInfo.active === "recipeValue") {
            sortInfo.active = "recipeValue";
        }
        this.currentSort = sortInfo;
    }

    profileAppendDialog() {/*
        let profile = new Profile();

        profile.definitionBranch.givenName = this.inquiryGeneral.customerName;
        
        this.dialog.open(ProfileAppendDialog, {
            width: "500px",
            data: this.inquiryGeneral.customer
        }).afterClosed().subscribe((profile: Profile) => {
            if (profile) {
                this.inquiryGeneral.customerName = profile.definitionBranch.value;
                this.inquiryGeneral.profile = profile;
                this.inquiryGeneral.profileId = profile.id;
            }
        });*/
    }

    inquiryAppendDialog() {
        this.dialog.open(InquiryAppendComponent, {
            width: "500px",
            data: new Inquiry(this.inquiryGeneral.id)
        }).afterClosed().subscribe((inquiry: Inquiry) => {
            if (inquiry.recipeId) {
                let inquiries = this.inquiriesDataSource.data;
                inquiries.push(inquiry);
                this.inquiriesDataSource.data = inquiries;
            }
        });
    }
    ngOnInit() {
        this.appService.title$.next("詢價細節");
        this.appService.leftIcon$.next("arrow_back");
        this.dialog.afterAllClosed.subscribe(() => {
            console.log("no dialog");
        });

        this.dialog.afterOpen.subscribe((dialogRef: MatDialogRef<any>) => {
            console.log(`new dialog:${dialogRef.id}`);
            console.log(`has ${this.dialog.openDialogs.length} dialogs`);
        });

        this.inquiryGeneralsService.find(this.route.snapshot.paramMap.get("id")).subscribe((inquiryGeneral: InquiryGeneral) => {
            this.inquiryGeneral = inquiryGeneral;
            this.appService.title$.next(this.inquiryGeneral.owner.no);
            this.inquiriesService.select({ ownerId: this.inquiryGeneral.id }, false).subscribe((inquiries: Inquiry[]) => {

                this.inquiriesDataSource.data = inquiries;
            });
        });
    }

    removeInquiry(id: string) {
        this.deleteInquiryIds.push(id);
        let inquiries = this.inquiriesDataSource.data;
        inquiries = inquiries.filter(x => x.id !== id);
        this.inquiriesDataSource.data = inquiries;
    }

    deleteInquiry(id: string): Promise<any> {
        return new Promise((resolve) => {
            this.inquiriesService.delete(id).subscribe(() => {
                resolve();

            });
        });
    }

    createInquiry(inquiry: Inquiry): Promise<Inquiry> {
        return new Promise((resolve) => {
            this.inquiriesService.create(inquiry).subscribe((createdInquiry: Inquiry) => {
                resolve(createdInquiry);
            });
        });
    }

    updatePerson(person: Person): Promise<Person> {
        return new Promise((resolve) => {
            this.peopleService.update(person.id,person).subscribe((updatedPerson: Person) => {
                resolve(updatedPerson);
            });
        });
    }

    updateContact(contact: Contact): Promise<Contact> {
        return new Promise((resolve) => {
            this.contactsService.update(contact.id, contact).subscribe((createdContact: Contact) => {
                resolve(createdContact);
            });
        });
    }

    save() {
        let ps: Promise<any>[] = new Array<Promise<any>>();
        ps.push();
        this.inquiriesDataSource.data.filter(x => x.createdDate === null).forEach((inquiry: Inquiry) => {
            ps.push(this.createInquiry(inquiry));
        });
        this.deleteInquiryIds.forEach((inquiryId: string) => {
            ps.push(this.deleteInquiry(inquiryId));
        });
        ps.push(this.updatePerson(this.inquiryGeneral.contact.owner));
        ps.push(this.updatePerson(this.inquiryGeneral.customer));
        ps.push(this.updateContact(this.inquiryGeneral.contact));
        Promise.all(ps).then(() => {
            this.inquiryGeneralsService.update(this.inquiryGeneral.id, this.inquiryGeneral).subscribe((updatedInquiryGeneral: InquiryGeneral) => {
                this.matSnackBar.open("儲存成功", "萬德佛!ಥ◡ಥ", {
                    duration: 5000,
                });
                this.router.navigate(["./views/inquiries/generals"]);
            });
        });
    }
}
