import * as tslib_1 from "tslib";
import { removeInvalidCharacters } from "@core/helpers/app.helpers";
import { Injectable } from "@angular/core";
var ListService = /** @class */ (function () {
    function ListService() {
    }
    ListService.prototype.sort = function (lists, field, sortField) {
        var _this = this;
        debugger;
        if (field.substring(1) === sortField.substring(1) && sortField.substring(0, 1) === "-") {
            field = "-" + field;
            field = field.replace("-+", "-");
            field = field.replace("--", "+");
        }
        field = field.replace("+-", "-");
        field = field.replace("++", "+");
        sortField = field;
        var type = field.substring(0, 1) + "any";
        field = field.replace("+", "").replace("-", "");
        if (field.substring(0, 1) === "#")
            type = type.replace("any", "string");
        if (field.substring(0, 1) === "$")
            type = type.replace("any", "date");
        field = field.replace("#", "").replace("$", "");
        var fieldStartAt = 0;
        var fieldLength = 0;
        fieldStartAt = Number(field.split("^")[1]);
        fieldLength = Number(field.split("^")[2]);
        field = field.split("^")[0];
        var fields = field.split(".");
        lists = lists.sort(function (a, b) {
            switch (type + fields.length) {
                case "+string1": return _this.setSortString(a[field], fieldStartAt) - _this.setSortString(b[field], fieldStartAt);
                case "-string1": return _this.setSortString(b[field], fieldStartAt) - _this.setSortString(a[field], fieldStartAt);
                case "+string2": return _this.setSortString(a[fields[0]][fields[1]], fieldStartAt) - _this.setSortString(b[fields[0]][fields[1]], fieldStartAt);
                case "-string2": return _this.setSortString(b[fields[0]][fields[1]], fieldStartAt) - _this.setSortString(a[fields[0]][fields[1]], fieldStartAt);
                case "+string3": return _this.setSortString(a[fields[0]][fields[1]][fields[2]], fieldStartAt) - _this.setSortString(b[fields[0]][fields[1]][fields[2]], fieldStartAt);
                case "-string3": return _this.setSortString(b[fields[0]][fields[1]][fields[2]], fieldStartAt) - _this.setSortString(a[fields[0]][fields[1]][fields[2]], fieldStartAt);
                case "+date1": return _this.setSortDate(a[field]) - _this.setSortDate(b[field]);
                case "-date1": return _this.setSortDate(b[field]) - _this.setSortDate(a[field]);
                case "+date2": return _this.setSortDate(a[fields[0]][fields[1]]) - _this.setSortDate(b[fields[0]][fields[1]]);
                case "-date2": return _this.setSortDate(b[fields[0]][fields[1]]) - _this.setSortDate(a[fields[0]][fields[1]]);
                case "+date3": return _this.setSortDate(a[fields[0]][fields[1]][fields[2]]) - _this.setSortDate(b[fields[0]][fields[1]][fields[2]]);
                case "-date3": return _this.setSortDate(b[fields[0]][fields[1]][fields[2]]) - _this.setSortDate(a[fields[0]][fields[1]][fields[2]]);
                case "+any1": return _this.setSortAny(a[field], fieldStartAt) - _this.setSortAny(b[field], fieldStartAt);
                case "-any1": return _this.setSortAny(b[field], fieldStartAt) - _this.setSortAny(a[field], fieldStartAt);
                case "+any2": return _this.setSortAny(a[fields[0]][fields[1]], fieldStartAt) - _this.setSortAny(b[fields[0]][fields[1]], fieldStartAt);
                case "-any2": return _this.setSortAny(b[fields[0]][fields[1]], fieldStartAt) - _this.setSortAny(a[fields[0]][fields[1]], fieldStartAt);
                case "+any3": return _this.setSortAny(a[fields[0]][fields[1]][fields[2]], fieldStartAt) - _this.setSortAny(b[fields[0]][fields[1]][fields[2]], fieldStartAt);
                case "-any3": return _this.setSortAny(b[fields[0]][fields[1]][fields[2]], fieldStartAt) - _this.setSortAny(a[fields[0]][fields[1]][fields[2]], fieldStartAt);
                default: return _this.setSortAny(a[field], fieldStartAt) - _this.setSortAny(b[field], fieldStartAt);
            }
        });
        return sortField;
    };
    ListService.prototype.filter = function (items, filterField, filterString) {
        debugger;
        if (items) {
            var fields_1 = filterField.split(".");
            switch (fields_1.length) {
                case 1:
                    return items.filter(function (item) { return new RegExp(removeInvalidCharacters(filterString)).test(item[fields_1[0]]); });
                case 2:
                    return items.filter(function (item) { return new RegExp(removeInvalidCharacters(filterString)).test(item[fields_1[0]][fields_1[1]]); });
                case 3:
                    return items.filter(function (item) { return new RegExp(removeInvalidCharacters(filterString)).test(item[fields_1[0]][fields_1[1]][fields_1[2]]); });
                default:
                    return items.filter(function (item) { return new RegExp(removeInvalidCharacters(filterString)).test(item[fields_1[0]]); });
            }
        }
    };
    ListService.prototype.setSortString = function (s, start) {
        debugger;
        if (start)
            s = s.toString().substring(start);
        var r = "";
        var r1 = "00000";
        var r2 = "00000";
        var r3 = "00000";
        if (s.length > 0) {
            r1 = s.charCodeAt(0).toString();
            while (r1.length < 5)
                r1 = "0" + r1;
        }
        if (s.length > 1) {
            r2 = s.charCodeAt(1).toString();
            while (r2.length < 5)
                r2 = "0" + r2;
        }
        if (s.length > 2) {
            r3 = s.charCodeAt(2).toString();
            while (r3.length < 5)
                r3 = "0" + r3;
        }
        r = s.length.toString();
        while (r.length < 3)
            r = "0" + r;
        return Number(r + r1 + r2 + r3);
    };
    ListService.prototype.setSortAny = function (s, start) {
        if (start)
            s = s.toString().substring(start);
        return s;
    };
    ListService.prototype.setSortDate = function (s) {
        s = s.replace("年", "/").replace("月", "/").replace("日", "");
        s = new Date((Number(s.split("/")[0]) + 1911).toString() + "/" + s.split("/")[1] + "/" + s.split("/")[2]).getDate().toString();
        return Number(s);
    };
    ListService = tslib_1.__decorate([
        Injectable(),
        tslib_1.__metadata("design:paramtypes", [])
    ], ListService);
    return ListService;
}());
export { ListService };
//# sourceMappingURL=list.service.js.map