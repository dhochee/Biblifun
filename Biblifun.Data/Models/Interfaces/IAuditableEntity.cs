// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using System;

namespace Biblifun.Data.Models.Interfaces
{
    public interface IAuditableEntity
    {
        DateTime CreatedDate { get; set; }

        string CreatedBy { get; set; }

        DateTime UpdatedDate { get; set; }

        string UpdatedBy { get; set; }

    }
}
