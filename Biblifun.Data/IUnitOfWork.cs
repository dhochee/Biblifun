// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using Biblifun.Data.Repositories.Interfaces;

namespace Biblifun.Data
{
    public interface IUnitOfWork
    {
        IVerseCacheRepository VerseCacheRepo { get; }

        int SaveChanges();
    }
}
