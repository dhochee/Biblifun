using System.Collections.Generic;

namespace Biblifun.Data.Models
{
    public class ScriptureSet : AuditableEntity
    {
        public ScriptureSet()
        {
            this.ScriptureSetItems = new HashSet<ScriptureSetItem>();
            this.ScriptureSetCategories = new HashSet<ScriptureSetCategory>();
        }

        public int ScriptureSetId { get; set; }

        public string OwnedByUserId { get; set; }

        public ApplicationUser OwnedByUser { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public bool IsPassageSet { get; set; }

        public ICollection<ScriptureSetItem> ScriptureSetItems { get; set; }

        public ICollection<ScriptureSetCategory> ScriptureSetCategories { get; set; }
    }
}
