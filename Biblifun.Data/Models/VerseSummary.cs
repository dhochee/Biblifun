using Biblifun.Data.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class VerseSummary : AuditableEntity
    {
        public int VerseSummaryId { get; set; }

        public string VerseSetCode { get; set; }

        public string Summary { get; set; }

        public string OwnedByUserId { get; set; }

        public ApplicationUser OwnedByUser { get; set; }
    }
}
