using ProductApi.Domain.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace ProductApi.Domain.Entities
{
    public class Product : BaseEntity
    {
        [Required]
        public string DescricaoProduto { get; set; }
        public DateTime DataFabricacao { get; set; }
        public DateTime DataValidade { get; set; }
        public int CodigoFornecedor { get; set; }
        public string DescricaoFornecedor { get; set; }
        public string CNPJ { get; set; }
        public eSituacao Situacao { get; set; } = eSituacao.Ativo;
    }
}
