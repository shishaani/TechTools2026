using TechTools.Models;

namespace TechTools.WebShop.Models
{
    public class CPUDetailsViewModel
    {
        public CPU CPU { get; set; } = new();
        public List<Review> Reviews { get; set; } = [];
        public Review NewReview { get; set; } = new();
    }
}
