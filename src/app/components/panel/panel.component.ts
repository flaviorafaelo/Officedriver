import { Component, Input, ViewChild, ElementRef, AfterContentInit, ViewChildren, OnInit, ViewEncapsulation, ɵConsole } from '@angular/core';

@Component({
  selector: 'app-panel',
  templateUrl: './panel.component.html',
  //template: '<div><label classname="e-label-top">{{header}}</label><div #Div></div></div>',
  styleUrls: ['./panel.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class PanelComponent {
  @ViewChild('Div') private div: ElementRef;

  @Input() public title: string;
  @Input() public content: Object;

  ngAfterViewInit() {

    //console.log(this.content);

    this.div.nativeElement.append(this.content);
    //this.items.forEach(e => {
    //  console.log(this.div);
    //  this.div.nativeElement.append(e);
    //  console.log("1");
    //});

  }

}
