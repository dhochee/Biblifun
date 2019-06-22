import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';


@Injectable()
export class ScriptureLookupEndpoint extends EndpointFactory {

  private readonly _lookupUrl: string = '/api/lookup/';

  get lookupUrl() {
    return this.configurations.baseUrl + this._lookupUrl;
  }
  
  constructor(http: HttpClient, configurations: ConfigurationService, injector: Injector) {

    super(http, configurations, injector);
  }

  lookupScriptureEndpoint<T>(language: string, verse: string): Observable<T> {
    const endpointUrl = `${this.lookupUrl}/${language}/${verse}`;

    return this.http.get<T>(endpointUrl, this.getRequestHeaders()).pipe<T>(
      catchError(error => {
        return this.handleError(error, () => this.lookupScriptureEndpoint(language, verse));
      }));
  }
}
