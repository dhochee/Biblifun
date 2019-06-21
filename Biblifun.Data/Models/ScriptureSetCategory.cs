using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class ScriptureSetCategory
    {
        public int ScriptureSetId { get; set; }

        public ScriptureSet ScriptureSet { get; set; }

        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}
