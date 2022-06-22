using ProductManagement.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManagement.Helpers.Helpers.Response
{
    public class ProductTypeGetResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Product> products { get; set; } = new List<Product>();
        public ProductTypeGetResponse(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}
