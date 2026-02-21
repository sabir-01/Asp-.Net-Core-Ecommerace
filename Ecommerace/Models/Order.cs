using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ecommerace.Models
{
    public class Order
    {
        [Key]
        public int order_id { get; set; }

        public int customer_id { get; set; }

        public DateTime order_date { get; set; } = DateTime.Now;

        public string order_status { get; set; } = "Pending";
        // Pending / Shipped / Completed / Cancelled

        public string shipping_address { get; set; }

        public decimal total_amount { get; set; }

        [ForeignKey("customer_id")]
        public Customer customer { get; set; }

        public List<OrderDetails> orderDetails { get; set; }
    

    }
}
