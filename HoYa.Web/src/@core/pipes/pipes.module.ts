import { NgModule } from "@angular/core";
import { LikePipe } from "./like.pipe";
import { NotInPipe } from "./notin.pipe";
import { HighLightPipe } from "./highlight.pipe";
import { EqualPipe } from "./equal.pipe";
import { OrderByPipe } from "./orderby.pipe";
@NgModule({
    declarations: [
        NotInPipe,
        EqualPipe,
        OrderByPipe,
        HighLightPipe,
        LikePipe
    ],
    exports: [
        NotInPipe,
        EqualPipe,
        OrderByPipe,
        HighLightPipe,
        LikePipe
    ]
})

export class PipesModule { }