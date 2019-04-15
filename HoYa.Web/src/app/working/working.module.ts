import { NgModule } from "@angular/core";
import { CommonModule } from '@angular/common';
import { WorkingComponent } from './working.component';
import { FormsModule } from '@angular/forms';
import { WorkingRoutingModule } from './working.routing.module';

@NgModule({
    imports: [
        FormsModule,
        CommonModule,
        WorkingRoutingModule
    ],
    declarations: [WorkingComponent]
})
export class WorkingModule { }
