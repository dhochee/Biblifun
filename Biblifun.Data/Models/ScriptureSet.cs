using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class ScriptureSet : AuditableEntity
    {
        public int ScriptureSetId { get; set; }

        public string OwnedByUserId { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public bool IsPassageSet { get; set; }


        public ICollection<Category> Categories { get; set; }
    }
}
