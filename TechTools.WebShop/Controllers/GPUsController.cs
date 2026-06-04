using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTools.Models;
using TechTools.Services;
using TechTools.WebShop.Models;

namespace TechTools.WebShop.Controllers
{
    public class GPUsController(GPUService gpuService, ReviewService reviewService) : Controller
    {
        public IActionResult Index()
        {
            List<GPU> gpus = gpuService.ReadAll();
            return View(gpus);
        }

        [Authorize(Roles = "Admin,Shopmanager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GPU g, IFormFile? picture)
        {
            if (ModelState.IsValid)
            {

                if(picture != null)
                {
                    string extension = Path.GetExtension(picture.FileName).ToLowerInvariant();
                    string[] allowedExtensions = [".jpg", ".jpeg", ".png"];

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Picture", "Verkeerde type afbeelding!");
                        return View(g);
                    }
                    
                    string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/gpus");
                    string pictureName = Guid.NewGuid() + extension;
                    string pictureFullPath = Path.Combine(imageFolder, pictureName);

                    using (var stream = new FileStream(pictureFullPath, FileMode.Create))
                    {
                        picture.CopyTo(stream);
                    }

                    g.Picture = pictureName;
                }

                gpuService.Create(g);
                // terugkeren naar index pagina
                return RedirectToAction("Index");

            }

            return View(g);
        }

        public IActionResult Details(int id)
        {
            GPU? gpu = gpuService.Read(id);

            if (gpu == null)
            {
                TempData["ErrorMessage"] = $"Geen GPU gevonden met id {id}.";
                return RedirectToAction("Index");
            }

            return View(new GPUDetailsViewModel
            {
                GPU = gpu,
                Reviews = reviewService.ReadForGPU(id),
                NewReview = new Review { GPUId = id }
            });
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Shopmanager,Klant")]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(Review review) 
        {
            GPU? gpu = gpuService.Read(review.GPUId);
            if (gpu == null)
            {
                TempData["ErrorMessage"] = $"Geen GPU gevonden met id {review.GPUId}.";
                return RedirectToAction("Index");
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Review niet opgeslagen. Controleer naam, beoordeling en score.";
                return RedirectToAction("Details", new { id = review.GPUId });
            }

            reviewService.Create(review);
            TempData["SuccessMessage"] = "Review toegevoegd.";

            return RedirectToAction("Details", new { id = review.GPUId });
        }

        public IActionResult Delete(int id)
        {
            GPU? gpu = gpuService.Read(id);

            if (gpu == null)
            {
                TempData["ErrorMessage"] = $"Geen GPU gevonden met id {id}.";
                return RedirectToAction("Index");
            }
            return View(gpu);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleteSuccessful = gpuService.Delete(id);
            if (!deleteSuccessful)
            {
                TempData["SuccessMessage"] = "GPU verwijderd.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            GPU? gpu = gpuService.Read(id);

            if (gpu == null)
            {
                TempData["ErrorMessage"] = $"Geen GPU gevonden met id {id}.";
                return RedirectToAction("Index");
            }

            return View(gpu);
        }

        [HttpPost]
        public IActionResult Edit(GPU updatedGPU)
        {
            if (ModelState.IsValid)
            {
                bool updateSuccessful = gpuService.Update(updatedGPU);
                if (updateSuccessful)
                {
                    // boodschap als het gelukt is
                    TempData["SuccesMessage"] = "Data voor"
                                                + updatedGPU.Price + " "
                                                + updatedGPU.Description + " "
                                                + updatedGPU.Picture + " "
                                                + "aangepast";
                                                
                }
                else
                {
                    // boodschap als er iets fout gelopen is
                    TempData["ErrorMessage"] = "Update niet gelukt";
                }

                // terugkeren naar index
                return RedirectToAction("Index");
            }

            return View(updatedGPU);
        }


    }
}
