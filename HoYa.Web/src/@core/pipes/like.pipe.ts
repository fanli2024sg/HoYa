import { Pipe, PipeTransform } from "@angular/core";

@Pipe({
    name: 'like',
    pure: false
})
export class LikePipe implements PipeTransform {
    transform(options: Array<any>, conditions: { [field: string]: any }): Array<any> {
        return options.filter(option => {
            for (let field in conditions) {
                if (!RegExp(conditions[field]).test(option[field]))
                    return false;
            }
            return true;
        });
    }
}