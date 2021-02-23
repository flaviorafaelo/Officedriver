import { Injector, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';
import { createCustomElement } from '@angular/elements';
import { FormsModule } from '@angular/forms'
import { ReactiveFormsModule} from '@angular/forms'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { InputComponent } from './components/input/input.component';
import { CkeckboxComponent } from './components/ckeckbox/ckeckbox.component';
import { FrameComponent } from './components/frame/frame.component';
import { DateComponent } from './components/date/date.component';
import { ComboboxFixedComponent } from './components/combobox-fixed/combobox-fixed.component';
import { DividerComponent } from './components/divider/divider.component';
import { PanelComponent } from './components/panel/panel.component';
import { DataEditComponent } from './components/data-edit/data-edit.component';
import { CheckBoxModule, ButtonModule } from '@syncfusion/ej2-angular-buttons';
import { TextBoxModule } from '@syncfusion/ej2-angular-inputs';
import { DatePickerModule } from '@syncfusion/ej2-angular-calendars';
import { TabModule, AccordionModule } from '@syncfusion/ej2-angular-navigations';
import { ComboBoxModule } from '@syncfusion/ej2-angular-dropdowns';
import { SplitterModule } from '@syncfusion/ej2-angular-layouts';
import { setCulture } from '@syncfusion/ej2-base';
import { GroupComponent } from './components/group/group.component';
import { DataActionComponent } from './components/data-action/data-action.component';

@NgModule({
  declarations: [
    AppComponent,
	  InputComponent,
    CkeckboxComponent,
    FrameComponent,
    DateComponent,
    ComboboxFixedComponent,
    DividerComponent,
    PanelComponent,
    DataEditComponent,
    GroupComponent,
    DataActionComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,

    ReactiveFormsModule,FormsModule,

    ButtonModule, CheckBoxModule, TextBoxModule, DatePickerModule, TabModule, ComboBoxModule, SplitterModule, AccordionModule
  ],
  providers: [],
  bootstrap: [AppComponent],
  entryComponents: [CkeckboxComponent, InputComponent, DateComponent, ComboboxFixedComponent]
})
export class AppModule {

	constructor(injector: Injector) {
		customElements.define('app-checkbox', createCustomElement(CkeckboxComponent, { injector }));
		customElements.define('app-input', createCustomElement(InputComponent, { injector }));
		customElements.define('app-date', createCustomElement(DateComponent, { injector }));
		customElements.define('app-combobox-fixed', createCustomElement(ComboboxFixedComponent, { injector }));
		customElements.define('app-divider', createCustomElement(DividerComponent, { injector }));
		customElements.define('app-panel', createCustomElement(PanelComponent, { injector }));
    customElements.define('app-group', createCustomElement(GroupComponent, { injector }));
	}

}
