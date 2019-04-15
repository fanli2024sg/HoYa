import * as tslib_1 from "tslib";
import { TypeSimpleNode, TypeEntity } from "./entity";
import { Change } from "./process";
//實例化Material
var Inventory = /** @class */ (function (_super) {
    tslib_1.__extends(Inventory, _super);
    function Inventory() {
        return _super.call(this) || this;
    }
    return Inventory;
}(TypeEntity));
export { Inventory };
var InventoryChange = /** @class */ (function (_super) {
    tslib_1.__extends(InventoryChange, _super);
    function InventoryChange() {
        return _super.call(this) || this;
    }
    return InventoryChange;
}(Change));
export { InventoryChange };
var Storage = /** @class */ (function (_super) {
    tslib_1.__extends(Storage, _super);
    function Storage() {
        return _super.call(this) || this;
    }
    return Storage;
}(TypeSimpleNode));
export { Storage };
var StorageChange = /** @class */ (function (_super) {
    tslib_1.__extends(StorageChange, _super);
    function StorageChange() {
        return _super.call(this) || this;
    }
    return StorageChange;
}(Change));
export { StorageChange };
//# sourceMappingURL=inventory.js.map