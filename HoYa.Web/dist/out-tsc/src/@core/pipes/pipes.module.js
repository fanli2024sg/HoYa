import * as tslib_1 from "tslib";
import { NgModule } from "@angular/core";
import { LikePipe } from "./like.pipe";
import { NotInPipe } from "./notin.pipe";
import { HighLightPipe } from "./highlight.pipe";
import { EqualPipe } from "./equal.pipe";
import { OrderByPipe } from "./orderby.pipe";
var PipesModule = /** @class */ (function () {
    function PipesModule() {
    }
    PipesModule = tslib_1.__decorate([
        NgModule({
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
    ], PipesModule);
    return PipesModule;
}());
export { PipesModule };
//# sourceMappingURL=pipes.module.js.map