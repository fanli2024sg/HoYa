import { NgModule } from"@angular/core";
import { RouterModule, Routes } from"@angular/router";
import { SearchComponent } from"./search.component"; 
export const searchRoutes: Routes = [
    {
        path:"",
        component: SearchComponent 
    }
];
@NgModule({
    imports: [
        RouterModule.forChild(searchRoutes)
    ],
    exports: [
        RouterModule
    ]
})
export class SearchRouting { }