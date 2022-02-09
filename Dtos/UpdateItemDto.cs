using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Catalog.Dtos
{
    public record UpdateItemDto
    {
        [Required]
        public String Name { get; set; }
        [Required]
        [Range(1, 10000)]
        public Decimal Price { get; set; }
    }
}