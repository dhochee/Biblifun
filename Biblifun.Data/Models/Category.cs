using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class Category : AuditableEntity
    {
        public int CategoryId { get; set; }

        public string UserId { get; set; }

        public string Name { get; set; }

        public string Language { get; set; }


        public ICollection<ScriptureSetItem> ScriptureSetItems { get; set; }

        public ICollection<ScriptureSet> ScriptureSets { get; set; }
    }
}
