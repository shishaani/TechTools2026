using TechTools.Models;

namespace TechTools.WebShop.Models
{
    public class GPUDetailsViewModel
    {
        public GPU GPU { get; set; } = new();
        public List<Review> Reviews { get; set; } = [];
        public Review NewReview { get; set; } = new();
    }
}
