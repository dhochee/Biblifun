namespace Biblifun.Common
{

    public interface IVerseId
    {
        int BookId { get; }

        int Chapter { get; }

        /// <summary>
        /// The verse
        /// </summary>
        int Verse { get; }

        /// <summary>
        /// Returns a fixed length string that identifies either a single verse 
        /// or sequence of verses from a single book and chapter.
        /// </summary>
        string Id { get; }
    }

    public interface IVerseSetId : IVerseId
    {
        /// <summary>
        ///  The starting verse of the set.
        /// </summary>
        int Start { get; }

        /// <summary>
        /// The ending verse of the set. This is the same as the Start if the set represents a single verse.
        /// </summary>
        int End { get; }
    }

}
