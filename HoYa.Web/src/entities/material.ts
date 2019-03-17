import { TypeSimpleDetail, Detail, Option, TypeSimple, Period, SimpleDetail } from "./entity";
import { Change } from "./process";
//設備
//物料
export class Material extends TypeSimple {
    code: string;
    unitId: string;
    unit: Option;
    costId: string;
    cost: Cost;//市價
    constructor() {
        super();
    }
}

export class MaterialChange extends Change<Material>
{
    constructor() {
        super();
    }
}

//模擬工序(詢價產生 未投入實際生產 Process.ParentId=詢價ProcessId)
//標準加工(工序需經主管審批 Process.ParentId is null)
//買入實物(提領物料)
export class MaterialProcedure extends TypeSimpleDetail<Material>
{
    equipmentId: string;
    equipment: Material;//標準加工extends Equipment.Cost * Time = 估算折舊成本; 買入實物extends Equipment.Cost = 估算消耗物料成本
    time: number;//耗時
    humanId: string;
    human: Cost;//預估消耗人力
    PowerId: string;
    power: Cost;//預估消耗能源
    _index: string;
    constructor() {
        super();
        this.equipment = new Material();
    }
}

//時間
//金錢
export class Cost extends Period<Material>
{
    unitId: string;
    unit: Option;
    amount: number;//能源成本
    constructor() {
        super();
    }
}


export class MaterialProcedureInput extends Detail<MaterialProcedure>
{
    inputId: string;
    input: MaterialProcedure;
    constructor() {
        super();
    }
}

export class Recipe extends SimpleDetail<Material>
{
    procedureId: string;
    procedure: Procedure;//Material集合=BOM表
    constructor() {
        super();
    }
}

export class Procedure extends SimpleDetail<Material>
{
    constructor() {
        super();
    }
}
