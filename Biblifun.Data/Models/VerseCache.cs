namespace Biblifun.Data.Models
{
    public class VerseCache : AuditableEntity
    {
        public int VerseCacheId { get; set; }

        public string VerseSetCode { get; set; }

        public string Language { get; set; }

        public string Html { get; set; }

        public int SearchCount { get; set; }
    }
}
