import { Instance } from "./entity";
import { Item, RecipeChange } from "./item";
//實例化Recipe
export class Inventory extends Instance<Item, RecipeChange>
{
    constructor() {
        super();
    }
}