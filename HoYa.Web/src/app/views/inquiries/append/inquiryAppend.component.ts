import { Component, OnInit, Inject } from "@angular/core";
import { RecipesService } from "services/recipes.service";
import { Recipe } from "entities/item";
import { Inquiry } from "entities/inquiry";
import { Observable, Subject } from "rxjs";
import { switchMap, debounceTime, distinctUntilChanged } from "rxjs/operators";
import { MatDialogRef, MAT_DIALOG_DATA, MatDialog } from '@angular/material';

import { RecipeAppendDialog } from "../../recipes/append/recipeAppend.dialog";
@Component({
    selector: "inquiryAppend",
    templateUrl: "inquiryAppend.component.html",
    styleUrls: ["inquiryAppend.component.css"],
    providers: [RecipesService]
})
export class InquiryAppendComponent implements OnInit {
    withRefresh = false;
    filteredRecipes$: Observable<Recipe[]>;
    private anyLike$ = new Subject<string>();
    constructor(
        private dialogRef: MatDialogRef<InquiryAppendComponent>,
        public dialog: MatDialog,
        private recipesService: RecipesService,
        @Inject(MAT_DIALOG_DATA) public inquiry: Inquiry
    ) {
        //this.selectedRecipe = new Recipe();
    }
    displayRecipe(recipe?: Recipe): string | undefined {
        return recipe ? recipe.value : undefined;
    }

    recipeAppendDialog() {
        this.dialog.open(RecipeAppendDialog, {
            width: "500px",
            data: new Recipe()
        }).afterClosed().subscribe((recipe: Recipe) => {
            console.log(recipe);
        });
    }

    ngOnInit() {
       
        this.filteredRecipes$ = this.anyLike$.pipe(
            debounceTime(200),
            distinctUntilChanged(),
            switchMap(anyLike => this.recipesService.search(anyLike, this.withRefresh))
        );
    }
    search(anyLike: string) {
        this.anyLike$.next(anyLike);
    }
    selectRecipe(recipe:Recipe) {
        this.inquiry.recipe = recipe;
        this.inquiry.recipeId = recipe.id;
    }

}
