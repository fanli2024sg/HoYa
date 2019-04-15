import * as tslib_1 from "tslib";
import { Injectable } from '@angular/core';
/** Pass untouched request through to the next request handler. */
var NoopInterceptor = /** @class */ (function () {
    function NoopInterceptor() {
    }
    NoopInterceptor.prototype.intercept = function (req, next) {
        return next.handle(req);
    };
    NoopInterceptor = tslib_1.__decorate([
        Injectable()
    ], NoopInterceptor);
    return NoopInterceptor;
}());
export { NoopInterceptor };
//# sourceMappingURL=noop.interceptor.js.map