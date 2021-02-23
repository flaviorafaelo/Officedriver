import { Component, Input, OnInit, ViewChild, ViewEncapsulation} from '@angular/core';
import { MetadataService } from '../../metadata/metadata.service'
import { Page, Property } from '../../metadata/ViewModel'
import { TabComponent } from '@syncfusion/ej2-angular-navigations';

@Component({
  selector: 'app-frame',
  templateUrl: './frame.component.html',
  styleUrls: ['./frame.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class FrameComponent implements OnInit {
  @ViewChild('Tab', { static: false })
  public tabObj: TabComponent;

  @Input() model: string;

  private mapControls = new Map();

  public animation: object = { previous: { effect: '', duration: 0, easing: '' }, next: { effect: '', duration: 0, easing: '' } };


  constructor(private metadataService: MetadataService) {
    this.mapControls.set("string", "app-input");
    this.mapControls.set("date", "app-date");
    this.mapControls.set("list", "app-combobox-fixed");
 }

  private createGroupElement(description: string): HTMLDivElement {

    const id  = "chck" + Math.random().toString()

    const elContainer = document.createElement("div")
    elContainer.className = "ctr-group"

//    const elHeader = document.createElement("div");
//    elHeader.classList.add('ctr-group-header');


    const elCkd =  document.createElement('input')
    elCkd.type = "checkbox"
    elCkd.className = "ctr-group-input"
    elCkd.setAttribute("checked", "true");
    elCkd.id = id;

    const label = document.createElement('label');
    label.className = "e-label-top e-control e-textbox e-lib ctr-group-label";
    label.innerHTML = '<b>' + description + '</b>';
    label.setAttribute("for", id)


    const elContent = document.createElement("div");
    elContent.classList.add('ctr-group-content');

    elContainer.appendChild(elCkd)
    elContainer.appendChild(label)
    elContainer.appendChild(elContent);

    return elContainer;

    //const root = document.createElement("div");
    //this.createControls(root, p.properties);
    //let panel = document.createElement('app-panel') as any;
    //panel.setAttribute("title", p.name);
    //panel.content = root;
    //root.appendChild(panel);
  }


  private createDividerElement(description: string): HTMLDivElement {

    const el = document.createElement("div");
    el.className = "e-control e-textbox e-lib ctr-divider";

    const label = document.createElement('label');
    label.className = "e-label-top ctr-divider-label";
    label.innerHTML = '<b>' + description + '</b>';
    el.appendChild(label);
    return el;
  }

  private createControls(root: HTMLElement, property: Property) {

    if (property.typeName == "object") {
      console.log(property);
      const group = this.createGroupElement(property.name);
      property.properties.forEach(prop => {
        this.createControls((group.getElementsByClassName("ctr-group-content")[0] as HTMLElement) , prop);
      });
      root.appendChild(group);
    }
    else {
      let type = this.mapControls.get(property.typeName);
      let el: any;
      if (type != null) {
        el = document.createElement(type);
        el.setAttribute("caption", property.name);
        el.setAttribute("name", property.name);
        //el.setAttribute("formControlName", property.name);
        if (property.values != null)
          el.values = property.values;
      } else {
        el = document.createElement('div');
        el.style = "height: 25px; border: dotted 1px; padding: 2px; margin-top: 2px;";
        let label = document.createElement('label');
        label.innerHTML = "Tipo não suportado para " + property.name;
        el.appendChild(label);
      }
      root.appendChild(el);
    }
  }

  private createPages(root: HTMLElement, page: Page) {

    page.groups.forEach(group => {
      let groupElement: HTMLDivElement;
      if (group.description == null)
        groupElement = document.createElement("div")
      else
        groupElement = this.createDividerElement(group.description);
      group.items.forEach(item => {
        this.createControls(groupElement, item.property);
      });

      root.appendChild(groupElement);
    });
  }

  public addTab(name: string, content: HTMLElement) {
    const count: number = this.tabObj.items.length;
    const item: Object = {
      header: { text: name },
      content: content
    };
    this.tabObj.addTab([item], this.tabObj.items.length);
    this.tabObj.select(0);
  };

  private buildControls() {
      this.metadataService.geMetaview(this.model).subscribe(view => {
      /*let properties: string[] = [];
      view.pages.forEach(page => {
        page.groups.forEach(group=>{
          group.items.forEach(item=>{
            properties.push(item.property.name);
          })
        });
      });*/
      view.pages.forEach(page => {
        let root = document.createElement('div');
        this.createPages(root, page);
        this.addTab(page.description.toUpperCase()||"(TEMPORÁRIO)", root);
      });
    })
  }


  private buldBindsTeste(){
    /*debugger;

    this.form =  new FormGroup({
      matricula: new FormControl()
    });;

    this.form.setValue({matricula:"001"});

    let div = this.renderer.createElement('form');
    div.setAttribute("formGroup","form");

    let teste = this.renderer.createElement("input");
    teste.setAttribute("formControlName","matricula");
    teste.setAttribute("name","matricula");
    teste.setAttribute("formControlName","matricula");
    teste.setAttribute("caption","matricula");

    div.appendChild(teste);

    let p = this.renderer.createElement('p');
    p.innerHTML = 'Form Value: {{ form.value | json }}';

    div.appendChild(p);

    this.tpl.createEmbeddedView(null);
    //this.vc.element.nativeElement.appendChild(div);
    //this.renderer.appendChild(this.el.nativeElement,div);*/
  }

  ngOnInit() {
    this.buildControls();
  }

}
