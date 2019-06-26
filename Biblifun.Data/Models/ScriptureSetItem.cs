using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class ScriptureSetItem : AuditableEntity
    {
        public int ScriptureSetItemId { get; set; }

        public int ScriptureSetId { get; set; }

        public virtual ScriptureSet ScriptureSet { get; set; }

        public string VerseSetCode { get; set; }

        public string Summary { get; set; }

        public virtual ICollection<ScriptureSetItemCategory> ScriptureSetItemCategories { get; set; }
    }
}
