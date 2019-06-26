import { Component, ViewEncapsulation } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { Observable, Subject, forkJoin } from 'rxjs';
import { Flashcard } from '../../../models/flashcard.model';


@Component({
    selector: 'flashcard',
    templateUrl: './flashcard.component.html',
    styleUrls: ['./flashcard.component.scss'],
    animations: [fadeInOut],
    encapsulation: ViewEncapsulation.None
})
export class FlashcardComponent {

  constructor() {
  }

  card: Flashcard;

  isAnswerShown: boolean;

  showAnswer() {
    this.isAnswerShown = true;
  }
}
