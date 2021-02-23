import { Component, ElementRef, Input, ViewChild, AfterViewInit } from '@angular/core';

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements AfterViewInit {
  @ViewChild('content') content: ElementRef;

  @Input() public title: string;
  @Input() public controls: Object;

  constructor() { }

  ngAfterViewInit(): void {

    this.content.nativeElement.append(this.controls);
  }


}
