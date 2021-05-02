import { AfterViewInit, Component, ElementRef, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { RouteService } from 'src/app/services/route.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SidebarComponent implements AfterViewInit {

  @ViewChild('sidebar') public navRef: ElementRef;

  constructor(private routeService: RouteService) {
  }

  private createGroupLink() {
    const ul = document.createElement("ul");
    ul.className = "sub-menu";
    ul.classList.add("sub-menu");
    return ul;
  }


  private createLink(displayName: string, href: string) {
    const li = document.createElement("li");
    li.className = displayName;

    const a = document.createElement("a");
    a.href = href.toLocaleLowerCase();

    const i = document.createElement("i");
    i.classList.add("icon-desktop");
    a.textContent = displayName;

    a.appendChild(i);
    li.appendChild(a);

    return li;
  }

  private findSubMenu(root: HTMLElement, name: string): Element {
    let elements = root.getElementsByClassName(name);
    let subMenu: Element = undefined;

    for (var i = 0; elements.length > 0 ; i++) {
      subMenu = elements.item(i).getElementsByClassName("sub-menu")[0];
      if (subMenu != undefined)
        break;
    };
    return subMenu;
  }

  ngAfterViewInit(): void {

    debugger;

    const routes = this.routeService.currentRoutes.sort((n1, n2) => {
      if (n1.name > n2.name) {
        return 1;
      }

      if (n1.name < n2.name) {
        return -1;
      }

      return 0;
    });

    const ul = document.createElement("ul");
    routes.forEach(module => {
      module.routes.forEach(route => {

        let group = "";
        let display = route.display;
        let curLink: HTMLElement;

        //temporariamente vamos suportar somente 2 niveis, futuramente substituir componente por treeview
        let backslash = display.indexOf("/", 0);
        if (backslash) {
          group = display.substring(0, backslash);
          display = display.substring(backslash + 1, display.length);
        }

        if (group == "") {
          curLink = this.createLink(display, route.display);
          ul.appendChild(curLink);
        } else {
          let subMenuEl = this.findSubMenu(ul, group);

          if (subMenuEl == undefined) {
            let groupEl = this.createLink(group, "javascript:void(0);");
            let subMenu = this.createGroupLink();
            let curLink = this.createLink(display, route.display);

            subMenu.appendChild(curLink);
            groupEl.appendChild(subMenu);

            ul.appendChild(groupEl);
          } else {

            let curLink = this.createLink(display, route.display);
            subMenuEl.appendChild(curLink);
          }

        }
      });
    });

    this.navRef.nativeElement.appendChild(ul);
  
  }

}
