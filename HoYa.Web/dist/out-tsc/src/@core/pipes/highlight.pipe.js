import * as tslib_1 from "tslib";
import { Pipe } from '@angular/core';
var HighLightPipe = /** @class */ (function () {
    function HighLightPipe() {
    }
    HighLightPipe.prototype.transform = function (text, search) {
        return (search && text) ? text.replace(search, "<span class=\"highlight2\">" + search + "</span>") : text;
    };
    HighLightPipe = tslib_1.__decorate([
        Pipe({ name: 'highlight' })
    ], HighLightPipe);
    return HighLightPipe;
}());
export { HighLightPipe };
//# sourceMappingURL=highlight.pipe.js.map