using System.ComponentModel.DataAnnotations;

namespace Ecommerace.Models
{
    public class Catagory
    {
        [Key]
        public int category_id { get; set; }
        public string category_name { get; set; }

        public List<Product> products   { get; set; }
    }
}
