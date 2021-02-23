import { Component,  Input, ViewEncapsulation } from '@angular/core';
import { Value } from 'src/app/metadata/ViewModel';


@Component({
  selector: 'app-combobox-fixed',
  templateUrl: './combobox-fixed.component.html',
  encapsulation: ViewEncapsulation.None
})
export class ComboboxFixedComponent{

  public fields: Object = { text: 'value', value: 'key' };

  @Input() public caption: string;
  @Input() public values: Value[] ;

}
