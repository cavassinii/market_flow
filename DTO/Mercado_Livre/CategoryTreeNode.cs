using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Mercado_Livre
{
    public class CategoryTreeNode
    {
        public string MlId { get; set; } = null!;       // Id da categoria no Mercado Livre
        public string Name { get; set; } = null!;       // Nome da categoria

        public string? ParentMlId { get; set; }          // Id do pai direto (null se for raiz)

        public List<CategoryTreeNode> Children { get; set; } = new();  // Filhos para montar a árvore
    }

}
