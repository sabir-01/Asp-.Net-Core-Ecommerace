using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerace.Models
{
    public class OrderDetails
    {
        [Key]
        public int orderDetails_id { get; set; }

        public int order_id { get; set; }

        public int product_id { get; set; }

        public int quantity { get; set; }

        public decimal price { get; set; }

        public decimal sub_total { get; set; }

        [ForeignKey("order_id")]
        public Order order { get; set; }

        [ForeignKey("product_id")]
        public Product product { get; set; }
    }
}