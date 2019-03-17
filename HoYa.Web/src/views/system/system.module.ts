//核心
import { NgModule } from "@angular/core";
import { CommonModule } from "@angular/common";
import { FormsModule } from "@angular/forms";
//服務
import { SystemRoutingModule } from "./system.routing.module";
//import { CoreComponentsModule } from "@core/components/corecomponents.module";
import { PipesModule } from "@core/pipes/pipes.module";
//物件
import { SystemComponent } from "./system.component";
//插件

@NgModule({
    declarations: [
        SystemComponent
    ],
    imports: [
        //CoreComponentsModule,
        SystemRoutingModule,
        FormsModule,
        CommonModule,
        PipesModule
    ],
})

export class SystemModule { }
