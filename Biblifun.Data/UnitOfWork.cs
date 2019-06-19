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

        /*
        ICustomerRepository _customers;
        */


        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }


        /*
        public ICustomerRepository Customers
        {
            get
            {
                if (_customers == null)
                    _customers = new CustomerRepository(_context);

                return _customers;
            }
        }
        */

        public int SaveChanges()
        {
            return _context.SaveChanges();
        }
    }
}
