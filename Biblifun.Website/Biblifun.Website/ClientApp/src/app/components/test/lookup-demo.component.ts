// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

import { Component } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { Observable, Subject, forkJoin } from 'rxjs';
import { ScriptureLookupService } from '../../services/scripture-lookup.service';


@Component({
    selector: 'customers',
    templateUrl: './customers.component.html',
    styleUrls: ['./customers.component.scss'],
    animations: [fadeInOut]
})
export class LookupDemoComponent {

  constructor(private scriptureLookupService: ScriptureLookupService) {
  }

  language: string;

  citation: string;

  resultHtml: Observable<string>;

  lookupScripture() {

    this.language = "en";
    this.citation = "Matthew 24:14";

    this.resultHtml = this.scriptureLookupService.lookupScripture(this.language, this.citation);
  }
}
