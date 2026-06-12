using TechTools.Data;
using TechTools.Models;

namespace TechTools.Services
{
    public class ReviewService(ApplicationDbContext db)
    {
        public List<Review> ReadForGPU(int gpuId)
        {
            return db.Reviews
                .Where(review => review.GPUId == gpuId)
                .OrderByDescending(review => review.Id)
                .ToList();
        }

        public List<Review> ReadForCPU(int cpuId)
        {
            return db.Reviews
                .Where(review => review.CPUId == cpuId)
                .OrderByDescending(review => review.Id)
                .ToList();
        }


        public void Create(Review review)
        {
            db.Reviews.Add(review);
            db.SaveChanges();
        }
    }
}
