import { Component, OnInit, Inject } from "@angular/core";
import { CategoriesService } from "services/categories.service";
import { Category } from "entities/entity";
import { Recipe } from "entities/item";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material';
@Component({
    selector: "recipeAppend",
    templateUrl: "recipeAppend.dialog.html",
    styleUrls: ["recipeAppend.dialog.css"],
    providers: [
        CategoriesService
    ]
})
export class RecipeAppendDialog implements OnInit {
    withRefresh = false;
    categories: Category[];
    filteredCategories$: Observable<Category[]>;
    private anyLike$ = new Subject<string>();
    constructor(
        private dialogRef: MatDialogRef<RecipeAppendDialog>,
        private categoriesService: CategoriesService,
        @Inject(MAT_DIALOG_DATA) public recipe: Recipe
    ) {
        //this.selectedRecipe = new Recipe();
    }
    displayCategory(category?: Category): string | undefined {
        return category ? category.value : undefined;
    }

    ngOnInit() {
       
        this.filteredCategories$ = this.anyLike$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            switchMap(anyLike => this.categoriesService.search(anyLike, this.withRefresh))
        );
    }
    search(anyLike: string) {
        this.anyLike$.next(anyLike);
    }

    selectRecipe(category: Category) {
        this.categories.push(category);
    }
}
