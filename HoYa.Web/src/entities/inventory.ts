import { TypeSimpleNode, TypeEntity } from "./entity";
import { Change } from "./process";
import { MaterialProcedure } from "entities/material";
//實例化Material
export class Inventory extends TypeEntity {
    storageId: string;
    storage: Storage;
    remark: string;
    amount: number;
    materialId: string;
    materialP: MaterialProcedure;
    constructor() {
        super();
    }
}
export class InventoryChange extends Change<Inventory>
{
    constructor() {
        super();
    }
}

export class Storage extends TypeSimpleNode<Storage>
{
    code: string;
    description: string;
    constructor() {
        super();
    }
}
export class StorageChange extends Change<Storage>
{
    constructor() {
        super();
    }
}
