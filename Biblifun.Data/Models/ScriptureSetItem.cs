using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class ScriptureSetItem : AuditableEntity
    {
        public int ScriptureSetItemId { get; set; }

        public int ScriptureSetId { get; set; }

        public ScriptureSet ScriptureSet { get; set; }

        public string VerseSetCode { get; set; }

        public string Summary { get; set; }

        public ICollection<Category> Categories { get; set; }
    }
}
