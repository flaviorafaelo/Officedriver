import { Component, OnInit, Input } from '@angular/core';
//import { TextBoxModel} from "@syncfusion/ej2-angular-inputs"; 
//import { inputs } from '@syncfusion/ej2-angular-inputs/src/textbox/textbox.component';

@Component({
  selector: 'app-input',
  templateUrl: './input.component.html',
  //template:'',
  styleUrls: ['./input.component.scss'],

})
export class InputComponent  {

  @Input() caption: string;
  @Input() name: string;
  @Input() group: string;

}