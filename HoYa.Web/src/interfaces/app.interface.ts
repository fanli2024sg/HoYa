import { BehaviorSubject } from "rxjs";
import { PageEvent, Sort } from '@angular/material';

export abstract class AppInterface {
    abstract leftIcon$: BehaviorSubject<string>;
    abstract title$: BehaviorSubject<string>;
    abstract anyLike$: BehaviorSubject<string>;
    abstract rightIcons$: BehaviorSubject<Array<string>>;
    abstract profileMenus$: BehaviorSubject<Array<any>>;
    abstract page$: BehaviorSubject<PageEvent>;
    abstract sort$: BehaviorSubject<Sort>;
    abstract host$: BehaviorSubject<string>;
}
