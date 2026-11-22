using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStore.Models
{
    public partial class Product
    {
        [NotMapped]
        public string ImagePath { get; set; } = "";
    }
}
