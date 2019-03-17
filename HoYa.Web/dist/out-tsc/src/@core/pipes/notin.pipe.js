import * as tslib_1 from "tslib";
import { Pipe } from "@angular/core";
var NotInPipe = /** @class */ (function () {
    function NotInPipe() {
    }
    NotInPipe.prototype.transform = function (options, conditions) {
        return options.filter(function (option) {
            for (var field in conditions) {
                if (option[field] === conditions[field]) {
                    return false;
                }
            }
            return true;
        });
    };
    NotInPipe = tslib_1.__decorate([
        Pipe({
            name: "notin",
            pure: false
        })
    ], NotInPipe);
    return NotInPipe;
}());
export { NotInPipe };
//# sourceMappingURL=notin.pipe.js.map