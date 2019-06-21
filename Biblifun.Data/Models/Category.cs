using System.Collections.Generic;

namespace Biblifun.Data.Models
{
    public class Category : AuditableEntity
    {
        public Category()
        {
            this.ScriptureSetCategories = new HashSet<ScriptureSetCategory>();
            this.ScriptureSetItemCategories = new HashSet<ScriptureSetItemCategory>();
        }

        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }


        public ICollection<ScriptureSetCategory> ScriptureSetCategories { get; set; }

        public ICollection<ScriptureSetItemCategory> ScriptureSetItemCategories { get; set; }
    }
}
