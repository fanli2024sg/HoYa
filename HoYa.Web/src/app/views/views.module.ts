import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
import { ViewsComponent } from "./views.component";
import { ViewsRoutingModule } from "./views-routing.module";

import { CoreModule } from "core/core.module";
import { AuthGuard } from "core/guards/auth.guard";
@NgModule({
    imports: [
        CoreModule,
        ViewsRoutingModule
    ],
    declarations: [ViewsComponent], providers: [AuthGuard]
})

export class ViewsModule { }