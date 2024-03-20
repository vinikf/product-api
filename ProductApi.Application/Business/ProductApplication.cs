using AutoMapper;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.DTO;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Interfaces;
using ProductApi.Infra.Data.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductApi.Application.Business
{
    public class ProductApplication : IProductApplication
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ProductApiContext _dbContext;

        public ProductApplication(IProductRepository repository, IMapper mapper, ProductApiContext dbContext)
        {
            _repository = repository;
            _mapper = mapper;
            _dbContext = dbContext;
        }

        public async Task Add(ProductAddDTO dto)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    VerifyValidity(dto.DataFabricacao, dto.DataValidade);
                    var roductMapped = _mapper.Map<Product>(dto);
                    await _repository.Add(roductMapped);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task Delete(int id)
        {
            var product = await _repository.SearchById(id);

            if (product == null)
                throw new Exception("Product not found");

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    await _repository.Delete(product);

                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public async Task<List<ProductDTO>> List(ProductSearchDTO search)
        {
            search.Pagina = search.Pagina <= 0 ? 1 : search.Pagina;
            search.QuantidadePorPagina = search.QuantidadePorPagina <= 0 ? 10 : search.QuantidadePorPagina;

            var productList = await _repository.List(search);
            var productDTOList = _mapper.Map<List<ProductDTO>>(productList);
            return productDTOList;
        }

        public async Task<ProductDTO> SearchById(int id)
        {
            var product = await _repository.SearchById(id);

            if (product == null)
                throw new Exception("Product not found");

            return _mapper.Map<ProductDTO>(product);
        }

        public async Task Update(ProductUpdateDTO dto)
        {
            var product = await _repository.SearchById(dto.Id);

            if (product == null)
                throw new Exception("Product not found");

            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (dto.DataValidade.HasValue && dto.DataFabricacao.HasValue)
                    {
                        VerifyValidity(dto.DataFabricacao.Value, dto.DataValidade.Value);
                    }
                    else if (dto.DataValidade.HasValue)
                    {
                        VerifyValidity(product.DataFabricacao, dto.DataValidade.Value);
                    }
                    else if (dto.DataFabricacao.HasValue)
                    {
                        VerifyValidity(product.DataValidade, dto.DataFabricacao.Value);
                    }

                    _mapper.Map(dto, product);
                    await _repository.Update(product);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private bool VerifyValidity(DateTime productionDate, DateTime expirationDate)
        {
            if (productionDate >= expirationDate) 
                throw new Exception("Data de fabricação não pode ser maior ou igual a data de validade");

            return true;
        }
    }
}
