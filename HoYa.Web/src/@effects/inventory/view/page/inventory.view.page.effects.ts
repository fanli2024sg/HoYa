import { Injectable } from "@angular/core";
import { Actions, createEffect, ofType } from "@ngrx/effects";
import { of } from "rxjs";
import { switchMap, concatMap, withLatestFrom } from "rxjs/operators";
import { InventoryViewPageActions } from "@actions/inventory";
import * as inventoryReducers from "@reducers/inventory";
import { AppService } from "@services/app.service";
import { Store, select } from "@ngrx/store";
import { PresentationActions } from "@actions";

@Injectable()
export class InventoryViewPageEffects {/*
    oldActions$ = createEffect(() =>
        this.actions$.pipe(
            ofType(
                InventoryViewPageActions.addInventory,
                InventoryViewPageActions.addInventory,
                InventoryViewPageActions.importInventories,
                InventoryViewPageActions.printInventories,
                InventoryViewPageActions.edit,
                InventoryViewPageActions.remove
            ),
            concatMap(action => of(action).pipe(withLatestFrom(this.inventoryStore.pipe(select(inventoryReducers.inventoryViewPageState))))),
            switchMap(([payload, inventoryViewPageState]) => {
                
                this.appService.action$.next(inventoryViewPageState.oldAction);
                return of(PresentationActions.close({ message: "" }));
            })
        )
    );*/

    pickup$ = createEffect(() =>
        this.actions$.pipe(
            ofType(InventoryViewPageActions.pickup),
            concatMap(action =>
                of(action).pipe(
                    withLatestFrom(this.inventoryStore.pipe(select(inventoryReducers.inventoryViewPageState)))
                )
            ),
            switchMap(([payload, inventoryViewPageState]) => {
                this.appService.presentation$.next({ inventory: inventoryViewPageState.pickup, action: inventoryViewPageState.presentationAction });
                return of(InventoryViewPageActions.pickupOk());
            })
        )
    );

    printInventories$ = createEffect(() =>
        this.actions$.pipe(
            ofType(InventoryViewPageActions.printInventories),
            switchMap((payload) => {
                window.open(`print/inventories?inventoryId=${payload.inventory.id}`);
                return of(InventoryViewPageActions.printInventoriesOk());
            })
        )
    );
    constructor(
        private actions$: Actions,
        private inventoryStore: Store<inventoryReducers.State>,
        private appService: AppService
    ) { }
}
