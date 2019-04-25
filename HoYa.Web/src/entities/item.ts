import { Definition, RealTypeDefinition, Branch, Change, Detail, Relation, Category } from "./entity";


export class Item extends RealTypeDefinition {
    constructor() {
        super();
    }
}

export class Recipe extends Branch<Item>
{
    constructor() {
        super();
    }
}

export class RecipeChange extends Change<Recipe>
{
    constructor() {
        super();
    }
}

export class Procedure extends Detail<Recipe>
{
    ingredientId: string;
    ingredient: Recipe;
    quantity: number;
    constructor() {
        super();
    }
}

export class ItemCategory extends Relation<Item, Category>
{
    constructor() {
        super();
    }
}