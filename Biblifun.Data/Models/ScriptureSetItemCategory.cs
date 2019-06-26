using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class ScriptureSetItemCategory
    {
        public int ScriptureSetItemId { get; set; }

        public virtual ScriptureSetItem ScriptureSetItem { get; set; }

        public int CategoryId { get; set; }

        public virtual Category Category { get; set; }
    }
}
