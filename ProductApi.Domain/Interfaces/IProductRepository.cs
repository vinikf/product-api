using ProductApi.Domain.DTO;
using ProductApi.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Domain.Interfaces
{
    public interface IProductRepository
    {
        Task Add(Product dto);
        Task Delete(Product product);
        Task<Product> SearchById(int id);
        Task Update(Product product);
        Task<List<Product>> List(ProductSearchDTO dto);

    }
}
