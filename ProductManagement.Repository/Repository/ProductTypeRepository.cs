using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductManagement.DataAccess.Data;
using ProductManagement.Helpers.Helpers.Request;
using ProductManagement.Helpers.Helpers.Response;
using ProductManagement.Models;
using ProductManagement.Repository.IRepository;
using ProductManagementWebApi.Helpers.Request;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.Repository
{
    public class ProductTypeRepository : Repository<ProductType>, IProductTypeRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductTypeRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task Add(ProductTypePostRequest entity)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductTypePostRequest, ProductType>();
            });
            var mapper = new Mapper(config);
            ProductType AddedObj = mapper.Map<ProductType>(entity);
            await _db.ProductTypes.AddAsync(AddedObj);
        }
        public ProductType GetDetails(int Id)
        {
            ProductType productType = _db.ProductTypes.Include(pt => pt.products).FirstOrDefault(pt => pt.Id == Id);
            return productType;
        }

        // Get all product types. Support paging, searching, sorting
        public IEnumerable<ProductTypeGetResponse> GetAll(GetRequest req)
        {
            var CurrPageNumber = req.PageNumber ?? 1;
            var CurrPageSize = req.PageSize ?? 5;
            var productTypes = (from productType in _db.ProductTypes                               
                                select new ProductTypeGetResponse(
                                productType.Id,
                                productType.Name)).ToList();
            if (req.SearchReq != null)
            {
                // Filter the list by some search params
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(ProductTypeGetResponse)).Find(req.SearchReq.FieldName, true);
                if (prop != null)
                {
                    productTypes = (from p in productTypes
                                    where Convert.ToString(prop.GetValue(p)).Contains(req.SearchReq.Query)
                                select p).ToList();
                }
            }
            if (req.SortReq != null)
            {
                // Filter the list by some sort params
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(ProductTypeGetResponse)).Find(req.SortReq.FieldName, true);
                if (prop != null)
                {
                    if (req.SortReq.IsAsc) productTypes = (from p in productTypes
                                                           select p).OrderBy(p => prop.GetValue(p)).ToList();
                    else productTypes = (from p in productTypes
                                         select p).OrderByDescending(p => prop.GetValue(p)).ToList();
                }
            }
            return productTypes.Skip(CurrPageSize * (CurrPageNumber - 1)).Take(CurrPageSize).ToList();

        }

        public void Update(int Id, ProductTypePostRequest entity)
        {
            ProductType productType = GetFirstOrDefault(u => u.Id == Id);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductTypePostRequest, ProductType>();
            });
            var mapper = new Mapper(config);
            productType = mapper.Map<ProductType>(entity);
            _db.ProductTypes.Update(productType);
        }

    }
}
