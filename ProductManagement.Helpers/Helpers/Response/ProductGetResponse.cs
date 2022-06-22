using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Helpers.Helpers.Response
{
    public class ProductGetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public double Size { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsActive { get; set; } = true;
        public ProductGetResponse()
        {

        }
        public ProductGetResponse(int id, string name, double price, double size, string? imageUrl, bool isActive)
        {
            Id = id;
            Name = name;
            Price = price;
            Size = size;
            ImageUrl = imageUrl;
            IsActive = isActive;
        }
    }
}
