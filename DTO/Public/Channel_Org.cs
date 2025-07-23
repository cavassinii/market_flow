using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Public
{
    public class Channel_Org
    {
        public int Org_Id { get; set; }
        public int Cahnnel_Id { get; set; }
        public string Access_Token { get; set; } = string.Empty;
        public string Refresh_Token { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
