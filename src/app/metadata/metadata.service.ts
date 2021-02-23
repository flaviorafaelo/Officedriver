import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, of } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';
import { View } from './ViewModel';

@Injectable({
  providedIn: 'root'
})
export class MetadataService {

  private metadataUrl = 'http://api.officedriver.kinghost.net/api';
  //private  metadataUrl = 'http://localhost:52473/api';

  httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' })
  };

  constructor(private http: HttpClient) { }

  geMetaview(name: string): Observable<View> {
    const url = `${this.metadataUrl}/${name}/${name}/metaview`;
    return this.http.get<View>(url).pipe(
      tap(_ => this.log(`fetched view id=${name}`)),
      catchError(this.handleError<View>(`getMeta name=${name}`))
    );
  };

  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {

      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // TODO: better job of transforming error for user consumption
      this.log(`${operation} failed: ${error.message}`);

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }

  /** Log a HeroService message with the MessageService */
  private log(message: string) {
    console.log(message);
    //this.messageService.add(`HeroService: ${message}`);
  }

}
