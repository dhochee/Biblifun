// =============================
// Email: info@ebenmonney.com
// www.ebenmonney.com/templates
// =============================

using Biblifun.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Biblifun.Data
{
    public interface IUnitOfWork
    {
        /*
        ICustomerRepository Customers { get; }

        IProductRepository Products { get; }
        IOrdersRepository Orders { get; }
        */

        int SaveChanges();
    }
}
