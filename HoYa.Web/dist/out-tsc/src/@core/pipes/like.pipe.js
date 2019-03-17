import * as tslib_1 from "tslib";
import { Pipe } from "@angular/core";
var LikePipe = /** @class */ (function () {
    function LikePipe() {
    }
    LikePipe.prototype.transform = function (options, conditions) {
        return options.filter(function (option) {
            for (var field in conditions) {
                if (!RegExp(conditions[field]).test(option[field]))
                    return false;
            }
            return true;
        });
    };
    LikePipe = tslib_1.__decorate([
        Pipe({
            name: 'like',
            pure: false
        })
    ], LikePipe);
    return LikePipe;
}());
export { LikePipe };
//# sourceMappingURL=like.pipe.js.map