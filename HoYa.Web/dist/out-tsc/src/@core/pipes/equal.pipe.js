import * as tslib_1 from "tslib";
import { Pipe } from "@angular/core";
var EqualPipe = /** @class */ (function () {
    function EqualPipe() {
    }
    EqualPipe.prototype.transform = function (options, conditions) {
        return options.filter(function (option) {
            for (var field in conditions) {
                if (option[field] !== conditions[field]) {
                    return false;
                }
            }
            return true;
        });
    };
    EqualPipe = tslib_1.__decorate([
        Pipe({
            name: "equal",
            pure: false
        })
    ], EqualPipe);
    return EqualPipe;
}());
export { EqualPipe };
//# sourceMappingURL=equal.pipe.js.map