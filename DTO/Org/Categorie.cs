using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Org
{
    public class Categorie
    {
        public DateTime? Created_at { get; set; } = new DateTime();
        public int Id { get; set; }
        public bool Is_final { get; set; } = true;
        public string Name { get; set; } = string.Empty;
        public int? Parent_id { get; set; }
        public DateTime? Updated_at { get; set; } = new DateTime();
    }
}
