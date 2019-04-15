import { removeInvalidCharacters } from "core/helpers/app.helpers";
import { Injectable } from "@angular/core";

@Injectable()
export class ListService {
    constructor() {
    }

    sort(lists: any[], field: string, sortField: string): string {
        debugger
        if (field.substring(1) === sortField.substring(1) && sortField.substring(0, 1) === "-") {
            field = "-" + field;
            field = field.replace("-+", "-");
            field = field.replace("--", "+");
        }
        field = field.replace("+-", "-");
        field = field.replace("++", "+");
        sortField = field;
        let type: string = field.substring(0, 1) + "any";
        field = field.replace("+", "").replace("-", "");
        if (field.substring(0, 1) === "#") type = type.replace("any", "string");
        if (field.substring(0, 1) === "$") type = type.replace("any", "date");
        field = field.replace("#", "").replace("$", "");
        let fieldStartAt: number = 0;
        let fieldLength: number = 0;
        fieldStartAt = Number(field.split("^")[1]);
        fieldLength = Number(field.split("^")[2]);
        field = field.split("^")[0];
        let fields = field.split(".");
        lists = lists.sort((a, b) => {
            switch (type + fields.length) {
                case "+string1": return this.setSortString(a[field], fieldStartAt) - this.setSortString(b[field], fieldStartAt);
                case "-string1": return this.setSortString(b[field], fieldStartAt) - this.setSortString(a[field], fieldStartAt);
                case "+string2": return this.setSortString(a[fields[0]][fields[1]], fieldStartAt) - this.setSortString(b[fields[0]][fields[1]], fieldStartAt);
                case "-string2": return this.setSortString(b[fields[0]][fields[1]], fieldStartAt) - this.setSortString(a[fields[0]][fields[1]], fieldStartAt);
                case "+string3": return this.setSortString(a[fields[0]][fields[1]][fields[2]], fieldStartAt) - this.setSortString(b[fields[0]][fields[1]][fields[2]], fieldStartAt);
                case "-string3": return this.setSortString(b[fields[0]][fields[1]][fields[2]], fieldStartAt) - this.setSortString(a[fields[0]][fields[1]][fields[2]], fieldStartAt);
                case "+date1": return this.setSortDate(a[field]) - this.setSortDate(b[field]);
                case "-date1": return this.setSortDate(b[field]) - this.setSortDate(a[field]);
                case "+date2": return this.setSortDate(a[fields[0]][fields[1]]) - this.setSortDate(b[fields[0]][fields[1]]);
                case "-date2": return this.setSortDate(b[fields[0]][fields[1]]) - this.setSortDate(a[fields[0]][fields[1]]);
                case "+date3": return this.setSortDate(a[fields[0]][fields[1]][fields[2]]) - this.setSortDate(b[fields[0]][fields[1]][fields[2]]);
                case "-date3": return this.setSortDate(b[fields[0]][fields[1]][fields[2]]) - this.setSortDate(a[fields[0]][fields[1]][fields[2]]);
                case "+any1": return this.setSortAny(a[field], fieldStartAt) - this.setSortAny(b[field], fieldStartAt);
                case "-any1": return this.setSortAny(b[field], fieldStartAt) - this.setSortAny(a[field], fieldStartAt);
                case "+any2": return this.setSortAny(a[fields[0]][fields[1]], fieldStartAt) - this.setSortAny(b[fields[0]][fields[1]], fieldStartAt);
                case "-any2": return this.setSortAny(b[fields[0]][fields[1]], fieldStartAt) - this.setSortAny(a[fields[0]][fields[1]], fieldStartAt);
                case "+any3": return this.setSortAny(a[fields[0]][fields[1]][fields[2]], fieldStartAt) - this.setSortAny(b[fields[0]][fields[1]][fields[2]], fieldStartAt);
                case "-any3": return this.setSortAny(b[fields[0]][fields[1]][fields[2]], fieldStartAt) - this.setSortAny(a[fields[0]][fields[1]][fields[2]], fieldStartAt);
                default: return this.setSortAny(a[field], fieldStartAt) - this.setSortAny(b[field], fieldStartAt);
            }
        });
        return sortField;
    }

    filter(items: any[], filterField: string, filterString: string): any[] {
        debugger
        if (items) {
            let fields: string[] = filterField.split(".");
            switch (fields.length) {
                case 1:
                    return items.filter(item => new RegExp(removeInvalidCharacters(filterString)).test(item[fields[0]]));
                case 2:
                    return items.filter(item => new RegExp(removeInvalidCharacters(filterString)).test(item[fields[0]][fields[1]]));
                case 3:
                    return items.filter(item => new RegExp(removeInvalidCharacters(filterString)).test(item[fields[0]][fields[1]][fields[2]]));
                default:
                    return items.filter(item => new RegExp(removeInvalidCharacters(filterString)).test(item[fields[0]]));
            }
        }
    }

    private setSortString(s: any, start?: number): number {
        debugger
        if (start) s = s.toString().substring(start);
        let r = "";
        let r1 = "00000";
        let r2 = "00000";
        let r3 = "00000";

        if (s.length > 0) {
            r1 = s.charCodeAt(0).toString();
            while (r1.length < 5) r1 = "0" + r1;
        }
        if (s.length > 1) {
            r2 = s.charCodeAt(1).toString();
            while (r2.length < 5) r2 = "0" + r2;
        }
        if (s.length > 2) {
            r3 = s.charCodeAt(2).toString();
            while (r3.length < 5) r3 = "0" + r3;
        }
        r = s.length.toString();
        while (r.length < 3) r = "0" + r;
        return Number(r + r1 + r2 + r3);
    }

    private setSortAny(s: any, start?: number): any {
        if (start) s = s.toString().substring(start);
        return (s as any);
    }

    private setSortDate(s: any): number {
        s = s.replace("年", "/").replace("月", "/").replace("日", "");
        s = new Date((Number(s.split("/")[0]) + 1911).toString() + "/" + s.split("/")[1] + "/" + s.split("/")[2]).getDate().toString();
        return Number(s);
    }

}
