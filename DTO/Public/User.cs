﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO.Public
{
    public class User
    {
        public int Id { get; set; }
        public int Organization_id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public DateTime Created_at { get; set; } = new DateTime();
    }
}
