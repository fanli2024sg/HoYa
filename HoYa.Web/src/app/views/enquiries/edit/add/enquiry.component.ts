import { Component, OnInit, Inject } from "@angular/core";
import { MaterialsService } from "services/materials.service";
import { Material } from "entities/material";
import { Enquiry } from "entities/enquiry";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
@Component({
    selector: "enquiry",
    templateUrl: "./enquiry.component.html",
    providers: [MaterialsService],
    styleUrls: ["enquiry.component.css"]
})
export class EnquiryComponent implements OnInit {
    withRefresh = false;
    filteredMaterials$: Observable<Material[]>;
    private anyLike$ = new Subject<string>();
    constructor(
        private dialogRef: MatDialogRef<EnquiryComponent>,
        private materialsService: MaterialsService,
        @Inject(MAT_DIALOG_DATA) public enquiry: Enquiry
    ) {
        //this.selectedMaterial = new Material();
    }
    displayMaterial(material?: Material): string | undefined {
        return material ? material.value : undefined;
    }

    ngOnInit() {
       
        this.filteredMaterials$ = this.anyLike$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            switchMap(anyLike => this.materialsService.search(anyLike, this.withRefresh))
        );
    }
    search(anyLike: string) {
        this.anyLike$.next(anyLike);
    }
    selectMaterial(material:Material) {
        this.enquiry.material = material;
        this.enquiry.materialId = material.id;
    }

}
