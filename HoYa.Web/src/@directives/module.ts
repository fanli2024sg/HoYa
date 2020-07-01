import { NgModule } from"@angular/core";
import { LazyLoadDirective } from "./lazyLoad.directive";
import { LoadMoreDirective } from './loadMore.directive';
@NgModule({
    declarations: [
        LoadMoreDirective,
        LazyLoadDirective
    ],
    exports: [
        LoadMoreDirective,
        LazyLoadDirective
    ]
})

export class DirectivesModule { }