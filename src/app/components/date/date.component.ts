import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-date',
  template:'<ejs-datepicker placeholder={{caption}} floatLabelType="Always"></ejs-datepicker>',
  styleUrls: ['./date.component.scss']
})
export class DateComponent {

  @Input() caption: string;

}
