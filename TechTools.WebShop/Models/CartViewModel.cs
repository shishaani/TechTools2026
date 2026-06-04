namespace TechTools.WebShop.Models
{
    public class CartViewModel
    {
        public List<CartItem> Items { get; set; } = [];
        public string? DiscountCode { get; set; }
        public decimal DiscountAmount { get; set; }

        public decimal Subtotal => Items.Sum(item => item.Total);
        public decimal AppliedDiscount => Math.Min(Subtotal, DiscountAmount);
        public decimal Total => Subtotal - AppliedDiscount;
    }
}
