using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Org
{
    public class Product
    {
        public int Id { get; set; }
        public string Sku { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Reference { get; set; } = string.Empty;
        public string Url_image1 { get; set; } = string.Empty;
        public string Url_image2 { get; set; } = string.Empty;
        public string Url_image3 { get; set; } = string.Empty;
        public string Url_image4 { get; set; } = string.Empty;
        public string Url_image5 { get; set; } = string.Empty;
        public string Ncm { get; set; } = string.Empty;
        public string Cest { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Category_id { get; set; }
        public int Brand_id { get; set; }
        public decimal Weight_gross { get; set; }
        public decimal Weight_net { get; set; }
        public decimal Width { get; set; }
        public decimal Height { get; set; }
        public string Unit { get; set; } = string.Empty;
        public bool Is_active { get; set; } = true;
        public DateTime Created_at { get; set; } = new DateTime();
        public DateTime Updated_at { get; set; } = new DateTime();
    }
}
