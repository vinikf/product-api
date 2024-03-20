using ProductApi.Domain.DTO;
using ProductApi.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApi.Application.Interfaces
{
    public interface IProductApplication
    {
        Task Add(ProductAddDTO dto);
        Task Delete(int id);
        Task<List<ProductDTO>> List(ProductSearchDTO search);
        Task<ProductDTO> SearchById(int id);
        Task Update(ProductUpdateDTO dto);
    }
}
