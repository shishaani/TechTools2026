namespace TechTools.WebShop.Models
{
    public class CartItem
    {
        public int GPUId { get; set; }
        public string Brand { get; set; } = string.Empty;
        public string Model { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Picture { get; set; }
        public int Quantity { get; set; }

        public string Name => $"{Brand} {Model}";
        public decimal Total => Price * Quantity;

        public int CPUId { get; internal set; }
    }
}
