import { Component, OnInit, HostBinding } from "@angular/core";
import { AppService } from "@services/app.service";
import { Presentation } from "@models/app.model";
import { Observable, Subscription, BehaviorSubject } from "rxjs";
import { FilesService } from "@services/files.service";
import { FolderFilesService } from "@services/folderFiles.service";
import { ItemCategoriesService } from "@services/itemCategories.service";
import { ItemsService } from "@services/items.service";
import { FolderFile, File, Option } from "@entities/entity";
import { ActivatedRoute, Router, ParamMap } from "@angular/router";
import { Item, ItemCategory } from "@entities/item";
import { ItemSave } from "@models/item";
import { FileSave } from "@models/entity";
import { Location } from "@angular/common";
import { OptionsService } from "@services/options.service";
import * as LayoutActions from "@actions/layout.actions";
import * as reducers from "@reducers";
import { Store } from "@ngrx/store";
import * as itemReducers from "@reducers/item";
import { PresentationActions } from "@actions";
@Component({
    selector: "itemEdit",
    templateUrl: "item.edit.page.html",
    styleUrls: ["item.edit.page.css"],
    host: { "class": "SCxLW", "role": "main", "style": "margin-bottom:8px" }
})
export class ItemEditPage implements OnInit {
    itemSave: ItemSave;
    id: string;
    urlAction: string;
    categoryValues: string[];
    mainPhoto: File;
    @HostBinding("class.uzKWK") uzKWK: boolean = false;
    folderFiles: FolderFile[] = new Array<FolderFile>();
    itemsSubscription: Subscription;
    action: any;
    actionSubscription: Subscription;
    activatedRouteSubscription: Subscription;
    filesSubscription: Subscription;
    optionsSubscription: Subscription;
    loading: boolean;
    itemUnit$: BehaviorSubject<Option> = new BehaviorSubject<Option>(new Option());
    itemUnit: Option;
    itemUnitParent: Option;
    item$: BehaviorSubject<Item> = new BehaviorSubject<Item>(new Item());
    itemSubscription: Subscription;
    item: Item;
    isNew: boolean = false;
    itemValue: string = "";
    presentation$: Observable<Presentation> = this.appService.presentation$;
    constructor(
        public filesService: FilesService,
        public appService: AppService,
        public router: Router,
        public itemsService: ItemsService,
        public activatedRoute: ActivatedRoute,
        public folderFilesService: FolderFilesService,
        public itemCategoriesService: ItemCategoriesService,
        public location: Location,
        public store: Store<reducers.State>,
        public optionsService: OptionsService
    ) {
        this.loading = true;
        if (!this.appService.mobile) this.uzKWK = true;
    }

    ngOnDestroy() {
        if (this.actionSubscription) this.actionSubscription.unsubscribe();
        if (this.activatedRouteSubscription) this.activatedRouteSubscription.unsubscribe();
        if (this.itemSubscription) this.itemSubscription.unsubscribe();
        if (this.itemsSubscription) this.itemsSubscription.unsubscribe();
        if (this.optionsSubscription) this.optionsSubscription.unsubscribe();
        if (this.filesSubscription) this.filesSubscription.unsubscribe();
    }
    getItem(): Promise<Item> {
        if (this.itemsSubscription) this.itemsSubscription.unsubscribe();
        return new Promise((resolve) => {
            this.itemsSubscription = this.itemsService.find(this.appService.id).subscribe((item: Item) => {
                if (!item) this.router.navigate([`./items`]);
                this.item$.next(item);
                this.itemCategoriesService.select({ ownerId: item.id }, false).subscribe((itemCategories: ItemCategory[]) => {
                  //  item._itemCategories = itemCategories;
                    item.description = item.description || "";
                    this.folderFilesService.select({
                        ownerId: item.id
                    }, false).subscribe((folderFiles: FolderFile[]) => {
                        this.folderFiles = folderFiles;
                    });
                    this.loading = false;
                    resolve(item);
                });

            });
        });
    }

    ngOnInit() {
        this.optionsService.getBy({ code: "unit" }, false).subscribe((itemUnitParent: Option) => {
            this.itemUnitParent = itemUnitParent;
        });

        this.store.dispatch(LayoutActions.setTopLeft({ left: "取消" }));
        this.store.dispatch(LayoutActions.setTopTitle({ title: "新增品項" }));
        this.store.dispatch(LayoutActions.setTopRight({ right: "繼續" }));
        this.appService.bottom$.next(null);




        this.itemSubscription = this.store.select(itemReducers.itemEditTemplete_item).subscribe((item: Item) => {
            this.item = { ...item };
            this.itemValue = this.item.value;
            if (this.item.statusId === "cd8d8758-aee1-4dc6-b8a8-860ba61c1ae5") {

                this.isNew = true;
            } else {
                this.isNew = false;

            } switch (this.item.statusId) {
                case "cd8d8758-aee1-4dc6-b8a8-860ba61c1ae5":
                    this.store.dispatch(LayoutActions.setTopLeft({ left: "取消" }));
                    this.store.dispatch(LayoutActions.setTopTitle({ title: "待發佈品項" }));
                    this.store.dispatch(LayoutActions.setTopRight({ right: "發佈" }));
                    break;
                case "005617b3-d283-461c-abef-5c0c16c780d0":
                    this.store.dispatch(LayoutActions.setTopLeft({ left: "取消" }));
                    this.store.dispatch(LayoutActions.setTopTitle({ title: `編輯『${this.itemValue}』` }));
                    this.store.dispatch(LayoutActions.setTopRight({ right: "更新" }));
                    break;
                default:
                    this.store.dispatch(LayoutActions.setTopLeft({ left: "取消" }));
                    this.store.dispatch(LayoutActions.setTopTitle({ title: "待發佈品項" }));
                    this.store.dispatch(LayoutActions.setTopRight({ right: "發佈" }));
                    break;
            }
        });























        /*
        this.activatedRouteSubscription = this.activatedRoute.paramMap.subscribe(async (paramMap: ParamMap) => {
            this.appService.errors$.next([]);
            this.appService.id = paramMap.get("id");
            this.appService.module = this.activatedRoute.parent.snapshot.url[0].path;
            this.appService.action = this.activatedRoute.snapshot.url[1].path;
            this.item = await this.getItem();
            this.appService.action$.next("預覽");
            if (!this.actionSubscription) this.actionSubscription = this.appService.action$.subscribe((action: string) => {
                if (!action) action = "預覽";
                if (this.appService.module === "items" && this.appService.action === "edit" && action && this.action !== action) {
                    this.action = action;
                    this.appService.bottom$.next(null);
                    switch (this.action) {
                        case "返回(ItemEditPageActions)":
                            this.appService.presentation$.next(null);
                            this.appService.action$.next("預覽");
                            this.location.back();
                            break;
                        case "取消":
                            if (this.appService.errors$.getValue().length === 0) {
                                this.save().then(() => {
                                    this.location.back();
                                });
                            } else this.location.back();
                            break;
                        case "更新":
                            this.submit().then(() => {
                                this.appService.presentation$.next(null);
                                this.router.navigate([`./items`]);
                                this.appService.message$.next("更新品項成功");
                            });
                            break;
                        case "發佈":
                            this.submit().then(() => {
                                this.appService.presentation$.next(null);
                                this.router.navigate([`./items`]);
                                this.appService.message$.next("發佈品項成功");
                            });
                            break;
                        case "預覽":
                            if (!this.item.statusId) this.item.statusId = "";
                            switch (this.item.statusId) {
                                case "cd8d8758-aee1-4dc6-b8a8-860ba61c1ae5":
                                    this.store.dispatch(LayoutActions.setTopLeft({ left: "取消" }));
                                    this.store.dispatch(LayoutActions.setTopTitle({ title: "待發佈品項" }));
                                    this.store.dispatch(LayoutActions.setTopRight({ right: "發佈" }));
                                    break;
                                case "005617b3-d283-461c-abef-5c0c16c780d0":
                                    this.store.dispatch(LayoutActions.setTopLeft({ left: "取消" }));
                                    this.store.dispatch(LayoutActions.setTopTitle({ title: "編輯品項" }));
                                    this.store.dispatch(LayoutActions.setTopRight({ right: "更新" }));
                                    break;
                                default:
                                    this.store.dispatch(LayoutActions.setTopLeft({ left: "取消" }));
                                    this.store.dispatch(LayoutActions.setTopTitle({ title: "新增品項" }));
                                    this.store.dispatch(LayoutActions.setTopRight({ right: "發佈" }));
                                    break;
                            }
                        case "編輯":
                            this.appService.action$.next("預覽");
                            break;
                        default:
                            break;
                    }
                }
            });
        });*/
    }

    createItemPhoto(event) {

        if (event.target.files) {
            if (this.filesSubscription) this.filesSubscription.unsubscribe();
            this.filesSubscription = this.filesService.create(<Array<File>>event.target.files, `items/${this.item.id}`).subscribe((fileSave: FileSave) => {
                this.item.photoId = fileSave.photos[0].id;
               // this.item.photo = fileSave.photos[0];
            });
        }
    }
    deployOrUpdate() {

        if (this.item$.getValue().statusId === "cd8d8758-aee1-4dc6-b8a8-860ba61c1ae5") this.appService.action$.next("發佈");
        if (this.item$.getValue().statusId === "005617b3-d283-461c-abef-5c0c16c780d0") this.appService.action$.next("更新");
    }

    submit() {
        this.store.dispatch(PresentationActions.message({ message: { h3: "發佈中", div: "品項發佈中, 請稍後..." } }));
        return new Promise((resolve) => {
            this.itemSave = new ItemSave();
            this.itemSave.item = this.item;
            this.itemSave.item.description = this.itemSave.item.description.replace("\n", "");
            this.itemSave.item.statusId = "005617b3-d283-461c-abef-5c0c16c780d0";
            this.itemsService.save(this.item.id, this.itemSave).toPromise().then((updatedItemSave: ItemSave) => {
                resolve(updatedItemSave);
            });
        });
    }

    save() {
        this.store.dispatch(PresentationActions.message({ message: { h3: "更新中", div: "品項更新中, 請稍後..." } }));
        return new Promise((resolve) => {
            this.itemSave = new ItemSave();
            this.itemSave.item = this.item;
            this.itemSave.item.description = this.itemSave.item.description.replace("\n", "");
            this.itemsService.save(this.item.id, this.itemSave).toPromise().then((updatedItemSave: ItemSave) => {
                resolve(updatedItemSave);
            });
        });
    }

    isNewItem() {
        let item = this.item$.getValue();
        return (!item.createdById && item.value);
    }

    isNewItemUnit() {
        let itemUnit = this.itemUnit$.getValue();
        if (itemUnit) return (!itemUnit.createdById && itemUnit.value);
        else return false;
    }

    hasItem() {
        let item = this.item$.getValue();
        return item.value;
    }
}
