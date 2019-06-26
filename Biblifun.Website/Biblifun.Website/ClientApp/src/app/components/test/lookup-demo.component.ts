import { Component, ViewEncapsulation } from '@angular/core';
import { fadeInOut } from '../../services/animations';
import { Observable, Subject, forkJoin } from 'rxjs';
import { ScriptureLookupEndpoint } from '../../services/scripture-lookup-endpoint.service';


@Component({
    selector: 'lookup-demo',
    templateUrl: './lookup-demo.component.html',
    styleUrls: ['./lookup-demo.component.scss'],
    animations: [fadeInOut],
    encapsulation: ViewEncapsulation.None
})
export class LookupDemoComponent {

  constructor(private scriptureLookupService: ScriptureLookupEndpoint) {
  }

  language: string;

  citation: string;

  resultHtml: string;

  lookupScripture() {
    this.scriptureLookupService.lookupScriptureEndpoint(this.language, this.citation)
      .subscribe((resultHtml: string) =>
        this.resultHtml = resultHtml);
  }
}
