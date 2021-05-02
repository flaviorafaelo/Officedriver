import { BrowserModule } from '@angular/platform-browser';
import { NgModule, APP_INITIALIZER } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavbarComponent } from './pages/layout/components/navbar/navbar.component';
import { HomeComponent } from './pages/layout/components/home/home.component';
import { CrumbsComponent } from './pages/layout/components/crumbs/crumbs.component';
import { SidebarComponent } from './pages/layout/components/sidebar/sidebar.component';
import { LoginComponent } from './pages/login/login.component';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { PageContentComponent } from './pages/layout/components/page-content/page-content.component';
import { FrameComponent } from './components/frame/frame.component';
import { DataEditComponent } from './pages/editor/data-edit/data-edit.component';
import { EditorTitleComponent } from './pages/editor/editor-title/editor-title.component';
import { ToolbarComponent } from './components/toolbar/toolbar.component';

import { RouteService } from './services/route.service'
import { EditorContentComponent } from './pages/editor/editor-content/editor-content.component';

import { GridModule } from '@syncfusion/ej2-angular-grids';
import { EditorActionComponent } from './pages/editor/editor-action/editor-action.component';


export function initSettings(routeService: RouteService) {
  return () => routeService.loadRoutes();
}

@NgModule({
  declarations: [
    AppComponent,
    NavbarComponent,
    HomeComponent,
    CrumbsComponent,
    SidebarComponent,
    LoginComponent,
    DashboardComponent,
    PageContentComponent,
    FrameComponent,
    DataEditComponent,
    EditorTitleComponent,
    ToolbarComponent,
    EditorContentComponent,
    EditorActionComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,

    GridModule
  ],
  providers: [
    RouteService,
    {
      'provide': APP_INITIALIZER,
      'useFactory': initSettings,
      'deps': [RouteService],
      'multi': true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
