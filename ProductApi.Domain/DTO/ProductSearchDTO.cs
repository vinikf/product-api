using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Domain.DTO
{
    public class ProductSearchDTO
    {
        public string DescricaoProduto { get; set; } = string.Empty;
        public DateTime? DataFabricacao { get; set; }
        public DateTime? DataValidade { get; set; }
        public string DescricaoFornecedor { get; set; } = string.Empty;
        public int? CodigoFornecedor { get; set; }
        public string CNPJ { get; set; } = string.Empty;
        public int QuantidadePorPagina { get; set; }
        public int Pagina { get; set; }
    }
}
