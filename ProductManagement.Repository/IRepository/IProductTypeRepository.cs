using ProductManagement.Helpers.Helpers.Request;
using ProductManagement.Helpers.Helpers.Response;
using ProductManagement.Models;
using ProductManagementWebApi.Helpers.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.IRepository
{
    public interface IProductTypeRepository : IRepository<ProductType>
    {
        void Update(ProductType productType, ProductTypePostRequest entity);
        IEnumerable<ProductTypeGetResponse> GetAll(GetRequest req);
        Task Add(ProductTypePostRequest entity);
        ProductType GetDetails(int Id);
    }
}
