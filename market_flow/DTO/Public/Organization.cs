using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Public
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Schema_name { get; set; } = string.Empty;
        public DateTime Created_at { get; set; } = new DateTime();
    }
}
