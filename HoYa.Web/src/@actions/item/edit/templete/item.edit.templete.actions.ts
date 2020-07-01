import { createAction, props } from "@ngrx/store";
import { Item } from "@entities/item";


export const loadList = createAction("[IpLists] Load Item");
export const setEmpty = createAction("[itemsList] remove filter - all");
export const load = createAction("[item/guard] Load Inventory", props<{ item: Item }>()); 
export const get = createAction("[items/database] get");
export const getOk = createAction("[items/database] get ok", props<{ items: Item[] }>());
export const getError = createAction("[items/database] get error", props<{ errorMsg: any }>());
export const setMode = createAction("[item/templete] set mode", props<{ mode: string }>());

export const deleteItem = createAction("[item/database] delete");
export const locationBack = createAction("[item/templete] go to previous");


export const loadItem = createAction("[item/templete] find");
export const setItem = createAction('[item/templete] set item', props<{ id: string }>());
export const setRecipe = createAction('[item/templete] set recipe');
export const setInventory = createAction('[item/templete] set inventory');
export const editItem = createAction("[item/templete] go to edit");



export const disableItem = createAction("[item/database] update statusId");
export const enableItem = createAction("[item/database] update statusId");



export const more = createAction("[item/templete] more", props<{ buttons: any[] }>());
export const showAttributeInput = createAction("[item/templete] show attribute input");
export const addInventory = createAction("[inventory/presentation] add");
export const importInventories = createAction("[inventories/presentation] import");
export const exportInventories = createAction("[inventories/presentation] export");
export const printInventories = createAction("[inventories/presentation] print"); 

export const create = createAction("[item.edit.templete] create", props<{ item: Item }>());
export const createSuccess = createAction("[item.edit.templete] create success", props<{ item: Item }>());
export const createFailure = createAction("[item.edit.templete] create failure", props<{ item: Item }>());
export const closePresentation = createAction("[item.edit.templete] close presentation");




export const update = createAction("[item.edit.templete] update", props<{ item: Item }>());
export const updateSuccess = createAction("[item.edit.templete] update success", props<{ item: Item }>());
export const updateFailure = createAction("[item.edit.templete] update failure", props<{ item: Item }>());