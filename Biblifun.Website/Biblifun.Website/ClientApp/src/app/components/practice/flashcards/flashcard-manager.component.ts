import { Component, ViewEncapsulation } from '@angular/core';
import { fadeInOut } from '../../../services/animations';
import { Observable, Subject, forkJoin } from 'rxjs';
import { Flashcard } from '../../../models/flashcard.model';


@Component({
    selector: 'flashcard-manager',
    templateUrl: './flashcard-manager.component.html',
    styleUrls: ['./flashcard-manager.component.scss'],
    animations: [fadeInOut],
    encapsulation: ViewEncapsulation.None
})
export class FlashcardManagerComponent {

  constructor() {
  }
  
  flashcards: Flashcard[];

  currentCardIndex: number = 0;

  currentCard: Flashcard;

  isFinished: boolean;

  nextCard(): boolean {

    this.currentCardIndex++;

    if (this.currentCardIndex + 1 > this.flashcards.length) {
      this.currentCard = null;
      this.isFinished = true;
      return false;
    }
    else {
      this.currentCard = this.flashcards[this.currentCardIndex];
      return true;
    }
  }
}
