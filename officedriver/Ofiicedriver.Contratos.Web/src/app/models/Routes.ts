import { Type } from "@angular/core";

export enum ActionType {
    None = 0,
    Create = 1,
    Update = 2,
    Delete = 3,
    List = 4
}

export class Param {
    name: string;
    value: any;
}

export class Service {
    name: string;
    params: Param[];
}

export class Action {
    id: string;
    display: string;
    url: string;
    type: ActionType;
    targert: string;
    service: Service;
}

export class Route {
    id: string;
    display: string;
    url: string;
    target: Type<any>;
    service: Service; 
    actions: Action[];
}

export class Module {
    id: string;
    name: string;
    routes : Route[]  
}