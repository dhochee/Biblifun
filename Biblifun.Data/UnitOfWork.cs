// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Biblifun.Data.Repositories;
using Biblifun.Data.Repositories.Interfaces;

namespace Biblifun.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        readonly ApplicationDbContext _context;

        IVerseCacheRepository _cachedVerses;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IVerseCacheRepository VerseCacheRepo
        {
            get
            {
                if (_cachedVerses == null)
                    _cachedVerses = new VerseCacheRepository(_context);

                return _cachedVerses;
            }
        }

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
