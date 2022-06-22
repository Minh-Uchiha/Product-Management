using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public IUserRepository User { get; }
        public IProductRepository Product { get; }
        public IProductTypeRepository ProductType { get; }
        Task SaveAsync();

    }
}
