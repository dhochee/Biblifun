// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using Biblifun.Data.Models.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace Biblifun.Data.Models
{
    public class AuditableEntity : IAuditableEntity
    {
        public DateTime CreatedDate { get; set; }

        [MaxLength(256)]
        public string CreatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }

        [MaxLength(256)]
        public string UpdatedBy { get; set; }
    }
}
