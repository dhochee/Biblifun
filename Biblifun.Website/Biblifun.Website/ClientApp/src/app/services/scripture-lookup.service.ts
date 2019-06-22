// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

import { Injectable } from '@angular/core';
import { Observable, Subject, forkJoin } from 'rxjs';
import { mergeMap, tap } from 'rxjs/operators';

import { ScriptureLookupEndpoint } from './scripture-lookup-endpoint.service';

@Injectable()
export class ScriptureLookupService {

  constructor(
    private scriptureLookupEndpoint: ScriptureLookupEndpoint) {

  }
  
  lookupScripture(language: string, verse: string) {
    return this.scriptureLookupEndpoint.lookupScriptureEndpoint<string>(language, verse);
  }

}
