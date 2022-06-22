using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Helpers.Helpers.Request
{
    public class ProductPostRequest
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Brand { get; set; }
        public string? Color { get; set; }
        public double Size { get; set; }
        public IFormFile? Image { get; set; }
        public bool IsActive { get; set; } = true;
        public int ProductTypeId { get; set; }
    }
}
