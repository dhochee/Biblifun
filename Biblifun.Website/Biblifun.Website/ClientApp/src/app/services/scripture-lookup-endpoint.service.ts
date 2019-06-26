import { Injectable, Injector } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { EndpointFactory } from './endpoint-factory.service';
import { ConfigurationService } from './configuration.service';


@Injectable()
export class ScriptureLookupEndpoint extends EndpointFactory {

  private readonly _lookupUrl: string = '/api/ScriptureLookup/lookup/';

  get lookupUrl() {
    return this.configurations.baseUrl + this._lookupUrl;
  }
  
  constructor(http: HttpClient, configurations: ConfigurationService, injector: Injector) {

    super(http, configurations, injector);
  }

  lookupScriptureEndpoint(language: string, verse: string): Observable<string> {

    const endpointUrl = `${this.lookupUrl}/${language}/${verse}`;

    return this.http.get<string>(endpointUrl, this.getRequestHeaders()).pipe<string>(
      catchError(error => {
        return this.handleError(error, () => this.lookupScriptureEndpoint(language, verse));
      }));
  }
}
