import { ComponentFactoryResolver, Injectable, Injector } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, tap } from 'rxjs/operators';
import { Routes, Router } from '@angular/router';
import { DataEditComponent } from '../pages/editor/data-edit/data-edit.component';
import { Action, Module } from '../models/Routes';


@Injectable({
  providedIn: 'root'
})
export class RouteService {

  //private metadataUrl = 'http://api.officedriver.kinghost.net/api';
  private metadataUrl = 'http://localhost:52473/api';
  private routes: Routes = [];
  public currentRoutes: Module[] = [];

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient, private injector: Injector, private resolver: ComponentFactoryResolver) { }

  loadRoutes(): Promise<any> {
    return new Promise((resolve, reject) => {
      debugger;

      setTimeout(() => {
        const router = this.injector.get(Router);
        const url = `${this.metadataUrl}/routes`;

        this.http.get(url).subscribe(
          response => {
            this.currentRoutes = response as Module[];
            this.currentRoutes.forEach(module => {
              module.routes.forEach(route => {
                let pathLower = route.display.toLocaleLowerCase();
                router.config.push({ path: pathLower, component: DataEditComponent });
              });
            });
            resolve(response);
          },
          err => {
            console.log('Server error: ' + JSON.stringify(err));
            reject(false);
          });
      });
    });
  }

  getActionsByUrl(id: string): Observable<Action> {
    const url = `${this.metadataUrl}/routes/${btoa(id)}/actions`;
    return this.http.get<Action>(url).pipe(
      catchError(this.handleError<Action>(`getCooperado`))
    );

  }

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      console.error(error);
      this.log(`${operation} failed: ${error.message}`);
      return of(result as T);
    };
  }

  private log(message: string) {
    console.log(message);
  }

}
