using Microsoft.EntityFrameworkCore;
using ProductApi.Domain.DTO;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Enums;
using ProductApi.Domain.Interfaces;
using ProductApi.Infra.Data.Context;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductApi.Infra.Data.Repository
{
    public class ProductRepository : IProductRepository
    {
        protected ProductApiContext _db;

        public ProductRepository(ProductApiContext db)
        {
            _db = db;
        }

        public async Task Add(Product dto)
        {
            await _db.Products.AddAsync(dto);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(Product product)
        {
            product.Situacao = eSituacao.Inativo;
            _db.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task<Product> SearchById(int id)
        {
            return await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.Situacao == eSituacao.Ativo);
        }

        public async Task Update(Product product)
        {
            _db.Products.Update(product);
            await _db.SaveChangesAsync();
        }

        public async Task<List<Product>> List(ProductSearchDTO dto)
        {
            IQueryable<Product> query = _db.Products
                                        .Where(a => a.Situacao == eSituacao.Ativo);

            if (dto.DataValidade.HasValue)
                query = query.Where(p => p.DataValidade.Date == dto.DataValidade.Value.Date);

            if (dto.DataFabricacao.HasValue)
                query = query.Where(p => p.DataFabricacao.Date == dto.DataFabricacao.Value.Date);

            if (dto.CodigoFornecedor.HasValue)
                query = query.Where(p => p.CodigoFornecedor == dto.CodigoFornecedor);

            if (!string.IsNullOrEmpty(dto.DescricaoFornecedor))
                query = query.Where(p => p.DescricaoFornecedor.Contains(dto.DescricaoFornecedor));

            if (!string.IsNullOrEmpty(dto.DescricaoProduto))
                query = query.Where(p => p.DescricaoProduto.Contains(dto.DescricaoProduto));

            if (!string.IsNullOrEmpty(dto.CNPJ))
                query = query.Where(p => p.CNPJ.Equals(dto.CNPJ));

            query = query.Skip((dto.Pagina - 1) * dto.QuantidadePorPagina).Take(dto.QuantidadePorPagina);

            return await query.ToListAsync();

        }
    }
}
