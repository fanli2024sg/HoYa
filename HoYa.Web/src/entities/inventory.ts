import { Instance } from "./entity";
import { Material } from "./material";
//實例化Material
export class Inventory extends Instance<Material>
{
    constructor() {
        super();
    }
}