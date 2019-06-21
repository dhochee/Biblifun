using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class ScriptureSetItemCategory
    {
        public int ScriptureSetItemId { get; set; }

        public ScriptureSetItem ScriptureSetItem { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
