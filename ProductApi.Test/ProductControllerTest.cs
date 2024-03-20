using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Business;
using ProductApi.Controllers;
using ProductApi.Domain.DTO;
using ProductApi.Infra.Data.Context;
using ProductApi.Infra.Data.Repository;
using System.Threading.Tasks;
using Xunit;
using System;

namespace ProductApi.Test
{
    public class ProductControllerTest
    {
        private readonly DbContextOptions<ProductApiContext> _options;
        private readonly IMapper _mapper;
        private readonly ProductApiContext _dbContext;

        public ProductControllerTest()
        {
            var connectionString = "Server=(localdb)\\mssqllocaldb;Database=ProductApi;Trusted_Connection=True;MultipleActiveResultSets=true;";
            _options = new DbContextOptionsBuilder<ProductApiContext>()
                .UseSqlServer(connectionString)
                .Options;

            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>(); 
            });
            _mapper = mapperConfig.CreateMapper();

            _dbContext = new ProductApiContext(_options);
        }

        [Fact]
        public async Task GetProductById()
        {

            using (var context = new ProductApiContext(_options))
            {
                var repository = new ProductRepository(context);
                var application = new ProductApplication(repository, _mapper, _dbContext);
                var controller = new ProductController(application);

                var actionResult = await controller.SearchById(1006);

                var result = actionResult as OkObjectResult;
                var returnProduct = result.Value as ProductDTO;
                Assert.Equal("Lanterna traseira gol2", returnProduct.DescricaoFornecedor);
            }
        }

        [Fact]
        public async Task AddProduct()
        {
            using (var context = new ProductApiContext(_options))
            {
                var repository = new ProductRepository(context);
                var application = new ProductApplication(repository, _mapper, _dbContext);
                var controller = new ProductController(application);

                
                var productToAdd = new ProductAddDTO
                {
                    DescricaoProduto = "Um produto novo",
                    DescricaoFornecedor = "Um produto novo",
                    CodigoFornecedor = 12345678,
                    DataFabricacao = DateTime.Now,
                    DataValidade = DateTime.Now.AddDays(10),
                    CNPJ = "12345678145"
                };

                var actionResult = await controller.Add(productToAdd);

                Assert.IsType<OkResult>(actionResult);
                
            }
        }

        [Fact]
        public async Task UpdateProduct()
        {
            using (var context = new ProductApiContext(_options))
            {
                var repository = new ProductRepository(context);
                var application = new ProductApplication(repository, _mapper, _dbContext);
                var controller = new ProductController(application);

                var productToUpdate = new ProductUpdateDTO
                {
                    Id = 1006,
                    DescricaoProduto = "O produto mudou"
                };

                var actionResult = await controller.Update(productToUpdate);

                Assert.IsType<OkResult>(actionResult);
            }
        }

        [Fact]
        public async Task DeleteProduct()
        {
            using (var context = new ProductApiContext(_options))
            {
                var repository = new ProductRepository(context);
                var application = new ProductApplication(repository, _mapper, _dbContext);
                var controller = new ProductController(application);

                var productIdToDelete = 1004; 

                var actionResult = await controller.Delete(productIdToDelete);

                Assert.IsType<OkResult>(actionResult);
            }
        }
    }
}
