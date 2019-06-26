
export class Flashcard {
  
    public questionHtml: string;
    public answerHtml: string;

    public static Create(data: {}) {
      const newCard = new Flashcard();

        Object.assign(newCard, data);

        return newCard;
    }
}
