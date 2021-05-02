import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { DataManager, ODataV4Adaptor, WebApiAdaptor, ODataAdaptor } from '@syncfusion/ej2-data';
import {
  EditService, ToolbarService, PageService, FilterService,
  ToolbarItems, SortService, FilterSettingsModel, IFilter,
  PageSettingsModel, ContextMenuItem, ContextMenuService, ColumnMenuService, RecordDoubleClickEventArgs, GridComponent
} from '@syncfusion/ej2-angular-grids';
import { ActivatedRoute, Router } from '@angular/router';
import { RouteService } from 'src/app/services/route.service';
import { Action, ActionType } from 'src/app/models/Routes';



@Component({
  selector: 'app-editor-content',
  templateUrl: './editor-content.component.html',
  styleUrls: ['./editor-content.component.less'],
  providers: [ToolbarService, EditService, PageService, SortService, FilterService, ContextMenuService, ColumnMenuService]
})
export class EditorContentComponent implements OnInit {

  @Input() url: string;
  @ViewChild('grid') public grid: GridComponent;

  public data: DataManager;
  public filterOptions: FilterSettingsModel;
  public filter: IFilter;
  public initialPage: PageSettingsModel;
  public contextMenuItems: ContextMenuItem[];
  public actions: Action[];
  public toolbar: any;
  public toolbarItems: object[] = [];

  private icons: string[] = ['e-custom-icons e-action-record', 'e-custom-icons e-add-record', 'e-custom-icons e-edit-record', 'e-custom-icons e-delete-record', '', ''];

  constructor(private route: ActivatedRoute,
    private router: Router,
    private routeService: RouteService) { }

  ngOnInit(): void {

    this.routeService.getActionsByUrl(this.router.url).subscribe(actions => {
      this.actions = actions;
      actions.forEach(action => {
        let align = '';
        if (action.type == ActionType.None)
          align = 'Right';
        this.toolbarItems.push({ id: action.id, text: action.display, prefixIcon: this.icons[action.type], align: align });
      })

      //this.toolbarItems.push({ text: "Print", align: '' });
      this.toolbarItems.push({ text: "Search" });
      this.toolbar = this.toolbarItems;
    })

    this.filterOptions = {
      type: 'Menu'
    };
    this.filter = {
      type: 'CheckBox'
    };

    this.contextMenuItems = ['AutoFit', 'AutoFitAll', 'SortAscending', 'SortDescending',
      'Copy', 'Edit', 'Delete', 'Save', 'Cancel',
      'PdfExport', 'ExcelExport', 'CsvExport', 'FirstPage', 'PrevPage',
      'LastPage', 'NextPage'];

    this.initialPage = { pageSize: 15 }
    this.data = new DataManager({
      url: this.url,
      adaptor: new ODataV4Adaptor(),
      offline: false
    });

  }

  navigateTo(action: Action){
    this.router.navigate([action.url], { queryParams: { id: 1 } });
  }

  dblUpdateEvent(e: RecordDoubleClickEventArgs) {
    //alert(e.rowData);
    let action: Action;
    this.actions.forEach(a => {
      if (a.type == ActionType.Update)
        action = a;
    })
    this.navigateTo(action); 
  };

  toolbarClick(args) {
    let action: Action;
    this.actions.forEach(a => {
      if (a.id == args.item.id)
        action = a;
    })
    this.navigateTo(action);
  }

}
