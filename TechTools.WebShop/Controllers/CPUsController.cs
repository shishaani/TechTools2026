using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechTools.Models;
using TechTools.Services;
using TechTools.WebShop.Models;

namespace TechTools.WebShop.Controllers
{
    public class CPUsController(CPUService cpuService) : Controller
    {
        public IActionResult Index()
        {
            List<CPU> cpus = cpuService.ReadAll();
            return View(cpus);
        }

        [Authorize(Roles = "Admin,Shopmanager")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CPU c, IFormFile? picture)
        {
            if (ModelState.IsValid)
            {
                if (picture != null)
                {
                    string extension = Path.GetExtension(picture.FileName).ToLowerInvariant();
                    string[] allowedExtensions = [".jpg", ".jpeg", ".png"];

                    if (!allowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("Picture", "Verkeerde type afbeelding!");
                        return View(c);
                    }

                    string imageFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/gpus");
                    string pictureName = Guid.NewGuid() + extension;
                    string pictureFullPath = Path.Combine(imageFolder, pictureName);

                    using (var stream = new FileStream(pictureFullPath, FileMode.Create))
                    {
                        picture.CopyTo(stream);
                    }

                    c.Picture = pictureName;
                }
            }
            cpuService.Create(c);

            return View(c);
        }

        public IActionResult Details(int id)
        {
            CPU? c = cpuService.Read(id);

            if (c == null)
            {
                TempData["ErrorMessage"] = $"Geen CPU gevonden met id {id}.";
                return RedirectToAction("Index");
            }
            return View(c);
        }


        public IActionResult Delete(int id)
        {
            CPU? cpu = cpuService.Read(id);

            if (cpu == null)
            {
                TempData["ErrorMessage"] = $"Geen CPU gevonden met id {id}.";
                return RedirectToAction("Index");
            }
            return View(cpu);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            bool deleteSuccessful = cpuService.Delete(id);
            if (!deleteSuccessful)
            {
                TempData["SuccessMessage"] = "CPU verwijderd.";
            }

            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            CPU? cpu = cpuService.Read(id);

            if (cpu == null)
            {
                TempData["ErrorMessage"] = $"Geen CPU gevonden met id {id}.";
                return RedirectToAction("Index");
            }

            return View(cpu);
        }

        [HttpPost]
        public IActionResult Edit(CPU updatedCPU)
        {
            if (ModelState.IsValid)
            {
                bool updateSuccessful = cpuService.Update(updatedCPU);
                if (updateSuccessful)
                {
                    // boodschap als het gelukt is
                    TempData["SuccesMessage"] = "Data voor"
                                                + updatedCPU.Price + " "
                                                + updatedCPU.Description + " "
                                                + updatedCPU.Picture + " "
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

            return View(updatedCPU);
        }
    }
}
