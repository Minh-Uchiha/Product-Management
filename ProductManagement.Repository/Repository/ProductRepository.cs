using AutoMapper;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ProductManagement.DataAccess.Data;
using ProductManagement.Helpers.Helpers.Request;
using ProductManagement.Helpers.Helpers.Response;
using ProductManagement.Models;
using ProductManagement.Repository.IRepository;
using ProductManagementWebApi.Helpers.Common;
using ProductManagementWebApi.Helpers.Request;
using ProductManagementWebApi.Helpers.Response;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Repository.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IConfiguration _configuration;
        public ProductRepository(ApplicationDbContext db, IConfiguration configuration) : base(db)
        {
            _db = db;
            _configuration = configuration;
        }

        public async Task Add(ProductPostRequest entity)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductPostRequest, Product>();
            });
            var mapper = new Mapper(config);
            Product AddedObj = mapper.Map<Product>(entity);

            // Upload the user's avatar to azure storage account and store the avatar url in the database
            AddedObj.ImageUrl = await FileHelper.UploadProductImage(entity.Image, _configuration.GetSection("AzureStorageAcc")["ConnectionString"], _configuration.GetSection("AzureStorageAcc")["ProductContainerName"]);

            await _db.Products.AddAsync(AddedObj);
        }

        // Get all products. Support paging, searching, sorting
        public IEnumerable<ProductGetResponse> GetAll(GetRequest req)
        {
            var CurrPageNumber = req.PageNumber ?? 1;
            var CurrPageSize = req.PageSize ?? 5;
            var products = (from product in _db.Products
                            select new ProductGetResponse(
                            product.Id,
                            product.Name,
                            product.Price,
                            product.Size,
                            product.ImageUrl,
                            product.IsActive)).Where(p => p.IsActive == true).ToList();
            if (req.SearchReq != null)
            {
                // Filter the list by some search params
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(ProductGetResponse)).Find(req.SearchReq.FieldName, true);
                if (prop != null)
                {
                    products = (from p in products
                                where Convert.ToString(prop.GetValue(p)).Contains(req.SearchReq.Query)
                                select p).ToList();
                }
            }
            if (req.SortReq != null)
            {
                // Filter the list by some sort params
                PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(ProductGetResponse)).Find(req.SortReq.FieldName, true);
                if (prop != null)
                {
                    if (req.SortReq.IsAsc) products = (from p in products
                                                       select p).OrderBy(p => prop.GetValue(p)).ToList();
                    else products = (from p in products
                                     select p).OrderByDescending(p => prop.GetValue(p)).ToList();
                }
            }
            return products.Skip(CurrPageSize * (CurrPageNumber - 1)).Take(CurrPageSize).ToList();

        }

        // Export data to CSV files
        public byte[] CSV()
        {
            var products = (from product in _db.Products
                            select new ProductGetResponse(
                            product.Id,
                            product.Name,
                            product.Price,
                            product.Size,
                            product.ImageUrl,
                            product.IsActive)).ToList();
            using (var workbook = new XLWorkbook())
            {
                var worksheet = workbook.Worksheets.Add("Products");
                var currentRow = 1;
                worksheet.Cell(currentRow, 1).Value = "Id";
                worksheet.Cell(currentRow, 2).Value = "Name";
                worksheet.Cell(currentRow, 3).Value = "Price";
                worksheet.Cell(currentRow, 4).Value = "Size";
                worksheet.Cell(currentRow, 5).Value = "ImageUrl";
                worksheet.Cell(currentRow, 6).Value = "IsActive";
                foreach (var product in products)
                {
                    currentRow ++;
                    worksheet.Cell(currentRow, 1).Value = product.Id;
                    worksheet.Cell(currentRow, 2).Value = product.Name;
                    worksheet.Cell(currentRow, 3).Value = product.Price;
                    worksheet.Cell(currentRow, 4).Value = product.Size;
                    worksheet.Cell(currentRow, 5).Value = product.ImageUrl;
                    worksheet.Cell(currentRow, 6).Value = product.IsActive;
                }

                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    var content = stream.ToArray();
                    return content;
                }
            }

        }

        public void Update(Product product, ProductPostRequest productPostRequest)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<ProductPostRequest, Product>();
            });
            var mapper = new Mapper(config);
            product = mapper.Map<Product>(productPostRequest);
            _db.Products.Update(product);
        }

    }
}
