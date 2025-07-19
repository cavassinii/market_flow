using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Org
{
    public class Brand
    {
        public DateTime? Created_at { get; set; } = new DateTime();
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime? Updated_at { get; set; } = new DateTime();
    }
}
