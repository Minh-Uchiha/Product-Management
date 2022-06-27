using Microsoft.AspNetCore.Mvc;
using ProductManagement.Helpers.Helpers.Request;
using ProductManagement.Helpers.Helpers.Response;
using ProductManagement.Models;
using ProductManagementWebApi.Helpers.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.IRepository
{
    public interface IProductRepository : IRepository<Product>
    {
        void Update(Product product, ProductPostRequest productPostRequest);
        IEnumerable<ProductGetResponse> GetAll(GetRequest req);
        byte[] CSV();
        Task Add(ProductPostRequest entity);
    }
}
