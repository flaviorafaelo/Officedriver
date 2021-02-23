export interface Value{
    key: string;
    value: string;
} 

export interface Property {
  type: string;
  typeName: string;
  name: string;
  size: number;
  required: boolean;
  properties: Property[]
  values: Value[]; 
}

export interface Item {
  property: Property;
  label: string;
  visibilityRule: string;
  enablingRule: string;
  tooltip: string;
}

export interface Group {
  description: string;
  items: Item[];
}

export interface Page {
  description: string;
  groups: Group[];
}

export interface View {
  description: string;
  version: Number,
  pages: Page[] 
}