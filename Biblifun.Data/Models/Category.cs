using System.Collections.Generic;

namespace Biblifun.Data.Models
{
    public class Category : AuditableEntity
    {
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }


        public virtual ICollection<ScriptureSetCategory> ScriptureSetCategories { get; set; }

        public virtual ICollection<ScriptureSetItemCategory> ScriptureSetItemCategories { get; set; }
    }
}
