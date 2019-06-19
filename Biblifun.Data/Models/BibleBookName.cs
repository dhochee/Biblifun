using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class BibleBookName
    {
        public int BibleBookNameId { get; set; }


        public int BibleBookId { get; set; }

        public BibleBook Book { get; set; }


        public string Language { get; set; }

        public string Name { get; set; }

        public string Abbreviations { get; set; }
    }
}
