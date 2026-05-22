using Microsoft.AspNetCore.Mvc;
using TechTools.Models;
using TechTools.Services;

namespace TechTools.WebShop.Controllers
{
    public class GPUsController(GPUService gpuService) : Controller
    {
        public IActionResult Index()
        {
            List<GPU> gpus = gpuService.ReadAll();
            return View(gpus);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(GPU g)
        {
            if (ModelState.IsValid)
            {
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

            return View(gpu);
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
