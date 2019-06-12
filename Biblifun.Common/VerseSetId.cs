namespace Biblifun.Common
{
    /// <summary>
    /// Specifies a verse or sequence of verses with a specfic book, chapter, and verse.
    /// </summary>
    public class VerseSetId : IVerseSetId
    {
        public int BookId { get; set; }

        public int Chapter { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public int Verse => Start;

        public string Id
        {
            get
            {
                var end = Start == End ? "" : $"{End:D3}";

                return $"{BookId:D2}{Chapter:D3}{Start:D3}{end}";
            }
        }
    }
}
