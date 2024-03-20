using AutoMapper;
using ProductApi.Domain.DTO;
using ProductApi.Domain.Entities;

namespace ProductApi
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ProductDTO, Product>();
            CreateMap<Product, ProductDTO>();
            CreateMap<ProductAddDTO, Product>();
            CreateMap<ProductSearchDTO, Product>();
            CreateMap<ProductUpdateDTO, Product>()
                .ForMember(dest => dest.CodigoFornecedor, opt => opt.MapFrom((src, dest) => src.CodigoFornecedor == 0 || src.CodigoFornecedor == null ? dest.CodigoFornecedor : src.CodigoFornecedor))
                .ForMember(dest => dest.CNPJ, opt => opt.MapFrom((src, dest) => src.CNPJ ?? dest.CNPJ))
                .ForMember(dest => dest.DataFabricacao, opt => opt.MapFrom((src, dest) => src.DataFabricacao ?? dest.DataFabricacao))
                .ForMember(dest => dest.DescricaoFornecedor, opt => opt.MapFrom((src, dest) => src.DescricaoFornecedor ?? dest.DescricaoFornecedor))
                .ForMember(dest => dest.DataValidade, opt => opt.MapFrom((src, dest) => src.DataValidade ?? dest.DataValidade))
                .ForMember(dest => dest.DescricaoProduto, opt => opt.MapFrom((src, dest) => src.DescricaoProduto ?? dest.DescricaoProduto));
        }
    }
}
