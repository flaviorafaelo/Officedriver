import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DashboardComponent } from './pages/dashboard/dashboard.component';
import { HomeComponent } from './pages/layout/components/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RouteService } from './services/route.service';

const routes: Routes = [

  { path: 'login', component: LoginComponent },  
  { path: "home", component: HomeComponent },
  { path: "dashboard", component: DashboardComponent }

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {

  constructor(routeService: RouteService){
//   routeService.loadRoutes();
  }

}
