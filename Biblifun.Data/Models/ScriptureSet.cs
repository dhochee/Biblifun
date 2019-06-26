using System.Collections.Generic;

namespace Biblifun.Data.Models
{
    public class ScriptureSet : AuditableEntity
    {
        public int ScriptureSetId { get; set; }

        public string OwnedByUserId { get; set; }

        public virtual ApplicationUser OwnedByUser { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public bool IsPassageSet { get; set; }

        public virtual ICollection<ScriptureSetItem> ScriptureSetItems { get; set; }

        public virtual ICollection<ScriptureSetCategory> ScriptureSetCategories { get; set; }
    }
}
