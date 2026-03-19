namespace Ecommerace.Models
{
    public class CheckoutViewModel
    {
        public Customer Customer { get; set; }
        public List<Carts> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }

        // For form submission
        public string ShippingAddress { get; set; }
        public string PaymentMethod { get; set; }
        public string Notes { get; set; }
    }
}
