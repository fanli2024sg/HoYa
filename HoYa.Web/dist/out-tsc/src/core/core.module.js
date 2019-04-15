import * as tslib_1 from "tslib";
import { NgModule } from '@angular/core';
import { FlexLayoutModule } from "@angular/flex-layout";
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { MatIconModule, MatButtonModule, MatButtonToggleModule, MatRippleModule, MatSidenavModule, MatToolbarModule, MatListModule, MatMenuModule, MatStepperModule, MatFormFieldModule, MatInputModule, MatAutocompleteModule, MatDatepickerModule, MatSortModule, MatTableModule, MatPaginatorModule, MAT_DATE_LOCALE, MAT_DATE_FORMATS, MatSelectModule, MatCheckboxModule, MatRadioModule, MatSlideToggleModule, MatSliderModule, MatGridListModule, MatCardModule, MatProgressBarModule, MatProgressSpinnerModule, MatDialogModule } from '@angular/material';
import { MatFileUploadModule } from "angular-material-fileupload";
import { MatMomentDateModule } from '@angular/material-moment-adapter';
export var TW_FORMATS = {
    parse: {
        dateInput: 'YYYY/MM/DD'
    },
    display: {
        dateInput: 'YYYY/MM/DD',
        monthYearLabel: 'YYYY MMM',
        dateA11yLabel: 'YYYY/MM/DD',
        monthYearA11yLabel: 'YYYY MMM'
    }
};
var CoreModule = /** @class */ (function () {
    function CoreModule() {
    }
    CoreModule = tslib_1.__decorate([
        NgModule({
            exports: [
                FormsModule,
                MatSortModule,
                MatTableModule,
                MatPaginatorModule,
                CommonModule,
                FlexLayoutModule,
                MatButtonModule,
                MatIconModule,
                MatButtonModule,
                MatButtonToggleModule,
                MatRippleModule,
                MatSidenavModule,
                MatToolbarModule,
                MatListModule,
                MatMenuModule,
                MatStepperModule,
                MatFormFieldModule,
                MatInputModule,
                MatAutocompleteModule,
                MatDatepickerModule,
                MatMomentDateModule,
                MatSelectModule,
                MatCheckboxModule,
                MatRadioModule,
                MatSlideToggleModule,
                MatSliderModule,
                MatGridListModule,
                MatCardModule,
                MatProgressBarModule,
                MatProgressSpinnerModule,
                MatDialogModule,
                MatFileUploadModule
            ],
            providers: [
                { provide: MAT_DATE_LOCALE, useValue: 'zh-TW' },
                { provide: MAT_DATE_FORMATS, useValue: TW_FORMATS }
            ]
        })
    ], CoreModule);
    return CoreModule;
}());
export { CoreModule };
//# sourceMappingURL=core.module.js.map