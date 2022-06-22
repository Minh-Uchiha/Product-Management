using Microsoft.Extensions.Configuration;
using ProductManagement.DataAccess.Data;
using ProductManagement.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public IUserRepository User { get; private set; }

        public IProductRepository Product { get; private set; }

        public IProductTypeRepository ProductType { get; private set; }

        public UnitOfWork(ApplicationDbContext db, IConfiguration configuration)
        {
            _db = db;
            _configuration = configuration;
            User = new UserRepository(_db, _configuration);
            Product = new ProductRepository(_db, _configuration);
            ProductType = new ProductTypeRepository(_db);
        }

        // Save changes to the database asynchronously
        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
